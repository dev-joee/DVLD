using Data;
using Data.Models;

namespace Logic;

public class ManagePeople
{
    public static List<Person> ListAllPeople()
    {
        return People.GetAll();
    }
    public static int AddNewPerson(Person person)
    {
        int InsertedRecordID = 0;

        if (People.AddNew(person, ref InsertedRecordID))
        {
            if (InsertedRecordID == 0)
            {
                throw new Exception("Something went wrong while Fetching Inserted Person ID, Debug [Logic.ManagePeople.AddNewPerson]");
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

    public static void UpdatePerson(int ID, Person UpdatedPersonInfo) // or i can update with ID
    {
        bool Transaction_Completed = People.Update(ID, UpdatedPersonInfo);
        
        if (!Transaction_Completed)
        {
            throw new Exception($"Transaction Is NOT Completed, Failed to Update Person with ID[{ID}], Debug[Logic.ManagePeople.UpdatePerson]");    
        }
    }

    public static void DeletePerson(int ID)
    {
        bool Transaction_Completed = People.Delete(ID);
        
        if (!Transaction_Completed)
        {
            throw new Exception($"Transaction Is NOT Completed, Failed to Delete Person with ID[{ID}], Debug[Logic.ManagePeople.UpdatePerson]");    
        }
    }

    public static List<Person> Filter(string FilterProperty, string Term)
    {
        return People.Filter(FilterProperty, Term);
    }

    public static List<Person> Sort(bool ASC)
    {
        List<Person> PeopleList = ListAllPeople();

        if (ASC) 
            PeopleList.Sort((p1, p2) => p1.ID.CompareTo(p2.ID));
        else
            PeopleList.Sort((p1, p2) => p1.ID.CompareTo(p1.ID));

        return PeopleList;
    }

    public static Person? GetPersonDetails(int ID)
    {
        return People.GetById(ID); // returns null if it is not found
    }
}
