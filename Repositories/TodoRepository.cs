using DoDo.Interfaces;
using DoDo.Data;
using DoDo.Data.Entities;
using Microsoft.EntityFrameworkCore;

public class TodoRepository : ITodoRepository
{
    private readonly ApplicationDbContext _context;

    public TodoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Tarea>> GetAllAsync(int userId)
    {
        return await _context.Tareas
            .Where(t => t.UsuarioId == userId)
            .ToListAsync();
    }

    public async Task<Tarea?> GetByIdAsync(int id, int userId)
    {
        return await _context.Tareas
            .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == userId);
    }

    public async Task AddAsync(Tarea todo)
    {
        _context.Tareas.Add(todo);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tarea todo)
    {
        _context.Tareas.Update(todo);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Tarea todo)
    {
        _context.Tareas.Remove(todo);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Tarea>> GetByPriorityAsync(string priority, int userId)
    {
        return await _context.Tareas
            .Where(t => t.UsuarioId == userId && t.Prioridad == priority)
            .ToListAsync();
    }

    public async Task<List<Tarea>> SearchByTextAsync(string search, int userId)
    {
        return await _context.Tareas
            .Where(t => t.UsuarioId == userId &&
                        (t.Titulo.Contains(search) || t.Contenido.Contains(search)))
            .ToListAsync();
    }

    public async Task MarkAsCompletedAsync(int id, int userId)
    {
        var tarea = await _context.Tareas
            .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == userId);

        if (tarea == null)
            throw new KeyNotFoundException();

        tarea.Completado = true;
        await _context.SaveChangesAsync();
    }

    public async Task MarkAsUncompletedAsync(int id, int userId)
    {
        var tarea = await _context.Tareas
            .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == userId);

        if (tarea == null)
            throw new KeyNotFoundException();

        tarea.Completado = false;
        await _context.SaveChangesAsync();
    }
}
