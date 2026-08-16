using Data.Models;

namespace Data.Authentication;

public class AccountSettings
{
    public static User? GetCurrentUser()
    {
        return Users.GetById(Global.CurrentUserID); // Exception is already handeled in the GetById method
        // if not fount or something went wrong => returns null
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
