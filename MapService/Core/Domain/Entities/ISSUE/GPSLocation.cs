using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Map.Domain.Entities.ISSUE;

    public class GPSLocation : BaseEntity<Guid>
    {
       
        public string Latitude { get; set; }
        public string Longitude { get; set; }

      public  Issue Issue { get; set; } = null!;   
    }

