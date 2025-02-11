using RestSharp;
using System.Text.Json;
using TrainerizeMigrate.API;

namespace TrainerizeMigrate
{
    public static class Authenticate
    {
        public static AuthenticationSession AuthenticateWithOriginalTrainerize(Config config)
        {
            TrainerizeLoginRequest jsonBody = new TrainerizeLoginRequest()
            {
                email = config.Orignal_Username(),
                password = config.Original_Password(),
                groupUrl = config.Original_GroupName(),
                rememberMe = true
            };

            RestClient client = new RestClient();
            var request = new RestRequest();
            request.Resource = config.LoginUrl();
            request.Method = Method.Post;
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            RestResponse? queryResult = client.Execute(request);

            TrainerizeLoginResponse? response = JsonSerializer.Deserialize<TrainerizeLoginResponse>(queryResult.Content);

            if (response.code == 1)
                return new AuthenticationSession()
                {
                    token = response.token.access_token,
                    userId = response.userid
                };

            throw new Exception("Invalid login details");
        }

        public static AuthenticationSession AuthenticateWithNewTrainerize(Config config)
        {
            TrainerizeLoginRequest jsonBody = new TrainerizeLoginRequest()
            {
                email = config.New_Username(),
                password = config.New_Password(),
                groupUrl = config.New_GroupName(),
                rememberMe = true
            };

            RestClient client = new RestClient();
            var request = new RestRequest();
            request.Resource = config.LoginUrl();
            request.Method = Method.Post;
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            var queryResult = client.Execute(request);

            TrainerizeLoginResponse? response = JsonSerializer.Deserialize<TrainerizeLoginResponse>(queryResult.Content);

            if (response.code == 1)
                return new AuthenticationSession()
                {
                    token = response.token.access_token,
                    userId = response.userid
                };

            throw new Exception("Invalid login details");
        }

        public static AuthenticationSession AuthenticateWithNewTrainerizeAsAdmin(Config config)
        {
            TrainerizeLoginRequest jsonBody = new TrainerizeLoginRequest()
            {
                email = config.Admin_Username(),
                password = config.Admin_Password(),
                groupUrl = config.Admin_Group(),
                rememberMe = true
            };

            RestClient client = new RestClient();
            var request = new RestRequest();
            request.Resource = config.LoginUrl();
            request.Method = Method.Post;
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            var queryResult = client.Execute(request);

            TrainerizeLoginResponse? response = JsonSerializer.Deserialize<TrainerizeLoginResponse>(queryResult.Content);

            if (response.code == 1)
                return new AuthenticationSession()
                {
                    token = response.token.access_token,
                    userId = response.userid
                };

            throw new Exception("Invalid login details");
        }
    }
}
