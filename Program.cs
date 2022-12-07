using Microsoft.EntityFrameworkCore;
using MovieLibraryEntities.Context;
using MovieLibraryEntities.Models;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;

namespace MovieLibraryEntities
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = LoggerFactory.Create(b => b.AddConsole());
            var logger = factory.CreateLogger<Program>();

            string userInput;
            do
            {
                Console.WriteLine("Enter your selection:");
                Console.WriteLine("1) Display movie\n2) Add movie\n3) Update movie\n4) Delete movie\n5) Add user\n6) Add movie rating\n7) Exit");
                userInput = Convert.ToString(Console.ReadLine());

                switch (userInput)
                {
                    case "1":  //search movie
                        {
                            using (var db = new MovieContext())
                            {
                                Console.WriteLine("Would you like to search a specific title or view all movies?");
                                Console.WriteLine("1) Search by movie title\n2) View all movies");
                                string userInput2 = Console.ReadLine();
                                switch (userInput2)
                                {
                                    case "1":  //search by title
                                        {
                                            Console.WriteLine("Input the title of the movie you would like to search for:");
                                            var movieUserInput = Console.ReadLine();
                                            bool isNullInput = string.IsNullOrEmpty(movieUserInput);

                                            if (isNullInput == false)
                                            {
                                                var displayMovie = db.Movies.Include(x => x.MovieGenres).ThenInclude(x => x.Genre).Where(x => x.Title.ToUpper().Contains(movieUserInput.ToUpper()));

                                                if (displayMovie is not null)
                                                {
                                                    foreach (var movie in displayMovie)
                                                    {
                                                        Console.WriteLine($"Movie {movie.Id}: {movie.Title}, Release Date: {movie.ReleaseDate}");
                                                        foreach (var genre in movie.MovieGenres)
                                                        {
                                                            Console.WriteLine($"\tGenre {genre.Genre.Id}: {genre.Genre.Name}");
                                                        }
                                                        Console.WriteLine();
                                                    }
                                                    logger.LogInformation("Displaying movies.");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Inputted movie title does not exist\n");
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Movie title cannot be null\n");
                                                break;
                                            }
                                            break;
                                        }
                                    case "2":  //display all movies
                                        {
                                            Console.WriteLine("Here is the list of movies:\n---------------------------");
                                            foreach (var movie in db.Movies)
                                            {
                                                Console.WriteLine($"Movie {movie.Id}: {movie.Title}, Release Date: {movie.ReleaseDate}");
                                            }
                                            logger.LogInformation("Displaying movies.");
                                            Console.WriteLine();
                                            break;
                                        }
                                    default:
                                        {
                                            Console.WriteLine("Incorrect input\n");
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    case "2":  //add movie
                        {
                            using (var db = new MovieContext())
                            {
                                Console.WriteLine("Input the title of the movie you would like to add:");
                                var titleUserInput = Console.ReadLine();
                                bool isNullTitle = string.IsNullOrEmpty(titleUserInput);

                                if (isNullTitle == false)
                                {
                                    Console.WriteLine("Input the release date of the movie you would like to add (dd/mm/yyyy):");
                                    DateTime releaseDate;
                                    if (DateTime.TryParse(Console.ReadLine(), out releaseDate))
                                    {

                                    }
                                    else
                                    {
                                        Console.WriteLine("Incorrect input of release date\n");
                                        break;
                                    }

                                    var movie = new Movie()
                                    {
                                        Title = titleUserInput,
                                        ReleaseDate = releaseDate
                                    };
                                    db.Movies.Add(movie);
                                    logger.LogInformation("Adding movie.");
                                    db.SaveChanges();

                                    var newMovie = db.Movies.Where(x => x.Title == titleUserInput).FirstOrDefault();
                                    Console.WriteLine($"Movie {newMovie.Id}: {newMovie.Title}, Release Date: {newMovie.ReleaseDate}");
                                    Console.WriteLine("\tThis movie has been added\n");
                                }
                                else
                                {
                                    Console.WriteLine("Movie title cannot be null\n");
                                    break;
                                }
                            }
                            break;
                        }
                    case "3":  //update movie
                        {
                            using (var db = new MovieContext())
                            {
                                Console.WriteLine("Input the title of the movie you would like to update:");
                                var titleUserInput1 = Console.ReadLine();

                                var movieToUpdate = db.Movies.FirstOrDefault(x => x.Title.ToUpper().Contains(titleUserInput1.ToUpper()));
                                Console.WriteLine($"Movie {movieToUpdate.Id}: {movieToUpdate.Title}, Release Date: {movieToUpdate.ReleaseDate}\n");

                                Console.WriteLine("Input the updated title of the movie:");
                                var titleUpdate = Console.ReadLine();
                                bool isNullTitleUpdate = string.IsNullOrEmpty(titleUpdate);

                                if (isNullTitleUpdate == false)
                                {
                                    Console.WriteLine("Input the updated release date of the movie (dd/mm/yyyy):");
                                    DateTime releaseDateUpdate;
                                    if (DateTime.TryParse(Console.ReadLine(), out releaseDateUpdate))
                                    {

                                    }
                                    else
                                    {
                                        Console.WriteLine("Incorrect input of release date\n");
                                        break;
                                    }

                                    var updateMovie = db.Movies.FirstOrDefault(x => x.Title.ToUpper().Contains(titleUserInput1.ToUpper()));

                                    updateMovie.Title = titleUpdate;
                                    updateMovie.ReleaseDate = releaseDateUpdate;

                                    db.Movies.Update(updateMovie);
                                    logger.LogInformation("Updating movie.");
                                    db.SaveChanges();

                                    Console.WriteLine("\nHere is the updated movie information:");
                                    Console.WriteLine($"Movie {updateMovie.Id}: {updateMovie.Title}, Release Date: {updateMovie.ReleaseDate}\n");
                                }
                                else
                                {
                                    Console.WriteLine("Movie title cannot be null\n");
                                    break;
                                }
                            }
                            break;
                        }
                    case "4":  //delete movie
                        {
                            using (var db = new MovieContext())
                            {
                                string yesOrNo;
                                do
                                {
                                    Console.WriteLine("Input the title of the movie you would like to delete:");
                                    var titleUserInput2 = Console.ReadLine();
                                    bool isNullInput2 = string.IsNullOrEmpty(titleUserInput2);

                                    if (isNullInput2 == false)
                                    {
                                        var deleteMovie = db.Movies.FirstOrDefault(x => x.Title.ToUpper().Contains(titleUserInput2.ToUpper()));
                                        if (deleteMovie is not null)
                                        {
                                            Console.WriteLine($"Movie {deleteMovie.Id}: {deleteMovie.Title}, Release Date: {deleteMovie.ReleaseDate}");

                                            Console.WriteLine("\nIs this the movie you would like to delete? (1-Yes, 2-No)");
                                            yesOrNo = Convert.ToString(Console.ReadLine());
                                            switch (yesOrNo)
                                            {
                                                case "1":
                                                    {
                                                        db.Movies.Remove(deleteMovie);
                                                        logger.LogInformation("Deleting movie.");
                                                        db.SaveChanges();

                                                        Console.WriteLine($"Movie {deleteMovie.Id}: {deleteMovie.Title}, Release Date: {deleteMovie.ReleaseDate}");
                                                        Console.WriteLine("\tThis movie has been deleted\n");
                                                        break;
                                                    }
                                                case "2":
                                                    {
                                                        break;
                                                    }
                                                default:
                                                    {
                                                        Console.WriteLine("Incorrect input\n");
                                                        break;
                                                    }
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Inputted movie title does not exist\n");
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Movie title cannot be null\n");
                                        break;
                                    }
                            } while (yesOrNo == "2");
                            }
                            break;
                        }
                    case "5": //add user w/ occupation
                        {
                            using (var db = new MovieContext())
                            {
                                Console.WriteLine("Input the age of the user you would like to add:");
                                var ageUserInput = Console.ReadLine();
                                bool isNullAge = string.IsNullOrEmpty(Convert.ToString(ageUserInput));
                                int age;
                                bool isIntAge = int.TryParse(ageUserInput, out age);

                                if (isNullAge == false && isIntAge == true)
                                {
                                    Console.WriteLine("Input the gender of the user you would like to add:");
                                    var genderUserInput = Console.ReadLine();
                                    bool isNullGender = string.IsNullOrEmpty(genderUserInput);

                                    if (isNullGender == false)
                                    {
                                        Console.WriteLine("Input the zip code of the user you would like to add:");
                                        var zipCodeUserInput = Console.ReadLine();
                                        bool isNullZipCode = string.IsNullOrEmpty(zipCodeUserInput);

                                        if (isNullZipCode == false)
                                        {
                                            Console.WriteLine("\nHere is the list of occupations:\n---------------------------");
                                            foreach (var occ in db.Occupations)
                                            {
                                                Console.WriteLine($"Occupation ID: {occ.Id}, {occ.Name}");
                                            }
                                            Console.WriteLine("\nInput the occupation ID of the user you would like to add:");
                                            var occupationUserInput = Console.ReadLine();

                                            bool isNullOccupation = string.IsNullOrEmpty(Convert.ToString(occupationUserInput));
                                            int occupationInt;
                                            bool isIntOccupation = int.TryParse(occupationUserInput, out occupationInt);
                                            if (isNullOccupation == false && isIntOccupation == true)
                                            {
                                                var occupation = db.Occupations.Where(x => x.Id == occupationInt).FirstOrDefault();

                                                var user = new User()
                                                {
                                                    Age = age,
                                                    Gender = genderUserInput,
                                                    ZipCode = zipCodeUserInput,
                                                };
                                                user.Occupation = occupation;

                                                db.Users.Add(user);
                                                logger.LogInformation("Adding user.");
                                                db.SaveChanges();

                                                var newUser = db.Users.Where(x => x.Age == age && x.Gender == genderUserInput && x.ZipCode == zipCodeUserInput).FirstOrDefault();
                                                Console.WriteLine($"User {newUser.Id}, Age: {newUser.Age}, Gender: {newUser.Gender}, ZipCode: {newUser.ZipCode}, Occupation: {newUser.Occupation.Name}");
                                                Console.WriteLine("\tThis user has been added\n");
                                            }
                                            else if (isNullOccupation == true)
                                            {
                                                Console.WriteLine("User occupation ID cannot be null\n");
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Incorrect input of user occupation ID\n");
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("User zip code cannot be null\n");
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("User gender cannot be null\n");
                                        break;
                                    }
                                }
                                else if (isNullAge == true)
                                {
                                    Console.WriteLine("User age cannot be null\n");
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Incorrect input of user age\n");
                                    break;
                                }
                            }
                            break;
                        }
                    case "6": //add movie rating
                        {
                            using (var db = new MovieContext())
                            {
                                string yesOrNo;
                                do
                                {
                                    Console.WriteLine("Input the title of the movie you would like to rate:");
                                    var movieUserInput1 = Console.ReadLine();
                                    bool isNullTitle = string.IsNullOrEmpty(movieUserInput1);

                                    if (isNullTitle == false)
                                    {
                                        var rateMovie = db.Movies.FirstOrDefault(x => x.Title.ToUpper().Contains(movieUserInput1.ToUpper()));
                                        if (rateMovie is not null)
                                        {
                                            Console.WriteLine($"Movie {rateMovie.Id}: {rateMovie.Title}, Release Date: {rateMovie.ReleaseDate}");

                                            Console.WriteLine("\nIs this the movie you would like to rate? (1-Yes, 2-No)");
                                            yesOrNo = Convert.ToString(Console.ReadLine());
                                            switch (yesOrNo)
                                            {
                                                case "1":
                                                    {
                                                        Console.WriteLine("Input the ID of the user rating the movie:");
                                                        var userInput1 = Console.ReadLine();

                                                        bool isNullUser = string.IsNullOrEmpty(userInput1);
                                                        int userInt;
                                                        bool isIntUser = int.TryParse(userInput1, out userInt);

                                                        if (isNullUser == false && isIntUser == true)
                                                        {
                                                            var rateUser = db.Users.Where(x => x.Id == Convert.ToInt32(userInput1)).FirstOrDefault();

                                                            Console.WriteLine("Input the rating of the movie (1-5):");
                                                            var movieRatingInput = Console.ReadLine();

                                                            bool isNullRating = string.IsNullOrEmpty(movieRatingInput);
                                                            int ratingInt;
                                                            bool isIntRating = int.TryParse(movieRatingInput, out ratingInt);

                                                            if (isNullRating == false && isIntRating == true)
                                                            {
                                                                if (movieRatingInput == "1" || movieRatingInput == "2" || movieRatingInput == "3" || movieRatingInput == "4" || movieRatingInput == "5")
                                                                {
                                                                    var userMovie = new UserMovie()
                                                                    {
                                                                        Rating = ratingInt,
                                                                        RatedAt = DateTime.Now
                                                                    };

                                                                    userMovie.User = rateUser;
                                                                    userMovie.Movie = rateMovie;

                                                                    db.UserMovies.Add(userMovie);
                                                                    logger.LogInformation("Adding movie rating.");
                                                                    db.SaveChanges();

                                                                    var newUserMovie = db.UserMovies.Where(x => x.Rating == ratingInt && x.User == rateUser && x.Movie == rateMovie).FirstOrDefault();
                                                                    Console.WriteLine($"Movie {newUserMovie.Movie.Id}: {newUserMovie.Movie.Title}, Rated by User {newUserMovie.User.Id}:");
                                                                    Console.WriteLine($"User Movie {newUserMovie.Id}, Rating: {newUserMovie.Rating}, Rated at: {newUserMovie.RatedAt}");
                                                                    Console.WriteLine("\tThis movie rating has been added\n");
                                                                    break;
                                                                }
                                                                else
                                                                {
                                                                    Console.WriteLine("Incorrect input of movie rating, it must be 1-5\n");
                                                                    break;
                                                                }
                                                            }
                                                            else if (isNullRating == true)
                                                            {
                                                                Console.WriteLine("Movie rating cannot be null\n");
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("Incorrect input of movie rating\n");
                                                                break;
                                                            }
                                                        }
                                                        else if (isNullUser == true)
                                                        {
                                                            Console.WriteLine("User ID cannot be null\n");
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("Incorrect input of user ID\n");
                                                            break;
                                                        }
                                                    }
                                                case "2":
                                                    {
                                                        break;
                                                    }
                                                default:
                                                    {
                                                        Console.WriteLine("Incorrect input\n");
                                                        break;
                                                    }
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Inputted movie title does not exist\n");
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Movie title cannot be null\n");
                                        break;
                                    }
                                } while (yesOrNo == "2");
                            }
                            break;
                        }
                    case "7": //exit
                        {
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Incorrect input, try again\n");
                            break;
                        }
                }
            } while (userInput != "7");
        }
    }
}
