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
        public DbSet<PriceRates> Prices { get; set; }
        public DbSet<Board> InformationBoards { get; set; }
        public GarageDBContext()
                : base("name=GarageDBContext")
        {
            string entryTable = "EntryTable", exitTable = "exitTable";
            if (InformationBoards.Where(x => x.FunctionName == entryTable).Count() > 1 )
            {
                DeleteOldBoards(entryTable);
            }
            if (InformationBoards.Where(x => x.FunctionName == exitTable).Count() > 1)
            {
                DeleteOldBoards(exitTable);
            }
            System.Data.Entity.Database.SetInitializer<GarageDBContext>(new CreateDatabaseIfNotExists<GarageDBContext>());
            if (Prices.Count() == 0)
            {
                double[] defaultCosts = { 120, 50, 1.2, 3 };
                int[] defaultMinutes = { 0, 0, 30, 60 };
                int i = 0;
                foreach (var price in Enum.GetValues(typeof(PriceEnum)))
                {
                    Prices.Add(new PriceRates
                    {
                        PriceEnum = (PriceEnum)price,
                        Cost = defaultCosts[i],
                        Minutes = MakeNullableInt(defaultMinutes[i++])
                    });
                }
                SaveChanges();
            }

        }
        private int? MakeNullableInt(int minute)
        {
            if (minute == 0)
                return null;
            else
                return minute;
        }

        private void DeleteOldBoards(string name)
        {
            var tempInformationBoards = InformationBoards.Where(x => x.FunctionName == name).ToList();
            var tempInformationBoardsWithoutNewestBoard = tempInformationBoards.Where(x => x.ID < tempInformationBoards.OrderByDescending(y => x.ID).LastOrDefault().ID).ToList();
            foreach (var board in tempInformationBoardsWithoutNewestBoard)
            {
                InformationBoards.Remove(board);
            }
            SaveChanges();
        }
    }
}

