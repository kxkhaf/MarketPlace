using System.Text.Json;

namespace MarketPlace;
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public  string Password { get; set; }
    public long Balance { get; set; }
    
    public List<UserProduct> UserProducts { get; set; }

    public User(int id, string name, string password, long balance)
    {
        Id = id;
        Name = name;
        Password = password;
        Balance = balance;
        
    }

    public static async Task<string> ToJSON(User user)
    {
        return JsonSerializer.Serialize(user);
    }
}

public static class StaticUser
{
    public static async Task<string> ToJSON(this User user)
    {
        return JsonSerializer.Serialize(user);
    }
}