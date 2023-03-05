using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using Dapper;
using Microsoft.VisualBasic;
using Npgsql;

namespace MarketPlace;

public class UserProductRepository
{
    const string _connString = "Server=localhost;Port=5432;User Id=omr;Password=1234;Database=marketplace";

    public static async Task<int> AddUserProduct(UserProduct userProduct)
    {
        if (userProduct is null || await UserRepository.GetUser(userProduct.UserId) is null || await ProductRepository.GetProduct(userProduct.ProductId) is null)
        {
            return -1;
        }
        var product = await GetUserProduct(userProduct.UserId, userProduct.ProductId);
        if (product is null)
        {
            await using var db = new NpgsqlConnection(_connString);
            const string sqlQuery = 
                "Insert Into public.user_products " +
                "(user_id, product_id, product_count) Values " +
                @"(@userId, @productId, @productCount) RETURNING user_id";
            userProduct.UserId = await db.QuerySingleAsync<int>(sqlQuery, userProduct);
            return userProduct.UserId;
        }
        return await UpdateUserProduct(userProduct.UserId, userProduct.ProductId, userProduct.ProductCount);
    }

    public static async Task<UserProduct> GetUserProduct(int userId, int productId) 
    {
        await using var db = new NpgsqlConnection(_connString);
        const string sqlQuery = @"SELECT * FROM public.user_products WHERE user_id = @userId and product_id = @productId";
        return await db.QueryFirstOrDefaultAsync<UserProduct>(sqlQuery, new { userId, productId });
    }
    
    public static async Task<ProductIDCountDTO[]> GetAllUserProductsWithBalance(int userId) 
    {
        await using var db = new NpgsqlConnection(_connString);
        const string sqlQuery = @"SELECT product_id, product_count FROM public.user_products WHERE user_id = @userId";
        return (await db.QueryAsync<ProductIDCountDTO>(sqlQuery, new { userId })).ToArray();
    }

    public static async Task<Product[]> GetProductsByUserId(int userId)
    {
        await using var db = new NpgsqlConnection(_connString);
        const string sqlQuery = "SELECT products.id, products.name, products.information, products.rating " +
                                "FROM user_products, products " +
                                @"WHERE user_products.user_id = @user_id AND user_products.product_id = products.id";
        var a = (await db.QueryAsync<Product>(sqlQuery, new { userId })).ToArray();
        return a;
    }

    public static async Task<int> UpdateUserProduct(int userId, int productId, long productCount)
    {
        var product = await GetUserProduct(userId, productId);
        if (product is null)
        {
            if (productCount >= 1)
            {
                return await AddUserProduct(product);
            }
            return -1;
        }
        if (product.ProductCount <= -productCount)
        {
            await DeleteUserProduct(userId, productId);
            return -1;
        }
        await using var db = new NpgsqlConnection(_connString);
        const string sqlQuery = @"UPDATE public.user_products SET product_count = product_count + @productCount 
                            WHERE user_id = @userId and product_id = @productId RETURNING user_id";
        return await db.QueryFirstAsync<int>(sqlQuery, new { @userId, @productId, productCount});
    }
    
    
    public static async Task<int> UpdateUserProductWithStatusCodes(int userId, int productId, long productCount, UserProduct userProduct = null)
    {
        userProduct = GetUserProduct(userId, productId).Result;
        var product = ProductRepository.GetProduct(productId).Result;
        var balance = UserRepository.GetUserBalance(userId).Result;
        Console.WriteLine(product.Price);
        if (product.Price * productCount <= balance && product.Price >= 0)
        {
            if (userProduct is null)
            {
                if (productCount > 0)
                {
                    await UserRepository.UpdateUserBalance(userId,-productCount * product.Price);
                    var a = await new NpgsqlConnection("Server=localhost;Port=5432;User Id=omr;Password=1234;Database=marketplace").
                        QuerySingleAsync<int>("Insert Into public.user_products " +
                                              "(user_id, product_id, product_count) Values " +
                                              @"(@userId, @productId, @productCount) RETURNING user_id", new { userId, productId, productCount});;
                    return a;
                }
                return -1;
            }
            if (userProduct.ProductCount <= -productCount)
            {
                await UserRepository.UpdateUserBalance(userId, -productCount * product.Price);
                await DeleteUserProduct(userId, productId);
                return -205;
            }
            
            await UserRepository.UpdateUserBalance(userId,-productCount * product.Price);
            
            await using var db = new NpgsqlConnection(_connString);
            const string sqlQuery = @"UPDATE public.user_products SET product_count = product_count + @productCount 
                            WHERE user_id = @userId and product_id = @productId RETURNING user_id";
            return await db.QueryFirstAsync<int>(sqlQuery, new { @userId, @productId, productCount });
        }
        
        return -1;
    }
    
    public static async Task<int> UpdateUserProducts(int userId, int productId, long productCount, UserProduct userProduct = null)
    {
        Console.WriteLine(0);
        userProduct = GetUserProduct(userId, productId).Result;
        var product = ProductRepository.GetProduct(productId).Result;
        var balance = UserRepository.GetUserBalance(userId).Result;
        Console.WriteLine(product.Price);
        if (product.Price * productCount <= balance && product.Price >= 0)
        {
            Console.WriteLine(2);
            if (userProduct is null)
            {
                if (productCount > 0)
                {
                    await UserRepository.UpdateUserBalance(userId,-productCount * product.Price);
                    var a = await new NpgsqlConnection("Server=localhost;Port=5432;User Id=omr;Password=1234;Database=marketplace").
                        QuerySingleAsync<int>("Insert Into public.user_products " +
                                              "(user_id, product_id, product_count) Values " +
                                              @"(@userId, @productId, @productCount) RETURNING user_id", new { userId, productId, productCount});;
                    return a;
                }
                return -1;
            }
            if (userProduct.ProductCount <= -productCount)
            {
                await UserRepository.UpdateUserBalance(userId, -productCount * product.Price);
                await DeleteUserProduct(userId, productId);
                return -205;
            }
            
            await UserRepository.UpdateUserBalance(userId,-productCount * product.Price);
            
            await using var db = new NpgsqlConnection(_connString);
            const string sqlQuery = @"UPDATE public.user_products SET product_count = product_count + @productCount 
                            WHERE user_id = @userId and product_id = @productId RETURNING user_id";
            return await db.QueryFirstAsync<int>(sqlQuery, new { @userId, @productId, productCount });
        }
        
        return -1;
    }

    public static async Task DeleteUserProduct(int userId, int productId)
    {
        await using var db = new NpgsqlConnection(_connString);
        const string sqlQuery = @"DELETE FROM public.user_products WHERE user_id = @userId and product_id = @productId";
        await db.ExecuteAsync(sqlQuery, new { userId, productId });
    }

    public static async Task<UserProduct[]> GetUserProductsFromDB()
    {
        await using var db = new NpgsqlConnection(_connString);
        const string sqlQuery = @"SELECT * FROM public.user_products";
        return (await db.QueryAsync<UserProduct>(sqlQuery)).ToArray();
    }
    
    public static async Task<UserProduct[]> GetUserProductsFromDBWithRating()
    {
        await using var db = new NpgsqlConnection(_connString);
        const string sqlQuery = @"SELECT * FROM public.user_products";
        return (await db.QueryAsync<UserProduct>(sqlQuery)).ToArray();
    }
    
}