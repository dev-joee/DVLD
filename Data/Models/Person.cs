using System.ComponentModel.DataAnnotations;
using Data.Models.Enums;

namespace Data.Models;

public class Person
{
    public int ID { get; set; }
    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public string ThirdName { get; set; }
    public string LastName { get; set; }
    public string NationalNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; } // Gender: enum to char
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Country { get; set; }
    public string Address { get; set; }
    public string ImageRelativePath { get; set; }
    public int CountryID { get; set; }
}
