using System;
using System.Collections.Generic;
using MoviesAPI.Models;
namespace MoviesAPI.DAL
{
    internal interface IMovieRepository
    {
        List<Movie> GetAllMoviesFromDB();
        Movie GetMovieFromDB(int MovieID);
        Movie AddMovieToDB(string MovieName, int AgeRating, double Price, DateTime ReleaseDate, string Genre);
        List<Movie> DeleteMovieFromDB(int MovieID);
    }
}
