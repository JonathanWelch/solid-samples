using System;
using System.Collections.Generic;
using System.Linq;

namespace _2_OpenClosed.Common.Service.Configuration
{
    public interface IUnitCapParser
    {
        void Parse(IList<UnitCap> unitCaps);
    }

    public class UnitCapParser : IUnitCapParser
    {
        public void Parse(IList<UnitCap> unitCaps)
        {
            var exceptions = new List<Exception>();

            foreach (var unitCap in unitCaps)
            {
                if (unitCap.MinLevel < 1)
                {
                    exceptions.Add(new ArgumentOutOfRangeException(string.Format("The MinimumLevel for unit cap is set to {0} which is an invalid value.", unitCap.MinLevel)));
                }

                if (unitCap.MaxLevel < 1)
                {
                    exceptions.Add(new ArgumentOutOfRangeException(string.Format("The MaximumLevel for unit cap is set to {0} which is an invalid value.", unitCap.MaxLevel)));
                }

                if (unitCap.MinLevel > unitCap.MaxLevel)
                {
                    exceptions.Add(new ArgumentOutOfRangeException(string.Format("The MinimumLevel {0} for unit cap is higher than MaximumLevel {1} which is an invalid value.", unitCap.MaxLevel, unitCap.MaxLevel)));
                }

                if (unitCap.Percentage < 1 || unitCap.Percentage > 100)
                {
                    exceptions.Add(new ArgumentOutOfRangeException(string.Format("The Percentage for unit cap is set to {0} which is an invalid value. It must be between 1 and 100.", unitCap.Percentage)));
                }

                if (exceptions.Any())
                {
                    throw new AggregateException(exceptions);
                }
            }
        }
    }
}