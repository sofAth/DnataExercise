using DnataExercise.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DnataExercise.Common.Storage {

    //
    // Thanos:
    // An interface to separate the database layer from the controller
    // Interface-segregation and Dependency Injection according to SOLID
    //

    /// <summary>
    /// The interface to be implemented by classes that provide data persistance. As long as 
    /// the methods of the interface are implemented, it can be anything. e.g.: Memory, XML files
    /// another Web API or any DBMS.
    /// </summary>
    public interface IRepository {

        /// <summary>
        /// When implemented, should return all available movies.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Movie> GetAllMovies();

        /// <summary>
        /// Receives a predicate which executes against each movie. If the predicate for a particular movie returns true, 
        /// then the movie is included in the results.
        /// </summary>
        /// <param name="predicate">The predicate to execute against each movie</param>
        /// <returns>A array of movies that satisfy the predicate condition</returns>
        IEnumerable<Movie> GetMovies(Expression<Func<Movie, bool>> predicate);

        /// <summary>
        /// When implemented, should return the top movies based on the ratings of all users. 
        /// Note: The exercise mentions that I should return the top 5 movies. I chose to keep 
        /// this as an argument for flexibility. The controller indeed uses 5 for this argument.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        IEnumerable<TotalUserRating> GetTopMovies(int count);

        /// <summary>
        /// When implemented, should return the top movies based on the ratings of a 
        /// specific user. 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="userID">The user id for which to return the movies for</param>
        /// <returns></returns>
        IEnumerable<TotalUserRating> GetTopMoviesByUser(int count, int userID);


        /// <summary>
        /// When implemented, should set the rating for a movie for a particular user.
        /// If the user already has a rating for that movie, the method should update
        /// the existing rating
        /// </summary>
        /// <param name="userID">The user id</param>
        /// <param name="movieID">The movie for which to set the rating</param>
        /// <param name="rating">The rating. Should be between 0 and 5</param>
        void SetRatingForUser(int userID, int movieID, int rating);
    }
}
