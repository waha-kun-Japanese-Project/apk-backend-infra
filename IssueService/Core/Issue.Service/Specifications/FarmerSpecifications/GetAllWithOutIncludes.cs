using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.Specifications.FarmerSpecifications
{
    internal class GetAllWithOutIncludes : BaseSpecification<Issue.Domain.Entities.Issue.Comment>
    {
        public GetAllWithOutIncludes() : base(null!)
        {

        }
    }
}
