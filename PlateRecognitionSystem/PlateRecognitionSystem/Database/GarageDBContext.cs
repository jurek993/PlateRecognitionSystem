namespace PlateRecognitionSystem
{
    using PlateRecognitionSystem.Enums;
    using PlateRecognitionSystem.Model;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class GarageDBContext : DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SingleVisit> SingleVisits { get; set; }
        public DbSet<Price> Prices { get; set; }
        public GarageDBContext()
                : base("name=GarageDBContext")
        {
            System.Data.Entity.Database.SetInitializer<GarageDBContext>(new CreateDatabaseIfNotExists<GarageDBContext>());
            if (Prices.Count() == 0)
            {
                double[] defaultCosts = { 120, 50, 1.2, 3 };
                int i = 0;
                foreach (var price in Enum.GetValues(typeof(PriceEnum)))
                {
                    Prices.Add(new Price
                    {
                        PriceEnum = (PriceEnum)price,
                        Cost = defaultCosts[i++]
                    });
                }
                SaveChanges();
            }

        }
    }
}

