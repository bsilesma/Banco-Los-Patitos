using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.RegistrarEvento;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Comercios.RegistrarComercio;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using BancoLosPatitos.AccesoADatos.Entidades;
using BancoLosPatitos.LogicaDeNegocio.Bitacora.RegistrarEvento;
using System;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Comercios.RegistrarComercio
{
    public class RegistrarComercioLN : IRegistrarComercioLN
    {
        public void Registrar(ComercioDto comercio)
        {
            ValidadorDeComercio.ValidarRegistro(comercio);

            string identificacion = comercio.Identificacion.Trim();

            using (var contexto = new BancoLosPatitosContext())
            using (var transaccion = contexto.Database.BeginTransaction())
            {
                try
                {
                    bool existe = contexto.Comercios
                        .Any(x => x.Identificacion == identificacion);

                    if (existe)
                    {
                        throw new ReglaDeNegocioException("Ya existe un comercio registrado con la identificación " + identificacion + ".");
                    }

                    var nuevo = new Comercio
                    {
                        Identificacion = identificacion,
                        TipoIdentificacion = comercio.TipoIdentificacion,
                        Nombre = comercio.Nombre.Trim(),
                        TipoDeComercio = comercio.TipoDeComercio,
                        Telefono = comercio.Telefono.Trim(),
                        CorreoElectronico = comercio.CorreoElectronico.Trim(),
                        Direccion = comercio.Direccion.Trim(),
                        FechaDeRegistro = DateTime.Now,
                        FechaDeModificacion = null,
                        Estado = true
                    };

                    contexto.Comercios.Add(nuevo);
                    contexto.SaveChanges();

                    IRegistrarEventoLN bitacora = new RegistrarEventoLN(contexto);
                    bitacora.Registrar(new EventoBitacoraDto
                    {
                        TablaDeEvento = TablasDeEvento.Comercios,
                        TipoDeEvento = TiposDeEvento.Registrar,
                        DescripcionDeEvento = "Se registró el comercio " + nuevo.Nombre + ".",
                        DatosAnteriores = nuevo
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
    }
}
