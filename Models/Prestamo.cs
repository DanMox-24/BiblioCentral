using System.ComponentModel.DataAnnotations;

namespace SistemaBiblioteca.Models
{
    public class Prestamo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un libro")]
        [Display(Name = "Libro")]
        public int LibroId { get; set; }

        [Required(ErrorMessage = "El nombre del usuario es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        [Display(Name = "Nombre del Usuario")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de préstamo es obligatoria")]
        [Display(Name = "Fecha de Préstamo")]
        [DataType(DataType.Date)]
        public DateTime FechaPrestamo { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "La fecha de devolución es obligatoria")]
        [Display(Name = "Fecha de Devolución")]
        [DataType(DataType.Date)]
        public DateTime FechaDevolucion { get; set; } = DateTime.Today.AddDays(30);

        [Required(ErrorMessage = "El estado es obligatorio")]
        [StringLength(20, ErrorMessage = "El estado no puede exceder 20 caracteres")]
        public string Estado { get; set; } = "Activo";

        public string? Observaciones { get; set; }

        // Propiedad de navegación
        public virtual Libro? Libro { get; set; }

        // Propiedad calculada para determinar si está vencido
        public bool EstaVencido => Estado == "Activo" && DateTime.Today > FechaDevolucion;
    }
}