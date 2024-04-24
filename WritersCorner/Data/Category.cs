using System.ComponentModel.DataAnnotations;

namespace WritersCorner.Data;
public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Please insert a category name!")]
    public string CategoryName { get; set; }
    public ICollection<Book> Books { get; set; }
}