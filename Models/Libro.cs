using System.ComponentModel.DataAnnotations;

namespace SistemaBiblioteca.Models
{
    public class Libro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200, ErrorMessage = "El título no puede exceder 200 caracteres")]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es obligatorio")]
        [StringLength(100, ErrorMessage = "El autor no puede exceder 100 caracteres")]
        public string Autor { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "El ISBN no puede exceder 20 caracteres")]
        public string? ISBN { get; set; }

        [StringLength(50, ErrorMessage = "El género no puede exceder 50 caracteres")]
        [Display(Name = "Género")]
        public string? Genero { get; set; }

        [Display(Name = "Año de Publicación")]
        [Range(1000, 2100, ErrorMessage = "El año debe estar entre 1000 y 2100")]
        public int? AnioPublicacion { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(1000, ErrorMessage = "La descripción no puede exceder 1000 caracteres")]
        public string? Descripcion { get; set; }

        [Display(Name = "URL de Imagen")]
        public string? ImagenUrl { get; set; }

        public bool Disponible { get; set; } = true;

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}