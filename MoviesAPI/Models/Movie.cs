using System;
namespace MoviesAPI.Models
{
    public class Movie
    {
        public int MovieID { get; set; }
        public string MovieName { get; set; }
        public int AgeRating { get; set; }
        public double Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }


        //public Movie(string movieName, int ageRating, float price, DateTime releaseDate, string genre)
        //{
        //    MovieName = movieName;
        //    AgeRating = ageRating;
        //    Price = price;
        //    ReleaseDate = releaseDate;
        //    Genre = genre;
        //}
    }
}
