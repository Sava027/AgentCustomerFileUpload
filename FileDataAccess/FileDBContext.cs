using Microsoft.EntityFrameworkCore;

namespace AgentCustomer.FileDataAccess
{
    public class FileDBContext : DbContext
    {
        public FileDBContext(DbContextOptions<FileDBContext> options)
            : base(options) { }

         public DbSet<FileInfo> FileInfos { get; set; }
        public DbSet<FileTracking> FileTrackings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entity filters           
            modelBuilder.Entity<FileInfo>().HasQueryFilter(p => p.IsDeleted == false);
            modelBuilder.Entity<FileTracking>().HasQueryFilter(p => p.IsDeleted == false);
        

        }

        public async Task<int> SaveChanges(string userName= "DefaultUser")
        {
            var entities = ChangeTracker.Entries().Where(x =>
                x.State == EntityState.Added ||
                x.State == EntityState.Modified ||
                x.State == EntityState.Deleted);

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).DateCreated = DateTime.UtcNow;
                    ((BaseEntity)entity.Entity).UserCreated = userName;
                }

                if (entity.State == EntityState.Deleted || ((BaseEntity)entity.Entity).IsDeleted)
                {
                    ((BaseEntity)entity.Entity).UserDeleted = userName;
                    ((BaseEntity)entity.Entity).DateDeleted = DateTime.UtcNow;
                    ((BaseEntity)entity.Entity).IsDeleted = true;
                }
                ((BaseEntity)entity.Entity).DateUpdated = DateTime.UtcNow;
                ((BaseEntity)entity.Entity).UserUpdated = userName;
            }

            return await base.SaveChangesAsync();
        }
    }
}