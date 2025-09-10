using System.ComponentModel.DataAnnotations;

namespace SistemaBiblioteca.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un libro")]
        [Display(Name = "Libro")]
        public int LibroId { get; set; }

        [Required(ErrorMessage = "El nombre del usuario es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        [Display(Name = "Nombre del Usuario")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de reserva es obligatoria")]
        [Display(Name = "Fecha de Reserva")]
        [DataType(DataType.Date)]
        public DateTime FechaReserva { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "La fecha de expiración es obligatoria")]
        [Display(Name = "Fecha de Expiración")]
        [DataType(DataType.Date)]
        public DateTime FechaExpiracion { get; set; } = DateTime.Today.AddDays(7);

        [Required(ErrorMessage = "El estado es obligatorio")]
        [StringLength(20, ErrorMessage = "El estado no puede exceder 20 caracteres")]
        public string Estado { get; set; } = "Activa";

        public string? Observaciones { get; set; }

        // Propiedad de navegación
        public virtual Libro? Libro { get; set; }

        // Propiedad calculada para determinar si está expirada
        public bool EstaExpirada => Estado == "Activa" && DateTime.Today > FechaExpiracion;
    }
}