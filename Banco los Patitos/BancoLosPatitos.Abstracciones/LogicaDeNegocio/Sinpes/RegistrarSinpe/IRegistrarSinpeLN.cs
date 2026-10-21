using BancoLosPatitos.Abstracciones.ModelosParaUI;

namespace BancoLosPatitos.Abstracciones.LogicaDeNegocio.Sinpes.RegistrarSinpe
{
    public interface IRegistrarSinpeLN
    {
        // Devuelve el Id de la caja que recibió el pago.
        int Registrar(SinpeDto sinpe);
    }
}
