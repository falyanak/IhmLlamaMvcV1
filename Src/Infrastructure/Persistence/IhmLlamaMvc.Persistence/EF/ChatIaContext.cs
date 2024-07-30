using IhmLlamaMvc.Domain.Entites.Agents;
using IhmLlamaMvc.Domain.Entites.Conversations;
using IhmLlamaMvc.Domain.Entites.IaModels;
using IhmLlamaMvc.Domain.Entites.Questions;
using IhmLlamaMvc.Domain.Entites.Reponses;
using Microsoft.EntityFrameworkCore;

namespace IhmLlamaMvc.Persistence.EF
{

    public class ChatIaDbContext : DbContext
    {
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Conversation?> Conversations { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Reponse> Reponses { get; set; }
        public DbSet<ModeleIA> IaModels { get; set; }

        public ChatIaDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reponse>()
                .HasOne(e => e.QuestionPosee)
                .WithOne(e => e.Reponse)
                .HasForeignKey<Question>(e => e.Id)
                .IsRequired();

            modelBuilder.Entity<Question>()
                .HasOne(e => e.Reponse)
                .WithOne(e => e.QuestionPosee)
                .HasForeignKey<Reponse>(e => e.Id);
        }
    }
}
