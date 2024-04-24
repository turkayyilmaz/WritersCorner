using System.ComponentModel.DataAnnotations;

namespace WritersCorner.Data;
public class Country
{
    [Key]
    public int CountryId { get; set; }
 
    [Required(ErrorMessage = "Please insert a country name!")]
    public string CountryName { get; set;}
   
    [Required(ErrorMessage = "Please upload a country image")]
    public string CountryImage { get; set;}
    public ICollection<Book> Books{ get; set; }
    public ICollection<Author> Authors{ get;}
}