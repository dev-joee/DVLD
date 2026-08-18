using Data.Models;
using Microsoft.Data.SqlClient;

namespace Data;

public class Countries
{
    public static List<Country> GetAll()
    {
        List<Country> All = new List<Country>();
        
        var connection = new SqlConnection(Configration.ConnectionString);
        
        var query = "SELECT * FROM Countries";

        var command = new SqlCommand(query, connection);

        try
        {
            connection.Open();

            var data = command.ExecuteReader();
            
            while (data.Read())
            {
                var country = new Country
                {
                    ID = (int)data["CountryID"],
                    Name = (string)data["CountryName"],
                };
                                    
                All.Add(country);
            }

            data.Close();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            connection.Close();
        }

        return All;
    }

    public static Country? GetPersonCountry (int CountryID) // country name for now
    {
        var connection = new SqlConnection(Configration.ConnectionString);
        
        var query = "SELECT * FROM DVLD.dbo.Countries WHERE CountryID = @CountryID";

        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@CountryID", CountryID);

        try
        {
            connection.Open();

            var data = command.ExecuteReader();
            
            if(data.Read())
            {
                return new Country
                {
                    ID = (int)data["CountryID"],
                    Name = (string)data["CountryName"],
                };
            }

            data.Close();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            connection.Close();
        }

        return null;
    }
}
