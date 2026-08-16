namespace Data.Models;

public class User
{
    public int ID { get; set; }
    public int PersonID { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool IsActive { get; set; }
}
