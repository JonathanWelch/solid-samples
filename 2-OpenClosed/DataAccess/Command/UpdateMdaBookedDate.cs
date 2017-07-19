using System;
using System.Data;
using Dapper;

namespace _2_OpenClosed.DataAccess.Command
{
    public class UpdateMdaBookedDate : ICommand
    {
        private readonly string _mdaNumber;
        private readonly DateTime _bookedDate;

        public UpdateMdaBookedDate(string mdaNumber, DateTime bookedDate)
        {
            _mdaNumber = mdaNumber;
            _bookedDate = bookedDate;
        }

        public void Execute()
        {
            using (var conn = DbConnection.GetReliableOpenConnection(DbInstance.InterWarehouseTransfer))
            {
                conn.Execute(
                    "dbo.uspUpdateMdaBookedDate",
                    new
                    {
                        MDANumber = _mdaNumber,
                        BookedDate = _bookedDate
                    },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: DbSettings.CommandTimeout);
            }
        }
    }
}
