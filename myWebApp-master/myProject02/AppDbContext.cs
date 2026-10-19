using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using myProject02.Models.Interfaces;
using System.Security.Cryptography.X509Certificates;

namespace myProject02.Models

{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
           
    }
        public DbSet<Voter> Voters { get; set; }
        public DbSet<AuditLog> Audit { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Party> Parties { get; set; }

        public DbSet<Candidate> Candidates { get; set; }

        public DbSet<PartyElection> PartyElections { get; set; }
        public DbSet<Election> Elections { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // 1. Get all entities that are Added or Modified
            var modifiedEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            var auditLogs = new List<AuditLog>();

            foreach (var entry in modifiedEntries)
            {
                // 2. Capture changes (Example using simple JSON serialization)
                var changes = new
                {
                    Original = entry.State == EntityState.Modified ? entry.OriginalValues.ToObject() : null,
                    Current = entry.CurrentValues.ToObject()
                };

                auditLogs.Add(new AuditLog
                {
                    EntityName = entry.Entity.GetType().Name,
                    Action = entry.State.ToString(),
                    Changes = System.Text.Json.JsonSerializer.Serialize(changes),
                    Timestamp = DateTime.UtcNow
                });

                // 3. Handle IAuditable timestamps (as before)
                if (entry.Entity is IAuditable auditable)
                {
                    if (entry.State == EntityState.Added) auditable.CreatedAt = DateTime.UtcNow;
                    if (entry.State == EntityState.Modified) auditable.UpdatedAt = DateTime.UtcNow;
                }
            }

            // 4. Add logs to the context and save everything
            if (auditLogs.Any())
            {
                Audit.AddRange(auditLogs);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<Party>()
                .HasMany(p => p.Candidates)
                .WithOne(c => c.Party)
                .HasForeignKey(c => c.PartyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Composite Primary Key for PartyElection
            modelBuilder.Entity<PartyElection>()
                .HasKey(pe => new { pe.PartyId, pe.ElectionId });
            // Configure relationship with Party
            modelBuilder.Entity<PartyElection>()
                .HasOne(pe => pe.Party)
                .WithMany(p => p.PartyElections)
                .HasForeignKey(pe => pe.PartyId);

            // Configure relationship with Election
            modelBuilder.Entity<PartyElection>()
                .HasOne(pe => pe.Election)
                .WithMany(e => e.PartyElections)
                .HasForeignKey(pe => pe.ElectionId);
        }

    }
}
