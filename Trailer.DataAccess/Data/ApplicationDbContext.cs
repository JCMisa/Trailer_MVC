using Trailer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Trailer.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        }

        public DbSet<Category> Categories { get; set; } //create Categories table inside Trailer database

        public DbSet<Movie> Movies { get; set; } //create Movies table inside Trailer database

        public DbSet<ApplicationUser> ApplicationUsers { get; set; } //create Users table inside Trailer database


		//hard coding the adding of data for each column in Categories table
		/*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                    new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                    new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                    new Category { Id = 3, Name = "History", DisplayOrder = 3 }
                );
            base.OnModelCreating(modelBuilder);
        }
        */
	}
}
