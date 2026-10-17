using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Comercios.ObtenerComercio;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Comercios.ObtenerComercio
{
    public class ObtenerComercioLN : IObtenerComercioLN
    {
        public ComercioDto BuscarPorId(int idComercio)
        {
            using (var contexto = new BancoLosPatitosContext())
            {
                return contexto.Comercios
                    .Where(x => x.IdComercio == idComercio)
                    .Select(x => new ComercioDto
                    {
                        IdComercio = x.IdComercio,
                        Identificacion = x.Identificacion,
                        TipoIdentificacion = x.TipoIdentificacion,
                        Nombre = x.Nombre,
                        TipoDeComercio = x.TipoDeComercio,
                        Telefono = x.Telefono,
                        CorreoElectronico = x.CorreoElectronico,
                        Direccion = x.Direccion,
                        FechaDeRegistro = x.FechaDeRegistro,
                        FechaDeModificacion = x.FechaDeModificacion,
                        Estado = x.Estado
                    })
                    .FirstOrDefault();
            }
        }
    }
}
