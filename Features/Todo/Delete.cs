using MediatR;
using DoDo.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DoDo.Features.Todo
{
    public class Delete
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
                // Obtener el userId del token
                var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null) throw new UnauthorizedAccessException("No autorizado");

                var userId = int.Parse(userIdClaim.Value);

                // Buscar la tarea del usuario
                var tarea = await _repo.GetByIdAsync(request.Id, userId);

                if (tarea == null)
                {
                    throw new UnauthorizedAccessException("No tienes permiso para eliminar esta tarea.");
                }

                await _repo.DeleteAsync(tarea);
                return Unit.Value;
            }

            Task IRequestHandler<Command>.Handle(Command request, CancellationToken cancellationToken)
            {
                return Handle(request, cancellationToken);
            }
        }
    }
}
