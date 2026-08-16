/*
NOTES
- Profile Pictures I/O File Manipulation Functionality is not Added Yet, remove this note if added
*/

namespace Data;

using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;
using Data.Models;
using Data.Models.Enums;

public class People
{
    public static List<Person> GetAll()
    {
        List<Person> All = new List<Person>();
        
        var connection = new SqlConnection(Configration.ConnectionString);
        
        var query = "SELECT * FROM People";

        var command = new SqlCommand(query, connection);

        try
        {
            connection.Open();

            var data = command.ExecuteReader();
            
            while (data.Read())
            {
                var person = new Person
                {
                    ID = (int) data["PersonID"],
                    NationalNumber = (string) data["NationalNo"],
                    FirstName = (string) data["FirstName"],
                    SecondName = (string) data["SecondName"],
                    ThirdName = (string) data["ThirdName"],
                    LastName = (string) data["LastName"],
                    BirthDate = (DateTime) data["DateOfBirth"],
                    Address = (string) data["Address"],
                    Phone = (string) data["Phone"],
                    Email = (string) data["Email"],
                    CountryID = (int) data["NationalityCountryID"],
                    ImageRelativePath = (string) data["ImagePath"]
                };

                All.Add(person);
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
    public static bool AddNew(Person person, ref int ID)
    {
        var connection = new SqlConnection(Configration.ConnectionString);
        
        var query = @"INSERT INTO DVLD.dbo.People (PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath)
                    VALUES(@NationalNo, @FirstName, @SecondName, @ThirdName, @LastName, @DateOfBirth, @Gendor, @Address, @Phone, @NationalityCountryID, @ImagePath);
                    SELECT SCOPE_IDENTITY();";

        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@NationalNo", person.NationalNumber);
        command.Parameters.AddWithValue("@FirstName", person.FirstName);
        command.Parameters.AddWithValue("@SecondName", person.SecondName);
        command.Parameters.AddWithValue("@ThirdName", person.ThirdName);
        command.Parameters.AddWithValue("@LastName", person.LastName);
        command.Parameters.AddWithValue("@DateOfBirth", person.BirthDate);
        command.Parameters.AddWithValue("@Gendor", person.Gender == Gender.Male ? 0 : 1);
        command.Parameters.AddWithValue("@Address", person.Address);
        command.Parameters.AddWithValue("@Phone", person.Phone);
        command.Parameters.AddWithValue("@Email", person.Email);
        command.Parameters.AddWithValue("@NationalityCountryID", person.CountryID);
        command.Parameters.AddWithValue("@ImagePath", person.ImageRelativePath);

        connection.Open();

        try
        {
            // var rows_affected = command.ExecuteNonQuery();
            var result = command.ExecuteScalar();

            if (result == null || result == DBNull.Value)
            {
                throw new Exception("Transaction Failed - Error while Adding New Person, Person is not Added, Debug [Data.People.AddNew]");
                return false;
            }

            ID = Convert.ToInt32(result);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
            return false;
        }
        finally
        {
            connection.Close();
        }

        return true;
    }
    public static bool Update(int ID, Person person)
    {
        if (GetById(ID) == null)
        {
            throw new Exception($"Can Not Update Person Info, Person With ID {ID} is NOT Found in the Database");
            return false;
        }

        var connection = new SqlConnection(Configration.ConnectionString);
        
        var query = @"UPDATE DVLD.dbo.People
                    SET NationalNo='@NationalNo', FirstName='@FirstName', SecondName='@SecondName', ThirdName='@ThirdName', LastName='@LastName', DateOfBirth='@DateOfBirth', Gendor=@Gendor, Address='@Address', Phone='@Phone', Email='@Email', NationalityCountryID=@NationalityCountryID, ImagePath='@ImagePath'
                    WHERE PersonID=@PersonID;";

        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@PersonID", ID);
        command.Parameters.AddWithValue("@NationalNo", person.NationalNumber);
        command.Parameters.AddWithValue("@FirstName", person.FirstName);
        command.Parameters.AddWithValue("@SecondName", person.SecondName);
        command.Parameters.AddWithValue("@ThirdName", person.ThirdName);
        command.Parameters.AddWithValue("@LastName", person.LastName);
        command.Parameters.AddWithValue("@DateOfBirth", person.BirthDate);
        command.Parameters.AddWithValue("@Gendor", person.Gender == Gender.Male ? 0 : 1);
        command.Parameters.AddWithValue("@Address", person.Address);
        command.Parameters.AddWithValue("@Phone", person.Phone);
        command.Parameters.AddWithValue("@Email", person.Email);
        command.Parameters.AddWithValue("@NationalityCountryID", person.CountryID);
        command.Parameters.AddWithValue("@ImagePath", person.ImageRelativePath);

        try
        {
            connection.Open();

            var rows_affected = command.ExecuteNonQuery();

            if (rows_affected == 0)
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            connection.Close();
        }

        return true;
    }
    public static bool Delete(int ID)
    {
        if (GetById(ID) == null)
        {
            throw new Exception($"Can Not Update Person Info, Person With ID {ID} is NOT Found in the Database");
            return false;
        }

        var connection = new SqlConnection(Configration.ConnectionString);
        
        var query = @"DELETE FROM DVLD.dbo.People
                    WHERE PersonID=@PersonID;";

        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@PersonID", ID);
        
        try
        {
            connection.Open();

            var rows_affected = command.ExecuteNonQuery();
        
            if (rows_affected == 0)
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            
            throw new Exception(ex.Message);
        }
        finally
        {
            connection.Close();
        }

        return true;
    }
    public static Person? GetById(int ID)
    {
        var connection = new SqlConnection(Configration.ConnectionString);
        
        var query = @"SELECT * FROM People
                    WHERE PersonID=@PersonID";

        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@PersonID", ID);
    
        Person? person = null;

        try
        {
            connection.Open();

            var data = command.ExecuteReader();
        
            if (data.Read())
            {
                person = new Person
                {
                    ID = ID,
                    FirstName = (string) data["FirstName"],
                    SecondName = (string) data["SecondName"],
                    ThirdName = (string) data["ThirdName"],
                    LastName = (string) data["LastName"],
                    BirthDate = (DateTime) data["DateOfBirth"],
                    Address = (string) data["Address"],
                    Phone = (string) data["Phone"],
                    Email = (string) data["Email"],
                    CountryID = (int) data["NationalityCountryID"],
                    ImageRelativePath = (string) data["ImagePath"]
                };
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            connection.Close();
        }

        return person;
    }
    public static List<Person> Filter(string FilterProperty, string Term)
    {
        Term = Term.ToLower();

        List<Person> Filtered = new List<Person>();
        
        var connection = new SqlConnection(Configration.ConnectionString);
        
        var query = "SELECT PersonID FROM People WHERE @FilterProperty = @Term";

        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@FilterProperty", FilterProperty);
        command.Parameters.AddWithValue("@Term", Term);

        try
        {
            connection.Open();

            var data = command.ExecuteReader();
            
            while (data.Read())
            {
                Filtered.Add(GetById((int) data["PersonID"]));
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

        return Filtered;
    }
}
