using BancoLosPatitos.Abstracciones.ModelosParaUI;

namespace BancoLosPatitos.Abstracciones.LogicaDeNegocio.Configuraciones.ObtenerConfiguracion
{
    public interface IObtenerConfiguracionLN
    {
        ConfiguracionDto BuscarPorId(int idConfiguracion);
    }
}
