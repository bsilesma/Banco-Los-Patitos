using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.RegistrarEvento;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Cajas.RegistrarCaja;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using BancoLosPatitos.AccesoADatos.Entidades;
using BancoLosPatitos.LogicaDeNegocio.Bitacora.RegistrarEvento;
using System;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Cajas.RegistrarCaja
{
    public class RegistrarCajaLN : IRegistrarCajaLN
    {
        public void Registrar(CajaDto caja)
        {
            ValidadorDeCaja.ValidarRegistro(caja);

            string nombre = caja.Nombre.Trim();
            string telefono = caja.TelefonoSINPE.Trim();

            using (var contexto = new BancoLosPatitosContext())
            using (var transaccion = contexto.Database.BeginTransaction())
            {
                try
                {
                    var comercio = contexto.Comercios
                        .FirstOrDefault(x => x.IdComercio == caja.IdComercio);

                    if (comercio == null)
                    {
                        throw new ReglaDeNegocioException("El comercio de la caja no existe.");
                    }

                    if (!comercio.Estado)
                    {
                        throw new ReglaDeNegocioException("No se pueden registrar cajas en un comercio inactivo.");
                    }

                    bool nombreRepetido = contexto.Cajas
                        .Any(x => x.IdComercio == caja.IdComercio && x.Nombre == nombre);

                    if (nombreRepetido)
                    {
                        throw new ReglaDeNegocioException("El comercio ya tiene una caja con el nombre " + nombre + ".");
                    }

                    bool telefonoEnUso = contexto.Cajas
                        .Any(x => x.TelefonoSINPE == telefono && x.Estado);

                    if (telefonoEnUso)
                    {
                        throw new ReglaDeNegocioException("Ya existe una caja activa con el teléfono SINPE " + telefono + ".");
                    }

                    var nueva = new Caja
                    {
                        IdComercio = caja.IdComercio,
                        Nombre = nombre,
                        Descripcion = caja.Descripcion.Trim(),
                        TelefonoSINPE = telefono,
                        FechaDeRegistro = DateTime.Now,
                        FechaDeModificacion = null,
                        Estado = true
                    };

                    contexto.Cajas.Add(nueva);
                    contexto.SaveChanges();

                    IRegistrarEventoLN bitacora = new RegistrarEventoLN(contexto);
                    bitacora.Registrar(new EventoBitacoraDto
                    {
                        TablaDeEvento = TablasDeEvento.Cajas,
                        TipoDeEvento = TiposDeEvento.Registrar,
                        DescripcionDeEvento = "Se registró la caja " + nueva.Nombre + ".",
                        DatosAnteriores = nueva
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
