using Microsoft.EntityFrameworkCore;
using MvcCoreZapatillasPractica.Models;

namespace MvcCoreZapatillasPractica.Data
{
    public class ZapatillasContext : DbContext
    {
        public ZapatillasContext(DbContextOptions<ZapatillasContext> options) : base(options) { }

        public DbSet<Zapas> Zapas { get; set; }
        public DbSet<ImagenesZapas> ImagenesZapas { get; set; }
    }
}
