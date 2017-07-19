using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using _2_OpenClosed.Common.Service.Configuration;
using _2_OpenClosed.DataAccess.DTO;

namespace _2_OpenClosed.DataAccess.Queries
{
    public class RetrieveScheduledMDAList : IQuery<IList<ScheduledMDAListDTO>>
    {
        private readonly IConfigSettingsService _configSettingsService;
        private readonly string _fromExternalWarehouseId;
        private readonly string _toExternalWarehouseId;
        private readonly string _previousMdaForBatching;
        private AutoIwtSettings _autoIwtSettings;

        public RetrieveScheduledMDAList(string fromExternalWarehouseId, string toExternalWarehouseId, string previousMdaForBatching) :
            this(fromExternalWarehouseId, toExternalWarehouseId, previousMdaForBatching, new ConfigSettingsService().Load<AutoIwtSettings>())
        {

        }

        public RetrieveScheduledMDAList(string fromExternalWarehouseId, string toExternalWarehouseId, string previousMdaForBatching, AutoIwtSettings settings)
        {
            _fromExternalWarehouseId = fromExternalWarehouseId;
            _toExternalWarehouseId = toExternalWarehouseId;
            _previousMdaForBatching = previousMdaForBatching;
            _autoIwtSettings = settings;
        }

        public IList<ScheduledMDAListDTO> Run()
        {
            var lookup = new Dictionary<string, ScheduledMDAListDTO>();


            using (var conn = DbConnection.GetReliableOpenConnection(DbInstance.InterWarehouseTransfer))
            {
                conn.Query<ScheduledMDAListDTO, ScheduledMDAItemDTO, ScheduledMDAListDTO>(
                    "dbo.uspGetScheduledMDAList",
                    (mda, processedItem) =>
                    {
                        ScheduledMDAListDTO scheduledMdaListDto;
                        if (!lookup.TryGetValue(mda.MDANumber, out scheduledMdaListDto))
                        {
                            lookup.Add(mda.MDANumber, scheduledMdaListDto = mda);
                        }

                        if (scheduledMdaListDto.Items == null)
                        {
                            scheduledMdaListDto.Items = new List<ScheduledMDAItemDTO>();
                        }

                        if (processedItem != null)
                        {
                            scheduledMdaListDto.Items.Add(processedItem);
                        }

                        return scheduledMdaListDto;
                    },
                    new
                    {
                        FromExternalWarehouseId = _fromExternalWarehouseId,
                        ToExternalWarehouseId = _toExternalWarehouseId,
                        MDAFrom = _previousMdaForBatching,
                        rowCount = _autoIwtSettings.ScheduledMdasToRetrieve
                    },
                    splitOn: "SkuExternalId",
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: DbSettings.CommandTimeout);
            }

            return lookup.Values.ToList();
        }
    }
}
