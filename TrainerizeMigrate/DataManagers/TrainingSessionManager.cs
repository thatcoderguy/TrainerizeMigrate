using RestSharp.Authenticators;
using RestSharp;
using Spectre.Console;
using TrainerizeMigrate.API;
using TrainerizeMigrate.Migrations;
using Newtonsoft.Json;
using TrainerizeMigrate.Data;
using Microsoft.EntityFrameworkCore;

namespace TrainerizeMigrate.DataManagers
{
    public class TrainingSessionManager
    {
        private Config _config { get; set; }
        private ApplicationDbContext _context { get; set; }

        public TrainingSessionManager(Config config, ApplicationDbContext context)
        {
            _config = config;
            _context = context;
        }

        public bool ExtractAndStoreTrainingSessions()
        {
            AnsiConsole.Markup("[green]Authenticating with Trainerize\n[/]");
            AuthenticationSession authDetails = Authenticate.AuthenticateWithOriginalTrainerize(_config);
            AnsiConsole.Markup("[green]Authenticatiion successful\n[/]");

            AnsiConsole.Markup("[green]Pulling training sessions from trainerize\n[/]");
            WorkoutTrainingSessionListReponse trainingSessionWorkouts = PullTrainingSessionWorkoutList(authDetails);
            AnsiConsole.Markup("[green]Data retreieved successfully\n[/]");

            AnsiConsole.Markup("[green]Storing training sessions into database\n[/]");
            StoreTrainingSessionWorkouts(trainingSessionWorkouts);
            AnsiConsole.Markup("[green]Data storage successful\n[/]");

            return true;
        }

        private WorkoutTrainingSessionListReponse PullTrainingSessionWorkoutList(AuthenticationSession authDetails)
        {
            WorkoutTrainingSessionListRequest jsonBody = new WorkoutTrainingSessionListRequest()
            {
                count = 1000,
                endDate = DateTime.Now.ToString("yyyy-MM-dd"),
                start = 0,
                startDate = GetLastRetreivedTrainingSession(),
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
            request.Resource = _config.GetTrainingSessionWorkoutListUrl();
            request.Method = Method.Post;
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            var queryResult = client.Execute(request);

            WorkoutTrainingSessionListReponse? response = JsonConvert.DeserializeObject<WorkoutTrainingSessionListReponse>(queryResult.Content);

            if (response.workouts == null || response.workouts.Count == 0)
                return null;

            return response;
        }

        private string GetLastRetreivedTrainingSession()
        {
            string? lastDate = _context.TrainingSessionWorkout.OrderByDescending(x => x.date).FirstOrDefault()?.date;

            if (lastDate != null)
            {
                DateTime date = DateTime.Parse(lastDate);
                date = date.AddDays(1);
                return date.ToString("yyyy-MM-dd");
            }

            return DateTime.Now.AddYears(-10).ToString("yyyy-MM-dd");
        }

        private bool StoreTrainingSessionWorkouts(WorkoutTrainingSessionListReponse trainingSessionWorkouts)
        {
            foreach(WorkoutList workout in trainingSessionWorkouts.workouts)
            {
                TrainingSessionWorkout trainingSessionWorkout = new TrainingSessionWorkout()
                {
                    dailyWorkoutId = workout.dailyWorkoutId,
                    workoutId = GetWorkoutIdFromNewTrainerize(workout.workoutId),
                    date = workout.date,
                    notes = workout.notes,
                    numOfComments = workout.numOfComments,
                    rpe = workout.rpe,
                    workout = workout.workout
                };

                _context.TrainingSessionWorkout.Add(trainingSessionWorkout);
                _context.SaveChanges();
            }

            return true;
        }

        private int? GetWorkoutIdFromNewTrainerize(int? oldWorkoutId)
        {
            PlanWorkout workout = _context.TrainingPlanWorkout.FirstOrDefault(x => x.id == oldWorkoutId);
            return workout.new_id;
        }

        public bool ExtractAndStoreTrainingSessionStats()
        {
            AnsiConsole.Markup("[green]Authenticating with Trainerize\n[/]");
            AuthenticationSession authDetails = Authenticate.AuthenticateWithOriginalTrainerize(_config);
            AnsiConsole.Markup("[green]Authenticatiion successful\n[/]");

            AnsiConsole.Markup("[green]Reading training sessions from database\n[/]");
            List<TrainingSessionWorkout> trainingSessionWorkouts = ReadTrainingSessionsNotImported();
            AnsiConsole.Markup("[green]Data retreieved successfully\n[/]");

            AnsiConsole.Progress()
                .Columns(GetProgressColumns())
                .Start(async ctx =>
                {
                    var task = ctx.AddTask($"[green]Pulling stats for training session workouts...[/]", autoStart: false);
                    task.MaxValue = trainingSessionWorkouts.Count;
                    task.StartTask();

                    foreach (TrainingSessionWorkout workout in trainingSessionWorkouts)
                    {
                        AnsiConsole.Markup("[green]Pulling workout on: " + workout.date + "\n[/]");
                        TrainingSessionStatsResponse sessionStats = PullTrainingSessionStatsFromTrainerize(authDetails, workout.dailyWorkoutId);

                        AnsiConsole.Markup("[green]Storing session data into into database\n[/]");
                        if (sessionStats.dailyWorkouts.Count > 0 && sessionStats.dailyWorkouts[0].exercises.Count > 0)
                            StoreSessionStats(sessionStats, workout);
                        AnsiConsole.Markup("[green]Data storage successful\n[/]");

                        task.Increment(1);
                    }
                    task.StopTask();

                });

            return true;
        }

        private List<TrainingSessionWorkout> ReadTrainingSessionsNotImported()
        {
            return _context.TrainingSessionWorkout.Where(x => x.newdailyWorkoutId == null).ToList();
        }

        private TrainingSessionStatsResponse PullTrainingSessionStatsFromTrainerize(AuthenticationSession authDetails, int? dailyWorkoutId)
        {
            TrainingSessionStatsRequest jsonBody = new TrainingSessionStatsRequest()
            {
                ids = new List<int?> { dailyWorkoutId },
                unitDistance = "km",
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
            request.Resource = _config.GetTrainingSessionStatsUrl();
            request.Method = Method.Post;
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            var queryResult = client.Execute(request);

            TrainingSessionStatsResponse? response = null;

            try
            {
                response = JsonConvert.DeserializeObject<TrainingSessionStatsResponse>(queryResult.Content);

                if (response.dailyWorkouts == null || response.dailyWorkouts.Count == 0)
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }

            return response;
        }

        private bool StoreSessionStats(TrainingSessionStatsResponse sessionStats, TrainingSessionWorkout workout)
        {
            DailyWorkout dailyWorkout = sessionStats.dailyWorkouts[0];

            if (workout.stats == null)
                workout.stats = new List<TrainingSessionStat>();

            foreach (TrainingSessionStatsExercise exercise in dailyWorkout.exercises)
            {
                foreach(Stat trainingStats in exercise.stats)
                {
                    TrainingSessionStat trainingStat = new TrainingSessionStat()
                    {
                        excersizeId = exercise.def.type == "system" ? exercise.def.id : GetNewCustomExcersizeId(exercise.def.id),
                        calories = trainingStats.calories,
                        dailyExerciseID = exercise.dailyExerciseID,
                        distance = trainingStats.distance,
                        level = trainingStats.level,
                        reps = trainingStats.reps,
                        setNumber = trainingStats.setID,
                        speed = trainingStats.speed,
                        time = trainingStats.time,
                        weight = trainingStats.weight
                    };

                    workout.stats.Add(trainingStat);

                    _context.TrainingSessionStat.Add(trainingStat);
                }
            }

            _context.TrainingSessionWorkout.Update(workout);
            _context.SaveChanges();

            return true;
        }

        private int? GetNewCustomExcersizeId(int oldExcersizeId)
        {
            CustomExcersize excersize = _context.Excerisize.FirstOrDefault(x => x.id == oldExcersizeId);
            return excersize.new_id;
        }

        public bool ImportTrainingSessions()
        {
            AnsiConsole.Markup("[green]Authenticating with Trainerize\n[/]");
            AuthenticationSession authDetails = Authenticate.AuthenticateWithNewTrainerize(_config);
            AnsiConsole.Markup("[green]Authenticatiion successful\n[/]");

            AnsiConsole.Markup("[green]Reading training sessions from database\n[/]");
            List<TrainingSessionWorkout> trainingSessionWorkouts = ReadTrainingSessionsNotImported();
            AnsiConsole.Markup("[green]Data retreieved successfully\n[/]");

            AnsiConsole.Progress()
                .Columns(GetProgressColumns())
                .Start(async ctx =>
                {
                    var task = ctx.AddTask($"[green]Importing training sessions...[/]", autoStart: false);
                    task.MaxValue = trainingSessionWorkouts.Count;
                    task.StartTask();

                    foreach (TrainingSessionWorkout workout in trainingSessionWorkouts)
                    {
                        AnsiConsole.Markup("[green]Importing workout for: " + workout.date + "\n[/]");
                        AddTrainingSessionResponse? workoutResponse = PushTrainingSession(authDetails, workout);

                        if (workoutResponse != null && workoutResponse.dailyWorkoutIDs.Count > 0)
                        {
                            UpdateDailyWorkoutId(workout.dailyWorkoutId, workoutResponse.dailyWorkoutIDs[0]);
                            AnsiConsole.Markup("[green]Import successful\n[/]");
                        }
                        else
                            AnsiConsole.Markup("[red]Import failed\n[/]");

                        task.Increment(1);
                    }

                    task.StopTask();

                });

            return true;
        }

        private AddTrainingSessionResponse? PushTrainingSession(AuthenticationSession authDetails, TrainingSessionWorkout workout)
        {
            PlanWorkout? workoutDetails = GetWorkoutById(workout.workoutId);

            if (workoutDetails == null)
                return null;

            AddTrainingSessionRequest jsonBody = new AddTrainingSessionRequest()
            {
                unitDistance = "km",
                unitWeight = "kg",
                dailyWorkouts = new List<AddDailyWorkout>()
                {
                    new AddDailyWorkout()
                    {
                        date = workout.date,
                        id = 0,
                        name = workout.workout,
                        type = workoutDetails.type,
                        userID = authDetails.userId,
                        workoutID = workout.workoutId
                    }
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
            request.Resource = _config.AddTrainingSessionUrl();
            request.Method = Method.Post;
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            var queryResult = client.Execute(request);

            AddTrainingSessionResponse? response = null;

            try
            {
                response = JsonConvert.DeserializeObject<AddTrainingSessionResponse>(queryResult.Content);

                if (response.dailyWorkoutIDs == null || response.dailyWorkoutIDs.Count == 0)
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }

            return response;
        }

        private bool UpdateDailyWorkoutId(int dailyWorkoutId, int? newDailyWorkoutId)
        {
            TrainingSessionWorkout? workout = _context.TrainingSessionWorkout.FirstOrDefault(x => x.dailyWorkoutId == dailyWorkoutId);
            workout.newdailyWorkoutId = newDailyWorkoutId;
            _context.TrainingSessionWorkout.Update(workout);
            _context.SaveChanges();

            return true;
        }

        private PlanWorkout? GetWorkoutById(int? workoutId)
        {
            return _context.TrainingPlanWorkout.Where(x => x.new_id == workoutId).FirstOrDefault();
        }

        public bool ImportTrainingSessionStats()
        {
            AnsiConsole.Markup("[green]Authenticating with Trainerize\n[/]");
            AuthenticationSession authDetails = Authenticate.AuthenticateWithNewTrainerize(_config);
            AnsiConsole.Markup("[green]Authenticatiion successful\n[/]");

            AnsiConsole.Markup("[green]Reading training sessions from database\n[/]");
            List<TrainingSessionWorkout> trainingSessionWorkouts = ReadTrainingSessionStatsNotImported();
            AnsiConsole.Markup("[green]Data retreieved successfully\n[/]");

            AnsiConsole.Progress()
                .Columns(GetProgressColumns())
                .Start(async ctx =>
                {
                    var task = ctx.AddTask($"[green]Importing training session stat data...[/]", autoStart: false);
                    task.MaxValue = trainingSessionWorkouts.Count;
                    task.StartTask();

                    foreach (TrainingSessionWorkout workout in trainingSessionWorkouts)
                    {
                        task.MaxValue += workout.stats.Count;

                        AnsiConsole.Markup("[green]Linking excersize IDs for workout on: " + workout.date + "\n[/]");
                        if (LinkNewWorkoutExcersizesToOldWorkoutExcersizes(authDetails, workout))
                        {
                            AnsiConsole.Markup("[green]Excersizes linked OK\n[/]");

                            AnsiConsole.Markup("[green]Pushing training session into Trainerize\n[/]");
                            if(!PushTrainingSessionIntoTrainerize(workout))
                                AnsiConsole.Markup("[red]Training session push failed![/]");

                        }
                        else
                            AnsiConsole.Markup("[red]Excersize linking failed![/]");

                        task.Increment(1);
                    }
                    task.StopTask();

                });

            return true;
        }

        private bool LinkNewWorkoutExcersizesToOldWorkoutExcersizes(AuthenticationSession authDetails, TrainingSessionWorkout workout)
        {
            //pulled from new trainerize
            TrainingSessionStatsResponse newWorkout = PullTrainingSessionStatsFromTrainerize(authDetails, workout.newdailyWorkoutId);

            foreach(DailyWorkout dailyWorkout in newWorkout.dailyWorkouts)
            {
                TrainingSessionWorkout? excersizesToMap = _context.TrainingSessionWorkout.Include(x => x.stats).FirstOrDefault(x => x.newdailyWorkoutId == dailyWorkout.id);

                if (excersizesToMap == null)
                    return false;

                //loop through excersizes returned from trainerize
                foreach (TrainingSessionStatsExercise exercise in dailyWorkout.exercises)
                {
                    //link excersizes downloaded to excersizes in schedule workout
                    List<TrainingSessionStat>? statsToUpdate = excersizesToMap.stats.Where(x => x.excersizeId == exercise.def.id).ToList();
                    if (statsToUpdate == null || statsToUpdate.Count == 0)
                        return false;

                    //update the dailyID for all matches excersizes
                    foreach(TrainingSessionStat statToUpdate in statsToUpdate)
                    {
                        statToUpdate.newdailyExerciseID = exercise.dailyExerciseID;
                        _context.TrainingSessionStat.Update(statToUpdate);
                        _context.SaveChanges();
                    }
                }

                //there may be some excersizes in the old trainerize, which have been added or changed.
                List<TrainingSessionStat> unlinkedStats = excersizesToMap.stats.Where(x => x.newdailyExerciseID == null).ToList();

                //create a new ID for all of those
                foreach(TrainingSessionStat stat in unlinkedStats)
                {
                    DateTimeOffset dto = new DateTimeOffset(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Second, DateTime.Now.Nanosecond / 100, TimeSpan.Zero);

                    stat.newdailyExerciseID = dto.ToUnixTimeMilliseconds();
                    _context.TrainingSessionStat.Update(stat);
                    _context.SaveChanges();
                }
            }

            return true;
        }

        private bool PushTrainingSessionIntoTrainerize(TrainingSessionWorkout workout)
        {
            foreach (TrainingSessionStat stat in workout.stats)
            {

                // AnsiConsole.Markup("[green]Pulling workout on: " + workout.date + "\n[/]");
                //  TrainingSessionStatsResponse sessionStats = PullTrainingSessionStatsFromTrainerize(authDetails, workout.dailyWorkoutId);

                //  AnsiConsole.Markup("[green]Storing session data into into database\n[/]");
                //   if (sessionStats.dailyWorkouts.Count > 0 && sessionStats.dailyWorkouts[0].exercises.Count > 0)
                //       StoreSessionStats(sessionStats, workout);
                //   AnsiConsole.Markup("[green]Data storage successful\n[/]");


            }

            return false;
        }

        private List<TrainingSessionWorkout> ReadTrainingSessionStatsNotImported()
        {
            return _context.TrainingSessionWorkout.Include(x => x.stats.Where(t => t.newdailyExerciseID == null)).Where(y => y.newdailyWorkoutId != null).ToList();
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
