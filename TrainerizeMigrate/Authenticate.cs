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
        public static AuthenticationDetails AuthenticateWithTrainerize(Config config)
        {
            TrainerizeLoginRequest jsonBody = new TrainerizeLoginRequest()
            {
                email = config.Username(),
                password = config.Password(),
                groupUrl = config.GroupName(),
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
                return new AuthenticationDetails()
                {
                    token = response.token.access_token,
                    userId = response.userid
                };

            throw new Exception("Invalid login details");
        }
    }
}
