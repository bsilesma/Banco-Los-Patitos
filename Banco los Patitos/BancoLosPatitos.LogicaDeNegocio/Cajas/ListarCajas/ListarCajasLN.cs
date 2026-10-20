using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Cajas.ListarCajas;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using System.Collections.Generic;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Cajas.ListarCajas
{
    public class ListarCajasLN : IListarCajasLN
    {
        public List<CajaDto> ObtenerPorComercio(int idComercio)
        {
            using (var contexto = new BancoLosPatitosContext())
            {
                return contexto.Cajas
                    .Where(x => x.IdComercio == idComercio)
                    .OrderBy(x => x.IdCaja)
                    .Select(x => new CajaDto
                    {
                        IdCaja = x.IdCaja,
                        IdComercio = x.IdComercio,
                        Nombre = x.Nombre,
                        Descripcion = x.Descripcion,
                        TelefonoSINPE = x.TelefonoSINPE,
                        FechaDeRegistro = x.FechaDeRegistro,
                        FechaDeModificacion = x.FechaDeModificacion,
                        Estado = x.Estado
                    })
                    .ToList();
            }
        }
    }
}
