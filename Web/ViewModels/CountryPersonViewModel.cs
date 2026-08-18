using Data.Models;

namespace Web.ViewModels;

public class CountryPersonViewModel
{
    public Person Person { get; set; }
    public List<Country> Countries { get; set; }
}
