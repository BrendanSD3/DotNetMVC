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

        public async Task<IActionResult> Index(string movieGenre)
        {
            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;

            var movies = from m in _context.Movie
                         select m;

            IQueryable<decimal> budgetQuery = from m in _context.Movie
                                              select m.Price;
            IQueryable<int> ratingsQuery = from m in _context.Movie
                                              select m.Review;


            if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre);
            }
           
            finderVM = new FinderViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Movies = await movies.ToListAsync(),
                Budget = new SelectList(await budgetQuery.Distinct().ToListAsync()),
                starRatings=new SelectList(await ratingsQuery.Distinct().ToListAsync())


            };

            return View(finderVM);
        }


    }
}
