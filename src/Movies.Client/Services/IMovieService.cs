using Movies.Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Client.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetMovies();

        Task<Movie> GetMovie(int id);

        Task<bool> CreateMovie(Movie movie);
        
        Task<bool> UpdateMovie(int id, Movie movie);

        Task<bool> DeleteMovie(int id);

        Task<UserInfoViewModel> GetUserInfo();
    }
}
