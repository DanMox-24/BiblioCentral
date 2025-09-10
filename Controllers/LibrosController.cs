using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBiblioteca.Data;
using SistemaBiblioteca.Models;

namespace SistemaBiblioteca.Controllers
{
    public class LibrosController : Controller
    {
        private readonly BibliotecaContext _context;

        public LibrosController(BibliotecaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista todos los libros con funcionalidad de búsqueda
        /// Muestra tarjetas de libros con imagen, título, autor y botones de acción
        /// </summary>
        public async Task<IActionResult> Index(string busqueda)
        {
            var libros = from l in _context.Libros
                         select l;

            // Aplicar filtro de búsqueda si se proporciona
            if (!string.IsNullOrEmpty(busqueda))
            {
                libros = libros.Where(l => l.Titulo.Contains(busqueda) ||
                                         l.Autor.Contains(busqueda));
            }

            ViewData["BusquedaActual"] = busqueda;
            return View(await libros.OrderBy(l => l.Titulo).ToListAsync());
        }

        /// <summary>
        /// Muestra los detalles de un libro específico
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros.FirstOrDefaultAsync(m => m.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        /// <summary>
        /// Formulario para crear un nuevo libro
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Procesa la creación de un nuevo libro
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Autor,ISBN,Genero,AnioPublicacion,Descripcion,ImagenUrl,Disponible")] Libro libro)
        {
            if (ModelState.IsValid)
            {
                libro.FechaCreacion = DateTime.Now;
                _context.Add(libro);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "El libro ha sido creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        /// <summary>
        /// Formulario para editar un libro existente
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            return View(libro);
        }

        /// <summary>
        /// Procesa la edición de un libro
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Autor,ISBN,Genero,AnioPublicacion,Descripcion,ImagenUrl,Disponible,FechaCreacion")] Libro libro)
        {
            if (id != libro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libro);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "El libro ha sido actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.Id))
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
            return View(libro);
        }

        /// <summary>
        /// Confirmación para eliminar un libro
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros.FirstOrDefaultAsync(m => m.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        /// <summary>
        /// Procesa la eliminación de un libro
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                _context.Libros.Remove(libro);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "El libro ha sido eliminado exitosamente.";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Método para crear una reserva directamente desde la vista de libros
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CrearReserva(int libroId, string nombreUsuario)
        {
            if (string.IsNullOrEmpty(nombreUsuario))
            {
                TempData["ErrorMessage"] = "El nombre del usuario es obligatorio para crear una reserva.";
                return RedirectToAction(nameof(Index));
            }

            var libro = await _context.Libros.FindAsync(libroId);
            if (libro == null)
            {
                TempData["ErrorMessage"] = "El libro seleccionado no existe.";
                return RedirectToAction(nameof(Index));
            }

            // Verificar si ya existe una reserva activa para este libro por el mismo usuario
            var reservaExistente = await _context.Reservas
                .Where(r => r.LibroId == libroId && r.NombreUsuario == nombreUsuario && r.Estado == "Activa")
                .FirstOrDefaultAsync();

            if (reservaExistente != null)
            {
                TempData["ErrorMessage"] = "Ya tienes una reserva activa para este libro.";
                return RedirectToAction(nameof(Index));
            }

            var nuevaReserva = new Reserva
            {
                LibroId = libroId,
                NombreUsuario = nombreUsuario,
                FechaReserva = DateTime.Today,
                FechaExpiracion = DateTime.Today.AddDays(7),
                Estado = "Activa"
            };

            _context.Reservas.Add(nuevaReserva);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Reserva creada exitosamente para '{libro.Titulo}'.";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Método para crear un préstamo directamente desde la vista de libros
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CrearPrestamo(int libroId, string nombreUsuario)
        {
            if (string.IsNullOrEmpty(nombreUsuario))
            {
                TempData["ErrorMessage"] = "El nombre del usuario es obligatorio para crear un préstamo.";
                return RedirectToAction(nameof(Index));
            }

            var libro = await _context.Libros.FindAsync(libroId);
            if (libro == null)
            {
                TempData["ErrorMessage"] = "El libro seleccionado no existe.";
                return RedirectToAction(nameof(Index));
            }

            if (!libro.Disponible)
            {
                TempData["ErrorMessage"] = "El libro no está disponible para préstamo.";
                return RedirectToAction(nameof(Index));
            }

            // Verificar si el usuario ya tiene un préstamo activo de este libro
            var prestamoExistente = await _context.Prestamos
                .Where(p => p.LibroId == libroId && p.NombreUsuario == nombreUsuario && p.Estado == "Activo")
                .FirstOrDefaultAsync();

            if (prestamoExistente != null)
            {
                TempData["ErrorMessage"] = "Ya tienes un préstamo activo de este libro.";
                return RedirectToAction(nameof(Index));
            }

            var nuevoPrestamo = new Prestamo
            {
                LibroId = libroId,
                NombreUsuario = nombreUsuario,
                FechaPrestamo = DateTime.Today,
                FechaDevolucion = DateTime.Today.AddDays(30),
                Estado = "Activo"
            };

            // Marcar el libro como no disponible
            libro.Disponible = false;

            _context.Prestamos.Add(nuevoPrestamo);
            _context.Update(libro);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Préstamo creado exitosamente para '{libro.Titulo}'.";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Verifica si un libro existe
        /// </summary>
        private bool LibroExists(int id)
        {
            return _context.Libros.Any(e => e.Id == id);
        }
    }
}