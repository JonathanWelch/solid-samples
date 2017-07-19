using System;
using System.Collections.Generic;

namespace _2_OpenClosed.Common.Service.Configuration
{
    public interface IConfigSettingServiceListener
    {
        void LoadSettingsException(KeyValuePair<string, string> setting, Exception exception);
    }

    public class ConfigSettingServiceListener : IConfigSettingServiceListener
    {
        public void LoadSettingsException(KeyValuePair<string, string> setting, Exception exception)
        {
            Logger.Error(string.Format("An exception has been raised whilst loading setting {0} with value '{1}'", setting.Key, setting.Value), exception);
        }
    }
}
