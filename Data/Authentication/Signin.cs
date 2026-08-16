using Data.Authentication;

namespace Data.Authentication;

public class Signin
{
    public static bool Login (string username, string password)
    {
        var user = Users.GetByUsername(username);

        if (user == null)
            return false;

        if (user.Password != password)
            return false;

        if (!user.IsActive)
            return false;
        
        Global.CurrentUserID = user.ID;

        return true;
    }

    public static void Logout ()
    {
        Global.CurrentUserID = 0;
    }
}
