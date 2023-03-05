using System.Data.Common;
using Dapper;
using Microsoft.VisualBasic;
using Npgsql;

namespace MarketPlace;

public class ProductRepositoryWithCount
{
    const string _connString = "Server=localhost;Port=5432;User Id=omr;Password=1234;Database=marketplace";
    
    public static async Task<UserProductWithCount[]> GetProductFromDB(int id = -1)
    {
        await using var db = new NpgsqlConnection(_connString);
        string sqlQuery = "select p.id, p.name, information, " +
                                "Round((select avg(r.rating) from  reviews r where p.id = r.product_id and r.rating != -1), 1) as rating, " +
                                "up.product_count as count,  p.price from products p INNER JOIN user_products up ON p.id = up.product_id ";
        if (id != -1)
        {
            sqlQuery +=  @$"and up.user_id = {id}";
        }
        
        return (await db.QueryAsync<UserProductWithCount>(sqlQuery)).ToArray();
    }
    public static async Task<UserProductWithCount[]> GetAllProductFromDB(int id = -1)
    {
        await using var db = new NpgsqlConnection(_connString);
        string sqlQuery = "select p.id, p.name, information, " +
                          "Round((select avg(r.rating) from  reviews r where p.id = r.product_id and r.rating != -1), 1) as rating, " +
                          "up.product_count as count, p.price from products p LEFT JOIN user_products up ON p.id = up.product_id ";
        if (id != -1)
        {
            sqlQuery +=  @$"and up.user_id = {id}";
        }

        return (await db.QueryAsync<UserProductWithCount>(sqlQuery)).ToArray();
    }

    public static async Task<UserProductList[]> GetUserProductList(int id = -1)
    {
        await using var db = new NpgsqlConnection(_connString);
        string sqlQuery = @"select p.name, up.product_count as count, p.price 
        from products p LEFT JOIN user_products up ON p.id = up.product_id where up.product_count > 0 ";
        if (id != -1)
        {
            sqlQuery +=  @$"and up.user_id = {id}";
        }
        
        return db.QueryAsync<UserProductList>(sqlQuery).Result.ToArray();
    }
    
    public static async Task<int> BuyAllProducts(int id = -1)
    {
        await using var db = new NpgsqlConnection(_connString);
        /*var sqlQuery = @$"select sum(up.product_count * p.price) from products p LEFT JOIN user_products up ON p.id = up.product_id where up.product_count > 0 and up.user_id = {id}";
        var price =  await db.QueryFirstAsync<int>(sqlQuery);
        var balance = await UserRepository.GetUserBalance(id);
        if (price > balance)
        {
            return -1;
        }
        else
        {*/
            var sqlQuery = @$"delete from user_products up where up.user_id = {id}"; ;
            await db.ExecuteAsync(sqlQuery);
            //await UserRepository.UpdateUserBalance(id, -price);
            return id;
        //}
    }
    
    /*public static async Task<UserProductWithCount[]> GetSortedProductFromDB(bool inRating, bool inPrice, bool inDes, int id = -1)
    {
        await using var db = new NpgsqlConnection(_connString);
        string sqlQuery = "select p.id, p.name, information, " +
                          "Round((select avg(r.rating) from  reviews r where p.id = r.product_id and r.rating != -1), 1) as rating, " +
                          "up.product_count as count, p.price from products p LEFT JOIN user_products up ON p.id = up.product_id ";
        
        if (inRating)
        {
            
        }
        
        if (id != -1)
        {
            sqlQuery +=  @$"and up.user_id = {id}";
        }

        return (await db.QueryAsync<UserProductWithCount>(sqlQuery)).ToArray();
    }*/
    
    public static async Task<UserProductWithCount[]> GetFilteredProducts(ProductParams productParams, int id = -1)
    {
        if (productParams is not null)
        {
            using var db = new NpgsqlConnection(_connString);
            string sqlQuery = "select p.id, p.name, information, " +
                              "Round((select avg(r.rating) from  reviews r where p.id = r.product_id and r.rating != -1), 1) as rating, " +
                              @$"up.product_count as count, p.price from products p LEFT JOIN user_products up ON p.id = up.product_id and p.id < p.id ";
            if (productParams.SearchedName.Length > 0)
            {
                Console.WriteLine(productParams.SearchedName + " name");
                Console.WriteLine(productParams.SearchedName.Length);
                sqlQuery += @$"where lower(p.name) Like lower('{productParams.SearchedName}%') ";
            }
            if (productParams.InRating)
            {
                Console.WriteLine("RATE");
                sqlQuery += "order by rating ";
                Console.WriteLine(sqlQuery);
                if (productParams.InDes)
                {
                    Console.WriteLine("DESS");
                    sqlQuery += "desc ";
                }
            }
            else// if (productParams.InPrice)
            {
                Console.WriteLine("else");
                sqlQuery += "order by p.price ";
                if (productParams.InDes)
                {
                    sqlQuery += "desc ";
                }
            }

            Console.WriteLine();
            Console.WriteLine(sqlQuery);
            Console.WriteLine();
            return (await db.QueryAsync<UserProductWithCount>(sqlQuery)).ToArray();
        }
        return null!;
    }
}