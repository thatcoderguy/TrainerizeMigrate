using Spectre.Console;
using TrainerizeMigrate.API;
using TrainerizeMigrate.Migrations;

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
            return null;
        }

        private bool StoreTrainingSessionWorkouts(WorkoutTrainingSessionListReponse trainingSessionWorkouts)
        {
            return false;
        }

        //pull out training data for each workout
        //import into new system
    }
}
