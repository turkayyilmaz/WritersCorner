namespace WritersCorner.Data;
public class BookViewModel
{
    public List<Book> Books { get; set; }
    public List<Category> Categories { get; set; }
    public string SelectedCategory { get; set; }
}