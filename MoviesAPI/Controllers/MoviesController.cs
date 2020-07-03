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
        [Route("MoviesAPI/Movies/movie-details/{MovieID}")] //removed int from the route otherwise can't reach badrequest
        public ActionResult<Movie> GetById(int MovieID)
        {
            if (Validation.ValidateID(MovieID)) //ADD TO OTHER METHODS
            {
                return BadRequest(new { message = "invalid MovieID, please provide ID greater than 0" });
            }
            var connectionString = "server = (local); user id = sa; " +
                "password=dvc1174580;initial catalog=MoviesDB";

            Console.WriteLine($"user requested details for movie with MovieID: {MovieID}");
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    var oneMovie = db.Query<Movie>
                        ("SELECT * FROM [dbo].[Movies]" + "WHERE MovieID = @MovieID", new { MovieID }).SingleOrDefault();
                    if (oneMovie == null)
                    {
                        Console.WriteLine("user requested movie does not exist");
                        return NotFound(new { message = "Movie not found" });
                    }

                    Console.WriteLine("requested movie found");
                    return Ok(oneMovie);
                }
            }
            catch (SqlException error)
            {
                Console.WriteLine("something went wrong");
                return StatusCode(500, error.ToString());
            }
        }
           
       
        [HttpPost]
        [Route("MoviesAPI/Movies/Create-Movie")]
        public ActionResult<Movie> AddMovie(string movieName, int ageRating, double price, DateTime releaseDate, string genre)
        {
            //return BadRequest(new { message = "Please enter the correct information to create a movie" });
            //return Ok("add movie to database");
            Movie movieInstance = new Movie()
            {
                MovieName = movieName,
                AgeRating = ageRating,
                Price = price,
                ReleaseDate = releaseDate,
                Genre = genre
            };

            if (!Validation.ValidateMovie(movieInstance))
            {
                return BadRequest(new { message = "Please enter valid values" });
            }
            
            try
            {

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
                    return Ok(movieInstance);
                       
                }
            }
            catch (SqlException error)
            {
                Console.WriteLine("something went wrong");
                return StatusCode(500, error.ToString());
            }
        }

        [HttpDelete("{movieID}")]
        [Route("MoviesAPI/Movies/Delete-Movie/{movieID}")] //accept any entered 'id' then check if int
        public ActionResult<Movie> DeleteById(int movieID)
        {
            if (Validation.ValidateID(movieID))
            {
                return BadRequest(new { message = "invalid MovieID, please provide ID greater than 0" });
            }

            var connectionString = "server = (local); user id = sa; " +
                "password=dvc1174580;initial catalog=MoviesDB";

            Console.WriteLine($"user requested deletion of movie with MovieID: {movieID}");
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string deleteQuery = "DELETE FROM [dbo].[Movies] WHERE MovieID = @MovieID";
                    var result = db.Execute(deleteQuery, new { MovieID = movieID });

                    if (result == 1)
                    {
                        Console.WriteLine("movie successfully deleted");
                        return Ok(result);
                    }
                    else
                    {
                        Console.WriteLine("user requested movie does not exist");
                        return NotFound(new { message = "Movie not found" });
                    }
             
                }
            }
            catch (SqlException error)
            {
                Console.WriteLine("something went wrong");
                return StatusCode(500, error.ToString());
            }

        }
         




    }
}
