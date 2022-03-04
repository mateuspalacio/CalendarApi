using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Domain.Credentials
{
    public class ConfigSettings : IConfigSettings
    {
        private string _key1;
        public ConfigSettings()
        {
            Init();
        }
        public void Init()
        {
            var secretValues = JObject.Parse(Credentials.GetSecret());
            if (secretValues != null)
            {
                _key1 = secretValues["client_secret_googlecalendarapi"].ToString();
            }
        }
        public string Key1
        {
            get
            {
                return _key1;
            }
            set
            {
                _key1 = value;
            }
        }
    }
    public interface IConfigSettings
    {
        string Key1
        {
            get;
            set;
        }
    }

}
