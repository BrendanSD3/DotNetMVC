using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Controllers
{
    public class MovieFinderController : Controller
    {
        private readonly MvcMovieContext _context;

        public string Genre { get; set; }
        public FinderViewModel finderVM;
        public MovieFinderController(MvcMovieContext context)
        {
            _context = context;
        }



        public async Task<IActionResult> Index()
        {
        IQueryable<string> genreQuery = from m in _context.Movie
                                        orderby m.Genre
                                        select m.Genre;


            IQueryable<decimal> budgetQuery = from m in _context.Movie
                                              select m.Price;
            IQueryable<int> ratingsQuery = from m in _context.Movie
                                           select m.Review;

            var movies = from m in _context.Movie
                         select m;

            finderVM = new FinderViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Movies = await movies.ToListAsync(),
                Budget = new SelectList(await budgetQuery.Distinct().ToListAsync()),
                starRatings = new SelectList(await ratingsQuery.Distinct().ToListAsync())


            };


            return View(finderVM);
        }

        public async Task<IActionResult> Result(string movieGenre, decimal budget, int starrating)
        {
            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;

            var movies = from m in _context.Movie
                         select m;
            var budgets = from m in _context.Movie
                          where budget!=0 ? budget==m.Price:true
                         select m;

            IQueryable<decimal> budgetQuery = from m in _context.Movie
                                              select m.Price;
            IQueryable<int> ratingsQuery = from m in _context.Movie
                                              select m.Review;


            if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre);

            }
            else {
                movies = from m in _context.Movie
                         select m;
            }
            if (budget != 0)
            {
                movies = movies.Where(x => x.Price == budget);
            }
            if (starrating != 0)
            {
                movies = movies.Where(x => x.Review == starrating);
            }

            finderVM = new FinderViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Movies = await movies.ToListAsync(),
                Budget = new SelectList(await budgetQuery.Distinct().ToListAsync()),
                starRatings=new SelectList(await ratingsQuery.Distinct().ToListAsync())


            };

            return View("Results",finderVM);
        }


    }
}
