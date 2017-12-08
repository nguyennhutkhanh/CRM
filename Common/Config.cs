using System;
using System.Configuration;
using System.Web.Configuration;

namespace WcfService.Common
{
    public static class Config
    {
        static public string GetConfigValueAsString(string configKey)
        {
            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                if (key.Equals(configKey, StringComparison.CurrentCultureIgnoreCase))
                {
                    return ConfigurationManager.AppSettings[key];
                }
            }

            return null;
        }

        static public string GetConfigValueAsString(string configKey, bool useWebConfigurationManager)
        {
            if (useWebConfigurationManager)
            {
                foreach (string key in WebConfigurationManager.AppSettings.AllKeys)
                {
                    if (key.Equals(configKey, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return WebConfigurationManager.AppSettings[key];
                    }
                }
                return null;
            }
            else
                return GetConfigValueAsString(configKey);
        }

        static public bool GetConfigValueAsBool(string configKey, bool defaultValue)
        {
            string value = GetConfigValueAsString(configKey);

            if (!String.IsNullOrEmpty(value))
            {
                return Convert.ToBoolean(value.ToLower());
            }

            return defaultValue;
        }

        static public bool GetConfigValueAsBool(string configKey, bool defaultValue, bool useWebConfigurationManager)
        {
            if (useWebConfigurationManager)
            {
                string value = GetConfigValueAsString(configKey, useWebConfigurationManager);

                if (!String.IsNullOrEmpty(value))
                {
                    return Convert.ToBoolean(value.ToLower());
                }

                return defaultValue;
            }
            else
                return GetConfigValueAsBool(configKey, defaultValue);
        }

        #region Read From app.config

        static public string Domain
        {
            get
            {
                return (GetConfigValueAsString("domain"));
            }
        }

        static public string NodeJSPort
        {
            get
            {
                return (GetConfigValueAsString("NodeJSPort"));
            }
        }

        static public string ImageFolder
        {
            get
            {
                return (GetConfigValueAsString("FolderPath"));
            }
        }

        static public string LogPath
        {
            get
            {
                return (GetConfigValueAsString("logFilePath"));
            }
        }

        static public string ActivePath
        {
            get
            {
                return (GetConfigValueAsString("ActivePath"));
            }
        }

        static public string MailServer
        {
            get
            {
                return (GetConfigValueAsString("mailserver"));
            }
        }

        static public string MailMaster
        {
            get
            {
                return (GetConfigValueAsString("emailmaster"));
            }
        }

        static public string MailPassword
        {
            get
            {
                return (GetConfigValueAsString("emailpwd"));
            }
        }

        static public string ConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["PRV-ConnectString"];
            }
        }

        static public string WS_ConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["PRV-WS-cnn"];
            }
        }

        static public string lblSucess
        {
            get
            {
                return (GetConfigValueAsString("Success"));
            }
        }

        static public string lblFail
        {
            get
            {
                return (GetConfigValueAsString("Fail"));
            }
        }

        static public string lblAtleast
        {
            get
            {
                return (GetConfigValueAsString("Atleast"));
            }
        }

        static public string lblNothing
        {
            get
            {
                return (GetConfigValueAsString("Nothing"));
            }
        }

        static public string lblNoPermit
        {
            get
            {
                return (GetConfigValueAsString("NoPermit"));
            }
        }

        #endregion

        #region Constant

        static public String domain_ = "http://api.lixido.net";
        static public String[] object_WS = { "outletservice", "clientservice", "productservice", "programservice", "orderservice", "locationservice", "utilservice" };
        static public String prefix_SP = "sp_WS_";
        #endregion
    }
}