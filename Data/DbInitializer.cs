using mentalHealth.Models;

namespace mentalHealth.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Therapists.Any()) return;

            var therapists = new Therapist[]
            {
                new Therapist
                {
                    Name = "Dr. Sarah Johnson",
                    ProfileImage = "https://randomuser.me/api/portraits/women/65.jpg",
                    Specialization = "Clinical Psychologist",
                    Bio = "Specializes in cognitive behavioral therapy with 10 years of experience...",
                    AverageRating = 4.7M,
                    ReviewCount = 42,
                    Education = "PhD in Clinical Psychology, Harvard University",
                    Specializations = "Anxiety, Depression, PTSD"
                },
                new Therapist
                {
                    Name = "Dr. Michael Chen",
                    ProfileImage = "https://randomuser.me/api/portraits/men/22.jpg",
                    Specialization = "Marriage and Family Therapist",
                    Bio = "Specializes in family systems therapy and relationship counseling...",
                    AverageRating = 4.9M,
                    ReviewCount = 35,
                    Education = "MA in Marriage and Family Therapy, Stanford University",
                    Specializations = "Relationships, Family Therapy, Communication"
                }
            };

            context.Therapists.AddRange(therapists);
            context.SaveChanges();
        }
    }
}