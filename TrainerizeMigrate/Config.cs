using System.Text.Json;

namespace TrainerizeMigrate
{
    public class AuthenticationDetails
    {
        public string? original_username { get; set; }
        public string? original_password { get; set; }
        public string? original_groupname { get; set; }
        public string? new_username { get; set; }
        public string? new_password { get; set; }
        public string? new_groupname { get; set; }
        public string? new_trainer_username { get; set; }
        public string? new_trainer_password { get; set; }
        public string? new_trainer_groupname { get; set; }
    }

    public class TrainerizeUrls
    {
        public string? loginurl { get; set; }
        public string? getbodystatsdataurl { get; set; }
        public string? addbodystatsurl { get; set; }
        public string? addbodystatsdataurl { get; set; }
        public string? performedexcersizesurl { get; set; }
        public string? addcustomexcersizeurl {  get; set; }
        public string? gettrainingprogramsurl { get; set; }
        public string? gettrainingprogramphasesurl { get; set; }
        public string? gettrainingphaseworkoutsurl { get; set; }
        public string? getworkoutplandetailsurl { get; set; }
        public string? addprogramphaseurl { get; set; }
        public string? getphaseworkoutplansurl { get; set; }
        public string? addworkoutplantophaseurl { get; set; }
        public string? deletephaseurl {  get; set; }
        public string? deletecustomexcersizeurl { get; set; }
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

        public string Admin_Username()
        {
            return _authDetails.new_trainer_username;
        }

        public string Admin_Password()
        {
            return _authDetails.new_trainer_password;
        }

        public string Admin_Group()
        {
            return _authDetails.new_trainer_groupname;
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

        public string AddCustomExcersizeUrl()
        {
            return _trainerizeurls.addcustomexcersizeurl;
        }

        public string GetTrainingProgramsUrl()
        {
            return _trainerizeurls.gettrainingprogramsurl;
        }

        public string GetTrainingProgramPhasesUrl()
        {
            return _trainerizeurls.gettrainingprogramphasesurl;
        }

        public string GetPhaseWorkoutsUrl()
        {
            return _trainerizeurls.gettrainingphaseworkoutsurl;
        }

        public string GetWorkoutPlanDetailsUrl()
        {
            return _trainerizeurls.getworkoutplandetailsurl;
        }

        public string AddTrainingPhaseUrl()
        {
            return _trainerizeurls.addprogramphaseurl;
        }

        public string GetPhaseWorkoutPlansUrl()
        {
            return _trainerizeurls.getphaseworkoutplansurl;
        }

        public string AddWorkoutPlanToPhaseUrl()
        {
            return _trainerizeurls.addworkoutplantophaseurl;
        }

        public string DeleteCustomExcersizeUrl()
        {
            return _trainerizeurls.deletecustomexcersizeurl;
        }

        public string DeletePhaseUrl()
        {
            return _trainerizeurls.deletephaseurl;
        }
    }
}
