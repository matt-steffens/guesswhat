using System;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using GuessWhat.Web.Data;

namespace GuessWhat.Web.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<GuessWhatContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        public void InitializeDatabase(GuessWhatContext context)
        {
            Seed(context);
        }

        protected override void Seed(GuessWhatContext context)
        {
            if (context.Animals.Any())
            {
                return;
            }

            var elephant =
                new Animal
                {
                    Name = "Elephant",
                    AnimalDetails =
                        new[]
                        {
                            new AnimalDetail { Detail = "has a trunk" },
                            new AnimalDetail { Detail = "trumpets" },
                            new AnimalDetail { Detail = "is grey" }
                        }
                };

            var lion =
                new Animal
                {
                    Name = "Lion",
                    AnimalDetails =
                        new[]
                        {
                            new AnimalDetail { Detail = "has a mane" },
                            new AnimalDetail { Detail = "roars" },
                            new AnimalDetail { Detail = "is yellow" }
                        }
                };

            context.Animals.AddOrUpdate(a => a.Name,
                elephant,
                lion);

            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var error = e.EntityValidationErrors.Aggregate(string.Empty,
                    (current1, dbEntityValidationResult) =>
                        dbEntityValidationResult.ValidationErrors.Aggregate(current1,
                            (current, dbValidationError) =>
                                current +
                                $"Entity: {dbEntityValidationResult.Entry}, PropertyName: {dbValidationError.PropertyName}-{dbValidationError.ErrorMessage}; "));

                throw new Exception(error);
            }
        }
    }
}