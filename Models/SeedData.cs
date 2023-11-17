namespace SampleJWT.Models
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated(); // Ensure the database is created

            // Check if there are already users in the database
            if (context.Users.Any())
            {
                return; // Database has been seeded
            }

            // Add sample users
            var users = new User[]
            {
            new User { UserName = "john_doe", Password = "password123" },
            new User { UserName = "jane_smith", Password = "secret456" },
                // Add more sample users as needed
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
