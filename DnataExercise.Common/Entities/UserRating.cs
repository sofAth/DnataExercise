using System;
using System.Collections.Generic;

namespace DnataExercise.Common.Entities {
    public partial class UserRating {
        public long? MovieID { get; set; }
        public long? UserID { get; set; }
        public long? Rating { get; set; }
    }
}
