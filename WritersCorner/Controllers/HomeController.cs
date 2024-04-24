using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WritersCorner.Data;
using WritersCorner.Models;

namespace WritersCorner.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _appDbContext;
    public HomeController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public IActionResult Index()
    {
        return View();
    }
    //search string
    public async Task<IActionResult> Books(string search, string category)
    {
        var values = await _appDbContext.Books.Include(x => x.Author).Include(x => x.Category).Include(x => x.Country).ToListAsync();
        if(!String.IsNullOrEmpty(search))
        {
            ViewBag.Search = search;
            values = values.Where(x => x.BookName.ToLower().Contains(search.ToLower())).ToList();
        }
        if(!String.IsNullOrEmpty(category) && category != "0")
        {
            // values = values.Where(x => x.CategoryId == int.Parse(category)).ToList();
            values = values.Where(x => x.Category.CategoryName == category).ToList(); //this is for string search
        }
        // ViewBag.Categories = new SelectList(_appDbContext.Categories, "CategoryId", "CategoryName", category); //category valuesi selectlistte kalır sayfa yenilenince gitmez
        // ViewBag.Categories = new SelectList(_appDbContext.Categories, "CategoryName", "CategoryName"); this is for string search

        //view model ile çalışma
        var model = new BookViewModel {
            Books = values,
            Categories = _appDbContext.Categories.ToList(),
            SelectedCategory = category
        };

        return View(model);
    }
    public async Task<IActionResult> Authors(string search)
    {
        var values = await _appDbContext.Authors.Include(x => x.Country).ToListAsync();
        if(!String.IsNullOrEmpty(search))
        {
            ViewBag.Search = search;
            values = values.Where(x => x.AuthorFullName.ToLower().Contains(search.ToLower())).ToList();
        }
        return View(values);
    }
    public async Task<IActionResult> Countries(string search)
    {
        var values = await _appDbContext.Countries.ToListAsync();
        if (!String.IsNullOrEmpty(search))
        {
            ViewBag.Search = search;
            values = values.Where(x => x.CountryName.ToLower().Contains(search.ToLower())).ToList();
        }
        return View(values);
    }
}
