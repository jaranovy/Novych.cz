namespace Novych.Migrations
{
    using Novych.Models.Database;
    using Novych.Models.ParniCistic;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NovychDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(NovychDbContext context)
        {
            /* Seed Citerka */
            SeedCiterka(context);

            /* Seed Parni Cistic */
            SeedParniCisitc(context);

        }

        private void SeedCiterka(NovychDbContext context)
        {
            context.Songs.AddOrUpdate(
             p => p.HeaderText,

             new CiterkaSong
             {
                 Date = DateTime.Now,
                 HeaderText = "Stupnice",
                 Notes = "G1, A1, H1, C2, D2, E2, FIS2, G2, A2, H2, C3, D3, E3, FIS3, G3",
                 FooterText = "Připravil Jaroslav Nový"
             },
             new CiterkaSong
             {
                 Date = DateTime.Now,
                 HeaderText = "Stupnice 2 ",
                 Notes = "G1, A1, H1, C2, D2, E2, FIS2, G2, A2, H2, C3, D3, E3, FIS3, G3, X, G3, FIS3, E3, D3, C3, H2, A2, G2, FIS2, E2, D2, C2, H1, A1, G1",
                 FooterText = "Připravil Jaroslav Nový"
             },
             new CiterkaSong
             {
                 Date = DateTime.Now,
                 HeaderText = "Holka Modrooká",
                 Notes = "A2+, H2+, A2, A2, FIS2, A2, G2, G2, E2, G2, A2, A2, FIS2, G2, A2+, H2+, A2, A2, FIS2,A2, G2, G2, E2, G2, FIS2+, G2, G2, E2, G2, A2, A2, FIS2, A2, G2, G2, E2, G2, A2, A2, FIS2, G2, A2+, H2+, A2, A2, FIS2, A2, G2, G2, E2, G2, FIS2+",
                 FooterText = "Připravila Miroslava Nová"
             },
               new CiterkaSong
               {
                   Date = DateTime.Now,
                   HeaderText = "Pec nám spadla",
                   Notes = "D2, H1, H1, H1, D2, H1, H1, H1, D2, D2, E2, D2, D2, C2, C2+, C2, A1, A1, A1, C2, A1, A1, A1, C2, C2, D2, C2, C2, H1, H1+",
                   FooterText = "Připravila Miroslava Nová"
               },
               new CiterkaSong
               {
                   Date = DateTime.Now,
                   HeaderText = "Měla babka 4 jabka",
                   Notes = "G1, H1, D2+, D2+, G2, G2, D2+, D2+, C2, C2, A1+, A1+, G1, H1, D2+, G1, H1, D2+, D2+, G2, G2, D2+, D2+, C2, C2, A1+, A1+, G1, H1, G1+",
                   FooterText = ""
               }
           );
        }

        private void SeedParniCisitc(NovychDbContext context)
        {
            context.Reservations.AddOrUpdate(
      r => r.Date,
      new ParniCisticReservation { Date = new DateTime(2018, 3, 15), Name = "Jaroslav Nový", Email = "jara.novy@email.cz", Phone = "123456789", Address = "Slévačská 905", CreateDate = DateTime.Now, CreateDesc = "Seed", DeleteDate = null, DeleteDesc = "" },
      new ParniCisticReservation { Date = new DateTime(2018, 3, 16), Name = "Jaroslav Nový", Email = "jara.novy@email.cz", Phone = "123456789", Address = "Slévačská 905", CreateDate = DateTime.Now, CreateDesc = "Seed", DeleteDate = null, DeleteDesc = "" },
      new ParniCisticReservation { Date = new DateTime(2018, 3, 17), Name = "Jaroslav Nový", Email = "jara.novy@email.cz", Phone = "123456789", Address = "Slévačská 905", CreateDate = DateTime.Now, CreateDesc = "Seed", DeleteDate = DateTime.Now, DeleteDesc = "Seed" },
      new ParniCisticReservation { Date = new DateTime(2018, 3, 18), Name = "Jaroslav Nový", Email = "jara.novy@email.cz", Phone = "123456789", Address = "Slévačská 905", CreateDate = DateTime.Now, CreateDesc = "Seed", DeleteDate = DateTime.Now, DeleteDesc = "Seed" },
      new ParniCisticReservation { Date = new DateTime(2018, 3, 19), Name = "Jaroslav Nový", Email = "jara.novy@email.cz", Phone = "123456789", Address = "Slévačská 905", CreateDate = DateTime.Now, CreateDesc = "Seed", DeleteDate = null, DeleteDesc = "" }
    );
        }

    }
}
