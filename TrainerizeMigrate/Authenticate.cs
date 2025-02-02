using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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
            request.AddJsonBody(jsonBody, ContentType.Json);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var queryResult = client.Execute(request);

            TrainerizeLoginResponse response = JsonSerializer.Deserialize<TrainerizeLoginResponse>(queryResult.Content);

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
            request.AddJsonBody(jsonBody, ContentType.Json);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var queryResult = client.Execute(request);

            TrainerizeLoginResponse response = JsonSerializer.Deserialize<TrainerizeLoginResponse>(queryResult.Content);

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
