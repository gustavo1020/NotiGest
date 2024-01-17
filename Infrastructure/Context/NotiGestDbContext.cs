using Core.Entityes;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Context
{
    public class NotiGestDbContext : IdentityDbContext<User, Rol, Guid>
    {
        public NotiGestDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Comentario> Comentario { get; set; }
        public DbSet<Noticia> Noticia { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .Property(d => d.CreatedDate)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<User>()
                .Property(d => d.LastUpdateDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<User>()
                .Property(d => d.CreatedDate)
                .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Entity<User>()
                .Property(d => d.CreatedDate)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Entity<User>()
                .Property(d => d.LastUpdateDate)
                .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Entity<User>()
                .Property(d => d.LastUpdateDate)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);


            builder.Entity<Rol>()
                .Property(d => d.CreatedDate)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Rol>()
                .Property(d => d.LastUpdateDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Rol>()
                .Property(d => d.CreatedDate)
                .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Entity<Rol>()
                .Property(d => d.CreatedDate)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Entity<Rol>()
                .Property(d => d.LastUpdateDate)
                .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Entity<Rol>()
                .Property(d => d.LastUpdateDate)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);


            builder.Entity<Noticia>()
                .Property(d => d.CreatedDate)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Noticia>()
                .Property(d => d.LastUpdateDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Noticia>()
                .Property(d => d.CreatedDate)
                .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Entity<Noticia>()
                .Property(d => d.CreatedDate)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Entity<Noticia>()
                .Property(d => d.LastUpdateDate)
                .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Entity<Noticia>()
                .Property(d => d.LastUpdateDate)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);


            builder.Entity<Comentario>()
                .Property(d => d.CreatedDate)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Comentario>()
                .Property(d => d.LastUpdateDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Comentario>()
                .Property(d => d.CreatedDate)
                .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Entity<Comentario>()
                .Property(d => d.CreatedDate)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Entity<Comentario>()
                .Property(d => d.LastUpdateDate)
                .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Entity<Comentario>()
                .Property(d => d.LastUpdateDate)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Entity<Comentario>()
                .HasOne(c => c.Noticia)
                .WithMany(n => n.Comentarios)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Noticia>()
                .HasOne(n => n.Autor)
                .WithMany()
                .HasForeignKey(n => n.AutorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Comentario>()
               .HasOne(n => n.Autor)
               .WithMany()
               .HasForeignKey(n => n.AutorId)
               .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }
    }
}
