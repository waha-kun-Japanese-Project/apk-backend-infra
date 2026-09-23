using AutoMapper;
using Map.Domain.Contarcts;
using Map.ServiceAbsraction;
using Map.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Service
{
    public class MapService(IIssueRepo issueRepo,IMapper mapper ) : IMapSerevice
    {
        public async Task<MapResponseDto> SearchForIssueInMapAsync(Guid IssueId ,CancellationToken cancellationToken)
        {
            var issue = await issueRepo.GetByIdAsync(IssueId  );

            if (issue == null)
            {
                throw new KeyNotFoundException("Issue Not Found");
            }

            var result = mapper.Map<MapResponseDto>(issue);

            return result;



        }

        public async Task<IEnumerable<MapResponseDto>> ShowIssueInMapAsync(int pageSize, int page, CancellationToken cancellationToken)
        {
           var issues = await issueRepo.GetAllAsync(pageSize , page ,cancellationToken);
            //if(issues == null || !issues.Any())
            //{
            //    throw new KeyNotFoundException("No issues found");
            //}   
            var result = mapper.Map<IEnumerable<MapResponseDto>>(issues);
            return result;


        }
        public async Task<IEnumerable<MapResponseDto>> SearchForIssueByTitleInMapAsync(string title, int pageSize, int page, CancellationToken cancellationToken)
        {
            var issues = await issueRepo.GetByTitle(title, pageSize, page, cancellationToken);
            if (issues == null || !issues.Any())
            {
                throw new KeyNotFoundException("No issues found with the given title");
            }
            var result = mapper.Map<IEnumerable<MapResponseDto>>(issues);
            return result;
        }
    }
}
