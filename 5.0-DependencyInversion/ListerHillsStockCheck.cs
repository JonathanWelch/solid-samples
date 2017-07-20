using System;
using Quartz;
using _5._0_DependencyInversion.Common.DataAccess;
using _5._0_DependencyInversion.Common.DataAccess.Queries;
using _5._0_DependencyInversion.Common.Service;
using _5._0_DependencyInversion.Domain;

namespace _5._0_DependencyInversion
{
    public class ListerHillsStockCheck : IwtJob
    {
        private const int IntervalInMinutes = 30;

        private readonly IListerHillsStockListener listerHillsStockListener;

        private readonly ICommandAndQueryExecutor executor;

        public ListerHillsStockCheck()
            : this(new CommandAndQueryExecutor(), new ListerHillsStockListener())
        {
        }

        public ListerHillsStockCheck(ICommandAndQueryExecutor executor, IListerHillsStockListener listerHillsStockListener)
        {
            this.executor = executor;
            this.listerHillsStockListener = listerHillsStockListener;
        }

        protected override void ExecuteJob(IJobExecutionContext context)
        {
            var lastUpdated = this.executor.Query(new RetrieveListerHillsLastUpdate());

            var now = DateTime.UtcNow;
            var timeElapsed = now.Subtract(lastUpdated);
            var minutesElapsed = timeElapsed.TotalMinutes;
            if (minutesElapsed > IntervalInMinutes)
            {
                this.listerHillsStockListener.StockNotUpdatedInElapsedTime(lastUpdated, IntervalInMinutes);
            }
        }
    }
}
