using System;
namespace MoviesAPI.Models
{
    public class Validation
    {
        static public Boolean ValidateID(int MovieID) //can never be null, will convert null to 0
        {

            if (MovieID == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //static public Movie ValidateMovie(string movieName, int ageRating, float price, DateTime releaseDate, string genre)
        //{
        //    if (movieName.GetType() == typeof(string) && ageRating.GetType() == typeof(int) && price.GetType() == typeof(float) && DateTime.GetType() == && genre.GetType() == typeof(string)
        //    {

        //    }
        //}

        static public Boolean ValidateMovie(Movie movie)
        {
            DateTime tooEarly = new DateTime(1900, 01, 01);
            int compare = movie.ReleaseDate.CompareTo(tooEarly);
            if (movie.AgeRating == 0 || movie.Price == 0 || compare < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}


