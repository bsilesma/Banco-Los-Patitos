using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.RegistrarEvento;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Usuarios.EditarUsuario;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using BancoLosPatitos.AccesoADatos.Entidades;
using BancoLosPatitos.LogicaDeNegocio.Bitacora.RegistrarEvento;
using System;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Usuarios.EditarUsuario
{
    public class EditarUsuarioLN : IEditarUsuarioLN
    {
        public void Editar(UsuarioDto usuario)
        {
            ValidadorDeUsuario.ValidarEdicion(usuario);

            string identificacion = usuario.Identificacion.Trim();

            using (var contexto = new BancoLosPatitosContext())
            using (var transaccion = contexto.Database.BeginTransaction())
            {
                try
                {
                    var existente = contexto.Usuarios
                        .FirstOrDefault(x => x.IdUsuario == usuario.IdUsuario);

                    if (existente == null)
                    {
                        throw new ReglaDeNegocioException("El usuario que intenta editar no existe.");
                    }

                    // Se excluye al propio usuario para que pueda conservar su identificación.
                    bool identificacionRepetida = contexto.Usuarios
                        .Any(x => x.Identificacion == identificacion && x.IdUsuario != usuario.IdUsuario);

                    if (identificacionRepetida)
                    {
                        throw new ReglaDeNegocioException("Ya existe otro usuario registrado con la identificación " + identificacion + ".");
                    }

                    Usuario anterior = Copiar(existente);

                    existente.Nombres = usuario.Nombres.Trim();
                    existente.PrimerApellido = usuario.PrimerApellido.Trim();
                    existente.SegundoApellido = usuario.SegundoApellido.Trim();
                    existente.Identificacion = identificacion;
                    existente.CorreoElectronico = usuario.CorreoElectronico.Trim();
                    existente.Estado = usuario.Estado;
                    existente.FechaDeModificacion = DateTime.Now;

                    contexto.SaveChanges();

                    IRegistrarEventoLN bitacora = new RegistrarEventoLN(contexto);
                    bitacora.Registrar(new EventoBitacoraDto
                    {
                        TablaDeEvento = TablasDeEvento.Usuarios,
                        TipoDeEvento = TiposDeEvento.Editar,
                        DescripcionDeEvento = "Se editó el usuario " + existente.Nombres + " " + existente.PrimerApellido + ".",
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
        private static Usuario Copiar(Usuario usuario)
        {
            return new Usuario
            {
                IdUsuario = usuario.IdUsuario,
                IdComercio = usuario.IdComercio,
                IdNetUser = usuario.IdNetUser,
                Nombres = usuario.Nombres,
                PrimerApellido = usuario.PrimerApellido,
                SegundoApellido = usuario.SegundoApellido,
                Identificacion = usuario.Identificacion,
                CorreoElectronico = usuario.CorreoElectronico,
                FechaDeRegistro = usuario.FechaDeRegistro,
                FechaDeModificacion = usuario.FechaDeModificacion,
                Estado = usuario.Estado
            };
        }
    }
}
