using BancoLosPatitos.Abstracciones.ModelosParaUI;
using System.Collections.Generic;

namespace BancoLosPatitos.Abstracciones.LogicaDeNegocio.Sinpes.ListarSinpePorCaja
{
    public interface IListarSinpePorCajaLN
    {
        List<SinpeDto> ObtenerPorTelefono(string telefonoSINPE);
    }
}
