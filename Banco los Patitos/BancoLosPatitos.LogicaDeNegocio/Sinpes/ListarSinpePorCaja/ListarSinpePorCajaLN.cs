using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Sinpes.ListarSinpePorCaja;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using System.Collections.Generic;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Sinpes.ListarSinpePorCaja
{
    public class ListarSinpePorCajaLN : IListarSinpePorCajaLN
    {
        public List<SinpeDto> ObtenerPorTelefono(string telefonoSINPE)
        {
            using (var contexto = new BancoLosPatitosContext())
            {
                return contexto.Sinpes
                    .Where(x => x.TelefonoDestinatario == telefonoSINPE)
                    .OrderByDescending(x => x.FechaDeRegistro)
                    .ThenByDescending(x => x.IdSinpe)
                    .Select(x => new SinpeDto
                    {
                        IdSinpe = x.IdSinpe,
                        TelefonoOrigen = x.TelefonoOrigen,
                        NombreOrigen = x.NombreOrigen,
                        TelefonoDestinatario = x.TelefonoDestinatario,
                        NombreDestinatario = x.NombreDestinatario,
                        Monto = x.Monto,
                        FechaDeRegistro = x.FechaDeRegistro,
                        Descripcion = x.Descripcion,
                        Estado = x.Estado
                    })
                    .ToList();
            }
        }
    }
}
