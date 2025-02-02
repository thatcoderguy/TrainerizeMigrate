using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainerizeMigrate.Data;

namespace TrainerizeMigrate.Migrations
{
    public class ApplicationDbContext : DbContext
    {
        //public virtual DbSet<TrainerizeLoginModel> TrainerizeLogin { get; set; }

        public virtual DbSet<BodyWeight> Body_Weight { get; set; }
        public virtual DbSet<WeightPoint> Body_Weight_Point { get; set; }

        public string DbPath { get; }

        public ApplicationDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "trainerize" + Guid.NewGuid().ToString() + ".db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");

    }
}
