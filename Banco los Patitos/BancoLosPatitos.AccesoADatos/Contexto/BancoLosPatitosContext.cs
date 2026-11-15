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

        public DbSet<Caja> Cajas { get; set; }

        public DbSet<Sinpe> Sinpes { get; set; }

        public DbSet<Configuracion> Configuraciones { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<ReporteMensual> ReportesMensuales { get; set; }

        public DbSet<BitacoraEvento> BitacoraEventos { get; set; }
    }
}
