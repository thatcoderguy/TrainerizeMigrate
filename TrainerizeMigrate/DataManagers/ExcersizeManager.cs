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
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

            ExcersizeListResponse customExcersizes = FilterCustomExcersizes(exercizeList);

            AnsiConsole.Markup("[green]Storing custom excersizes into database\n[/]");
            StoreCustomExcersizes(customExcersizes);
            AnsiConsole.Markup("[green]Data storage successful\n[/]");

            return true;
        }

        public bool ImportExtractedData()
        {
            AnsiConsole.Markup("[green]Authenticating with Trainerize\n[/]");
            AuthenticationSession authDetails = Authenticate.AuthenticateWithNewTrainerizeAsAdmin(_config);
            AnsiConsole.Markup("[green]Authenticatiion successful\n[/]");

            
            AnsiConsole.Markup("[green]Retreving excersize data from database\n[/]");
            List<Data.CustomExcersize> excersizes = ReadCustomExcersizesNotImported();
            AnsiConsole.Markup("[green]Data retrival successful\n[/]");

            if (excersizes.Count > 0)
            {
                PushCustomExcersizes(authDetails, excersizes);
                AnsiConsole.Markup("[green]Import sucessful\n[/]");

                return true;
            }

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

        private ExcersizeListResponse FilterCustomExcersizes(ExcersizeListResponse excersizeList)
        {
            excersizeList.exercises = excersizeList.exercises.Where(x => x.type == "custom").ToList();
            return excersizeList;
        }

        private bool StoreCustomExcersizes(ExcersizeListResponse excersizeList)
        {
            foreach (Exercise excersize in excersizeList.exercises)
            {
                if (!_context.Excerisize.Any(x => x.id == excersize.id))
                {
                    CustomExcersize newExcersize = new CustomExcersize()
                    {
                        name = excersize.name,
                        id = excersize.id,
                        alternateName = excersize.alternateName,
                        description = excersize.description,
                        recordType = excersize.recordType,
                        videoType = excersize.videoType,
                        videoUrl = excersize.videoUrl,
                        new_id = null,
                        tags = null
                    };

                    List<Data.Tag> newTags = new List<Data.Tag>();
                    
                    foreach(API.Tag tag in excersize.tags)
                    {
                        newTags.Add(new Data.Tag()
                        {
                            name = tag.name,
                            type = tag.type,
                            Id = new Guid()
                        });
                    }

                    newExcersize.tags = newTags;

                    _context.Tag.AddRange(newTags);
                    _context.Excerisize.Add(newExcersize);

                    _context.SaveChanges();

                    AnsiConsole.Markup("[green]Added excersize: " + excersize.name + "\n[/]");

                } else
                    AnsiConsole.Markup("[red]Excersize: " + excersize.name +  " already exists!\n[/]");

            }

            return true;
        }

        private List<CustomExcersize> ReadCustomExcersizesNotImported()
        {
            List<CustomExcersize> excersizeList = _context.Excerisize.Include(x => x.tags).Where(x => x.new_id == null).ToList();
            return excersizeList;
        }

        private void UpdateExcersize(int excersizeId, int newExcersizeId)
        {
            CustomExcersize excersize = _context.Excerisize.FirstOrDefault(x => x.id == excersizeId);
            excersize.new_id = newExcersizeId;
            _context.Excerisize.Update(excersize);
            _context.SaveChanges();
        }

        private bool PushCustomExcersizes(AuthenticationSession authDetails, List<CustomExcersize> excersizeList)
        {

            AnsiConsole.Progress()
                .Columns(GetProgressColumns())
                .Start(async ctx =>
                {
                    var task = ctx.AddTask($"[green]Importing custom excersize data...[/]", autoStart: false);
                    task.MaxValue = excersizeList.Count;
                    task.StartTask();

                    foreach (CustomExcersize excersize in excersizeList)
                    {
                        int newExcersizeId = AddCustomExcersize(authDetails, excersize);
                        UpdateExcersize(excersize.id, newExcersizeId);

                        task.Increment(1);
                    }
                    task.StopTask();
                });


            return false;
        }

        private List<CustomExcersizeRequestTag> ConvertDBTagsToRequestTags(List<Data.Tag> tags)
        {
            List<CustomExcersizeRequestTag> tagsList = new List<CustomExcersizeRequestTag>();
            foreach (Data.Tag tag in tags)
            {
                tagsList.Add(new CustomExcersizeRequestTag()
                {
                    type = tag.type,
                    name = tag.name
                });
            }
            return tagsList;
        }

        private int AddCustomExcersize(AuthenticationSession authDetails, CustomExcersize excersize)
        {

            if (excersize.videoType == "youtube")
                if (!CheckYouTubeVideoExists(excersize.videoUrl)) ;
                { 
                    excersize.videoType = "none";
                    excersize.videoUrl = string.Empty;
                }

    AddCustomExcersizeRequest jsonBody = new AddCustomExcersizeRequest()
            {
                alternateName = excersize.alternateName,
                description = excersize.description,
                lastPerformed = null,
                name = excersize.name,
                recordType = excersize.recordType,
                superSetID = 0,
                tag = "none",
                type = "custom",
                videoType = excersize.videoType,
                videoUrl = excersize.videoUrl,
                media = new ExcersizeMedia()
                {
                    status = null,
                    token = excersize.videoUrl,
                    type = excersize.videoType
                },
                tags = ConvertDBTagsToRequestTags(excersize.tags)
            };

            var authenticator = new JwtAuthenticator(authDetails.token);
            var options = new RestClientOptions()
            {
                Authenticator = authenticator,
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            RestClient client = new RestClient(options);
            var request = new RestRequest();
            request.Resource = _config.AddCustomExcersizeUrl();
            request.Method = Method.Post;
            request.AddJsonBody(jsonBody, ContentType.Json);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            RestResponse queryResult =  queryResult = client.Execute(request);

            AddCustomExcersizeResponse response = JsonSerializer.Deserialize<AddCustomExcersizeResponse>(queryResult.Content);

            return response.id;
        }

        private bool CheckYouTubeVideoExists(string videoToken)
        {
            HttpClient _client = new HttpClient();
            HttpResponseMessage response = _client.GetAsync("https://www.youtube.com/embed/" + videoToken + "?autoplay=0&rel=0&modestbranding=1&wmode=transparent&showInfo=0").Result;

            string contentText =  response.Content.ReadAsStringAsync().Result;

            if(contentText.Contains("This video is unavailable"))
                return false;

            return true;
        }

        static ProgressColumn[] GetProgressColumns()
        {
            List<ProgressColumn> progressColumns;

            progressColumns = new List<ProgressColumn>()
            {
                new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new DownloadedColumn(), new RemainingTimeColumn()
            };

            return progressColumns.ToArray();
        }
    }
}
