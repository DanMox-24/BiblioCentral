using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBiblioteca.Data;
using SistemaBiblioteca.Models;

namespace SistemaBiblioteca.Controllers
{
    public class ReservasController : Controller
    {
        private readonly BibliotecaContext _context;

        public ReservasController(BibliotecaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Muestra la lista de reservas activas
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var reservas = await _context.Reservas
                .Include(r => r.Libro)
                .OrderByDescending(r => r.FechaReserva)
                .ToListAsync();

            return View(reservas);
        }

        /// <summary>
        /// Muestra los detalles de una reserva específica
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Libro)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        /// <summary>
        /// Formulario para crear una nueva reserva
        /// </summary>
        public IActionResult Create()
        {
            // Mostrar todos los libros (disponibles y no disponibles)
            ViewData["LibroId"] = new SelectList(_context.Libros, "Id", "Titulo");
            return View();
        }

        /// <summary>
        /// Procesa la creación de una nueva reserva
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LibroId,NombreUsuario,FechaReserva,FechaExpiracion,Estado,Observaciones")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                // Verificar si ya existe una reserva activa para este libro por el mismo usuario
                var reservaExistente = await _context.Reservas
                    .Where(r => r.LibroId == reserva.LibroId &&
                               r.NombreUsuario == reserva.NombreUsuario &&
                               r.Estado == "Activa")
                    .FirstOrDefaultAsync();

                if (reservaExistente != null)
                {
                    TempData["ErrorMessage"] = "Ya existe una reserva activa de este libro para el mismo usuario.";
                    ViewData["LibroId"] = new SelectList(_context.Libros, "Id", "Titulo", reserva.LibroId);
                    return View(reserva);
                }

                _context.Add(reserva);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Reserva creada exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["LibroId"] = new SelectList(_context.Libros, "Id", "Titulo", reserva.LibroId);
            return View(reserva);
        }

        /// <summary>
        /// Formulario para editar una reserva existente
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            ViewData["LibroId"] = new SelectList(_context.Libros, "Id", "Titulo", reserva.LibroId);
            return View(reserva);
        }

        /// <summary>
        /// Procesa la edición de una reserva
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LibroId,NombreUsuario,FechaReserva,FechaExpiracion,Estado,Observaciones")] Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Reserva actualizada exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["LibroId"] = new SelectList(_context.Libros, "Id", "Titulo", reserva.LibroId);
            return View(reserva);
        }

        /// <summary>
        /// Confirmación para eliminar una reserva
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Libro)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        /// <summary>
        /// Procesa la eliminación de una reserva
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Reserva eliminada exitosamente.";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Convierte una reserva en préstamo
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ConvertirAPrestamo(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Libro)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
            {
                TempData["ErrorMessage"] = "Reserva no encontrada.";
                return RedirectToAction(nameof(Index));
            }

            if (reserva.Estado != "Activa")
            {
                TempData["ErrorMessage"] = "Solo se pueden convertir reservas activas.";
                return RedirectToAction(nameof(Index));
            }

            if (reserva.Libro != null && !reserva.Libro.Disponible)
            {
                TempData["ErrorMessage"] = "El libro no está disponible para préstamo.";
                return RedirectToAction(nameof(Index));
            }

            // Crear el préstamo
            var nuevoPrestamo = new Prestamo
            {
                LibroId = reserva.LibroId,
                NombreUsuario = reserva.NombreUsuario,
                FechaPrestamo = DateTime.Today,
                FechaDevolucion = DateTime.Today.AddDays(30),
                Estado = "Activo",
                Observaciones = $"Convertido desde reserva #{reserva.Id}"
            };

            // Actualizar el estado de la reserva
            reserva.Estado = "Convertida";

            // Marcar el libro como no disponible
            if (reserva.Libro != null)
            {
                reserva.Libro.Disponible = false;
                _context.Update(reserva.Libro);
            }

            _context.Add(nuevoPrestamo);
            _context.Update(reserva);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Reserva convertida a préstamo exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Cancela una reserva activa
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CancelarReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                TempData["ErrorMessage"] = "Reserva no encontrada.";
                return RedirectToAction(nameof(Index));
            }

            if (reserva.Estado != "Activa")
            {
                TempData["ErrorMessage"] = "Solo se pueden cancelar reservas activas.";
                return RedirectToAction(nameof(Index));
            }

            reserva.Estado = "Cancelada";
            _context.Update(reserva);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Reserva cancelada exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Verifica si una reserva existe
        /// </summary>
        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }
    }
}