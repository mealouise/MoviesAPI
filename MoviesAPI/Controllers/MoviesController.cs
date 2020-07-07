using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models;
using MoviesAPI.DAL;
using Dapper;
using System.Data;
using System.Data.SqlClient;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MoviesAPI.Controllers
{
    public class MoviesController : Controller
    {
        private MovieRepository movieRepository;

        public MoviesController()
        {
            movieRepository = new MovieRepository();
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("MoviesAPI/Movies/All-Movies")]
        public ActionResult<List<Movie>> AllMovies()
        {
            try
            {

                Console.WriteLine("all movies requested...");

                //    //if (!response)
                //    //{
                //    //    return NotFound();
                //    //}
                //    return Ok(response);
                //}

                List<Movie> res = movieRepository.GetAllMoviesFromDB();

                if (res == null)
                {
                    return NotFound();
                }
                else
                {
                    Console.WriteLine("all movies retrieved");
                    return Ok(res);
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
            if (!Validation.ValidateID(MovieID)) //needs to not be true ie movieID not be valid to return a badrequest
            {
                return BadRequest(new { message = "invalid MovieID, please provide ID greater than 0" });
            }

            Console.WriteLine($"user requested details for movie with MovieID: {MovieID}");
            try
            {
                //if (oneMovie == null)
                //{
                //    Console.WriteLine("user requested movie does not exist");
                //    return NotFound(new { message = "Movie not found" });
                //}

                //Console.WriteLine("requested movie found");
                //return Ok(oneMovie);
                //}

                Movie oneMovie = movieRepository.GetMovieFromDB(MovieID);

                if (oneMovie == null)
                {
                    Console.WriteLine("user requested movie does not exist");
                    return NotFound(new { message = "Movie not found" });
                }
                else
                {
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
        public ActionResult<Movie> AddMovie(string MovieName, int AgeRating, double Price, DateTime ReleaseDate, string Genre)
        {
            //return BadRequest(new { message = "Please enter the correct information to create a movie" });
            //return Ok("add movie to database");
            Movie movieInstance = new Movie()
            {
                MovieName = MovieName,
                AgeRating = AgeRating,
                Price = Price,
                ReleaseDate = ReleaseDate,
                Genre = Genre
            };

            if (!Validation.ValidateMovie(movieInstance))
            {
                return BadRequest(new { message = "Please enter valid values" });
            }

            try
            {

                // if result = 1 return Ok(MovieInstance);
                // Console.WriteLine("movie successfully created");
                // return Ok(movieInstance);


                Console.WriteLine("user requested movie creation");
                Movie returnedMovie = movieRepository.AddMovieToDB(MovieName, AgeRating, Price, ReleaseDate, Genre);
                Console.WriteLine("movie successfully created");
                return Ok(returnedMovie);

            }
            catch (SqlException error)
            {
                Console.WriteLine("something went wrong");
                return StatusCode(500, error.ToString());
            }
        }

        [HttpDelete] // {movieid}
        [Route("MoviesAPI/Movies/Delete-Movie/{MovieID}")] //accept any entered 'id' then check if int
        public ActionResult<Movie> DeleteById([FromRoute]int MovieID)
        {
            if (!Validation.ValidateID(MovieID))
            {
                return BadRequest(new { message = "invalid MovieID, please provide ID greater than 0" });
            }

            Console.WriteLine($"user requested deletion of movie with MovieID: {MovieID}");
            try
            {

                //    if (result == 1)
                //    {
                //        Console.WriteLine("movie successfully deleted");
                //        return Ok(result);
                //    }
                //    else
                //    {
                //        Console.WriteLine("user requested movie does not exist");
                //        return NotFound(new { message = "Movie not found" });
                //    }

                //}
                Console.WriteLine("movie successfully deleted");
                List<Movie> res = movieRepository.DeleteMovieFromDB(MovieID);
                return Ok(res);


            }
            catch (SqlException error)
            {
                Console.WriteLine("something went wrong");
                return StatusCode(500, error.ToString());
            }

        }
         




    }
}
