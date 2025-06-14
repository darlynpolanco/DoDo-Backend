using AutoMapper;
using DoDo.DTOs;
using DoDo.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DoDo.Features.Todo
{
    public class Get
    {
        public record Query() : IRequest<List<TodoDto>>;

        public class Handler : IRequestHandler<Query, List<TodoDto>>
        {
            private readonly ITodoRepository _repo;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(ITodoRepository repo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            {
                _repo = repo;
                _mapper = mapper;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<List<TodoDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = _httpContextAccessor.HttpContext?.User;

                if (user == null || !user.Identity.IsAuthenticated)
                    throw new UnauthorizedAccessException("No autorizado");

                // Buscar el userId por nombre del claim
                var userIdClaim = user.Claims.FirstOrDefault(c => c.Type.Contains("nameidentifier"));

                if (userIdClaim == null)
                    throw new UnauthorizedAccessException("No autorizado");

                var userId = int.Parse(userIdClaim.Value);

                var todos = await _repo.GetAllAsync(userId);
                return _mapper.Map<List<TodoDto>>(todos);
            }
        }
    }
}
