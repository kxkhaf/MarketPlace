using Dapper;
using Npgsql;

namespace MarketPlace;

public static class UserReviewRepository
{
    const string _connString = "Server=localhost;Port=5432;User Id=omr;Password=1234;Database=marketplace";

    public static async Task<ReviewDTO[]> GetReviewsFromDB(int id = -1, int userId = -1)
    {
        await using var db = new NpgsqlConnection(_connString);
        string sqlQuery = @"select (select name from users where id = r.reviewer_id) as name, r.message, r.rating as rating," +
                          @$" p.id,  p.price from products p INNER JOIN reviews r ON p.id = r.product_id ";
        if (id != -1)
        {
            sqlQuery += @$"and r.reviewer_id != {userId} ";
        }
        if (id != -1)
        {
            sqlQuery += @$"and p.id = {id}";
        }

        Console.WriteLine(sqlQuery);
        return (await db.QueryAsync<ReviewDTO>(sqlQuery)).ToArray();
    }
    
    
    
    public static async Task<ReviewDTO[]> GetUsersReviewsFromDB(int id = -1, int userId = -1)
    {
        await using var db = new NpgsqlConnection(_connString);
        string sqlQuery = @"select (select name from users where id = r.reviewer_id) as name, r.message, r.rating " +
                          " as rating," +
                          @$" p.id,  p.price from products p INNER JOIN reviews r ON p.id = r.product_id  ";
        if (id != -1)
        {
            sqlQuery += @$"and r.reviewer_id = {userId} ";
        }
        if (id != -1)
        {
            sqlQuery += @$"and p.id = {id}";
        }

        Console.WriteLine(sqlQuery);
        return (await db.QueryAsync<ReviewDTO>(sqlQuery)).ToArray();
    }
}