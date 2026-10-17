using BancoLosPatitos.AccesoADatos.Entidades;
using System.Data.Entity;

namespace BancoLosPatitos.AccesoADatos.Contexto
{
    public class BancoLosPatitosContext : DbContext
    {
        public BancoLosPatitosContext()
            : base("BancoLosPatitosConnection")
        {
        }

        public DbSet<Comercio> Comercios { get; set; }

        public DbSet<BitacoraEvento> BitacoraEventos { get; set; }
    }
}
