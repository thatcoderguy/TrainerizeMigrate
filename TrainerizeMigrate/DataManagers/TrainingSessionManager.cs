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
        //pull out training data for each workout
        //import into new system
    }
}
