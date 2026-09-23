using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS.Query
{
    public class IssueQueryParameters
    {
        private const int  MAXPAGESIZE= 10;
        private const int MINPAGESIZE= 5;

        public  Guid ExpertId { get; set; }
        public SortingOptions? SortingOptions { get; set; }
  
        private int  pageSize= MINPAGESIZE;
        public int PageSize { 
            get => pageSize ;
            set=> pageSize=
                value >MAXPAGESIZE?MAXPAGESIZE
                :value <MINPAGESIZE?MINPAGESIZE:value ;
        }
        public int pageIndex { get; set; } = 1;
    }

  public  enum SortingOptions
    {
        DataAscending=1,
        DataDescending=2,
    }
}
