using System.Data.Entity;
using GuessWhat.Web.Migrations;

namespace GuessWhat.Web.Data
{
    public class GuessWhatContext : DbContext
    {
        public GuessWhatContext() : base("name=GuessWhatContext")
        {
            new Configuration().InitializeDatabase(this);
        }

        public System.Data.Entity.DbSet<Animal> Animals { get; set; }

        public System.Data.Entity.DbSet<AnimalDetail> AnimalDetails { get; set; }
    }
}