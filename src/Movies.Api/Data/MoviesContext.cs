using Microsoft.EntityFrameworkCore;
using Movies.Api.Models;

namespace Movies.Api.Data
{
    public class MoviesContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public MoviesContext (DbContextOptions<MoviesContext> options) : base(options)
        {
        }
    }
}
