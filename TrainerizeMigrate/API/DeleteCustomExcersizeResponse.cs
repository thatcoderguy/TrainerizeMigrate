using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainerizeMigrate.API
{
    public class DeletedExercise
    {
        public int id { get; set; }
        public bool deleted { get; set; }
    }

    public class DeleteCustomExcersizeResponse
    {
        public List<DeletedExercise> exercises { get; set; }
    }

}
