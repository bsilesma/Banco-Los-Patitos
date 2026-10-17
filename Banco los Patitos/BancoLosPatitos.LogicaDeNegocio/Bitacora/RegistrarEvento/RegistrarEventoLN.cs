using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.RegistrarEvento;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using BancoLosPatitos.AccesoADatos.Entidades;
using Newtonsoft.Json;
using System;

namespace BancoLosPatitos.LogicaDeNegocio.Bitacora.RegistrarEvento
{
    public class RegistrarEventoLN : IRegistrarEventoLN
    {
        private const string ValorPorDefecto = "N/A";

        private readonly BancoLosPatitosContext _contexto;

        public RegistrarEventoLN()
        {
        }

        // Recibe el contexto de otra operación para guardar el evento dentro de su misma transacción.
        internal RegistrarEventoLN(BancoLosPatitosContext contexto)
        {
            _contexto = contexto;
        }

        public void Registrar(EventoBitacoraDto evento)
        {
            BitacoraEvento entidad = CrearEntidad(evento);

            if (_contexto != null)
            {
                _contexto.BitacoraEventos.Add(entidad);
                _contexto.SaveChanges();
                return;
            }

            using (var contexto = new BancoLosPatitosContext())
            {
                contexto.BitacoraEventos.Add(entidad);
                contexto.SaveChanges();
            }
        }

        private static BitacoraEvento CrearEntidad(EventoBitacoraDto evento)
        {
            return new BitacoraEvento
            {
                TablaDeEvento = evento.TablaDeEvento,
                TipoDeEvento = evento.TipoDeEvento,
                FechaDeEvento = DateTime.Now,
                DescripcionDeEvento = ValorOPorDefecto(evento.DescripcionDeEvento),
                StackTrace = ValorOPorDefecto(evento.StackTrace),
                DatosAnteriores = ConvertirAJson(evento.DatosAnteriores),
                DatosPosteriores = ConvertirAJson(evento.DatosPosteriores)
            };
        }

        private static string ValorOPorDefecto(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? ValorPorDefecto : valor;
        }

        private static string ConvertirAJson(object datos)
        {
            return datos == null ? null : JsonConvert.SerializeObject(datos);
        }
    }
}
