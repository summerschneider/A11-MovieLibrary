﻿namespace MovieLibraryEntities.Models
{
    public class Movie
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }

        //entity framework navigation properties
        public virtual ICollection<MovieGenre> MovieGenres { get; set; }  //relationship between movies and genres
        public virtual ICollection<UserMovie> UserMovies { get; set; }  //relationship between users and movies
    }
}
