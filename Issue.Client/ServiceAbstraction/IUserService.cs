using Issue.Shared.DTOS.Client;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Client.ServiceAbstraction
{
    public  interface IUserService
    {
        [Get("/api/User/expert-details")]

        Task<IEnumerable<ExpertDetailsResponse>> GetExpertIdsAsync();

    }
}
