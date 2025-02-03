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
        //pull out phases
        //pull out workout plans
        //import phases into new system
        //import workout plans into new system
    }
}
