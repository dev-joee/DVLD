using Data;
using Data.Authentication;
using Data.Models;

namespace Logic;

public class UsersManagment
{
    public static List<User> ListAllUsers ()
    {
        return Users.GetAll();        
    }

    public static int StoreUserInDatabase(User user) // after we link this user to a person (has PersonID), pass it to this method to save it to database
    {
        int InsertedRecordID = 0;

        if (Users.AddNew(user, ref InsertedRecordID))
        {
            if (InsertedRecordID == 0)
            {
                throw new Exception("Something went wrong while Fetching Inserted User ID, Debug [Logic.ManagePeople.AddNewPerson]");
                return 0;            
            }
        }
        else
        {
            throw new Exception("Something went wrong while Adding New Person, Debug [Logic.ManagePeople.AddNewPerson]");
            return 0;
        }

        return InsertedRecordID;
    }

    public static List<Person> FetchPeopleList()
    {
        return People.GetAll();
    }

    public static List<Person> FilterPeopleList(string FilterProperty, string Term)
    {
        return People.Filter(FilterProperty, Term);
    }

    public static Person? FetchPerson(int ID)
    {
        return People.GetById(ID); // returns null if not found
    }

    public static int CreateUser(int PersonID, string Username, string Password)
    {
        int UserID = Users.IsPersonRegistered(PersonID); // check if person is already registerd into the system (person has a userid)

        if (UserID != 0) // userid == 0 -> no users found for personid - userid == number > 0 -> personid is already linked into a user -> terminate the process
        {
            throw new Exception($"This Person with PersonID {PersonID} is already registerd as a User with UserID {(int) UserID} - Person Can NOT be Registered Twice");
            return -1;
        }

        var NewUser = new User
        {
            PersonID = PersonID,
            Username = Username,
            Password = Password,
            IsActive = true
        };

        UserID = StoreUserInDatabase(NewUser);

        return UserID;
    }

    public static void UpdateUser(int ID, User UpdatedUserInfo) // or i can update with ID
    {
        bool Transaction_Completed = Users.Update(ID, UpdatedUserInfo);
        
        if (!Transaction_Completed)
        {
            throw new Exception($"Transaction Is NOT Completed, Failed to Update User with ID[{ID}], Debug[Logic.UsersManagment.UpdateUser]");    
        }
    }

    public static void DeleteUser(int ID)
    {
        bool Transaction_Completed = Users.Delete(ID);
        
        if (!Transaction_Completed)
        {
            throw new Exception($"Transaction Is NOT Completed, Failed to Delete User with ID[{ID}], Debug[Logic.UsersManagment.UpdatePerson]");    
        }
    }

    public static List<User> Filter(string FilterProperty, string Term)
    {
        return Users.Filter(FilterProperty, Term);
    }

    public static List<User> Sort(bool ASC)
    {
        List<User> UsersList = ListAllUsers();

        if (ASC) 
            UsersList.Sort((u1, u2) => u1.ID.CompareTo(u2.ID));
        else
            UsersList.Sort((u1, u2) => u1.ID.CompareTo(u1.ID));

        return UsersList;
    }    
}
