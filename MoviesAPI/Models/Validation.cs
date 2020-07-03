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
    }
}


