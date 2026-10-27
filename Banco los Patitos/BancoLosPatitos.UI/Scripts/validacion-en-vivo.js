// Validación en vivo: revisa cada campo mientras se escribe, con los mismos mensajes del servidor.
// Es solo una ayuda; la lógica de negocio vuelve a validar todo al guardar.
(function () {
    'use strict';

    var LARGO_DEL_TELEFONO = 8;
    var MAXIMO_DE_DIGITOS_ENTEROS = 16;

    // Sin puntos al inicio, al final ni seguidos, y el dominio debe terminar en una extensión como .com o .cr.
    var FORMATO_DE_CORREO = /^[A-Za-z0-9!#$%&'*+\/=?^_`{|}~-]+(\.[A-Za-z0-9!#$%&'*+\/=?^_`{|}~-]+)*@([A-Za-z0-9]([A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z]{2,}$/;

    var reglas = {
        texto: function (valor, campo) {
            return valor.trim() === '' ? 'El campo ' + campo + ' es obligatorio.' : null;
        },

        opcional: function () {
            return null;
        },

        seleccion: function (valor, campo) {
            return valor === '' ? 'Debe seleccionar un ' + campo.toLowerCase() + ' válido.' : null;
        },

        telefono: function (valor, campo) {
            if (valor.length !== LARGO_DEL_TELEFONO) {
                return 'El campo ' + campo + ' debe contener exactamente ' + LARGO_DEL_TELEFONO + ' dígitos (lleva ' + valor.length + ').';
            }

            return null;
        },

        correo: function (valor, campo) {
            if (valor.trim() === '') {
                return 'El campo ' + campo + ' es obligatorio.';
            }

            return FORMATO_DE_CORREO.test(valor.trim()) ? null : 'El correo electrónico no tiene un formato válido.';
        },

        monto: function (valor) {
            if (!/^\d+(,\d+)?$/.test(valor)) {
                return 'El campo Monto es obligatorio y debe ser un número válido, por ejemplo 4500,50.';
            }

            var partes = valor.split(',');

            if (partes[1] && partes[1].length > 2) {
                return 'El monto admite un máximo de 2 decimales.';
            }

            if (partes[0].replace(/^0+/, '').length > MAXIMO_DE_DIGITOS_ENTEROS) {
                return 'El monto supera el máximo permitido.';
            }

            return parseFloat(valor.replace(',', '.')) > 0 ? null : 'El monto debe ser mayor a 0.';
        }
    };

    // Mientras se escribe, quita lo que nunca sería válido en ese tipo de campo.
    var limpiezas = {
        telefono: function (valor) {
            return valor.replace(/\D/g, '');
        },

        monto: function (valor) {
            return valor.replace('.', ',').replace(/[^\d,]/g, '');
        }
    };

    function nombreDelCampo(control) {
        var etiqueta = document.querySelector('label[for="' + control.id + '"]');
        return etiqueta ? etiqueta.textContent.replace('(opcional)', '').trim() : control.name;
    }

    function crearAyuda(control) {
        var ayuda = document.createElement('small');
        ayuda.className = 'ayuda-campo';
        control.parentNode.appendChild(ayuda);
        return ayuda;
    }

    function revisar(control, ayuda, mostrarError) {
        var tipo = control.getAttribute('data-validar');
        var error = reglas[tipo](control.value, nombreDelCampo(control));
        var maximo = control.getAttribute('maxlength');

        control.classList.toggle('is-invalid', error !== null && mostrarError);
        // El verde solo aparece cuando el usuario ya tocó el campo, no al abrir el formulario.
        control.classList.toggle('is-valid', error === null && control.value !== '' && mostrarError);

        if (error !== null && mostrarError) {
            ayuda.textContent = error;
            ayuda.className = 'ayuda-campo error';
        } else if (maximo && tipo !== 'telefono') {
            ayuda.textContent = control.value.length + ' de ' + maximo + ' caracteres';
            ayuda.className = 'ayuda-campo';
        } else {
            ayuda.textContent = error === null && control.value !== '' && mostrarError ? 'Correcto' : '';
            ayuda.className = 'ayuda-campo correcto';
        }

        return error === null;
    }

    // Quita lo inválido sin mandar el cursor al final del campo.
    function limpiar(control, limpieza) {
        var limpio = limpieza(control.value);

        if (limpio === control.value) {
            return;
        }

        var posicion = Math.max(0, control.selectionStart - (control.value.length - limpio.length));
        control.value = limpio;
        control.setSelectionRange(posicion, posicion);
    }

    // Un data-validar mal escrito se avisa en la consola y ese campo se ignora, sin romper el resto.
    function tieneReglaConocida(control) {
        var tipo = control.getAttribute('data-validar');

        if (reglas[tipo]) {
            return true;
        }

        console.error('validacion-en-vivo: no existe la regla "' + tipo + '" del campo ' + control.name + '.');
        return false;
    }

    function prepararFormulario(formulario) {
        var controles = Array.prototype.slice.call(formulario.querySelectorAll('[data-validar]'))
            .filter(tieneReglaConocida);

        controles.forEach(function (control) {
            var ayuda = crearAyuda(control);
            var tipo = control.getAttribute('data-validar');
            var tocado = control.value !== '';

            control.addEventListener('input', function () {
                if (limpiezas[tipo]) {
                    limpiar(control, limpiezas[tipo]);
                }

                revisar(control, ayuda, tocado);
            });

            control.addEventListener('change', function () {
                tocado = true;
                revisar(control, ayuda, true);
            });

            control.addEventListener('blur', function () {
                tocado = true;
                revisar(control, ayuda, true);
            });

            control.revisarAhora = function () {
                tocado = true;
                return revisar(control, ayuda, true);
            };

            revisar(control, ayuda, false);
        });

        // Si algo está mal, no envía el formulario y lleva al primer campo con error.
        formulario.addEventListener('submit', function (evento) {
            var conError = controles.filter(function (control) {
                return !control.revisarAhora();
            });

            if (conError.length > 0) {
                evento.preventDefault();
                conError[0].focus();
            }
        });
    }

    Array.prototype.slice.call(document.querySelectorAll('form')).forEach(function (formulario) {
        if (formulario.querySelector('[data-validar]')) {
            prepararFormulario(formulario);
        }
    });
})();
