using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Comercios.ListarComercios;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using System.Collections.Generic;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Comercios.ListarComercios
{
    public class ListarComerciosLN : IListarComerciosLN
    {
        public List<ComercioDto> Obtener()
        {
            using (var contexto = new BancoLosPatitosContext())
            {
                return contexto.Comercios
                    .OrderBy(x => x.IdComercio)
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
                    .ToList();
            }
        }
    }
}
