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

                Console.WriteLine("all movies requested...");

                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    var response = db.Query<Movie>
                        ("SELECT * FROM [dbo].[Movies]").ToList();
                    //if (!response)
                    //{
                    //    return NotFound();
                    //}
                    Console.WriteLine("all movies retrieved");
                    return Ok(response);
                }
            }
           catch (SqlException error)
            {
                Console.WriteLine("something went wrong");
                return StatusCode(500, error.ToString());
            }
        }

        [HttpGet]
        [Route("MoviesAPI/Movies/movie-details/{MovieID:int}")]
        public ActionResult<Movie> GetById(int MovieID)
        {
            var connectionString = "server = (local); user id = sa; " +
            "password=dvc1174580;initial catalog=MoviesDB";

            Console.WriteLine($"user requested details for movie with MovieID: {MovieID}");

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var oneMovie = db.Query<Movie>
                    ("SELECT * FROM [dbo].[Movies]" + "WHERE MovieID = @MovieID", new { MovieID }).SingleOrDefault();
                if (oneMovie == null)
                {
                    // {message: "movie not found"}
                    Console.WriteLine("user requested movie does not exist");
                    return NotFound(new { message = "Movie not found" });
                }

                Console.WriteLine("requested movie found");
                return Ok(oneMovie);
            }
        }
       
        [HttpPost]
        [Route("MoviesAPI/Movies/Create-Movie")]
        public ActionResult<Movie> AddMovie(string movieName, int ageRating, float price, DateTime releaseDate, string genre)
        {
            //return Ok("add movie to database");

            // create a Movie instance
            try
            {

                Movie movieInstance = new Movie(movieName, ageRating, price, releaseDate, genre);
                //Console.WriteLine(movieInstance.ReleaseDate);

                var connectionString = "server = (local); user id = sa; " +
                    "password=dvc1174580;initial catalog=MoviesDB";

                Console.WriteLine("user requested movie creation");

                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    // instead of Values (@....)
                    // Values (MovieInstance.MovieName...)

                    string newMovieQuery = @"INSERT INTO Movies(MovieName, AgeRating, Price, ReleaseDate, Genre) " +
                        "VALUES (@MovieName, @AgeRating, @Price, @ReleaseDate, @Genre)";

                    //var result = db.Execute(newMovieQuery, new
                    //{
                    //    MovieName = movieName,
                    //    AgeRating = ageRating,
                    //    Price = price,
                    //    ReleaseDate = releaseDate,
                    //    Genre = genre,
                    //});

                    var result = db.Execute(newMovieQuery, movieInstance);

                    // if result = 1 return Ok(MovieInstance);
                    Console.WriteLine("movie successfully created");
                    return Ok(movieInstance); // just says 1 item has been created     
                }
            }
            catch
            {
                return BadRequest(new { message = "Please enter the correct information to create a movie" });
            }
        }

        [HttpDelete("{movieID}")]
        [Route("MoviesAPI/Movies/Delete-Movie/{movieID:int}")]
        public ActionResult<Movie> DeleteById(int movieID)
        {
            var connectionString = "server = (local); user id = sa; " +
                "password=dvc1174580;initial catalog=MoviesDB";

            Console.WriteLine($"user requested deletion of movie with MovieID: {movieID}");

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string deleteQuery = "DELETE FROM [dbo].[Movies] WHERE MovieID = @MovieID";
                var result = db.Execute(deleteQuery, new { MovieID = movieID });

                Console.WriteLine("movie successfully deleted");
                return Ok(result);
            }
        }




    }
}
