using DnataExercise.Common.Entities;
using DnataExercise.Common.Infrastructure.Extensions;
using DnataExercise.Common.Storage;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace DnataExercise.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repository;

        public HomeController(IRepository repository,
            ILogger<HomeController> logger) {
            _repository = repository;
            _logger = logger;
        }

        public IActionResult Index() {
            return View();
        }

        [HttpGet("GetMoviesWhere")]
        public IActionResult GetMoviesWhere(string title, int yearOfRelease, string genre) {
            try {

                if (string.IsNullOrEmpty(title) && yearOfRelease == -1 && genre == "*") {
                    _logger.LogInformation("GetMoviesWhere: Invalid arguments");
                    return StatusCode(400, "Invalid arguments");
                }

                //
                // Build up the expression that will be send to the repository. It can be extended in the future
                // to support more filtering options. I would've the expression on the controller's argument
                // list but serialisation wouldn't be easy
                // 
                var criteria = new List<Expression<Func<Movie, bool>>>();
                if (!string.IsNullOrEmpty(title)) {
                    criteria.Add(x => x.Title == title);
                }

                if (yearOfRelease != -1) {
                    criteria.Add(x => x.YearOfRelease == yearOfRelease);
                }

                if (!string.IsNullOrEmpty(genre)) {
                    criteria.Add(x => x.Genre == genre);
                }

                var lambda = EnumerableExtensions.AnyOf(criteria.ToArray());
                var movies = _repository.GetMovies(lambda);

                _logger.LogInformation($"GetMoviesWhere: Returning {movies.Count()}");

                return Ok(movies);
            }
            catch (Exception ex) {
                _logger.LogError(null, ex);
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

       

        [HttpGet("GetTopMovies")]
        public IActionResult GetTopMovies() {
            try {
                var totalUserRatings = _repository.GetTopMovies(5);
                if (totalUserRatings.Count() == 0) {
                    _logger.LogInformation("GetTopMovies: No movies found");
                    return StatusCode(404, "No movies found");
                }

                _logger.LogInformation($"GetTopMovies: Returning {totalUserRatings.Count()}");

                return Ok(totalUserRatings);
            }
            catch (Exception ex) {
                _logger.LogError(null, ex);
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("GetTopMoviesByUser")]
        public IActionResult GetTopMoviesByUser(int userID) {
            try {
                var userRatings = _repository.GetTopMoviesByUser(5, userID);
                if (userRatings.Count() == 0) {
                    _logger.LogInformation("GetTopMoviesByUser: No movies found");
                    return StatusCode(404, "No movies found");
                }

                _logger.LogInformation($"GetTopMoviesByUser: Returning {userRatings.Count()}");

                return Ok(userRatings);
            }
            catch (Exception ex) {
                _logger.LogError(null, ex);
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost("SetRatingForUser")]
        public IActionResult SetRatingForUser(int userID, int movieID, int rating) {
            try {

                if (rating < 0 || rating > 5) {
                    _logger.LogInformation("SetRatingForUser: Invalid rating");
                    return StatusCode(404, "Invalid rating");
                }

                _repository.SetRatingForUser(userID, movieID, rating);
                
                _logger.LogInformation($"SetRatingForUser: Success for arguments: userID: {userID}, movieID: {movieID}, rating: {rating}");

                return Ok();
            }
            catch (Exception ex) {
                _logger.LogError(null, ex);
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
