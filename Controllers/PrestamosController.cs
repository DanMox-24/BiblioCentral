using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBiblioteca.Data;
using SistemaBiblioteca.Models;

namespace SistemaBiblioteca.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly BibliotecaContext _context;

        public PrestamosController(BibliotecaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Muestra la tabla de historial de préstamos
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var prestamos = await _context.Prestamos
                .Include(p => p.Libro)
                .OrderByDescending(p => p.FechaPrestamo)
                .ToListAsync();

            return View(prestamos);
        }

        /// <summary>
        /// Muestra los detalles de un préstamo específico
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos
                .Include(p => p.Libro)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        /// <summary>
        /// Formulario para crear un nuevo préstamo
        /// </summary>
        public IActionResult Create()
        {
            // Solo mostrar libros disponibles
            ViewData["LibroId"] = new SelectList(
                _context.Libros.Where(l => l.Disponible),
                "Id",
                "Titulo"
            );
            return View();
        }

        /// <summary>
        /// Procesa la creación de un nuevo préstamo
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LibroId,NombreUsuario,FechaPrestamo,FechaDevolucion,Estado,Observaciones")] Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                // Verificar que el libro esté disponible
                var libro = await _context.Libros.FindAsync(prestamo.LibroId);
                if (libro != null && libro.Disponible)
                {
                    // Crear el préstamo
                    _context.Add(prestamo);

                    // Marcar el libro como no disponible
                    libro.Disponible = false;
                    _context.Update(libro);

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Préstamo registrado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "El libro seleccionado no está disponible.";
                }
            }

            ViewData["LibroId"] = new SelectList(
                _context.Libros.Where(l => l.Disponible),
                "Id",
                "Titulo",
                prestamo.LibroId
            );
            return View(prestamo);
        }

        /// <summary>
        /// Formulario para editar un préstamo existente
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }

            ViewData["LibroId"] = new SelectList(_context.Libros, "Id", "Titulo", prestamo.LibroId);
            return View(prestamo);
        }

        /// <summary>
        /// Procesa la edición de un préstamo
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LibroId,NombreUsuario,FechaPrestamo,FechaDevolucion,Estado,Observaciones")] Prestamo prestamo)
        {
            if (id != prestamo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Obtener el préstamo original para comparar estados
                    var prestamoOriginal = await _context.Prestamos.AsNoTracking()
                        .FirstOrDefaultAsync(p => p.Id == id);

                    _context.Update(prestamo);

                    // Si el estado cambió a "Devuelto", marcar el libro como disponible
                    if (prestamoOriginal?.Estado != "Devuelto" && prestamo.Estado == "Devuelto")
                    {
                        var libro = await _context.Libros.FindAsync(prestamo.LibroId);
                        if (libro != null)
                        {
                            libro.Disponible = true;
                            _context.Update(libro);
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Préstamo actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrestamoExists(prestamo.Id))
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

            ViewData["LibroId"] = new SelectList(_context.Libros, "Id", "Titulo", prestamo.LibroId);
            return View(prestamo);
        }

        /// <summary>
        /// Confirmación para eliminar un préstamo
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos
                .Include(p => p.Libro)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        /// <summary>
        /// Procesa la eliminación de un préstamo
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo != null)
            {
                // Si el préstamo estaba activo, marcar el libro como disponible
                if (prestamo.Estado == "Activo")
                {
                    var libro = await _context.Libros.FindAsync(prestamo.LibroId);
                    if (libro != null)
                    {
                        libro.Disponible = true;
                        _context.Update(libro);
                    }
                }

                _context.Prestamos.Remove(prestamo);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Préstamo eliminado exitosamente.";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Acción para marcar un préstamo como devuelto
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> MarcarDevuelto(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
            {
                TempData["ErrorMessage"] = "Préstamo no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            if (prestamo.Estado != "Activo")
            {
                TempData["ErrorMessage"] = "Solo se pueden marcar como devueltos los préstamos activos.";
                return RedirectToAction(nameof(Index));
            }

            // Actualizar el estado del préstamo
            prestamo.Estado = "Devuelto";
            prestamo.FechaDevolucion = DateTime.Today;

            // Marcar el libro como disponible
            var libro = await _context.Libros.FindAsync(prestamo.LibroId);
            if (libro != null)
            {
                libro.Disponible = true;
                _context.Update(libro);
            }

            _context.Update(prestamo);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Préstamo marcado como devuelto exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Verifica si un préstamo existe
        /// </summary>
        private bool PrestamoExists(int id)
        {
            return _context.Prestamos.Any(e => e.Id == id);
        }
    }
}