using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Domain.Entities.Report
{
    public  class BaseEntity<T>
    {
        public T Id { get; set; }
    }
}
