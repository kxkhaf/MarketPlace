using System.Data.Common;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks.Dataflow;
using Npgsql;
using Dapper;

namespace MarketPlace;

public static class UserRepository
{
    const string _connString = "Server=localhost;Port=5432;User Id=omr;Password=1234;Database=marketplace";

    public static async Task<int> AddUser(User user)
    {
        if (!(user is not null &&
              await GetUser(user.Name) is null &&
              (await new UserValidator().ValidateAsync(user)).IsValid))
        {
            return -1;
        }
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        await using var db = new NpgsqlConnection(_connString);
        const string sqlQuery = @"Insert Into public.users (name, password) Values (@name, @password) RETURNING id";
        user.Id = await db.QuerySingleAsync<int>(sqlQuery, user);
        return user.Id;
    }

    public static async Task<int> AddUser(string name, string password)
    {
        if (!(name is not null &&
              password is not null &&
              await GetUser(name, password) is null &&
              (await new NameValidator().ValidateAsync(name)).IsValid &&
              (await new PasswordValidator().ValidateAsync(password)).IsValid))
        {
            return -1;
        }
        await using var db = new NpgsqlConnection(_connString);
        await db.OpenAsync();
        await using var cmd = new NpgsqlCommand(@"INSERT INTO public.users (name, password) VALUES (@name, @password)", db);
        cmd.Parameters.AddWithValue("name", $@"{name}");
        cmd.Parameters.AddWithValue("password", $@"{BCrypt.Net.BCrypt.HashPassword(password)}");
        await cmd.ExecuteNonQueryAsync();
        return (await GetUser(name, password)).Id;
        //await db.CloseAsync();
    }

    public static async Task<User> GetUser(int id)
    {
        await using var db = new NpgsqlConnection(_connString);
        await db.OpenAsync();
        await using var cmd = new NpgsqlCommand($@"SELECT * FROM public.users WHERE id = @id", db);
        cmd.Parameters.AddWithValue("id", id);
        await using var reader = await cmd.ExecuteReaderAsync();
        return await GetUserFromReader(reader);
    }

    public static async Task<User> GetUser(string name, string password)
    {
        if (name is not null && password is not null)
        {
            await using var db = new NpgsqlConnection(_connString);
            await db.OpenAsync();
            await using var cmd = new NpgsqlCommand($@"SELECT * FROM public.users WHERE name = @name", db);
            cmd.Parameters.AddWithValue("name", $@"{name}");
            await using var reader = await cmd.ExecuteReaderAsync();
            var user = await GetUserFromReader(reader);
            await db.CloseAsync();
            Console.WriteLine(BCrypt.Net.BCrypt.Verify(password, user.Password));
            return user is not null && BCrypt.Net.BCrypt.Verify(password, user.Password) ? user : null;
        }

        return null!;
    }
    public static async Task<User> GetUser(string name)
    {
        await using var db = new NpgsqlConnection(_connString);
        await db.OpenAsync();
        await using var cmd = new NpgsqlCommand($@"SELECT * FROM public.users WHERE name = @name", db);
        cmd.Parameters.AddWithValue("name", name);
        await using var reader = await cmd.ExecuteReaderAsync();
        return await GetUserFromReader(reader);
    }

    public static async Task<int> GetUserBalance(int userId)
    {
       var user = GetUser(userId).Result;
       if (user?.Balance is not null)
       {
           await using var db = new NpgsqlConnection(_connString);
           const string sqlQuery = @"SELECT balance FROM public.users WHERE id = @userId";
           return await db.QueryFirstOrDefaultAsync<int>(sqlQuery, new { userId });
       }

       return -1;
    }

    public static async Task<int[]> GetUsersId()
    {
        await using var db = new NpgsqlConnection(_connString);
        const string sqlQuery = @"SELECT id FROM users";
        return (await db.QueryAsync<int>(sqlQuery)).ToArray();
    }
    
    public static async Task<int> UpdateUser(string name, string password, string? newname, string? newpassword)
    {
        if (!(name is not null &&
              password! is not null &&
              (await new NameValidator().ValidateAsync(newname!)).IsValid &&
              (await new PasswordValidator().ValidateAsync(newpassword!)).IsValid))
        {
            return -1;
        }
        newpassword = BCrypt.Net.BCrypt.HashPassword(newpassword);
        await using var db = new NpgsqlConnection(_connString);
        await db.OpenAsync();
        var user = await GetUser(name, password);
        if (user is not null)
        {
            await using var cmd =
                new NpgsqlCommand(@"UPDATE public.users SET name = @name, password = @password Where id = @id", db);
            cmd.Parameters.AddWithValue("name", $@"{newname}");
            cmd.Parameters.AddWithValue("password", $@"{newpassword}");
            cmd.Parameters.AddWithValue("id", user.Id);
            await using var reader = await cmd.ExecuteReaderAsync();
        }
        return user?.Id ?? -1 ;
        //await db.CloseAsync();
    }

    
    public static async Task<int> UpdateUserName(string password, string name, string? newname)
    {
        Console.WriteLine();
        Console.WriteLine(password);
        Console.WriteLine(name);
        Console.WriteLine(newname);
        if (!(name is not null &&
              password is not null &&
              (await new NameValidator().ValidateAsync(newname!)).IsValid))
        {
            return -1;
        }

        Console.WriteLine(1112222111);
        await using var db = new NpgsqlConnection(_connString);
        await db.OpenAsync();
        var user = await GetUser(name);
        if (user is not null)
        {
            await using var cmd =
                new NpgsqlCommand(@"UPDATE public.users SET name = @name Where id = @id", db);
            cmd.Parameters.AddWithValue("name", $@"{newname}");
            cmd.Parameters.AddWithValue("id", user.Id);
            await using var reader = await cmd.ExecuteReaderAsync();
        }
        return user?.Id ?? -1 ;
    }
    
    public static async Task<int> UpdateUserPassword(string password,  string name, string? newpassword)
    {
        if (!(password is not null &&
              name is not null &&
              (await new PasswordValidator().ValidateAsync(newpassword!)).IsValid))
        {
            return -1;
        }
        newpassword = BCrypt.Net.BCrypt.HashPassword(newpassword);
        await using var db = new NpgsqlConnection(_connString);
        await db.OpenAsync();
        var user = await GetUser(name);
        if (user is not null)
        {
            await using var cmd =
                new NpgsqlCommand(@"UPDATE public.users SET password = @password Where id = @id", db);
            cmd.Parameters.AddWithValue("password", $@"{newpassword}");
            cmd.Parameters.AddWithValue("id", user.Id);
            await using var reader = await cmd.ExecuteReaderAsync();
        }
        return user?.Id ?? -1 ;
    }
    
    public static async Task<int> UpdateUserBalance(int id, decimal addingBalance)
    {
        Console.WriteLine(addingBalance);
        Console.WriteLine("ab");
        var user = GetUser(id).Result;
        var balance = user.Balance;
        Console.WriteLine("Тут");
        Console.WriteLine(addingBalance + " " +balance);
        Console.WriteLine(balance + addingBalance);
        Console.WriteLine(balance + addingBalance < 0);
        if (balance + addingBalance < 0)
        {
            Console.WriteLine("Тут1");
            return -1;
        }
        await using var db = new NpgsqlConnection(_connString);
        const string sqlQuery = @"UPDATE users SET balance = users.balance + @addingBalance
                                WHERE id = @id RETURNING id";
        return await db.QueryFirstAsync<int>(sqlQuery, new { @id, @addingBalance});
    }

    public static async Task DeleteUser(int id)
    {
        await using var db = new NpgsqlConnection(_connString);
        const string sqlQuery = @"DELETE FROM public.users WHERE id = @id";
        await db.ExecuteAsync(sqlQuery, new { id });
    }

    private static async Task<User> GetUserFromReader(DbDataReader reader)
    {
        try
        {
            while (await reader.ReadAsync())
            {
                User user = new(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetInt32(3));
                return user;
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Не удалось преобразовать пользователя из БД");
            return null!;
        }

        return null!;
    }
    //const string _connString = "Host=localhost;User Id=omr;Password=1234;Database=marketplace";
    /*var connString = "Host=myserver;Username=mylogin;Password=mypass;Database=mydatabase";

    await using var conn = new NpgsqlConnection(connString);
    await conn.OpenAsync();

// Insert some data
    await using (var cmd = new NpgsqlCommand("INSERT INTO data (some_field) VALUES (@p)", conn))
    {
        cmd.Parameters.AddWithValue("p", "Hello world");
        await cmd.ExecuteNonQueryAsync();
    }

// Retrieve all rows
    await using (var cmd = new NpgsqlCommand("SELECT some_field FROM data", conn))
    await using (var reader = await cmd.ExecuteReaderAsync())
    {
        while (await reader.ReadAsync())
            Console.WriteLine(reader.GetString(0));
    }*/
}