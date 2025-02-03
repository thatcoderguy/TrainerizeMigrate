using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TrainerizeMigrate
{
    public class AuthenticationDetails
    {
        public string original_username { get; set; }
        public string original_password { get; set; }
        public string original_groupname { get; set; }
        public string new_username { get; set; }
        public string new_password { get; set; }
        public string new_groupname { get; set; }
    }

    public class TrainerizeUrls
    {
        public string loginurl { get; set; }
        public string getbodystatsdataurl { get; set; }
        public string addbodystatsurl { get; set; }
        public string addbodystatsdataurl { get; set; }
        public string performedexcersizesurl { get; set; }

    }

    public class Config
    {
        private AuthenticationDetails _authDetails { get; set; }
        private TrainerizeUrls _trainerizeurls { get; set; }

        public Config() {
            using (StreamReader r = new StreamReader("config.json"))
            {
                string json = r.ReadToEnd();
                _authDetails = JsonSerializer.Deserialize<AuthenticationDetails>(json);
            }

            using (StreamReader r = new StreamReader("trainerize_urls.json"))
            {
                string json = r.ReadToEnd();
                _trainerizeurls = JsonSerializer.Deserialize<TrainerizeUrls>(json);
            }

        }

        public string Orignal_Username()
        {
            return _authDetails.original_username;
        }

        public string Original_Password()
        {
            return _authDetails.original_password;
        }

        public string New_Password()
        {
            return _authDetails.new_password;
        }

        public string New_Username()
        {
            return _authDetails.new_username;
        }

        public string Original_GroupName()
        {
            return _authDetails.original_groupname;
        }

        public string New_GroupName()
        {
            return _authDetails.new_groupname;
        }

        public string LoginUrl()
        {
            return _trainerizeurls.loginurl;
        }

        public string GetBodyStatsDataUrl()
        {
            return _trainerizeurls.getbodystatsdataurl;
        }

        public string AddBodyStatUrl()
        {
            return _trainerizeurls.addbodystatsurl;
        }

        public string AddBodyStatDataUrl()
        {
            return _trainerizeurls.addbodystatsdataurl;
        }

        public string GetPerformedExcersizesUrl()
        {
            return _trainerizeurls.performedexcersizesurl;
        }
    }
}
