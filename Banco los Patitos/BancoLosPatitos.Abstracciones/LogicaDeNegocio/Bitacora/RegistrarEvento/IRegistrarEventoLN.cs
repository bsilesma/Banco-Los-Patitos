using BancoLosPatitos.Abstracciones.ModelosParaUI;

namespace BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.RegistrarEvento
{
    public interface IRegistrarEventoLN
    {
        void Registrar(EventoBitacoraDto evento);
    }
}
