using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WritersCorner.Data;
using WritersCorner.Models;

namespace WritersCorner.Controllers;
public class AdminController : Controller
{
    private readonly AppDbContext _context;
    public AdminController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Index(string username, string password)
    {
        if (username == "admin" && password == "1234")
        {
            return RedirectToAction("Main", "Admin");
        }
        return NotFound();
    }
    public IActionResult Main()
    {
        return View();
    }

    //Book Section

    public IActionResult Book()
    {
        var values = _context.Books.Include(x => x.Author).Include(x => x.Category).Include(x => x.Country).ToList();
        return View(values);
    }
    [HttpGet]
    public IActionResult CreateBook()
    {
        ViewBag.authors = _context.Authors.ToList();
        ViewBag.countries = _context.Countries.ToList();
        ViewBag.categories = _context.Categories.ToList();
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> CreateBook(Book model, IFormFile imageFile)
    {
        if (imageFile != null)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(imageFile.FileName); //.jpg, .png etc. we take them.
            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("imageFile", "Image must have .jpg, .jpeg ve .png .");
                ViewBag.authors = _context.Authors.ToList();
                ViewBag.countries = _context.Countries.ToList();
                ViewBag.categories = _context.Categories.ToList();
                return View(model);
            }
            if (imageFile != null)
            {
                var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}"); //we create a new random name and added the extension.
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/books", randomFileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                model.BookImage = randomFileName;
                _context.Books.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Book", "Admin");
            }
        }
        ViewBag.authors = _context.Authors.ToList();
        ViewBag.countries = _context.Countries.ToList();
        ViewBag.categories = _context.Categories.ToList();
        return View(model);
    }
    [HttpGet]
    public IActionResult BookUpdate(int? id)
    {
        if (id != null)
        {
            var values = _context.Books.FirstOrDefault(x => x.BookId == id);
            //You can also type like this.
            ViewBag.Authors = new SelectList(_context.Authors.ToList(), "AuthorId", "AuthorName");
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "CategoryName");
            ViewBag.Countries = new SelectList(_context.Countries.ToList(), "CountryId", "CountryName");
            if (values != null)
            {
                return View(values);
            }
            else
            {
                return NotFound();
            }
        }
        return NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> BookUpdate(Book model, IFormFile? imageFile) //don't forget to make nullable the IFormFile
    {
        if (model.BookId == 0)
        {
            return View();
        }
        if (imageFile != null)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(imageFile.FileName); //.jpg, .png etc. we take them.
            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("imageFile", "Image must have .jpg, .jpeg ve .png .");
                ViewBag.Authors = new SelectList(_context.Authors.ToList(), "AuthorId", "AuthorName");
                ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "CategoryName");
                ViewBag.Countries = new SelectList(_context.Countries.ToList(), "CountryId", "CountryName");
                return View(model);
            }
            var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}"); //we create a new random name and added the extension.
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/books", randomFileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            model.BookImage = randomFileName;
        }
        _context.Books.Update(model);
        await _context.SaveChangesAsync();
        return RedirectToAction("Book", "Admin");
    }
    public IActionResult BookDelete(int? id)
    {
        if (id != null)
        {
            var value = _context.Books.FirstOrDefault(x => x.BookId == id);
            _context.Books.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("Book", "Admin");
        }
        return NotFound();
    }

    //Author Section

    public IActionResult Author()
    {
        var values = _context.Authors.Include(x => x.Country).ToList();
        return View(values);
    }
    [HttpGet]
    public IActionResult CreateAuthor()
    {

        ViewBag.authors = _context.Authors.ToList();
        ViewBag.countries = _context.Countries.ToList();
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> CreateAuthor(Author model, IFormFile imageFile)
    {
        if (imageFile != null)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(imageFile.FileName); //.jpg, .png etc. we take them.
            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("imageFile", "Image must have .jpg, .jpeg ve .png .");
                ViewBag.countries = _context.Countries.ToList();
                return View(model);
            }
            if (imageFile != null)
            {
                var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}"); //we create a new random name and added the extension.
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/authors", randomFileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                model.AuthorImage = randomFileName;
                _context.Authors.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Author", "Admin");
            }
        }
        ViewBag.countries = _context.Countries.ToList();
        return View(model);
    }
    [HttpGet]
    public IActionResult AuthorUpdate(int? id)
    {
        if (id != null)
        {
            var values = _context.Authors.FirstOrDefault(x => x.AuthorId == id);
            //You can also type like this.
            ViewBag.Countries = new SelectList(_context.Countries.ToList(), "CountryId", "CountryName");
            if (values != null)
            {
                return View(values);
            }
            else
            {
                return NotFound();
            }
        }
        return NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> AuthorUpdate(Author model, IFormFile? imageFile)
    {
        if (model.AuthorId == 0)
        {
            return View();
        }
        if (imageFile != null)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(imageFile.FileName); //.jpg, .png etc. we take them.
            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("imageFile", "Image must have .jpg, .jpeg ve .png .");
                ViewBag.Countries = new SelectList(_context.Countries.ToList(), "CountryId", "CountryName");
                return View(model);
            }
            var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}"); //we create a new random name and added the extension.
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/authors", randomFileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            model.AuthorImage = randomFileName;
        }
        _context.Authors.Update(model);
        await _context.SaveChangesAsync();
        return RedirectToAction("Author", "Admin");
    }
    public IActionResult AuthorDelete(int? id)
    {
        if (id != null)
        {
            var value = _context.Authors.FirstOrDefault(x => x.AuthorId == id);
            _context.Authors.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("Author", "Admin");
        }
        return NotFound();
    }

    //Category Section

    public IActionResult Category()
    {
        var values = _context.Categories.ToList();
        return View(values);
    }
    [HttpGet]
    public IActionResult CreateCategory()
    {
        return View();
    }
    [HttpPost]
    public IActionResult CreateCategory(Category model)
    {
        _context.Categories.Add(model);
        _context.SaveChanges();
        return RedirectToAction("Category", "Admin");
    }
    public IActionResult CategoryUpdate(int? id)
    {
        if (id != null)
        {
            var values = _context.Categories.FirstOrDefault(x => x.CategoryId == id);
            if (values != null)
            {
                return View(values);
            }
            else
            {
                return NotFound();
            }
        }
        return NotFound();
    }
    [HttpPost]
    public IActionResult CategoryUpdate(Category model)
    {
        if (model.CategoryId == 0)
        {
            return View();
        }
        if (ModelState.IsValid)
        {
            _context.Categories.Update(model);
            _context.SaveChanges();
            return RedirectToAction("Category", "Admin");
        }
        return NotFound();
    }
    public IActionResult CategoryDelete(int? id)
    {
        if (id != null)
        {
            var value = _context.Categories.FirstOrDefault(x => x.CategoryId == id);
            _context.Categories.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("Category", "Admin");
        }
        return NotFound();
    }

    //Country Section

    public IActionResult Country()
    {
        var values = _context.Countries.ToList();
        return View(values);
    }

    [HttpGet]
    public IActionResult CreateCountry()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> CreateCountry(Country model, IFormFile imageFile)
    {
        if (imageFile != null)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(imageFile.FileName); //.jpg, .png etc. we take them.
            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("imageFile", "Image must have .jpg, .jpeg ve .png .");
                return View(model);
            }
            if (imageFile != null)
            {
                var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}"); //we create a new random name and added the extension.
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/countries", randomFileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                model.CountryImage = randomFileName;
                _context.Countries.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Country", "Admin");
            }
        }
        return View(model);
    }
    public IActionResult CountryUpdate(int? id)
    {
        if (id != null)
        {
            var values = _context.Countries.FirstOrDefault(x => x.CountryId == id);
            if (values != null)
            {
                return View(values);
            }
            else
            {
                return NotFound();
            }
        }
        return NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> CountryUpdate(Country model, IFormFile? imageFile)
    {
      if (model.CountryId == 0)
        {
            return View();
        }
        if (imageFile != null)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(imageFile.FileName); //.jpg, .png etc. we take them.
            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("imageFile", "Image must have .jpg, .jpeg ve .png .");
                return View(model);
            }
            var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}"); //we create a new random name and added the extension.
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/countries", randomFileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            model.CountryImage = randomFileName;
        }
        _context.Countries.Update(model);
        await _context.SaveChangesAsync();
        return RedirectToAction("Country", "Admin");
    }
    public IActionResult CountryDelete(int? id)
    {
        if (id != null)
        {
            var value = _context.Countries.FirstOrDefault(x => x.CountryId == id);
            _context.Countries.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("Country", "Admin");
        }
        return NotFound();
    }
}