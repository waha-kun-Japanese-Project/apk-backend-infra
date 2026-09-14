using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS.Query
{
    public class IssueQueryParameters
    {
        public  Guid ExpertId { get; set; }
        public SortingOptions? SortingOptions { get; set; }
    }
  public  enum SortingOptions
    {
        DataAscending=1,
        DataDescending=2,
    }
}
