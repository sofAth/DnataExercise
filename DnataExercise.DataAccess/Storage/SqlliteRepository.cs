using DnataExercise.Common.Entities;
using DnataExercise.Common.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DnataExercise.DataAccess.Storage {
    //
    // Thanos:
    // I chose sqlite because it's light and portable and easy to set up; doesn't 
    // require any configuration. Any class that implements the "IRepository" 
    // interface can be used. Liskov-Substitution (SOLID). There are no 
    // hard-references to this class. Changing the implementation of this
    // interface is just a matter of re-configuring the "Program.cs" file of the 
    // main project
    //


    /// <summary>
    /// A sqlite implementation of the "IRepository" interface
    /// </summary>
    public class SqlliteRepository : IRepository {
        private readonly ILogger<IRepository> _logger;
        private readonly DatabaseContext _context;

        public SqlliteRepository(ILogger<IRepository> logger,
            DatabaseContext context) {
            _logger = logger;
            _context = context;

            _logger.LogInformation($"{nameof(SqlliteRepository)} is initializing");
        }

        public IEnumerable<Movie> GetAllMovies() {
            var result = _context.Movies;

            _logger.LogInformation($"Retrieving movies with count: {result.Count()}");

            return result;
        }

        public IEnumerable<Movie> GetMovies(Expression<Func<Movie, bool>> predicate) {
            var result = _context.Movies
                .Where(predicate)
                .ToArray();

            _logger.LogInformation($"Retrieving movies with count: {result.Count()}");

            return result;
        }

        public IEnumerable<TotalUserRating> GetTopMovies(int count) {
            var groups = from userRatings in _context.UserRatings.ToArray()
                         group userRatings by userRatings.MovieID into groupedUserRatings
                         where groupedUserRatings.Count() != 0
                         join movie in _context.Movies on groupedUserRatings.First().MovieID equals movie.ID
                         let average = groupedUserRatings.Average(x => x.Rating)
                         orderby average descending, movie.Title descending
                         select new TotalUserRating(movie, average);

            _logger.LogInformation($"Retrieving GetTopMovies with count: {groups.Count()}");

            return groups.Take(5).ToArray();
        }

        public IEnumerable<TotalUserRating> GetTopMoviesByUser(int count, int userID) {
            var groups = from userRatings in _context.UserRatings.ToArray()
                         where userRatings.UserID == userID
                         group userRatings by userRatings.MovieID into groupedUserRatings
                         where groupedUserRatings.Count() != 0 
                         join movie in _context.Movies on groupedUserRatings.First().MovieID equals movie.ID
                         let average = groupedUserRatings.Average(x => x.Rating)
                         orderby average descending, movie.Title descending
                         select new TotalUserRating(movie, average);

            _logger.LogInformation($"Retrieving GetTopMoviesByUser ({userID}) with count: {groups.Count()}");

            return groups.Take(5).ToArray();
        }

        public void SetRatingForUser(int userID, int movieID, int rating) {
            var userRating = _context.UserRatings
                 .FirstOrDefault(x => x.UserID == userID && x.MovieID == movieID);

            if (userRating == null) {
                // New
                var ur = new UserRating();
                ur.UserID = userID;
                ur.MovieID = movieID;
                ur.Rating = rating;

                _context.UserRatings.Add(ur);
                _context.SaveChanges();

                _logger.LogInformation($"Saved new UserRating");
            }
            else { 
                // Existing
                userRating.Rating = rating;
                _context.Entry(userRating).State = EntityState.Modified;
                _context.SaveChanges(true);

                _logger.LogInformation($"Updated existing UserRating");
            }

        }
    }
}
