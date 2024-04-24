using System.ComponentModel.DataAnnotations;

namespace WritersCorner.Data;
public class Book
{
    [Key]
    public int BookId { get; set; }
   
    [Required(ErrorMessage = "Please insert a book name!")]
    public string BookName { get; set; }
  
    [Required(ErrorMessage = "Please insert a book description!")]
    public string BookDescription { get; set; }
  
    [Required(ErrorMessage = "Please insert a book price!")]
    public decimal BookPrice { get; set; }
  
    [Required(ErrorMessage = "Please upload a book image!")]
    public string BookImage { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public int CountryId { get; set; }
    public Country Country { get; set; }
}