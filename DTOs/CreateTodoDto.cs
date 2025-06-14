namespace DoDo.DTOs
{
    public class CreateTodoDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Contenido { get; set; } = string.Empty;
        public string Prioridad { get; set; } = "Media";
        
    }
}
