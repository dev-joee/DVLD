using Data.Models;
using Microsoft.Data.SqlClient;

namespace Data.Authentication;

public class Users
{
    public static List<User> GetAll()
    {
        List<User> All = new List<User>();
        
        var connection = new SqlConnection(Configration.ConnectionString);
        
        var query = "SELECT * FROM Users";

        var command = new SqlCommand(query, connection);

        try
        {
            connection.Open();

            var data = command.ExecuteReader();
            
            while (data.Read())
            {
                var user = new User
                {
                    ID = (int) data["ID"],
                    PersonID = (int) data["PersonID"],
                    Username = (string) data["Username"],
                    Password = (string) data["Password"],
                    IsActive = (bool) data["IsActive"],
                };

                All.Add(user);
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
    public static bool AddNew(User user, ref int ID)
    {
        var connection = new SqlConnection(Configration.ConnectionString);
        
        var query = @"INSERT INTO DVLD.dbo.Users
                    (UserID, PersonID, UserName, Password, IsActive)
                    VALUES(@UserID, @PersonID, @UserName, @Password, @IsActive);
                    SELECT SCOPE_IDENTITY();";

        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@ID", user.ID);
        command.Parameters.AddWithValue("@PersonID", user.PersonID);
        command.Parameters.AddWithValue("@Username", user.Username);
        command.Parameters.AddWithValue("@Password", user.Password);
        command.Parameters.AddWithValue("@IsActive", user.IsActive);

        connection.Open();

        try
        {
            var result = command.ExecuteScalar();

            if (result == null || result == DBNull.Value)
            {
                throw new Exception("Transaction Failed - Error while Adding New User, User is not Added, Debug [Data.Users.AddNew]");
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
    public static bool Update(int ID, User user)
    {
        if (GetById(ID) == null)
        {
            throw new Exception($"Can Not Update User Info, User With ID {ID} is NOT Found in the Database");
            return false;
        }

        var connection = new SqlConnection(Configration.ConnectionString);
        
        var query = @"UPDATE DVLD.dbo.Users
                    SET PersonID=@PersonID, UserName=@UserName, Password=@Password, IsActive=@IsActive
                    WHERE UserID=@UserID;";

        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@UserID", ID);
        command.Parameters.AddWithValue("@PersonID", user.PersonID);
        command.Parameters.AddWithValue("@Username", user.Username);
        command.Parameters.AddWithValue("@Password", user.Password);
        command.Parameters.AddWithValue("@IsActive", user.IsActive);

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
            throw new Exception($"Can Not Update User Info, User With ID {ID} is NOT Found in the Database");
            return false;
        }

        var connection = new SqlConnection(Configration.ConnectionString);
        
        var query = @"DELETE FROM DVLD.dbo.Users
                    WHERE UserID=@UserID;";

        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@UserID", ID);
        
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
    public static User? GetById(int ID)
    {
        var connection = new SqlConnection(Configration.ConnectionString);
        
        var query = @"SELECT * FROM People
                    WHERE UserID=@UserID";

        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@UserID", ID);
    
        User? user = null;

        try
        {
            connection.Open();

            var data = command.ExecuteReader();
        
            if (data.Read())
            {
                user = new User
                {
                    ID = ID,
                    PersonID = (int) data["PersonID"],
                    Username = (string) data["Username"],
                    Password = (string) data["Password"],
                    IsActive = (bool) data["LastName"],
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

        return user;
    }
    public static User? GetByUsername(string username)
    {
        var connection = new SqlConnection(Configration.ConnectionString);
        
        var query = @"SELECT * FROM People
                    WHERE Username=@Username";

        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Username", username);
    
        User? user = null;

        try
        {
            connection.Open();

            var data = command.ExecuteReader();
        
            if (data.Read())
            {
                user = new User
                {
                    ID = (int) data["UserID"],
                    PersonID = (int) data["PersonID"],
                    Username = username,
                    Password = (string) data["Password"],
                    IsActive = (bool) data["LastName"],
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

        return user;
    }
    public static List<User> Filter(string FilterProperty, string Term)
    {
        Term = Term.ToLower();

        List<User> Filtered = new List<User>();
        
        var connection = new SqlConnection(Configration.ConnectionString);
        
        var query = "SELECT * FROM Users WHERE @FilterProperty = @Term";

        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@FilterProperty", FilterProperty);
        command.Parameters.AddWithValue("@Term", Term);

        try
        {
            connection.Open();

            var data = command.ExecuteReader();
            
            while (data.Read())
            {
                Filtered.Add(GetById((int) data["UserID"]));
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
    public static int IsPersonRegistered(int PersonID)
    {
        int UserID = 0;

        var connection = new SqlConnection(Configration.ConnectionString);

        var query = "SELECT UserID FROM Users WHERE PersonID = @PersonID";

        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@PersonID", PersonID);

        try
        {
            connection.Open();

            var result = command.ExecuteScalar(); // found -> (object) UserID | not fount -> DBNull.Value 
        
            if (!(result == null || result == DBNull.Value)) // if it is not null so the person exists -> true (person is already registered)
                UserID = (int) result;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            connection.Close();
        }

        return UserID;
    }
}
