using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;

namespace _2_OpenClosed.DataAccess.Queries
{
    public class RetrieveConfigSetting : IQuery<KeyValuePair<string, string>>
    {
        public RetrieveConfigSetting(string settingKey)
        {
            SettingKey = settingKey;
        }

        public string SettingKey { get; set; }

        public KeyValuePair<string, string> Run()
        {
            using (var conn = DbConnection.GetReliableOpenConnection(DbInstance.InterWarehouseTransfer))
            {
                return conn.Query(
                    "uspGetConfigSettingBySettingKey",
                        new
                        {
                            SettingKey
                        },
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: DbSettings.CommandTimeout).Select(x => new KeyValuePair<string, string>(x.SettingKey, x.SettingValue)).FirstOrDefault();
            }
        }
    }
}
