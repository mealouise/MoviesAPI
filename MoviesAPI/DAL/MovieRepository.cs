using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using MoviesAPI.Models;

namespace MoviesAPI.DAL
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IDbConnection db;

        public MovieRepository()
        {
            //db = new SqlConnection("server = (local); user id = sa; " +
            //    "password=dvc1174580;initial catalog=MoviesDB");
            db = new SqlConnection("Server=tcp:talktalkprojectssqlserver.database.windows.net,1433;Initial Catalog=talktalkprojectssqldatabase;Persist Security Info=False;User ID=emma;Password=Codenation12;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            //string newConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            //db = new SqlConnection(newConnection);
        }

        public List<Movie> GetAllMoviesFromDB()
        {
            return db.Query<Movie>("GetAllMovies", commandType: CommandType.StoredProcedure).ToList();
            //"SELECT * FROM [dbo].[Movies]".ToList());
        }

        public Movie GetMovieFromDB(int MovieID)
        {
            return db.Query<Movie>("GetMovie", new { MovieID }, commandType: CommandType.StoredProcedure).SingleOrDefault();
        }

        public Movie AddMovieToDB(string MovieName, int AgeRating, double Price, DateTime ReleaseDate, string Genre)
        {
            Movie movieInstance = new Movie()
            {
                MovieName = MovieName,
                AgeRating = AgeRating,
                Price = Price,
                ReleaseDate = ReleaseDate,
                Genre = Genre
            };

            //string newMovieQuery = @"INSERT INTO Movies(MovieName, AgeRating, Price, ReleaseDate, Genre) " + "VALUES (@MovieName, @AgeRating, @Price, @ReleaseDate, @Genre)";

            //var result = db.Execute(newMovieQuery, movieInstance);
            db.Execute("InsertMovie", new { MovieName, AgeRating, Price, ReleaseDate, Genre }, commandType: CommandType.StoredProcedure);

            return movieInstance;

            //if (result == 1)
            //{
            //    return movieInstance;
            //}
        }

        public List<Movie> DeleteMovieFromDB(int MovieID)
        {
            //string deleteQuery = "DELETE FROM [dbo].[Movies] WHERE MovieID = @MovieID";
            //var result = db.Execute(deleteQuery, new { MovieID });
            db.Execute("DeleteMovie", new { MovieID }, commandType: CommandType.StoredProcedure);

            return db.Query<Movie>("GetAllMovies", commandType: CommandType.StoredProcedure).ToList();
        }
    }
}
