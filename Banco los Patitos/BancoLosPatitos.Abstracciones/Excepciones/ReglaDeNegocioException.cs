using System;

namespace BancoLosPatitos.Abstracciones.Excepciones
{
    // Error esperado: incumple una regla del negocio y su mensaje se muestra al usuario.
    public class ReglaDeNegocioException : Exception
    {
        public ReglaDeNegocioException(string mensaje)
            : base(mensaje)
        {
        }
    }
}
