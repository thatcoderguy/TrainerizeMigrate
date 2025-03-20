using RestSharp.Authenticators;
using RestSharp;
using Spectre.Console;
using System.Text.Json;
using TrainerizeMigrate.API;
using TrainerizeMigrate.Migrations;
using TrainerizeMigrate.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Numerics;

namespace TrainerizeMigrate.DataManagers
{
    public class WorkoutManager
    {
        private Config _config { get; set; }
        private ApplicationDbContext _context { get; set; }

        public WorkoutManager(Config config, ApplicationDbContext context)
        {
            _config = config;
            _context = context;
        }

        public bool ExtractAndStoreTrainingPrograms()
        {
            AnsiConsole.Markup("[green]Authenticating with Trainerize\n[/]");
            AuthenticationSession authDetails = Authenticate.AuthenticateWithOriginalTrainerize(_config);
            AnsiConsole.Markup("[green]Authenticatiion successful\n[/]");

            AnsiConsole.Markup("[green]Pulling training programs from trainerize\n[/]");
            TrainingProgramListResponse trainingPrograms = PullTrainingProgramList(authDetails);
            AnsiConsole.Markup("[green]Data retreieved successfully\n[/]");

            AnsiConsole.Markup("[green]Storing training programs into database\n[/]");
            StoreTrainingPrograms(trainingPrograms);
            AnsiConsole.Markup("[green]Data storage successful\n[/]");

            return true;
        }

        private TrainingProgramListResponse PullTrainingProgramList(AuthenticationSession authDetails)
        {
            TrainingProgramListRequest jsonBody = new TrainingProgramListRequest()
            {
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
            request.Resource = _config.GetTrainingProgramsUrl();
            request.Method = Method.Post;
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            var queryResult = client.Execute(request);

            TrainingProgramListResponse response = null;

            try
            {
                response = JsonConvert.DeserializeObject<TrainingProgramListResponse>(queryResult.Content);
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup("[red]Error: " + ex.Message + "\n[/]");
                return null;
            }

            return response;
        }

        private bool StoreTrainingPrograms(TrainingProgramListResponse trainingPrograms)
        {
            foreach (API.Program program in trainingPrograms.programs)
            {
                TrainingProgram trainingProgram = new TrainingProgram()
                {
                    accessLevel = program.accessLevel,
                    durationType = program.durationType,
                    endDate = program.endDate,
                    name = program.name,
                    startDate = program.startDate,
                    subscriptionType = program.subscribeType
                };

                _context.TrainingProgram.Add(trainingProgram);
                _context.SaveChanges();
            }

            return true;
        }

        public void ImportTrainingPrograms()
        {
            //NOT NEEDED
            throw new NotImplementedException();
        }

        public void ExtractAndStoreTrainingProgramPhases()
        {
            AnsiConsole.Markup("[green]Authenticating with Trainerize\n[/]");
            AuthenticationSession authDetails = Authenticate.AuthenticateWithOriginalTrainerize(_config);
            AnsiConsole.Markup("[green]Authenticatiion successful\n[/]");

            AnsiConsole.Markup("[green]Pulling training phases from trainerize\n[/]");
            ProgramPhasesResponse programPhases = PullProgramPhases(authDetails);
            AnsiConsole.Markup("[green]Data retreieved successfully\n[/]");

            AnsiConsole.Markup("[green]Storing training phases into database\n[/]");
            StoreProgramPhases(programPhases);
            AnsiConsole.Markup("[green]Data storage successful\n[/]");
        }

        private ProgramPhasesResponse PullProgramPhases(AuthenticationSession authDetails)
        {
            ProgramPhasesRequest jsonBody = new ProgramPhasesRequest()
            {
                userID = authDetails.userId,
                userProgramID = null
            };

            var authenticator = new JwtAuthenticator(authDetails.token);
            var options = new RestClientOptions()
            {
                Authenticator = authenticator,
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            RestClient client = new RestClient(options);
            var request = new RestRequest();
            request.Resource = _config.GetTrainingProgramPhasesUrl();
            request.Method = Method.Post;
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            var queryResult = client.Execute(request);

            ProgramPhasesResponse response  = JsonConvert.DeserializeObject<ProgramPhasesResponse>(queryResult.Content);

            if (response.plans == null || response.plans.Count == 0)
                return null;

            return response;
        }

        private bool StoreProgramPhases(ProgramPhasesResponse programPhases)
        {
            foreach (Plan phase in programPhases.plans)
            {
                if (!_context.TrainingProgramPhase.Any(x => x.id == phase.id))
                {
                    _context.TrainingProgramPhase.Add(new ProgramPhase()
                    {
                        id = phase.id,
                        endDate = phase.endDate,
                        instruction = phase.instruction,
                        modified = phase.modified,
                        name = phase.name,
                        planType = phase.planType,
                        startDate = phase.startDate,
                        durationType = phase.durationType,
                        duration = phase.duration
                    });

                    _context.SaveChanges();
                }
            }

            return true;
        }

        public bool ImportTrainingProgramPhases()
        {
            AnsiConsole.Markup("[green]Authenticating with Trainerize as admin\n[/]");
            AuthenticationSession trainerAuthDetails = Authenticate.AuthenticateWithNewTrainerizeAsAdmin(_config);
            AnsiConsole.Markup("[green]Authenticatiion successful\n[/]");

            AnsiConsole.Markup("[green]Authenticating with Trainerize to get Client Id\n[/]");
            AuthenticationSession clientAuthDetails = Authenticate.AuthenticateWithNewTrainerize(_config);
            AnsiConsole.Markup("[green]Authenticatiion successful\n[/]");

            AnsiConsole.Markup("[green]Retreving program phases from database\n[/]");
            List<ProgramPhase> phases = ReadTrainingPhasesNotImported();
            AnsiConsole.Markup("[green]Data retrival successful\n[/]");

            if (phases.Count > 0)
            {
                PushProgramPhases(trainerAuthDetails, phases, clientAuthDetails.userId);
                AnsiConsole.Markup("[green]Import sucessful\n[/]");

                return true;
            }

            return false;
        }

        private bool PushProgramPhases(AuthenticationSession authDetails, List<ProgramPhase> phases, int clientID)
        {
            AnsiConsole.Progress()
                .Columns(GetProgressColumns())
                .Start(async ctx =>
                {
                    var task = ctx.AddTask($"[green]Importing training phases...[/]", autoStart: false);
                    task.MaxValue = phases.Count;
                    task.StartTask();

                    foreach (ProgramPhase phase in phases)
                    {
                        //AnsiConsole.Markup("[green]Adding Phase " + phase.name + "\n[/]");
                        int? newPhaseId = AddProgramPhase(authDetails, phase, clientID);
                        if (newPhaseId is not null)
                            UpdatePhase(phase.id, newPhaseId);

                        task.Increment(1);
                    }
                    task.StopTask();
                });

            AnsiConsole.Markup("[green]Training phases imported\n[/]");

            return false;
        }

        private int? AddProgramPhase(AuthenticationSession authDetails, ProgramPhase phase, int clientID)
        {
            AddProgramPhaseRequest jsonBody = new AddProgramPhaseRequest()
            {
                userid = clientID,
                plan = new PlanRequest()
                {
                    name = phase.name,
                    duration = phase.duration,
                    durationType = phase.durationType,
                    endDate = phase.endDate,
                    startDate = phase.startDate
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
            request.Resource = _config.AddTrainingPhaseUrl();
            request.Method = Method.Post;
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            RestResponse queryResult = client.Execute(request);

            AddCustomExcersizeResponse response = JsonConvert.DeserializeObject<AddCustomExcersizeResponse>(queryResult.Content);
            
            if (response.id == 0)
                return null;

            return response.id;
        }

        private bool UpdatePhase(int oldPhaseId, int? newPhaseId)
        {
            ProgramPhase programPhase = _context.TrainingProgramPhase.FirstOrDefault(x => x.id == oldPhaseId);
            programPhase.new_id = newPhaseId;
            _context.TrainingProgramPhase.Update(programPhase);
            _context.SaveChanges();
            return true;
        }

        private int? GetTrainingProgramId(AuthenticationSession authDetails)
        {
            TrainingProgramListRequest jsonBody = new TrainingProgramListRequest()
            {
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
            request.Resource = _config.GetTrainingProgramsUrl();
            request.Method = Method.Post;
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
             var queryResult = client.Execute(request);

            TrainingProgramListResponse response = JsonConvert.DeserializeObject<TrainingProgramListResponse>(queryResult.Content);

            if (response.programs == null || response.programs.Count == 0)
                return null;

            int programID = response.programs[0].id;

            return programID;
        }

        private List<ProgramPhase> ReadTrainingPhasesNotImported()
        {
            return _context.TrainingProgramPhase.Where(x => x.new_id == null).ToList();
        }

        public void ExtractAndStoreWorkoutsForPhases()
        {
            AnsiConsole.Markup("[green]Authenticating with Trainerize\n[/]");
            AuthenticationSession authDetails = Authenticate.AuthenticateWithOriginalTrainerize(_config);
            AnsiConsole.Markup("[green]Authenticatiion successful\n[/]");

            AnsiConsole.Markup("[green]Reading all phases without imported workouts\n[/]");
            List<ProgramPhase> phases = ReadAllPhasesWithoutImportedWorkouts();
            AnsiConsole.Markup("[green]Data retreieved successfully\n[/]");

            AnsiConsole.Progress()
                .Columns(GetProgressColumns())
                .Start(async ctx =>
                {
                    var task = ctx.AddTask($"[green]Pulling phase workouts...[/]", autoStart: false);
                    task.MaxValue = phases.Count;
                    task.StartTask();

                    foreach (ProgramPhase phase in phases)
                    {
                        //AnsiConsole.Markup("[green]Pulling workouts for phase: " + phase.name + "\n[/]");
                        PhaseWorkoutPlansResponse phaseWorkouts = PullPhaseWorkouts(authDetails, phase.id);

                        //AnsiConsole.Markup("[green]Storing training programs for phase into database\n[/]");
                        if (phaseWorkouts.workouts.Count > 0)
                            StorePhaseWorkouts(phaseWorkouts, phase.id);
                        

                        task.Increment(1);
                    }
                    task.StopTask();

                });

            AnsiConsole.Markup("[green]Data storage successful\n[/]");
        }

        private void StorePhaseWorkouts(PhaseWorkoutPlansResponse phaseWorkouts, int phaseId)
        {
            ProgramPhase phase = _context.TrainingProgramPhase.Include(x => x.workouts).FirstOrDefault(x => x.id == phaseId);

            if (phase.workouts == null)
                phase.workouts = new List<PlanWorkout>();

            if (phase != null)
            {
                foreach (Workout workout in phaseWorkouts.workouts)
                {
                    PlanWorkout workoutPlan = new PlanWorkout()
                    {
                        id = workout.id,
                        instruction = workout.instruction,
                        name = workout.name,
                        new_id = null,
                        type = workout.type,
                        excersizes = new List<WorkoutExcersize>()
                    };

                    int order = 0;

                    foreach (PhaseWorkoutPlanExercise exercise in workout.exercises)
                    {
                        //check if the excersize is a custom excersize
                        CustomExcersize storedCustomExcersize = _context.Excerisize.FirstOrDefault(x => x.id == exercise.id);
                        int? excersizeId = 0;

                        //if it is, then link it to the excersize Id in the new trainerize
                        if(storedCustomExcersize != null)
                            excersizeId = storedCustomExcersize.new_id;
                        else
                            excersizeId = exercise.id;

                        WorkoutExcersize workoutExcersize = new WorkoutExcersize()
                        {
                            id = excersizeId,
                            restTime = exercise.restTime,
                            sets = exercise.sets,
                            superSetID = exercise.superSetID,
                            target = exercise.target,
                            targetDetailText = exercise.targetDetail is null ? null : exercise.targetDetail.text,
                            targetDetailTime = exercise.targetDetail is null ? null : exercise.targetDetail.time,
                            targetDetailType = exercise.targetDetail is null ? null : exercise.targetDetail.type,
                            SystemId = new Guid(),
                            intervalTime = exercise.intervalTime,
                            order = order,
                            recordType = exercise.recordType
                        };

                        order++;

                        workoutPlan.excersizes.Add(workoutExcersize);
                        _context.WorkoutExcersize.Add(workoutExcersize);
                    }

                    _context.TrainingPlanWorkout.Add(workoutPlan);

                    phase.workouts.Add(workoutPlan);
                    _context.TrainingProgramPhase.Update(phase);

                    _context.SaveChanges();
                }
            }
        }

        private List<ProgramPhase> ReadAllPhasesWithoutImportedWorkouts()
        {
            return _context.TrainingProgramPhase.Where(x => x.new_id != null && x.workoutsimported == false).ToList();
        }

        private PhaseWorkoutPlansResponse PullPhaseWorkouts(AuthenticationSession authDetails, int PhaseId)
        {
            PhaseWorkoutPlansRequest jsonBody = new PhaseWorkoutPlansRequest()
            {
                planID = PhaseId,
                count = 100,
                filter = new Filter() { duration = null, equipments = null },
                searchTerm = string.Empty,
                sort = "name",
                start = 0
            };

            var authenticator = new JwtAuthenticator(authDetails.token);
            var options = new RestClientOptions()
            {
                Authenticator = authenticator,
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            RestClient client = new RestClient(options);
            var request = new RestRequest();
            request.Resource = _config.GetPhaseWorkoutPlansUrl();
            request.Method = Method.Post;
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            var queryResult = client.Execute(request);

            PhaseWorkoutPlansResponse response = JsonConvert.DeserializeObject<PhaseWorkoutPlansResponse>(queryResult.Content);

            return response;
        }

        public void ImportWorkoutPlansForPhases()
        {
            AnsiConsole.Markup("[green]Authenticating with Trainerize as admin\n[/]");
            AuthenticationSession trainerAuthDetails = Authenticate.AuthenticateWithNewTrainerizeAsAdmin(_config);
            AnsiConsole.Markup("[green]Authenticatiion successful\n[/]");

            AnsiConsole.Markup("[green]Authenticating with Trainerize to get Client Id\n[/]");
            AuthenticationSession clientAuthDetails = Authenticate.AuthenticateWithNewTrainerize(_config);
            AnsiConsole.Markup("[green]Authenticatiion successful\n[/]");

            AnsiConsole.Markup("[green]Retreving workout plans from database\n[/]");
            List<ProgramPhase> phases = ReadPhasesAndWorkoutsAndExcersizesNotImported();
            AnsiConsole.Markup("[green]Data retrival successful\n[/]");

            AnsiConsole.Progress()
                .Columns(GetProgressColumns())
                .Start(async ctx =>
                {
                    var task = ctx.AddTask($"[green]Importing phase workouts...[/]", autoStart: false);
                    task.MaxValue = phases.Count;
                    task.StartTask();

                    foreach (ProgramPhase phase in phases)
                    {
                        //AnsiConsole.Markup("[green]Importing phase: " + phase.name + "\n[/]");

                        task.MaxValue = task.MaxValue + phase.workouts.Count;

                        foreach (PlanWorkout workout in phase.workouts)
                        {
                            int? newWorkoutId = PushWorkoutForPhase(trainerAuthDetails, clientAuthDetails.userId, phase, workout);

                            if (newWorkoutId != null)
                                SetWorkoutAsImported(workout.id, newWorkoutId);

                            task.Increment(1);

                        }

                        SetPhaseAsImported(phase.new_id);

                        task.Increment(1);
                    }

                    task.StopTask();
                });

            AnsiConsole.Markup("[green]Workout import successful\n[/]");
        }

        private int? PushWorkoutForPhase(AuthenticationSession trainerAuthDetails, int clientId, ProgramPhase phase, PlanWorkout workout)
        {
            //AnsiConsole.Markup("[green]Importing workout: " + workout.name + "\n[/]");

            AddWorkoutRequest jsonBody = new AddWorkoutRequest()
            {
                trainingPlanID = phase.new_id,
                userID = clientId,
                workoutDef = new WorkoutDef(),
                type = "trainingPlan"
            };

            jsonBody.workoutDef = BuildWorkoutRequest(workout);

            var authenticator = new JwtAuthenticator(trainerAuthDetails.token);
            var options = new RestClientOptions()
            {
                Authenticator = authenticator,
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            RestClient client = new RestClient(options);
            var request = new RestRequest();
            request.Resource = _config.AddWorkoutPlanToPhaseUrl();
            request.Method = Method.Post;
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            var queryResult = client.Execute(request);

            AddWorkoutResponse response = JsonConvert.DeserializeObject<AddWorkoutResponse>(queryResult.Content);

            return response.workoutID;
        }

        private WorkoutDef BuildWorkoutRequest(PlanWorkout workout) 
        {
            WorkoutDef workoutDet = new WorkoutDef()
            {
                exercises = new List<AddWorkoutxercise>(),
                instructions = workout.instruction,
                name = workout.name,
                rounds = 1,
                type = workout.type,
                tags = new List<string>(),
                trackingStats = new TrackingStats()
                {
                    def = new WrapperDef()
                    {
                        def = new TrackingDef()
                        {
                            avgHeartRate = false,
                            effortInterval = false,
                            restInterval = false,
                            maxHeartRate = false,
                            minHeartRate = false,
                            zone = false
                        }
                    }
                }
            };

            workoutDet.exercises = BuildWorkoutExcersizes(workout);

            return workoutDet;
        }

        private List<AddWorkoutxercise> BuildWorkoutExcersizes(PlanWorkout workout)
        {
            List<AddWorkoutxercise> excersizes = new List<AddWorkoutxercise>();

            foreach (WorkoutExcersize excersize in workout.excersizes.OrderBy(x => x.order))
            {
                excersizes.Add(new AddWorkoutxercise()
                {
                    def = new ExcersizeDef()
                    {
                        id = excersize.id,
                        intervalTime = excersize.intervalTime,
                        restTime = excersize.restTime,
                        sets = excersize.sets,
                        superSetID = excersize.superSetID,
                        supersetType = excersize.superSetID > 0 ? "superset" : null,
                        target = excersize.target,
                        targetDetail = new AddWorkoutTargetDetail()
                        {
                            text = excersize.targetDetailText,
                            time = excersize.targetDetailTime,
                            type = excersize.targetDetailType
                        }
                    }
                });
            }

            return excersizes;
        }

        private bool SetWorkoutAsImported(int? workoutId, int? newWorkoutId)
        {
            PlanWorkout workout = _context.TrainingPlanWorkout.FirstOrDefault(x => x.id == workoutId);
            workout.new_id = newWorkoutId;
            _context.TrainingPlanWorkout.Update(workout);
            _context.SaveChanges(true);

            return true;
        }

        private bool SetPhaseAsImported(int? phaseId)
        {
            ProgramPhase phase = _context.TrainingProgramPhase.FirstOrDefault(x => x.new_id  == phaseId);
            phase.workoutsimported = true;
            _context.TrainingProgramPhase.Update(phase);
            _context.SaveChanges(true);

            return true;
        }

        private List<ProgramPhase> ReadPhasesAndWorkoutsAndExcersizesNotImported()
        {
            return _context.TrainingProgramPhase.Include(x => x.workouts).ThenInclude(x => x.excersizes).Where(x => !x.workoutsimported && x.new_id != null).ToList();
        }

        public bool DeleteAllImportedPhases()
        {
            AnsiConsole.Markup("[green]Authenticating with Trainerize as admin\n[/]");
            AuthenticationSession trainerAuthDetails = Authenticate.AuthenticateWithNewTrainerizeAsAdmin(_config);
            AnsiConsole.Markup("[green]Authenticatiion successful\n[/]");

            AnsiConsole.Markup("[green]Retreving imported program phases from database\n[/]");
            List<ProgramPhase> phases = ReadAllImportedPhases();
            AnsiConsole.Markup("[green]Data retrival successful\n[/]");

            if (phases.Count > 0)
            {
                DeleteProgramPhases(trainerAuthDetails, phases);
                AnsiConsole.Markup("[green]Deletion sucessful\n[/]");

                return true;
            }

            return false;
        }

        public List<ProgramPhase> ReadAllImportedPhases()
        {
            return _context.TrainingProgramPhase.Where(x => x.new_id != null).ToList();
        }

        private bool DeleteProgramPhases(AuthenticationSession trainerAuthDetails, List<ProgramPhase> phases)
        {
            bool deleteAll = true;

            AnsiConsole.Progress()
                .Columns(GetProgressColumns())
                .Start(async ctx =>
                {
                    var task = ctx.AddTask($"[green]Importing custom excersize data...[/]", autoStart: false);
                    task.MaxValue = phases.Count;
                    task.StartTask();

                    foreach (ProgramPhase phase in phases)
                    {
                        AnsiConsole.Markup("[green]Deleteing Phase " + phase.name + "\n[/]");
                        if (DeleteProgramPhaseFromTrainerize(trainerAuthDetails, phase.new_id))
                            UpdateStoredTrainingPhase(phase.new_id);
                        else
                            deleteAll = false;

                        task.Increment(1);
                    }
                    task.StopTask();
                });

            if (deleteAll)
                DeleteAllTrainingData();

            return false;
        }

        private bool DeleteProgramPhaseFromTrainerize(AuthenticationSession trainerAuthDetails, int? newPhaseId)
        {
            DeletePhaseRequest jsonBody = new DeletePhaseRequest()
            {
                closeGap = false,
                planid = newPhaseId
            };

            var authenticator = new JwtAuthenticator(trainerAuthDetails.token);
            var options = new RestClientOptions()
            {
                Authenticator = authenticator,
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            RestClient client = new RestClient(options);
            var request = new RestRequest();
            request.Resource = _config.DeletePhaseUrl();
            request.Method = Method.Post;
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            var queryResult = client.Execute(request);

            DeletePhaseResponse response = JsonConvert.DeserializeObject<DeletePhaseResponse>(queryResult.Content);

            if (response.code != "0")
                return false;

            return true;

        }

        private void DeleteAllTrainingData()
        {
            _context.Database.ExecuteSqlRaw("TRUNCATE TABLE TrainingSessionStat");
            _context.Database.ExecuteSqlRaw("TRUNCATE TABLE TrainingSessionWorkout");
            _context.Database.ExecuteSqlRaw("TRUNCATE TABLE TrainingPlanWorkout");
            _context.Database.ExecuteSqlRaw("TRUNCATE TABLE TrainingProgramPhase");
            _context.SaveChanges();
        }

        private void UpdateStoredTrainingPhase(int? phaseId)
        {
            ProgramPhase? programPhase = _context.TrainingProgramPhase.FirstOrDefault(x => x.new_id == phaseId);
            programPhase.new_id = null;
            programPhase.workoutsimported = false;
            _context.TrainingProgramPhase.Update(programPhase);
            _context.SaveChanges();
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
