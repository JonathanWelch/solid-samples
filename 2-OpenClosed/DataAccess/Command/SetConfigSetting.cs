using System.Data;
using Dapper;

namespace _2_OpenClosed.DataAccess.Command
{
    public class SetConfigSetting : ICommand
    {
        private readonly string settingKey;
        private readonly string settingValue;

        public SetConfigSetting(string settingKey, string settingValue)
        {
            this.settingKey = settingKey;
            this.settingValue = settingValue;
        }

        public void Execute()
        {
            var lower = settingKey.ToLower();
            using (var conn = DbConnection.GetReliableOpenConnection(DbInstance.InterWarehouseTransfer))
            {
                conn.Execute(@"dbo.uspSetConfigSetting", new { Key = lower, value = settingValue }, commandType: CommandType.StoredProcedure, commandTimeout: DbSettings.CommandTimeout);
            }
        }
    }
}
