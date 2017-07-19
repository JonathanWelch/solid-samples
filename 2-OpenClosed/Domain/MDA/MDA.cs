using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using _2_OpenClosed.Common.Utilities;
using _2_OpenClosed.DataAccess.DTO;

namespace _2_OpenClosed.Domain.MDA
{
    public class MDA
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly IList<MDAItem> _items;

        public MDA(IDateTimeService dateTimeService, string mdaNumber, short transferPercent)
        {
            _dateTimeService = dateTimeService;
            MDANumber = mdaNumber;
            TransferPercent = transferPercent;
            UncommittedEvents = new List<IEvent>();
            _items = new Collection<MDAItem>();
        }

        public MDA(IDateTimeService dateTimeService, ScheduledMDAListDTO scheduledMdaListDto)
            : this(dateTimeService, scheduledMdaListDto.MDANumber, scheduledMdaListDto.TransferPercent)
        {
        }

        public string MDANumber { get; private set; }

        public short TransferPercent { get; private set; }

        public DateTime? BookedDate { get; set; }

        public IList<IEvent> UncommittedEvents { get; private set; }

        public IReadOnlyCollection<MDAItem> Items
        {
            get
            {
                return new ReadOnlyCollection<MDAItem>(_items);
            }
        }

        public void AddItem(string skuExternalId, MerretMDAItemDTO mdaItemDto, ScheduledMDAItemDTO existingItem)
        {
            if (existingItem != null)
            {
                existingItem.Validate();

                _items.Add(new MDAItem(_dateTimeService, this, skuExternalId, mdaItemDto.SkuExternalId, mdaItemDto.MDAQuantity, mdaItemDto.MDAReceivedQuantity, new MDAItemProcessingInformation(existingItem)));
            }
            else
            {
                _items.Add(new MDAItem(_dateTimeService, this, skuExternalId, mdaItemDto.SkuExternalId, mdaItemDto.MDAQuantity, mdaItemDto.MDAReceivedQuantity));
            }
        }

        public void AddBookedDate()
        {
            if (BookedDate == null)
            {
                return;
            }

            UncommittedEvents.Add(new MDABookedDateEvent(MDANumber, (DateTime)BookedDate));
        }

        public void MarkAsProcessedIfNecessary()
        {
            if (Items.All(x => x.IsProcessed))
            {
                UncommittedEvents.Add(new MDAProcessedEvent(MDANumber, _dateTimeService.GetDate()));
            }
        }
    }
}
