using System;
using System.Collections.Generic;

namespace _2_OpenClosed.Common.Service.Configuration
{
    public interface IDepartmentParser
    {
        void Parse(IList<Department> departments);
    }

    public class DepartmentParser : IDepartmentParser
    {
        public void Parse(IList<Department> departments)
        {
            foreach (var department in departments)
            {
                if (department.OptimalStockLevelRatio > 100 || department.OptimalStockLevelRatio < 0)
                {
                    throw new ArgumentOutOfRangeException(string.Format("The OptimalStockLevelRatio for department {0} is set to {1} which is an invalid value.  It must be between 0 and 100.", department.Name, department.OptimalStockLevelRatio));
                }

                if (department.MaximumStockLevelRatio > 100 || department.MaximumStockLevelRatio < 0)
                {
                    throw new ArgumentOutOfRangeException(string.Format("The MaximumStockLevelRatio for department {0} is set to {1} which is an invalid value.  It must be between 0 and 100.", department.Name, department.MaximumStockLevelRatio));
                }

                if (department.OptimalStockLevelRatio >= department.MaximumStockLevelRatio)
                {
                    throw new ArgumentOutOfRangeException(string.Format("The OptimalStockLevelRatio for department {0} is set to {1} which equals or exceeds the MaximumStockLevelRatio {2}", department.Name, department.OptimalStockLevelRatio, department.MaximumStockLevelRatio));
                }
            }
        }
    }
}