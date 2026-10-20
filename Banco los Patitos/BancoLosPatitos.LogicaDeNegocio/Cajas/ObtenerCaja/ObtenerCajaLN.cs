using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Cajas.ObtenerCaja;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Cajas.ObtenerCaja
{
    public class ObtenerCajaLN : IObtenerCajaLN
    {
        public CajaDto BuscarPorId(int idCaja)
        {
            using (var contexto = new BancoLosPatitosContext())
            {
                return contexto.Cajas
                    .Where(x => x.IdCaja == idCaja)
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
                    .FirstOrDefault();
            }
        }
    }
}
