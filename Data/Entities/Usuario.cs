using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DoDo.Data.Entities
{
        [Table("Usuarios")]
        public class Usuario
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Required]
            [StringLength(100)]
            public string? Nombre { get; set; }

            [Required]
            [EmailAddress]
            [StringLength(100)]
            public string? CorreoElectronico { get; set; }

            [Required]
            [StringLength(100)]
            public string? Contrasena { get; set; } // Aquí almacenaremos el hash

            [Required]
            public DateTime FechaRegistro { get; set; } = DateTime.Now;

            [Required]
            public bool Activo { get; set; } = true;
        }
    }

