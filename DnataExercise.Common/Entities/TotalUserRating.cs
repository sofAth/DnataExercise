using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnataExercise.Common.Entities {

    [DebuggerDisplay("Movie: {Movie.Title}, Rating: {Rating}")]
    public class TotalUserRating {

        public TotalUserRating(Movie movie, double? rating) {
            Movie = movie;
            Rating = rating;
        }

        public Movie Movie { get; init; }

        public double? Rating { get; init; }
    }
}
