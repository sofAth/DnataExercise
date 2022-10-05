using System;
using System.Collections.Generic;

namespace DnataExercise.Common.Entities {
    public partial class Movie {
        public long ID { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public long? YearOfRelease { get; set; }
        public long? RunningTime { get; set; }
    }
}
