// Funciones JavaScript para el Sistema de Biblioteca

// Inicialización cuando se carga el DOM
document.addEventListener('DOMContentLoaded', function () {
    // Auto-dismiss alerts después de 5 segundos
    const alerts = document.querySelectorAll('.alert-dismissible');
    alerts.forEach(function (alert) {
        setTimeout(function () {
            const bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        }, 5000);
    });

    // Inicializar tooltips
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Validación en tiempo real para formularios
    const forms = document.querySelectorAll('form');
    forms.forEach(function (form) {
        form.addEventListener('submit', function (event) {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        });
    });
});

// Función para confirmar eliminación
function confirmarEliminacion(mensaje) {
    return confirm(mensaje || '¿Estás seguro de que deseas eliminar este elemento?');
}

// Función para mostrar loading en botones
function mostrarLoading(button) {
    const originalText = button.innerHTML;
    button.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Procesando...';
    button.disabled = true;

    // Restaurar después de 3 segundos (fallback)
    setTimeout(function () {
        button.innerHTML = originalText;
        button.disabled = false;
    }, 3000);
}

// Función para validar fechas
function validarFechas(fechaInicio, fechaFin) {
    const inicio = new Date(fechaInicio);
    const fin = new Date(fechaFin);

    if (fin <= inicio) {
        alert('La fecha de fin debe ser posterior a la fecha de inicio.');
        return false;
    }
    return true;
}

// Función para formatear fechas
function formatearFecha(fecha) {
    const opciones = {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    };
    return new Date(fecha).toLocaleDateString('es-ES', opciones);
}

// Función para búsqueda en tiempo real (opcional)
function busquedaEnTiempoReal(input, url) {
    let timeout;
    input.addEventListener('input', function () {
        clearTimeout(timeout);
        timeout = setTimeout(function () {
            if (input.value.length >= 3) {
                // Implementar búsqueda AJAX aquí si es necesario
                console.log('Buscando:', input.value);
            }
        }, 300);
    });
}

// Función para actualizar contadores
function actualizarContadores() {
    const elementos = document.querySelectorAll('[data-contador]');
    elementos.forEach(function (elemento) {
        const valor = parseInt(elemento.textContent);
        elemento.style.transform = 'scale(1.1)';
        setTimeout(function () {
            elemento.style.transform = 'scale(1)';
        }, 200);
    });
}

// Función para animaciones suaves al hacer scroll
function animarAlHacerScroll() {
    const elementos = document.querySelectorAll('.fade-in');
    const observer = new IntersectionObserver(function (entries) {
        entries.forEach(function (entry) {
            if (entry.isIntersecting) {
                entry.target.style.opacity = '1';
                entry.target.style.transform = 'translateY(0)';
            }
        });
    });

    elementos.forEach(function (elemento) {
        elemento.style.opacity = '0';
        elemento.style.transform = 'translateY(20px)';
        elemento.style.transition = 'opacity 0.6s ease, transform 0.6s ease';
        observer.observe(elemento);
    });
}

// Inicializar animaciones
animarAlHacerScroll();