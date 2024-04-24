using System.ComponentModel.DataAnnotations;

namespace WritersCorner.Data;
public class Author
{
    [Key]
    public int AuthorId { get; set; }

    [Required(ErrorMessage = "Please insert a author name!")]
    public string AuthorName { get; set; }

    [Required(ErrorMessage = "Please insert a author surname!")]
    public string AuthorSurname { get; set; }

    [Required(ErrorMessage = "Please upload a author image!")]
    public string AuthorFullName
    {
        get
        {
            return this.AuthorName + " " + this.AuthorSurname;
        }
    }
    public string AuthorImage { get; set; }
    public int CountryId { get; set; }
    public Country Country { get; set; }
    public ICollection<Book> Books { get; set; }
}