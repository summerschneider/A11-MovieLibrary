using Microsoft.EntityFrameworkCore;
using MovieLibraryEntities.Context;
using MovieLibraryEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibraryEntities
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string userInput;
            do
            {
                Console.WriteLine("Enter your selection:");
                Console.WriteLine("1) Search movie\n2) Add movie\n3) Update movie\n4) Delete movie\n5) Exit");
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
                                                var displayMovie = db.Movies.Include(x => x.MovieGenres).FirstOrDefault(x => x.Title.ToUpper().Contains(movieUserInput.ToUpper()));

                                                if (displayMovie is not null)
                                                {
                                                    Console.WriteLine($"Movie {displayMovie.Id}: {displayMovie.Title}, Release Date: {displayMovie.ReleaseDate}");
                                                    //foreach (var genre in displayMovie.MovieGenres)
                                                    //{
                                                    //    Console.WriteLine($"Genre {genre.Genre.Id}: {genre.Genre.Name}");
                                                    //}
                                                    Console.WriteLine();
                                                }
                                                else if (displayMovie is null)
                                                {
                                                    Console.WriteLine("Inputted movie title does not exist\n");
                                                }
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Movie title cannot be null\n");
                                                break;
                                            }
                                        }
                                    case "2":  //display all movies
                                        {
                                            Console.WriteLine("Here is the list of movies:\n---------------------------");
                                            foreach (var movie in db.Movies)
                                            {
                                                Console.WriteLine($"Movie {movie.Id}: {movie.Title}, Release Date: {movie.ReleaseDate}");
                                            }
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
                                        if (deleteMovie is not null && isNullInput2 == false)
                                        {
                                            Console.WriteLine($"Movie {deleteMovie.Id}: {deleteMovie.Title}, Release Date: {deleteMovie.ReleaseDate}");

                                            Console.WriteLine("\nIs this the movie you would like to delete? (1-Yes, 2-No)");
                                            yesOrNo = Convert.ToString(Console.ReadLine());
                                            switch (yesOrNo)
                                            {
                                                case "1":
                                                    {
                                                        db.Movies.Remove(deleteMovie);
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
                    case"5":
                        {
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Incorrect input, try again\n");
                            break;
                        }
                }

            } while (userInput != "5");
        }
    }
}
