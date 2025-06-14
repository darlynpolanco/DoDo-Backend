namespace DoDo.DTOs
{
    public class TodoDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Contenido { get; set; } = string.Empty;
        public string Prioridad { get; set; } = "Media";
        public bool Completado { get; set; }
        public int UsuarioId { get; set; }
    }
}
