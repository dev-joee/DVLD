using Data;
using Data.Models;

namespace Logic;

public class ManageCountries
{
    public static List<Country> ListAllCountries()
    {
        return Countries.GetAll();
    } 
}
