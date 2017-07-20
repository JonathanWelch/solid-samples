using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace _5._0_DependencyInversion.Common.DataAccess
{
    public enum DbInstance
    {
        BackOffice, Checkout, InterWarehouseTransfer, MerretReport, WarehouseStock
    }

    public class DbConnection
    {
        private static readonly Dictionary<DbInstance, string> ConnectionStringNames = new Dictionary<DbInstance, string>
        {
            { DbInstance.BackOffice, "Sql.BackOffice.ConnectionString" },
            { DbInstance.Checkout, "Sql.Checkout.ConnectionString" },
            { DbInstance.MerretReport, "Sql.MerretReport.ConnectionString" },
            { DbInstance.InterWarehouseTransfer, "Sql.IwtInTransit.ConnectionString" },
            { DbInstance.WarehouseStock, "Sql.WarehouseStock.Connection.String" }
        };

        static DbConnection()
        {
            const string defaultRetryStrategyName = "fixed";
            const int retryCount = 10;
            var retryInterval = TimeSpan.FromSeconds(3);

            var strategy = new FixedInterval(defaultRetryStrategyName, retryCount, retryInterval);
            var strategies = new List<RetryStrategy> { strategy };
            var manager = new RetryManager(strategies, defaultRetryStrategyName);

            RetryManager.SetDefault(manager);
        }

        public static IDbConnection GetReliableOpenConnection(DbInstance instance)
        {
            var connectionName = ConnectionStringNames[instance];
            var connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;

            var conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }

        public static IDbConnection GetOpenConnection(DbInstance instance)
        {
            var connectionName = ConnectionStringNames[instance];
            var connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;

            var conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }
    }
}