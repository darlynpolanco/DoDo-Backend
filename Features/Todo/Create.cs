using AutoMapper;
using DoDo.DTOs;
using DoDo.Interfaces;
using DoDo.Data.Entities;
using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace DoDo.Features.Todo
{
    public class Create
    {
        public record Command(CreateTodoDto Todo) : IRequest<TodoDto>;

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
                var todo = _mapper.Map<Tarea>(request.Todo);

                // Obtenemos el ID del usuario autenticado desde el token
                var usuarioId = int.Parse(
                    _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value
                );

                todo.UsuarioId = usuarioId;

                await _repo.AddAsync(todo);
                return _mapper.Map<TodoDto>(todo);
            }
        }
    }
}
