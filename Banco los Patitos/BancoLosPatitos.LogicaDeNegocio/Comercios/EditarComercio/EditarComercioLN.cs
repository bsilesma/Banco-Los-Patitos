using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.RegistrarEvento;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Comercios.EditarComercio;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using BancoLosPatitos.AccesoADatos.Entidades;
using BancoLosPatitos.LogicaDeNegocio.Bitacora.RegistrarEvento;
using System;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Comercios.EditarComercio
{
    public class EditarComercioLN : IEditarComercioLN
    {
        public void Editar(ComercioDto comercio)
        {
            ValidadorDeComercio.ValidarEdicion(comercio);

            using (var contexto = new BancoLosPatitosContext())
            using (var transaccion = contexto.Database.BeginTransaction())
            {
                try
                {
                    var existente = contexto.Comercios
                        .FirstOrDefault(x => x.IdComercio == comercio.IdComercio);

                    if (existente == null)
                    {
                        throw new ReglaDeNegocioException("El comercio que intenta editar no existe.");
                    }

                    Comercio anterior = Copiar(existente);

                    existente.Nombre = comercio.Nombre.Trim();
                    existente.TipoDeComercio = comercio.TipoDeComercio;
                    existente.Telefono = comercio.Telefono.Trim();
                    existente.CorreoElectronico = comercio.CorreoElectronico.Trim();
                    existente.Direccion = comercio.Direccion.Trim();
                    existente.Estado = comercio.Estado;
                    existente.FechaDeModificacion = DateTime.Now;

                    contexto.SaveChanges();

                    IRegistrarEventoLN bitacora = new RegistrarEventoLN(contexto);
                    bitacora.Registrar(new EventoBitacoraDto
                    {
                        TablaDeEvento = TablasDeEvento.Comercios,
                        TipoDeEvento = TiposDeEvento.Editar,
                        DescripcionDeEvento = "Se editó el comercio " + existente.Nombre + ".",
                        DatosAnteriores = anterior,
                        DatosPosteriores = existente
                    });

                    transaccion.Commit();
                }
                catch
                {
                    transaccion.Rollback();
                    throw;
                }
            }
        }

        // Copia los valores antes de modificarlos, porque la entidad original cambia al editar.
        private static Comercio Copiar(Comercio comercio)
        {
            return new Comercio
            {
                IdComercio = comercio.IdComercio,
                Identificacion = comercio.Identificacion,
                TipoIdentificacion = comercio.TipoIdentificacion,
                Nombre = comercio.Nombre,
                TipoDeComercio = comercio.TipoDeComercio,
                Telefono = comercio.Telefono,
                CorreoElectronico = comercio.CorreoElectronico,
                Direccion = comercio.Direccion,
                FechaDeRegistro = comercio.FechaDeRegistro,
                FechaDeModificacion = comercio.FechaDeModificacion,
                Estado = comercio.Estado
            };
        }
    }
}
