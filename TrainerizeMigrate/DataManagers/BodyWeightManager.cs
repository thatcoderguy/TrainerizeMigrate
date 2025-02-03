using RestSharp.Authenticators;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TrainerizeMigrate.API;
using TrainerizeMigrate.Migrations;
using TrainerizeMigrate.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;

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
            AuthenticationSession authDetails = Authenticate.AuthenticateWithOriginalTrainerize(_config);
            BodyWeightResponse bodyWeightData = PullBodyWeightData(authDetails);

            StoreBodyWeightData(bodyWeightData);

            return true;
        }

        public bool ImportExtractedData()
        {
            AuthenticationSession authDetails = Authenticate.AuthenticateWithNewTrainerize(_config);
            BodyWeight bodyWeightData = ReadBodyWeightData();

            if (bodyWeightData != null)
            {
                PushBodyWeightData(authDetails, bodyWeightData);
                return true;
            }

            return false;
        }

        private string GetLastRetreivedBodyWeightEntry()
        {
            string? lastDate = _context.Body_Weight_Point.OrderByDescending(x => x.date).FirstOrDefault()?.date;

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
                //"2025-02-28"
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
            request.Resource = _config.GetBodyStatsUrl();
            request.Method = Method.Post;
            request.AddJsonBody(jsonBody, ContentType.Json);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var queryResult = client.Execute(request);

            BodyWeightResponse response = JsonSerializer.Deserialize<BodyWeightResponse>(queryResult.Content);

            return response;
        }

        private bool StoreBodyWeightData(BodyWeightResponse bodyWeightData)
        {
            List<WeightPoint> weightPoints = new List<WeightPoint>();

            foreach(Point datapoint in bodyWeightData.points)
            {
                weightPoints.Add(new WeightPoint()
                {
                    date = datapoint.date,
                    id = datapoint.id,
                    value = datapoint.value,
                    newbodystatid = null
                });
            }

            BodyWeight bodyWeightRecord = _context.Body_Weight.FirstOrDefault();

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

            _context.Body_Weight_Point.AddRange(weightPoints);

            _context.SaveChanges();
            return true;
        }

        private BodyWeight ReadBodyWeightData()
        {
            return _context.Body_Weight.Include(x => x.points.Where(y => y.newbodystatid == null)).FirstOrDefault();
        }

        private bool PushBodyWeightData(AuthenticationSession authDetails, BodyWeight bodyWeightData)
        {
            foreach(WeightPoint weightPoint in bodyWeightData.points)
            {
                int BodyStatId = CreateBodyStat(authDetails, weightPoint.date);

                if (!AddBodyStatsData(authDetails, BodyStatId, weightPoint.value, weightPoint.date))
                    throw new Exception("Could not add Body Stat Data");

                UpdateBodyWeightPointToUpdated(weightPoint.id, BodyStatId);
            }

            return true;
        }

        private bool UpdateBodyWeightPointToUpdated(int bodyWeightId, int newBodyStatsId)
        {
            WeightPoint point = _context.Body_Weight_Point.FirstOrDefault(x => x.id == bodyWeightId);
            point.newbodystatid = newBodyStatsId;

            _context.Body_Weight_Point.Update(point);
            _context.SaveChanges();

            return true;
        }

        private int CreateBodyStat(AuthenticationSession authDetails, string date)
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
            request.Resource = _config.AddBodyStatsUrl();
            request.Method = Method.Post;
            request.AddJsonBody(jsonBody, ContentType.Json);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var queryResult = client.Execute(request);

            AddBodyStatResponse response = JsonSerializer.Deserialize<AddBodyStatResponse>(queryResult.Content);

            return response.bodyStatsID;
        }

        private bool AddBodyStatsData(AuthenticationSession authDetails, int bodyStatsID, double bodyWeight, string date)
        {
            AddBodyStatDataRequest jsonBody = new AddBodyStatDataRequest() { 
                date = date,
                id  = bodyStatsID,
                unitBodystats  = "cm",
                unitWeight = "kg",
                userID  = authDetails.userId,
                bodyMeasures = new BodyMeasures()
                {
                    date = date,
                    bodyWeight = bodyWeight.ToString(),
                    caliperMode  = 7,
                    bloodPressureDiastolic = string.Empty,
                    bloodPressureSystolic = string.Empty,  
                    bodyFatPercent = string.Empty,
                    caliperAbdomen = string.Empty,
                    caliperAxilla = string.Empty,
                    caliperBF = string.Empty,
                    caliperChest = string.Empty,
                    caliperSubscapular = string.Empty,
                    caliperSuprailiac = string.Empty,
                    caliperThigh = string.Empty,
                    caliperTriceps = string.Empty,
                    chest = string.Empty,
                    hips = string.Empty,
                    leftBicep = string.Empty,
                    leftCalf = string.Empty,
                    leftForearm = string.Empty,
                    leftThigh = string.Empty,
                    neck = string.Empty,
                    rightBicep = string.Empty,
                    rightCalf = string.Empty,
                    rightForearm = string.Empty,
                    rightThigh = string.Empty,
                    shoulders = string.Empty,
                    waist = string.Empty
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
            request.Resource = _config.AddBodyStatsDataUrl();
            request.Method = Method.Post;
            request.AddJsonBody(jsonBody, ContentType.Json);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var queryResult = client.Execute(request);
            
            AddBodyStatDataReponse response = JsonSerializer.Deserialize<AddBodyStatDataReponse>(queryResult.Content);

            if (response.code == 0)
                return true;

            return false;
        }

    }

}
