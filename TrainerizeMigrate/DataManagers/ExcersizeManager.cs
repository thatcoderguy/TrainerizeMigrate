using RestSharp.Authenticators;
using RestSharp;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainerizeMigrate.API;
using TrainerizeMigrate.Data;
using TrainerizeMigrate.Migrations;
using System.Text.Json;

namespace TrainerizeMigrate.DataManagers
{
    public class ExcersizeManager
    {
        private Config _config { get; set; }
        private ApplicationDbContext _context { get; set; }

        public ExcersizeManager(Config config, ApplicationDbContext context)
        {
            _config = config;
            _context = context;
        }

        public bool ExtractAndStoreData()
        {
            AnsiConsole.Markup("[green]Authenticating with Trainerize\n[/]");
            AuthenticationSession authDetails = Authenticate.AuthenticateWithOriginalTrainerize(_config);
            AnsiConsole.Markup("[green]Authenticatiion successful\n[/]");

            AnsiConsole.Markup("[green]Pulling list of performed excersizes\n[/]");
            ExcersizeListResponse exercizeList = PullExcersizeData(authDetails);
            AnsiConsole.Markup("[green]Data retreieved successfully\n[/]");

            AnsiConsole.Markup("[green]Storing custom excersizes into database\n[/]");
            //StoreCustomExcersizes(exercizeList);
            AnsiConsole.Markup("[green]Data storage successful\n[/]");

            return true;
        }

        public bool ImportExtractedData()
        {
            AnsiConsole.Markup("[green]Authenticating with Trainerize\n[/]");
            AuthenticationSession authDetails = Authenticate.AuthenticateWithNewTrainerize(_config);
            AnsiConsole.Markup("[green]Authenticatiion successful\n[/]");

            /*
            AnsiConsole.Markup("[green]Retreving body weight data from database\n[/]");
            BodyWeight bodyWeightData = ReadBodyWeightData();
            AnsiConsole.Markup("[green]Data retrival successful\n[/]");

            if (bodyWeightData != null)
            {
                AnsiConsole.Markup("[green]Importing body weight data into trainerize\n[/]");
                PushBodyWeightData(authDetails, bodyWeightData);
                AnsiConsole.Markup("[green]Import sucessful\n[/]");

                return true;
            }*/

            return false;
        }

        private ExcersizeListResponse PullExcersizeData(AuthenticationSession authDetails)
        {
            ExcersizeListRequest jsonBody = new ExcersizeListRequest()
            {
                userID = authDetails.userId,
                type = "standard"
            };

            var authenticator = new JwtAuthenticator(authDetails.token);
            var options = new RestClientOptions()
            {
                Authenticator = authenticator,
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            RestClient client = new RestClient(options);
            var request = new RestRequest();
            request.Resource = _config.GetPerformedExcersizesUrl();
            request.Method = Method.Post;
            request.AddJsonBody(jsonBody, ContentType.Json);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var queryResult = client.Execute(request);

            ExcersizeListResponse response = JsonSerializer.Deserialize<ExcersizeListResponse>(queryResult.Content);

            return response;
        }
    }
}
