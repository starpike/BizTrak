using FinanceApp.Domain;

namespace FinanceApp.Data;

public static class DbInitializer
{
    public static void Seed(FinanceAppDbContext financeAppDbContext)
    {

        if (!financeAppDbContext.Clients.Any())
        {
            financeAppDbContext.Clients.Add(new Client()
            {
                FirstName = "Simon",
                LastName = "Pegg",
                AddressLine1 = "37 Pellatt Road",
                AddressLine2 = "London",
                Postcode = "SE22 9JA"
            });

            financeAppDbContext.Clients.Add(new Client()
            {
                FirstName = "Peter",
                LastName = "Jones",
                AddressLine1 = "12 Ulverscroft Road",
                AddressLine2 = "London",
                Postcode = "SE22 9EB"
            });

            financeAppDbContext.Clients.Add(new Client()
            {
                FirstName = "Ruby",
                LastName = "Kinlochan",
                AddressLine1 = "35 Pellatt Road",
                AddressLine2 = "London",
                Postcode = "SE22 9JA"
            });

            financeAppDbContext.SaveChanges();
        }

        if (!financeAppDbContext.Quotes.Any())
        {
            var quoteA = new Quote()
            {
                QuoteTitle = "Mounting tasks",
                Client = financeAppDbContext.Clients.Find(1),
                QuoteDate = DateTime.Now.ToUniversalTime()
            };

            financeAppDbContext.Jobs.Add(new Job()
            {
                JobTitle = "Hang a mirror",
                Description = "Hang a large mirror on a stud wall in the lounge",
                EstimatedCost = 40m,
                Quote = quoteA
            });

            financeAppDbContext.Jobs.Add(new Job()
            {
                JobTitle = "Hang 5 pictures",
                Description = "Hang 5 medium sized pictures around the house",
                EstimatedCost = 40m,
                Quote = quoteA
            });

            var quoteB = new Quote()
            {
                QuoteTitle = "Fill hole in wall",
                Client = financeAppDbContext.Clients.Find(2),
                QuoteDate = DateTime.Now.ToUniversalTime()
            };

            financeAppDbContext.Jobs.Add(new Job()
            {
                JobTitle = "Fill hole in wall",
                Description = "Fill a small hole in plasterboard wall",
                EstimatedCost = 40m,
                Quote = quoteB
            });

            var quoteC = new Quote()
            {
                QuoteTitle = "Silicone Jobs",
                Client = financeAppDbContext.Clients.Find(3),
                QuoteDate = DateTime.Now.ToUniversalTime()
            };

            financeAppDbContext.Jobs.Add(new Job()
            {
                JobTitle = "Silicone Bath",
                Description = "Remove ols silicone and apply new silicone bead around bath",
                EstimatedCost = 120m,
                Quote = quoteC
            });

            financeAppDbContext.Jobs.Add(new Job()
            {
                JobTitle = "Silicone Sink",
                Description = "Remove old silicone and re-apply to sink",
                EstimatedCost = 40m,
                Quote = quoteC
            });



            financeAppDbContext.SaveChanges();
        }
    }
}