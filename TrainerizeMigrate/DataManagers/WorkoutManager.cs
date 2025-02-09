using RestSharp.Authenticators;
using RestSharp;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TrainerizeMigrate.API;
using TrainerizeMigrate.Migrations;
using TrainerizeMigrate.Data;
using System.Linq.Expressions;
using System.Text.Json.Nodes;

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
            request.AddJsonBody(jsonBody, ContentType.Json);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var queryResult = client.Execute(request);

            TrainingProgramListResponse response = null;

            try
            {
                response = JsonSerializer.Deserialize<TrainingProgramListResponse>(queryResult.Content);
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
            request.AddJsonBody(jsonBody, ContentType.Json);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var queryResult = client.Execute(request);

            ProgramPhasesResponse response = null;

            try
            {
                response = JsonSerializer.Deserialize<ProgramPhasesResponse>(queryResult.Content);
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup("[red]Error: " + ex.Message + "\n[/]");
                return null;
            }

            return response;
        }

        private bool StoreProgramPhases(ProgramPhasesResponse programPhases)
        {
            foreach (Plan phase in programPhases.plans)
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
                    var task = ctx.AddTask($"[green]Importing custom excersize data...[/]", autoStart: false);
                    task.MaxValue = phases.Count;
                    task.StartTask();

                    foreach (ProgramPhase phase in phases)
                    {
                        AnsiConsole.Markup("[green]Adding Phase " + phase.name + "\n[/]");
                        int? newPhaseId = AddProgramPhase(authDetails, phase, clientID);
                        if (newPhaseId is not null)
                            UpdatePhase(phase.id, newPhaseId);

                        task.Increment(1);
                    }
                    task.StopTask();
                });

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
            request.AddJsonBody(jsonBody, ContentType.Json);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            RestResponse queryResult = client.Execute(request);

            AddCustomExcersizeResponse response = null;

            try
            {
                response = JsonSerializer.Deserialize<AddCustomExcersizeResponse>(queryResult.Content);
                if (response.id == 0)
                    return null;

            }
            catch (Exception ex)
            {
                AnsiConsole.Markup("[red]Error: " + ex.Message + "\n[/]");
                return null;
            }

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
            request.AddJsonBody(jsonBody, ContentType.Json);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var queryResult = client.Execute(request);

            TrainingProgramListResponse response = null;

            try
            {
                response = JsonSerializer.Deserialize<TrainingProgramListResponse>(queryResult.Content);
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup("[red]Error: " + ex.Message + "\n[/]");
                return null;
            }

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

            AnsiConsole.Markup("[green]Pulling workouts for phase from trainerize\n[/]");
            List<PhaseWorkoutPlansResponse> phasedWorkouts = GetWorkoutsForPhases(authDetails, phases);
            AnsiConsole.Markup("[green]Data retreieved successfully\n[/]");

            AnsiConsole.Markup("[green]Storing training programs into database\n[/]");
            StorePhasedWorkouts(phasedWorkouts);
            AnsiConsole.Markup("[green]Data storage successful\n[/]");
        }

        private void StorePhasedWorkouts(List<PhaseWorkoutPlansResponse> phasedWorkouts)
        {
            foreach(PhaseWorkoutPlansResponse phaseWorkoutPlansResponse in phasedWorkouts)
            {
                //store workout

                //loop through excersizes
                    //store and link excersizes
            }
        }

        private List<ProgramPhase> ReadAllPhasesWithoutImportedWorkouts()
        {
            return _context.TrainingProgramPhase.Where(x => x.new_id != null && x.workoutsimported == false).ToList();
        }

        private List<PhaseWorkoutPlansResponse> GetWorkoutsForPhases(AuthenticationSession authDetails, List<ProgramPhase> phases)
        {
            List<PhaseWorkoutPlansResponse> phaseWorkoutPlansResponses = new List<PhaseWorkoutPlansResponse>();

            AnsiConsole.Progress()
                .Columns(GetProgressColumns())
                .Start(async ctx =>
                {
                    var task = ctx.AddTask($"[green]Pulling phase workouts...[/]", autoStart: false);
                    task.MaxValue = phases.Count;
                    task.StartTask();

                    foreach (ProgramPhase phase in phases)
                    {
                        AnsiConsole.Markup("[green]Pulling workouts for phase: " + phase.name + "\n[/]");
                        PhaseWorkoutPlansResponse phaseWorkouts = PullPhaseWorkouts(authDetails, phase.id);

                        phaseWorkoutPlansResponses.Add(phaseWorkouts);

                        task.Increment(1);
                    }
                    task.StopTask();
                });

            return phaseWorkoutPlansResponses;
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
            request.AddJsonBody(jsonBody, ContentType.Json);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var queryResult = client.Execute(request);

            PhaseWorkoutPlansResponse response = null;

            try
            {
                response = JsonSerializer.Deserialize<PhaseWorkoutPlansResponse>(queryResult.Content);
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup("[red]Error: " + ex.Message + "\n[/]");
                return null;
            }


            return response;
        }

        public void ImportWorkoutPlans()
        {
            throw new NotImplementedException();
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
                        if(DeleteProgramPhaseFromTrainerize(trainerAuthDetails, phase.new_id))
                            UpdateStoredTrainingPhase(phase.new_id);

                        task.Increment(1);
                    }
                    task.StopTask();
                });

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
            request.AddJsonBody(jsonBody, ContentType.Json);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var queryResult = client.Execute(request);

            DeletePhaseResponse response = null;

            try
            {
                response = JsonSerializer.Deserialize<DeletePhaseResponse>(queryResult.Content);
                if (response.code != "0")
                    return false;
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup("[red]Error: " + ex.Message + "\n[/]");
                return false;
            }

            return true;

        }

        private void UpdateStoredTrainingPhase(int? phaseId)
        {
            ProgramPhase programPhase = _context.TrainingProgramPhase.FirstOrDefault(x => x.new_id == phaseId);
            programPhase.new_id = null;
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
