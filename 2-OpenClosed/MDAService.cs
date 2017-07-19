using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using _2_OpenClosed.Common.Utilities;
using _2_OpenClosed.DataAccess;
using _2_OpenClosed.DataAccess.Command;
using _2_OpenClosed.DataAccess.DTO;
using _2_OpenClosed.DataAccess.Queries;
using MDA = _2_OpenClosed.Domain.MDA;

namespace _2_OpenClosed
{
    public interface IMDAService
    {
        IEnumerable<MDA.MDA> BuildMDAList(string warehouseExternalId, string fromWarehouseExternalId);

        void CommitChanges(IEnumerable<MDA.MDA> mdaList);
    }

    public class MDAService : IMDAService
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly ICommandAndQueryExecutor _executor;

        public MDAService() : this(new CommandAndQueryExecutor(), new DateTimeService())
        {
        }

        public MDAService(ICommandAndQueryExecutor executor, IDateTimeService dateTimeService)
        {
            _executor = executor;
            _dateTimeService = dateTimeService;
        }

        public IEnumerable<MDA.MDA> BuildMDAList(string fromWarehouseExternalId, string toWarehouseExternalId)
        {
            var mdaList = new List<MDA.MDA>();

            string previousMdaForBatching = null;

            while (true)
            {
                var batchOfScheduledMdaListDto = LookUpMdasFromIwtDatabase(fromWarehouseExternalId, toWarehouseExternalId, previousMdaForBatching);

                if (batchOfScheduledMdaListDto.Count == 0)
                {
                    break;
                }

                // TODO: combine LookUpMdaItemsFromMerretReportDatabase + LookUpMDAsFromMerretReportDB queries - Talk to Tim/Vidhya
                var merretMdaItemsList = LookUpMdaItemsFromMerretReportDatabase(batchOfScheduledMdaListDto).ToList();
                var merretMdaListDto = LookUpMdAsFromMerretReportDatabase(batchOfScheduledMdaListDto).ToList();

                var currentMdaList = BuildMdaListFromBatch(batchOfScheduledMdaListDto, merretMdaItemsList).ToList();

                UpdateMdaBookedDateFromMerret(currentMdaList, merretMdaListDto);

                mdaList.AddRange(currentMdaList);

                previousMdaForBatching = batchOfScheduledMdaListDto.Last().MDANumber;
            }

            return mdaList;
        }

        private IEnumerable<MerretMDAItemDTO> LookUpMdaItemsFromMerretReportDatabase(IList<ScheduledMDAListDTO> batchOfScheduledMdaListDto)
        {
            return _executor.Query(new RetrieveMerretMDAItemsList(batchOfScheduledMdaListDto.Select(x => x.MDANumber).Distinct()));
        }

        private IEnumerable<MerretMDADTO> LookUpMdAsFromMerretReportDatabase(IList<ScheduledMDAListDTO> batchOfScheduledMdaListDto)
        {
            return _executor.Query(new RetrieveMerretMDAList(batchOfScheduledMdaListDto.Select(x => x.MDANumber).Distinct()));
        }

        private IList<ScheduledMDAListDTO> LookUpMdasFromIwtDatabase(string fromWarehouseExternalId, string toWarehouseExternalId, string previousMdaForBatching)
        {
            var batchOfScheduledMdaListDto =
                _executor.Query(new RetrieveScheduledMDAList(fromWarehouseExternalId, toWarehouseExternalId, previousMdaForBatching));
            return batchOfScheduledMdaListDto;
        }

        private static void UpdateMdaBookedDateFromMerret(IEnumerable<MDA.MDA> mdaList, List<MerretMDADTO> merretMdaListDto)
        {
            foreach (var mda in mdaList)
            {
                var merretMdaDto = merretMdaListDto.FirstOrDefault(x => x.MDANumber == mda.MDANumber);
                if (merretMdaDto != null && merretMdaDto.BookingDateTime != null)
                {
                    mda.BookedDate = merretMdaDto.BookingDateTime;
                }
            }
        }

        private Dictionary<int, string> MapExternalIdsToSkus(IEnumerable<MerretMDAItemDTO> merretMdaItemsListDto)
        {
            var distinctExternalIds = merretMdaItemsListDto.Select(x => x.SkuExternalId).Distinct();
            var batchesOfDistinctIds = distinctExternalIds.Batch(1000);
            return batchesOfDistinctIds.Select(batchOfDistinctIds => _executor.Query(new RetrieveExternalIdToSkuMap(batchOfDistinctIds))).SelectMany(skuExternalIdMap => skuExternalIdMap).ToDictionary(skuMap => skuMap.Key, skuMap => skuMap.Value);
        }

        public void CommitChanges(IEnumerable<MDA.MDA> mdaList)
        {
            foreach (MDA.IEvent uncommittedEvent in mdaList.SelectMany(mda => mda.UncommittedEvents))
            {
                if (uncommittedEvent.GetType() == typeof(MDA.MDABookedDateEvent))
                {
                    var @event = (MDA.MDABookedDateEvent)uncommittedEvent;
                    _executor.Execute(new UpdateMdaBookedDate(@event.MDANumber, @event.BookedDate));
                }
                else if (uncommittedEvent.GetType() == typeof(MDA.MDAItemProcessedEvent))
                {
                    var @event = (MDA.MDAItemProcessedEvent)uncommittedEvent;
                    _executor.Execute(new UpdateProcessedMdaItem(
                        @event.MDANumber,
                        @event.SKUExternalId,
                        @event.ReceiptThresholdAtTimeOfTransfer,
                        @event.Quantity,
                        @event.ReceivedQuantityAtTimeOfTransfer,
                        @event.TransferPercent,
                        @event.TransferQuantity,
                        @event.ProcessedDate,
                        @event.DestinationWarehouse));
                }
                else if (uncommittedEvent.GetType() == typeof(MDA.MDAProcessedEvent))
                {
                    var @event = (MDA.MDAProcessedEvent)uncommittedEvent;

                    _executor.Execute(new MarkMdaAsProcessedCommand(@event.MDANumber, @event.ProcessedDate));
                }
                else
                {
                    throw new InvalidOperationException("Found unknown event type.");
                }
            }
        }

        private IEnumerable<MDA.MDA> BuildMdaListFromBatch(IEnumerable<ScheduledMDAListDTO> scheduledMdaListDto, IList<MerretMDAItemDTO> merretMdaItemsListDto)
        {
            IList<MDA.MDA> mdaList = new List<MDA.MDA>();

            var skuExternalIdMap = MapExternalIdsToSkus(merretMdaItemsListDto);

            foreach (var scheduledMda in scheduledMdaListDto)
            {
                var mda = new MDA.MDA(_dateTimeService, scheduledMda);

                var mdaItemsByMda = FindMatchingMerretMda(merretMdaItemsListDto, scheduledMda);

                foreach (var mdaItemDto in mdaItemsByMda)
                {
                    var existingItem = FindMatchingIwtMdaItem(scheduledMda, mdaItemDto);
                    var skuExternalId = skuExternalIdMap[mdaItemDto.SkuExternalId];

                    mda.AddItem(skuExternalId, mdaItemDto, existingItem);
                }

                mdaList.Add(mda);
            }

            return mdaList;
        }

        private static IEnumerable<MerretMDAItemDTO> FindMatchingMerretMda(IList<MerretMDAItemDTO> merretMdaItemsListDto, ScheduledMDAListDTO scheduledMda)
        {
            return merretMdaItemsListDto.Where(x => x.MDANumber == scheduledMda.MDANumber);
        }

        private static ScheduledMDAItemDTO FindMatchingIwtMdaItem(ScheduledMDAListDTO scheduledMda, MerretMDAItemDTO mdaItemDto)
        {
            return scheduledMda.Items.SingleOrDefault(x => x.SkuExternalId == mdaItemDto.SkuExternalId);
        }
    }
}
