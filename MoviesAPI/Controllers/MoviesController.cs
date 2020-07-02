using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MoviesAPI.Controllers
{
    public class MoviesController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route ("MoviesAPI/Movies/All-Movies")]
        public ActionResult<List<Movie>>AllMovies()
        {
            try
            {
                var connectionString = "server = (local); user id = sa; " +
                "password=dvc1174580;initial catalog=MoviesDB";

                //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                //builder.DataSource = "localhost,1433";
                //builder.UserID = "sa";
                //builder.Password = "password999>";
                //builder.InitialCatalog = "Movies";

                Console.WriteLine("connecting to SQL server");

                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    var response = db.Query<Movie>
                        ("SELECT * FROM [dbo].[Movies]").ToList();
                    //if (!response)
                    //{
                    //    return NotFound();
                    //}
                    return Ok(response);
                }
            }
           catch (SqlException error)
            {
                return Ok(error.ToString());
            }
        }

        [HttpGet]
        [Route("MoviesAPI/Movies/movie-details/{MovieID:int}")]
        public ActionResult<Movie> GetById(int MovieID)
        {
            // is it an int?
            var connectionString = "server = (local); user id = sa; " +
            "password=dvc1174580;initial catalog=MoviesDB";

            Console.WriteLine("now connecting to SQL server - getting movie details...");

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var oneMovie = db.Query<Movie>
                    ("SELECT * FROM [dbo].[Movies]" + "WHERE MovieID = @MovieID", new { MovieID }).SingleOrDefault();
                if (oneMovie == null)
                {
                    // {message: "movie not found"}
                    return NotFound(new { message = "Movie not found" });
                }
                return Ok(oneMovie);
            }
        }
       
        [HttpPost]
        [Route("MoviesAPI/Movies/Create-Movie")]
        public ActionResult<Movie> AddMovie(string movieName, int ageRating, float price, DateTime releaseDate, string genre)
        {
            //return Ok("add movie to database");

            // create a Movie instance

            var connectionString = "server = (local); user id = sa; " +
                "password=dvc1174580;initial catalog=MoviesDB";

            Console.WriteLine("now connecting to SQL server");
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                // instead of Values (@....)
                // Values (MovieInstance.MovieName...)

                string newMovieQuery = @"INSERT INTO Movies(MovieName, AgeRating, Price, ReleaseDate, Genre) " +
                    "VALUES (@MovieName, @AgeRating, @Price, @ReleaseDate, @Genre)";

                var result = db.Execute(newMovieQuery, new
                {
                    MovieName = movieName,
                    AgeRating = ageRating,
                    Price = price,
                    ReleaseDate = releaseDate,
                    Genre = genre,
                });

                // if result = 1 return Ok(MovieInstance);

                return Ok(result); // just says 1 item has been created     
            }
        }

        [HttpDelete("{movieID}")]
        [Route("MoviesAPI/Movies/Delete-Movie/{movieID:int}")]
        public ActionResult<Movie> DeleteById(int movieID)
        {
            var connectionString = "server = (local); user id = sa; " +
                "password=dvc1174580;initial catalog=MoviesDB";

            Console.WriteLine("now connecting to SQL server");
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string deleteQuery = "DELETE FROM [dbo].[Movies] WHERE MovieID = @MovieID";
                var result = db.Execute(deleteQuery, new { MovieID = movieID });
                return Ok(result);
            }
        }




    }
}
