using SistemaBiblioteca.Models;

namespace SistemaBiblioteca.Data
{
    public static class SeedData
    {
        public static void Initialize(BibliotecaContext context)
        {
            // Asegurar que la base de datos existe
            context.Database.EnsureCreated();

            // Si ya hay datos, no hacer nada
            if (context.Libros.Any())
            {
                return;
            }

            // Crear libros de prueba
            var libros = new Libro[]
            {
                new() {
                    Titulo = "Cien Años de Soledad",
                    Autor = "Gabriel García Márquez",
                    ISBN = "978-0060883287",
                    Genero = "Realismo Mágico",
                    AnioPublicacion = 1967,
                    Descripcion = "Una obra maestra del realismo mágico que narra la historia de la familia Buendía.",
                    ImagenUrl = "book1.jpg",
                    Disponible = true
                },
                new() {
                    Titulo = "Don Quijote de la Mancha",
                    Autor = "Miguel de Cervantes",
                    ISBN = "978-8420412146",
                    Genero = "Clásico",
                    AnioPublicacion = 1605,
                    Descripcion = "La historia del ingenioso hidalgo Don Quijote de la Mancha.",
                    ImagenUrl = "book2.jpg",
                    Disponible = true
                },
                new() {
                    Titulo = "El Amor en los Tiempos del Cólera",
                    Autor = "Gabriel García Márquez",
                    ISBN = "978-0307387581",
                    Genero = "Romance",
                    AnioPublicacion = 1985,
                    Descripcion = "Una historia de amor que perdura a través del tiempo.",
                    ImagenUrl = "book3.jpg",
                    Disponible = false
                },
                new() {
                    Titulo = "La Casa de los Espíritus",
                    Autor = "Isabel Allende",
                    ISBN = "978-8497592406",
                    Genero = "Realismo Mágico",
                    AnioPublicacion = 1982,
                    Descripcion = "La saga de la familia del Valle a través de cuatro generaciones.",
                    ImagenUrl = "book4.jpg",
                    Disponible = true
                },
                new() {
                    Titulo = "Rayuela",
                    Autor = "Julio Cortázar",
                    ISBN = "978-8437604572",
                    Genero = "Experimental",
                    AnioPublicacion = 1963,
                    Descripcion = "Una novela experimental que puede leerse de múltiples formas.",
                    ImagenUrl = "book5.jpg",
                    Disponible = true
                },
                new() {
                    Titulo = "Pedro Páramo",
                    Autor = "Juan Rulfo",
                    ISBN = "978-8437505015",
                    Genero = "Realismo Mágico",
                    AnioPublicacion = 1955,
                    Descripcion = "Un viaje al pueblo fantasmal de Comala en busca del padre perdido.",
                    ImagenUrl = "book6.jpg",
                    Disponible = true
                },
                new() {
                    Titulo = "La Tregua",
                    Autor = "Mario Benedetti",
                    ISBN = "978-8420635428",
                    Genero = "Romance",
                    AnioPublicacion = 1960,
                    Descripcion = "La historia de amor entre un viudo maduro y una joven empleada.",
                    ImagenUrl = "book7.jpg",
                    Disponible = false
                },
                new() {
                    Titulo = "El Túnel",
                    Autor = "Ernesto Sabato",
                    ISBN = "978-8432248535",
                    Genero = "Psicológico",
                    AnioPublicacion = 1948,
                    Descripcion = "La obsesión amorosa de un pintor que lo lleva al crimen.",
                    ImagenUrl = "book8.jpg",
                    Disponible = true
                },
                new() {
                    Titulo = "Como Agua para Chocolate",
                    Autor = "Laura Esquivel",
                    ISBN = "978-8489669819",
                    Genero = "Realismo Mágico",
                    AnioPublicacion = 1989,
                    Descripcion = "Una novela donde la cocina y las emociones se entrelazan mágicamente.",
                    ImagenUrl = "book9.jpg",
                    Disponible = true
                },
                new() {
                    Titulo = "El Aleph",
                    Autor = "Jorge Luis Borges",
                    ISBN = "978-8420635590",
                    Genero = "Fantástico",
                    AnioPublicacion = 1949,
                    Descripcion = "Cuentos que exploran los laberintos de la realidad y la ficción.",
                    ImagenUrl = "book10.jpg",
                    Disponible = true
                },
                new() {
                    Titulo = "María",
                    Autor = "Jorge Isaacs",
                    ISBN = "978-8437633589",
                    Genero = "Romántico",
                    AnioPublicacion = 1867,
                    Descripcion = "Una historia de amor trágico en el Valle del Cauca.",
                    ImagenUrl = "book11.jpg",
                    Disponible = true
                },
                new() {
                    Titulo = "Los Pasos Perdidos",
                    Autor = "Alejo Carpentier",
                    ISBN = "978-8437505572",
                    Genero = "Realismo Mágico",
                    AnioPublicacion = 1953,
                    Descripcion = "Un músico emprende un viaje hacia el origen de la música.",
                    ImagenUrl = "book12.jpg",
                    Disponible = false
                },
                new() {
                    Titulo = "La Vorágine",
                    Autor = "José Eustasio Rivera",
                    ISBN = "978-8437635484",
                    Genero = "Aventura",
                    AnioPublicacion = 1924,
                    Descripcion = "La lucha del hombre contra la selva amazónica.",
                    ImagenUrl = "book13.jpg",
                    Disponible = true
                },
                new() {
                    Titulo = "Doña Bárbara",
                    Autor = "Rómulo Gallegos",
                    ISBN = "978-8437635491",
                    Genero = "Regionalista",
                    AnioPublicacion = 1929,
                    Descripcion = "La lucha entre la civilización y la barbarie en los llanos venezolanos.",
                    ImagenUrl = "book14.jpg",
                    Disponible = true
                },
                new() {
                    Titulo = "El Señor Presidente",
                    Autor = "Miguel Ángel Asturias",
                    ISBN = "978-8437635507",
                    Genero = "Político",
                    AnioPublicacion = 1946,
                    Descripcion = "Un retrato de la dictadura en América Latina.",
                    ImagenUrl = "book15.jpg",
                    Disponible = true
                }
            };

            context.Libros.AddRange(libros);
            context.SaveChanges();

            // Crear préstamos de prueba
            var prestamos = new Prestamo[]
            {
                new() {
                    LibroId = 3, // El Amor en los Tiempos del Cólera
                    NombreUsuario = "Ana García",
                    FechaPrestamo = DateTime.Now.AddDays(-15),
                    FechaDevolucion = DateTime.Now.AddDays(15),
                    Estado = "Activo"
                },
                new() {
                    LibroId = 7, // La Tregua
                    NombreUsuario = "Carlos Mendez",
                    FechaPrestamo = DateTime.Now.AddDays(-10),
                    FechaDevolucion = DateTime.Now.AddDays(20),
                    Estado = "Activo"
                },
                new() {
                    LibroId = 12, // Los Pasos Perdidos
                    NombreUsuario = "María López",
                    FechaPrestamo = DateTime.Now.AddDays(-25),
                    FechaDevolucion = DateTime.Now.AddDays(-5),
                    Estado = "Vencido"
                },
                new() {
                    LibroId = 1, // Cien Años de Soledad (devuelto)
                    NombreUsuario = "Pedro Ramírez",
                    FechaPrestamo = DateTime.Now.AddDays(-30),
                    FechaDevolucion = DateTime.Now.AddDays(-10),
                    Estado = "Devuelto"
                },
                new() {
                    LibroId = 5, // Rayuela (devuelto)
                    NombreUsuario = "Laura Fernández",
                    FechaPrestamo = DateTime.Now.AddDays(-40),
                    FechaDevolucion = DateTime.Now.AddDays(-20),
                    Estado = "Devuelto"
                }
            };

            context.Prestamos.AddRange(prestamos);
            context.SaveChanges();

            // Crear reservas de prueba
            var reservas = new Reserva[]
            {
                new() {
                    LibroId = 1, // Cien Años de Soledad
                    NombreUsuario = "Roberto Silva",
                    FechaReserva = DateTime.Now.AddDays(-2),
                    FechaExpiracion = DateTime.Now.AddDays(5),
                    Estado = "Activa"
                },
                new() {
                    LibroId = 4, // La Casa de los Espíritus
                    NombreUsuario = "Carmen Torres",
                    FechaReserva = DateTime.Now.AddDays(-1),
                    FechaExpiracion = DateTime.Now.AddDays(6),
                    Estado = "Activa"
                },
                new() {
                    LibroId = 8, // El Túnel
                    NombreUsuario = "Diego Morales",
                    FechaReserva = DateTime.Now,
                    FechaExpiracion = DateTime.Now.AddDays(7),
                    Estado = "Activa"
                },
                new() {
                    LibroId = 10, // El Aleph
                    NombreUsuario = "Elena Vargas",
                    FechaReserva = DateTime.Now.AddDays(-5),
                    FechaExpiracion = DateTime.Now.AddDays(2),
                    Estado = "Activa"
                },
                new() {
                    LibroId = 2, // Don Quijote (expirada)
                    NombreUsuario = "Francisco Ruiz",
                    FechaReserva = DateTime.Now.AddDays(-10),
                    FechaExpiracion = DateTime.Now.AddDays(-3),
                    Estado = "Expirada"
                }
            };

            context.Reservas.AddRange(reservas);
            context.SaveChanges();
        }
    }
}