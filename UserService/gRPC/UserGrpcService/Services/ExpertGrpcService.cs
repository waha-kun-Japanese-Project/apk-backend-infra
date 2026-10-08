using ExpertService.Grpc;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using User.Persistence.Context;

namespace UserGrpcService.Services;

public class ExpertGrpcService : ExpertService.Grpc.ExpertService.ExpertServiceBase
{
    private readonly AppDbContext _context;

    public ExpertGrpcService(AppDbContext context)
    {
        _context = context;
    }
    public override async Task<ExpertIdsRespone> GetAllExpert(Empty request, ServerCallContext context)
    {
        var expertIds = await(
           from user in _context.Users

           join userRole in _context.UserRoles
               on user.Id equals userRole.UserId

           join role in _context.Roles
               on userRole.RoleId equals role.Id

           where role.Name == "EXPERT"

           select user.Id
       )
       .ToListAsync(context.CancellationToken);

        var response = new ExpertIdsRespone();
        
        response.ExpertIds.AddRange(
                    expertIds.Select(id => id.ToString()));


        return response;
    }
    public override async Task<GetExpertsResponse> GetExpertsByIds(
        GetExpertsRequest request,
        ServerCallContext context)
    {
        var userIds = request.UserIds
              .Where(id => Guid.TryParse(id, out _))
                .Select(Guid.Parse)
            .Distinct()
             .ToList();

        var experts = await (
            from user in _context.Users

            join userRole in _context.UserRoles
                on user.Id equals userRole.UserId

            join role in _context.Roles
                on userRole.RoleId equals role.Id

            where userIds.Contains(user.Id)
                  && role.Name == "EXPERT"

            select new ExpertsResponse
            {
                Id = user.Id.ToString(),
                Name = user.UserName,
                PhotoUrl = user.pictures ?? string.Empty
            }
        )
        .ToListAsync(context.CancellationToken);

        var response = new GetExpertsResponse();

        response.Users.AddRange(experts);

        return response;
    }
 }
