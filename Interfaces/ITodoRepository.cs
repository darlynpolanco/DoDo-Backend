using DoDo.Data.Entities;

public interface ITodoRepository
{
    Task<List<Tarea>> GetAllAsync(int userId);
    Task<Tarea?> GetByIdAsync(int id, int userId);
    Task AddAsync(Tarea todo);
    Task UpdateAsync(Tarea todo);
    Task DeleteAsync(Tarea todo);

    Task<List<Tarea>> GetByPriorityAsync(string priority, int userId);
    Task<List<Tarea>> SearchByTextAsync(string search, int userId);
    Task MarkAsCompletedAsync(int id, int userId);
    Task MarkAsUncompletedAsync(int id, int userId);
}

