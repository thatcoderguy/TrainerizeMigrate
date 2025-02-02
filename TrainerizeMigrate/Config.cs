using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TrainerizeMigrate
{
    public class ConfigDetails
    {
        public string username { get; set; }
        public string password { get; set; }
        public string groupname { get; set; }
        public string ptusername { get; set; }
        public string ptpassword { get; set; }
        public string ptgroupname { get; set; }
        public string loginurl { get; set; }
        public string bodystatsdataurl { get; set; }
    }


    public class Config
    {
        private ConfigDetails _configDetails { get; set; }

        public Config() {
            using (StreamReader r = new StreamReader("config.json"))
            {
                string json = r.ReadToEnd();
                _configDetails = JsonSerializer.Deserialize<ConfigDetails>(json);
            }

        }

        public string Username()
        {
            return _configDetails.username;
        }

        public string Password()
        {
            return _configDetails.password;
        }

        public string PTPassword()
        {
            return _configDetails.ptpassword;
        }

        public string PTUsername()
        {
            return _configDetails.ptusername;
        }

        public string GroupName()
        {
            return _configDetails.groupname;
        }

        public string PTGroupName()
        {
            return _configDetails.ptgroupname;
        }

        public string LoginUrl()
        {
            return _configDetails.loginurl;
        }

        public string BodyStatsUrl()
        {
            return _configDetails.bodystatsdataurl;
        }
    }
}
