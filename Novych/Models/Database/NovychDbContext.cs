namespace Novych.Models.Database
{
    using System.Data.Entity;

    public class NovychDbContext : DbContext
    {
        public NovychDbContext()
            : base("name=NovychDb")
        {
            Database.SetInitializer<NovychDbContext>(null);
        }

        /* Citerka */
        public virtual DbSet<CiterkaSong> Songs { get; set; }

        /* Parni Cistic */
        public virtual DbSet<ParniCisticReservation> Reservations { get; set; }

        /* Common */
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Visitor> Visitors { get; set; }

    }
}