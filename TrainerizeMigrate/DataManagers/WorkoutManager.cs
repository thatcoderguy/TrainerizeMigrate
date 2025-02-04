using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainerizeMigrate.Migrations;

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

        public void ExtractAndStoreTrainingPrograms()
        {

        }
        public void ImportTrainingPrograms()
        {

        }
        public void ExtractAndStoreTrainingProgramPhases()
        {

        }

        public void ImportTrainingProgramPhases()
        {

        }

        public void ExtractAndStoreWorkouts()
        {

        }

        public void ImportWorkouts()
        {

        }

        public void ExtractAndStoreWorkoutPlans()
        {

        }

        public void ImportWorkoutPlans()
        {

        }
    }
}
