using AutoMapper;
using DoDo.Data.Entities;
using DoDo.DTOs;
using DoDo.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DoDo.Features.Todo
{
    public class Update
    {
        public record Command(int Id, CreateTodoDto Todo) : IRequest<TodoDto>;

        public class Handler : IRequestHandler<Command, TodoDto>
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

            public async Task<TodoDto> Handle(Command request, CancellationToken cancellationToken)
            {
                // Obtener el userId del token
                var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null) throw new UnauthorizedAccessException("No autorizado");

                var userId = int.Parse(userIdClaim.Value);

                // Buscar la tarea y verificar que le pertenezca
                var todoExistente = await _repo.GetByIdAsync(request.Id, userId);
                if (todoExistente == null) throw new UnauthorizedAccessException("No tienes permiso para editar esta tarea.");

                // Aplicar los cambios
                _mapper.Map(request.Todo, todoExistente);
                await _repo.UpdateAsync(todoExistente);

                return _mapper.Map<TodoDto>(todoExistente);
            }
        }
    }
}

