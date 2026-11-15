using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.RegistrarEvento;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Usuarios.RegistrarUsuario;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using BancoLosPatitos.AccesoADatos.Entidades;
using BancoLosPatitos.LogicaDeNegocio.Bitacora.RegistrarEvento;
using System;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Usuarios.RegistrarUsuario
{
    public class RegistrarUsuarioLN : IRegistrarUsuarioLN
    {
        public void Registrar(UsuarioDto usuario)
        {
            ValidadorDeUsuario.ValidarRegistro(usuario);

            string identificacion = usuario.Identificacion.Trim();

            using (var contexto = new BancoLosPatitosContext())
            using (var transaccion = contexto.Database.BeginTransaction())
            {
                try
                {
                    var comercio = contexto.Comercios
                        .FirstOrDefault(x => x.IdComercio == usuario.IdComercio);

                    if (comercio == null)
                    {
                        throw new ReglaDeNegocioException("El comercio del usuario no existe.");
                    }

                    if (!comercio.Estado)
                    {
                        throw new ReglaDeNegocioException("No se pueden registrar usuarios en un comercio inactivo.");
                    }

                    bool existe = contexto.Usuarios
                        .Any(x => x.Identificacion == identificacion);

                    if (existe)
                    {
                        throw new ReglaDeNegocioException("Ya existe un usuario registrado con la identificación " + identificacion + ".");
                    }

                    var nuevo = new Usuario
                    {
                        IdComercio = usuario.IdComercio,
                        IdNetUser = null,
                        Nombres = usuario.Nombres.Trim(),
                        PrimerApellido = usuario.PrimerApellido.Trim(),
                        SegundoApellido = usuario.SegundoApellido.Trim(),
                        Identificacion = identificacion,
                        CorreoElectronico = usuario.CorreoElectronico.Trim(),
                        FechaDeRegistro = DateTime.Now,
                        FechaDeModificacion = null,
                        Estado = true
                    };

                    contexto.Usuarios.Add(nuevo);
                    contexto.SaveChanges();

                    IRegistrarEventoLN bitacora = new RegistrarEventoLN(contexto);
                    bitacora.Registrar(new EventoBitacoraDto
                    {
                        TablaDeEvento = TablasDeEvento.Usuarios,
                        TipoDeEvento = TiposDeEvento.Registrar,
                        DescripcionDeEvento = "Se registró el usuario " + nuevo.Nombres + " " + nuevo.PrimerApellido + " en el comercio " + comercio.Nombre + ".",
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
