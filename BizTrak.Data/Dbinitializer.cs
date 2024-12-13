using BizTrak.Domain.Entities;

namespace BizTrak.Data;

public static class DbInitializer
{
    public static void Seed(BizTrakDbContext BizTrakDbContext)
    {

    //     if (!BizTrakDbContext.Quotes.Any())
    //     {
    //         var random = new Random();

    //         string[] firstNames = { "Simon", "John", "Sarah", "Jessica", "Mike", "Emma", "Robert", "Lucy", "David", "Mia" };
    //         string[] lastNames = { "Pegg", "Smith", "Jones", "Taylor", "Brown", "Wilson", "Johnson", "White", "Lewis", "Walker" };
    //         string[] quoteTitles = { "Mounting tasks", "Decorating tasks", "Carpentry", "Cleaning jobs", "Gardening" };
    //         string[] jobTitles = { "Hang a mirror", "Hang 5 pictures", "Install a shelf", "Wall painting", "Door installation", "Window cleaning", "Carpet fitting", "Fence repair", "Garden landscaping", "Roof inspection", "Build Shed", "Silicone Bath", "Toilet Seat", "Build Cupboard", "Assemble Sofa", "Mount Television" };
    //         string[] jobDescriptions = {
    //     "Hang a large mirror on a stud wall in the lounge",
    //     "Hang 5 medium sized pictures around the house",
    //     "Install a wooden shelf in the bedroom",
    //     "Paint the kitchen walls with waterproof paint",
    //     "Install a new wooden front door",
    //     "Clean all upstairs windows externally",
    //     "Fit new carpet in the living room",
    //     "Repair 5m of garden fence",
    //     "Landscape the backyard garden with new plants",
    //     "Conduct a yearly inspection of the roof",
    //     "Build a kitchen cupboard",
    //     "Re-silicone the bath tub",
    //     "Fix a wobbly toilet seat",
    //     "Build shed in back yard",
    //     "Mount a 50 inch flat screen television",
    //     "Assemble an Ikea sofa"
    // };

    //         for (int i = 0; i < 15; i++)
    //         {
    //             var client = new Client
    //             {
    //                 FirstName = firstNames[random.Next(firstNames.Length)],
    //                 LastName = lastNames[random.Next(lastNames.Length)],
    //                 AddressLine1 = $"{random.Next(1, 101)} Pellatt Road",
    //                 AddressLine2 = "London",
    //                 Postcode = $"SE{random.Next(1, 23):00} 9JA"
    //             };

    //             BizTrakDbContext.Customers.Add(client);
    //             BizTrakDbContext.SaveChanges();

    //             var quote = new Quote
    //             {
    //                 QuoteTitle = quoteTitles[random.Next(quoteTitles.Length)],
    //                 Client = client,
    //                 QuoteDate = DateTime.Now.ToUniversalTime()
    //             };

    //             BizTrakDbContext.Quotes.Add(quote);
    //             BizTrakDbContext.SaveChanges();

    //             quote.QuoteRef = quote.GenerateUniqueReference(quote.Id);

    //             var jobIndex = random.Next(jobTitles.Length);
    //             var job1 = new QuoteTask
    //             {
    //                 Description = jobTitles[jobIndex],
    //                 Description = jobDescriptions[jobIndex],
    //                 EstimatedCost = 40m + random.Next(0, 100),
    //                 Quote = quote
    //             };

    //             BizTrakDbContext.Jobs.Add(job1);

    //             var job2 = new QuoteTask
    //             {
    //                 Description = jobTitles[(jobIndex + 1) % jobTitles.Length],
    //                 Description = jobDescriptions[(jobIndex + 1) % jobTitles.Length],
    //                 EstimatedCost = 40m + random.Next(0, 100),
    //                 Quote = quote
    //             };

    //             BizTrakDbContext.Jobs.Add(job2);
    //             BizTrakDbContext.SaveChanges();
    //         }
    //     }
    }
}