
using Map.Domain.Entities.REPORT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Map.Domain.Entities.REPORT;

public class Report : BaseEntity<Guid>
{
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public Guid ReporterId { get; set; }  // user from auth context  frammmer
    
    public Domain.Entities.REPORT.GpsLocation? Location { get; set; }
    public SeverityLevel severity { get; set; }
    public Guid? LocationId { get; set; }


    }
