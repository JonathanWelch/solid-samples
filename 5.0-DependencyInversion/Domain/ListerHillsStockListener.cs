using System;
using _5._0_DependencyInversion.Common;

namespace _5._0_DependencyInversion.Domain
{
    public interface IListerHillsStockListener
    {
        void StockNotUpdatedInElapsedTime(DateTime lastUpdated, int elapsedMinutes);
    }

    public class ListerHillsStockListener : IListerHillsStockListener
    {
        public void StockNotUpdatedInElapsedTime(DateTime lastUpdated, int elapsedMinutes)
        {
            var errorMessage = string.Format("ListerHills stock data hasn't been updated in the lapsed time ({1} minutes). Last update was {0}.", lastUpdated.ToString("dd/MM/yyyy H:mm:ss zzz"), elapsedMinutes);
            var exception = new InvalidOperationException(errorMessage);
            Logger.Error(errorMessage, exception);
        }
    }
}
