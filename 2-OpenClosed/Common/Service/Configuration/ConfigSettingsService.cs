using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using _2_OpenClosed.DataAccess;
using _2_OpenClosed.DataAccess.Command;
using _2_OpenClosed.DataAccess.Queries;

namespace _2_OpenClosed.Common.Service.Configuration
{
    public interface IConfigSettingsService
    {
        T Load<T>() where T : ISetting;
    }

    public class ConfigSettingsService : IConfigSettingsService
    {
        private readonly ICommandAndQueryExecutor _executor;
        private readonly IConfigSettingServiceListener _configSettingServiceListener;

        public ConfigSettingsService() : this(new CommandAndQueryExecutor(), new ConfigSettingServiceListener())
        {
        }

        public ConfigSettingsService(ICommandAndQueryExecutor executor, IConfigSettingServiceListener configSettingServiceListener)
        {
            _executor = executor;
            _configSettingServiceListener = configSettingServiceListener;
        }

        public T Load<T>() where T : ISetting
        {
            return FromCache(LoadSettingsFromDb<T>);
        }

        public void Save<T>(T settings) where T : ISetting
        {
            SaveSettingsIntoDb<T>(settings);
        }

        private T FromCache<T>(Func<T> callback)
        {
            var cacheKey = typeof(T).FullName;

            if (MemoryCache.Default.Contains(cacheKey))
            {
                return (T)MemoryCache.Default[cacheKey];
            }

            var cacheItemPolicy = new CacheItemPolicy { AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(15)) };

            var value = callback();
            MemoryCache.Default.Set(cacheKey, value, cacheItemPolicy);

            return value;
        }

        private T LoadSettingsFromDb<T>() where T : ISetting
        {
            var settings = Activator.CreateInstance<T>();

            foreach (var prop in typeof(T).GetProperties())
            {
                // get properties we can read and write to
                if (!prop.CanRead || !prop.CanWrite)
                {
                    continue;
                }

                var key = (typeof(T).Name + "." + prop.Name).ToLower();
                var setting = _executor.Query(new RetrieveConfigSetting(key));
                if (setting.Equals(new KeyValuePair<string, string>()))
                {
                    continue;
                }

                object value;

                try
                {
                    value = ConfigTypeConverter.GetCustomTypeConverter(prop.PropertyType).ConvertFromInvariantString(setting.Value);
                }
                catch (Exception e)
                {
                    _configSettingServiceListener.LoadSettingsException(setting, e);
                    throw;
                }

                // set property
                prop.SetValue(settings, value, null);
            }

            return settings;
        }

        private void SaveSettingsIntoDb<T>(T settings) where T : ISetting
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                // get properties we can read and write to
                if (!prop.CanRead || !prop.CanWrite)
                {
                    continue;
                }

                var key = (typeof(T).Name + "." + prop.Name).ToLower();
                var value = prop.GetValue(settings);

                var databaseValue = string.Empty;
                if (value != null)
                {
                    databaseValue = value.ToString();
                }

                _executor.Execute(new SetConfigSetting(key, databaseValue));
            }
        }
    }
}
