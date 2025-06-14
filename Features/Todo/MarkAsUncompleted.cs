using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DoDo.Features.Todo
{
    public class MarkAsUncompleted
    {
        public record Command(int Id) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly ITodoRepository _repo;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(ITodoRepository repo, IHttpContextAccessor httpContextAccessor)
            {
                _repo = repo;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = _httpContextAccessor.HttpContext?.User;

                if (user == null || !user.Identity.IsAuthenticated)
                    throw new UnauthorizedAccessException("No autorizado");

                var userIdClaim = user.Claims.FirstOrDefault(c => c.Type.Contains("nameidentifier"));

                if (userIdClaim == null)
                    throw new UnauthorizedAccessException("No autorizado");

                var userId = int.Parse(userIdClaim.Value);

                await _repo.MarkAsUncompletedAsync(request.Id, userId);
                return Unit.Value;
            }

            Task IRequestHandler<Command>.Handle(Command request, CancellationToken cancellationToken)
            {
                return Handle(request, cancellationToken);
            }
        }
    }
}
