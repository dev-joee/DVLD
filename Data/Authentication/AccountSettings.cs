using Data.Models;

namespace Data.Authentication;

public class AccountSettings
{
    public static User? GetCurrentUser()
    {
        User? CurrentUser = null;

        try
        {
            CurrentUser =  Users.GetById(Global.CurrentUserID);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return CurrentUser;
    }

    public static bool ChangePassword (string NewPassword)
    {
        var user = GetCurrentUser();
        
        if (user == null)
        {
            throw new Exception("Failed While Fetching Current User Data, Debug [Data.Authentication.AccountSettings]");
            return false;
        }

        user.Password = NewPassword;

        if (!Users.Update(user.ID, user))
        {
            throw new Exception("Failed While Updating User Password, Debug [Data.Authentication.AccountSettings]");
            return false;
        }

        return true;
    }
}
