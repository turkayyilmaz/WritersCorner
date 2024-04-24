using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WritersCorner.Data;
using WritersCorner.Models;

namespace WritersCorner;

public class DetailController : Controller
{
    private readonly AppDbContext _appDbContext;
    public DetailController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public IActionResult AuthorDetail(int? id)
    {
        List<Author> values = _appDbContext.Authors.Include(x => x.Country).Include(x => x.Books).ThenInclude(x => x.Category).ToList();
        if (id != null)
        {
            var value = values.FirstOrDefault(x => x.AuthorId == id);
            if (value != null)
            {
                ViewBag.Books = value.Books;
                return View(value);
            }
        }
        return RedirectToAction("Authors", "Home");
    }
    public IActionResult BookDetail(int? id)
    {
        List<Book> values = _appDbContext.Books.Include(x => x.Author).Include(x => x.Country).Include(x => x.Category).ToList();
        if (id != null)
        {
            var value = values.FirstOrDefault(x => x.BookId == id);
            if (value != null)
            {
                return View(value);
            }
        }
        return RedirectToAction("Books", "Home");
    }
    public IActionResult CountryAuthors(int id)
    {
        var values = _appDbContext.Authors.Include(x => x.Country).Where(x => x.CountryId == id).ToList();
        ViewBag.CountryName = values.Select(x => x.Country.CountryName).ToList();
        return View(values);
    }
    public IActionResult CountryBooks(int id)
    {
        var values = _appDbContext.Books.Include(x => x.Country).Include(x => x.Category).Include(x => x.Author).Where(x => x.CountryId == id).ToList();
        ViewBag.CountryName = values.Select(x => x.Country.CountryName).ToList();
        return View(values);
    }
}
