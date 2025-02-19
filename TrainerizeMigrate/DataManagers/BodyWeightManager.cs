using RestSharp.Authenticators;
using RestSharp;
using System.Text.Json;
using TrainerizeMigrate.API;
using TrainerizeMigrate.Migrations;
using TrainerizeMigrate.Data;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace TrainerizeMigrate.DataManagers
{
    public class BodyWeightManager
    {
        private Config _config { get; set; }
        private ApplicationDbContext _context { get; set; }

        public BodyWeightManager(Config config, ApplicationDbContext context)
        {
            _config = config;
            _context = context;
        }

        public bool ExtractAndStoreData()
        {
            AnsiConsole.Markup("[green]Authenticating with Trainerize\n[/]");
            AuthenticationSession authDetails = Authenticate.AuthenticateWithOriginalTrainerize(_config);
            AnsiConsole.Markup("[green]Authenticatiion successful\n[/]");

            AnsiConsole.Markup("[green]Pulling body weight data from trainerize\n[/]");
            BodyWeightResponse bodyWeightData = PullBodyWeightData(authDetails);
            AnsiConsole.Markup("[green]Data retreieved successfully\n[/]");


            AnsiConsole.Progress()
                .Columns(GetProgressColumns())
                .Start(async ctx =>
                {
                    var task = ctx.AddTask($"[green]Extracting all body stats data...[/]", autoStart: false);
                    task.MaxValue = bodyWeightData.points.Count;
                    task.StartTask();

                    foreach (Point weightPoint in bodyWeightData.points)
                    {
                        AnsiConsole.Markup("[green]Extracting body stats data for: " + weightPoint.date + "\n[/]");
                        BodyStatsResponse bodyStats = PullBodyStatsData(authDetails, weightPoint.id, weightPoint.date);

                        if (bodyStats == null)
                            AnsiConsole.Markup("[red]Extract for: " + weightPoint.date + " failed!\n[/]");
                        else
                            StoreBodyStatData(bodyStats);
                        AnsiConsole.Markup("[green]Data stored successfully\n[/]");


                        task.Increment(1);
                    }
                    task.StopTask();
                });


            return true;
        }

        public bool ImportExtractedData()
        {
            AnsiConsole.Markup("[green]Authenticating with Trainerize\n[/]");
            AuthenticationSession authDetails = Authenticate.AuthenticateWithNewTrainerize(_config);
            AnsiConsole.Markup("[green]Authenticatiion successful\n[/]");

            AnsiConsole.Markup("[green]Retreving body stats data from database\n[/]");
            BodyWeight bodyWeightData = ReadBodyStatData();
            AnsiConsole.Markup("[green]Data retrival successful\n[/]");

            if (bodyWeightData != null)
            {
                PushBodyStatData(authDetails, bodyWeightData);
                AnsiConsole.Markup("[green]Import sucessful\n[/]");

                return true;
            }

            return false;
        }

        private string GetLastRetreivedBodyWeightEntry()
        {
            string? lastDate = _context.Body_Stat_Point.OrderByDescending(x => x.date).FirstOrDefault()?.date;

            if(lastDate != null)
            {
                DateTime date = DateTime.Parse(lastDate);
                date = date.AddDays(1);
                return date.ToString("yyyy-MM-dd");
            }

            return DateTime.Now.AddYears(-10).ToString("yyyy-MM-dd");
        }

        private BodyWeightResponse PullBodyWeightData(AuthenticationSession authDetails)
        {
            BodyWeightRequest jsonBody = new BodyWeightRequest()
            {
                startDate = GetLastRetreivedBodyWeightEntry(),
                endDate = DateTime.Now.ToString("yyyy-MM-dd"),
                type = "bodyweight",
                unit = "kg",
                userid = authDetails.userId
            };

            var authenticator = new JwtAuthenticator(authDetails.token);
            var options = new RestClientOptions()
            {
                Authenticator = authenticator,
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            RestClient client = new RestClient(options);
            var request = new RestRequest();
            request.Resource = _config.GetBodyWeightDataUrl();
            request.Method = Method.Post;
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            var queryResult = client.Execute(request);

            BodyWeightResponse? response = JsonSerializer.Deserialize<BodyWeightResponse>(queryResult.Content);

            if (response.points == null || response.points.Count == 0)
                return null;

            return response;
        }

        private BodyStatsResponse PullBodyStatsData(AuthenticationSession authDetails, int? bodyStatsId, string bodyStatsDate)
        {
            BodyStatsRequest jsonBody = new BodyStatsRequest()
            {
                date = bodyStatsDate,
                id = bodyStatsId,
                unitBodystats = "cm",
                unitWeight = "kg",
                userID = authDetails.userId
            };

            var authenticator = new JwtAuthenticator(authDetails.token);
            var options = new RestClientOptions()
            {
                Authenticator = authenticator,
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            RestClient client = new RestClient(options);
            var request = new RestRequest();
            request.Resource = _config.GetBodyWeightDataUrl();
            request.Method = Method.Post;
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            var queryResult = client.Execute(request);

            BodyStatsResponse? response = JsonSerializer.Deserialize<BodyStatsResponse>(queryResult.Content);

            if (response == null || response.bodyMeasures == null)
                return null;

            return response;
        }

        private bool StoreBodyStatData(BodyStatsResponse bodyStatsData)
        {
            BodyMeasurePoint bodyStat = new BodyMeasurePoint()
            {
                bodyWeight = bodyStatsData.bodyMeasures.bodyWeight,
                bodyFatPercent = bodyStatsData.bodyMeasures.bodyFatPercent,
                bodyMassIndex = bodyStatsData.bodyMeasures.bodyMassIndex,
                caliperMode = bodyStatsData.bodyMeasures.caliperMode,
                chest = bodyStatsData.bodyMeasures.chest,
                date = bodyStatsData.date,
                id = bodyStatsData.id,
                leftBicep = bodyStatsData.bodyMeasures.leftBicep,
                leftCalf = bodyStatsData.bodyMeasures.leftCalf,
                leftThigh = bodyStatsData.bodyMeasures.leftThigh,
                restingHeartRate = bodyStatsData.bodyMeasures.restingHeartRate,
                rightBicep = bodyStatsData.bodyMeasures.rightBicep,
                rightCalf = bodyStatsData.bodyMeasures.rightCalf,
                rightThigh = bodyStatsData.bodyMeasures.rightThigh,
                shoulders = bodyStatsData.bodyMeasures.shoulders,
                waist = bodyStatsData.bodyMeasures.waist,
                newbodystatid = null
            };

            _context.Body_Stat_Point.Add(bodyStat);
            _context.SaveChanges();

            return true;
        }

        private bool StoreBodyWeightData(BodyWeightResponse bodyWeightData)
        {
            List<BodyMeasurePoint> weightPoints = new List<BodyMeasurePoint>();

            foreach(Point datapoint in bodyWeightData.points)
            {
                weightPoints.Add(new BodyMeasurePoint()
                {
                    date = datapoint.date,
                    id = datapoint.id,
                    bodyWeight = datapoint.value,
                    newbodystatid = null
                });
            }

            BodyWeight? bodyWeightRecord = _context.Body_Weight.FirstOrDefault();

            if (bodyWeightRecord != null)
            {
                bodyWeightRecord.points.AddRange(weightPoints);
                _context.Body_Weight.Update(bodyWeightRecord);
            }
            else
            {
                _context.Body_Weight.Add(new BodyWeight()
                {
                    goal = bodyWeightData.goal,
                    points = weightPoints,
                    unit = bodyWeightData.unit
                });
            }

            _context.Body_Stat_Point.AddRange(weightPoints);

            _context.SaveChanges();
            return true;
        }

        private BodyWeight? ReadBodyStatData()
        {
            return _context.Body_Weight.Include(x => x.points.Where(y => y.newbodystatid == null)).FirstOrDefault();
        }

        private bool PushBodyStatData(AuthenticationSession authDetails, BodyWeight bodyWeightData)
        {

            AnsiConsole.Progress()
                .Columns(GetProgressColumns())
                .Start(async ctx =>
                {
                    var task = ctx.AddTask($"[green]Importing body weight data...[/]", autoStart: false);
                    task.MaxValue = bodyWeightData.points.Count;
                    task.StartTask();

                    foreach (BodyMeasurePoint weightPoint in bodyWeightData.points)
                    {
                        int? BodyStatId = CreateBodyStat(authDetails, weightPoint.date);

                        if (BodyStatId != null)
                        {
                            if (AddBodyStatsData(authDetails, BodyStatId, weightPoint))
                                UpdateBodyStatPointToUpdated(weightPoint.id, BodyStatId);
                            else
                                AnsiConsole.Markup("[red]Body stat already exists for date: " + weightPoint.date + "\n[/]");
                        }

                        task.Increment(1);
                    }
                    task.StopTask();
                });


            return true;
        }

        private bool UpdateBodyStatPointToUpdated(int bodyWeightId, int? newBodyStatsId)
        {
            BodyMeasurePoint? point = _context.Body_Stat_Point.FirstOrDefault(x => x.id == bodyWeightId);
            point.newbodystatid = newBodyStatsId;

            _context.Body_Stat_Point.Update(point);
            _context.SaveChanges();

            return true;
        }

        private int? CreateBodyStat(AuthenticationSession authDetails, string date)
        {
            AddBodyStatRequest jsonBody = new AddBodyStatRequest()
            {
                date = date,
                status = "scheduled",
                userID = authDetails.userId
            };

            var authenticator = new JwtAuthenticator(authDetails.token);
            var options = new RestClientOptions()
            {
                Authenticator = authenticator,
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            RestClient client = new RestClient(options);
            var request = new RestRequest();
            request.Resource = _config.AddBodyWeightUrl();
            request.Method = Method.Post;
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            var queryResult = client.Execute(request);

            AddBodyStatResponse? response = JsonSerializer.Deserialize<AddBodyStatResponse>(queryResult.Content);

            if (response.bodyStatsID == null || response.bodyStatsID == 0)
                return null;

            return response.bodyStatsID;
        }

        private bool AddBodyStatsData(AuthenticationSession authDetails, int? bodyStatsID, BodyMeasurePoint weightPoint)
        {
            AddBodyStatDataRequest jsonBody = new AddBodyStatDataRequest() { 
                date = weightPoint.date,
                id  = bodyStatsID,
                unitBodystats  = "cm",
                unitWeight = "kg",
                userID  = authDetails.userId,
                bodyMeasures = new BodyMeasures()
                {
                    date = weightPoint.date,
                    bodyWeight = weightPoint.bodyWeight.ToString(),
                    caliperMode  = weightPoint.caliperMode,
                    bloodPressureDiastolic = string.Empty,
                    bloodPressureSystolic = string.Empty,  
                    bodyFatPercent = weightPoint.bodyFatPercent.ToString(),
                    caliperAbdomen = string.Empty,
                    caliperAxilla = string.Empty,
                    caliperBF = string.Empty,
                    caliperChest = string.Empty,
                    caliperSubscapular = string.Empty,
                    caliperSuprailiac = string.Empty,
                    caliperThigh = string.Empty,
                    caliperTriceps = string.Empty,
                    chest = weightPoint.chest.ToString(),
                    hips = string.Empty,
                    leftBicep = weightPoint.leftBicep.ToString(),
                    leftCalf = weightPoint.leftCalf.ToString(),
                    leftForearm = string.Empty,
                    leftThigh = weightPoint.leftThigh.ToString(),
                    neck = string.Empty,
                    rightBicep = weightPoint.rightBicep.ToString(),
                    rightCalf = weightPoint.rightCalf.ToString(),
                    rightForearm = string.Empty,
                    rightThigh = string.Empty,
                    shoulders = weightPoint.shoulders.ToString(),
                    waist = weightPoint.waist.ToString()
                }
            };


            var authenticator = new JwtAuthenticator(authDetails.token);
            var options = new RestClientOptions()
            {
                Authenticator = authenticator,
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            RestClient client = new RestClient(options);
            var request = new RestRequest();
            request.Resource = _config.AddBodyStatDataUrl();
            request.Method = Method.Post;
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            var queryResult = client.Execute(request);

            AddBodyStatDataReponse? response  = JsonSerializer.Deserialize<AddBodyStatDataReponse>(queryResult.Content);

            if (response.code != null && response.code == 0)
                return true;

            return false;
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
