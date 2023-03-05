using System.Data.Common;
using Dapper;
using Microsoft.VisualBasic;
using Npgsql;

namespace MarketPlace;

public class ProductRepository
{
    const string _connString = "Server=localhost;Port=5432;User Id=omr;Password=1234;Database=marketplace";

    public static async Task<int> AddProduct(Product product)
    {
        if (!(product is not null &&
              (await new ProductValidator().ValidateAsync(product.Name)).IsValid))
        {
            return -1;
        }
        await using var db = new NpgsqlConnection(_connString);
        const string sqlQuery = @"Insert Into public.products (name, information) Values (@name, @information) RETURNING id";
        product.Id = await db.QuerySingleAsync<int>(sqlQuery, product);
        return product.Id;
    }
    public static async Task<int> AddProduct(string? name, string information)
    {
        if (!(await new ProductValidator().ValidateAsync(name!)).IsValid)
        {
            return -1;
        }
        await using var db = new NpgsqlConnection(_connString);
        await db.OpenAsync();
        await using var cmd = new NpgsqlCommand(@"INSERT INTO public.products (name, information) VALUES (@name, @information) RETURNING id", db);
        cmd.Parameters.AddWithValue("name", $@"{name}");
        cmd.Parameters.AddWithValue("information", $@"{information}");
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            return reader.GetInt32(0);
        return -1;
        //await db.CloseAsync();
    }

    public static async Task<Product> GetProduct(int id)
    {
        await using var db = new NpgsqlConnection(_connString);
        await db.OpenAsync();
        await using var cmd = new NpgsqlCommand($@"SELECT * FROM public.products WHERE id = @id", db);
        cmd.Parameters.AddWithValue("id", id);
        await using var reader = await cmd.ExecuteReaderAsync();
        return await GetProductFromReader(reader);
    }

    public static async Task<int> UpdateProduct(int id, string? newname, string? newinfromation)
    {
        if (!(await new NameValidator().ValidateAsync(newname!)).IsValid)
        {
            return -1;
        }
        await using var db = new NpgsqlConnection(_connString);
        await db.OpenAsync();
        var product = await GetProduct(id);
        if (product is not null)
        {
            await using var cmd =
                new NpgsqlCommand(@"UPDATE public.products SET name = @newname, information = @newinformation Where id = @id", db);
            cmd.Parameters.AddWithValue("newname", $@"{newname!}");
            cmd.Parameters.AddWithValue("newinformation", $@"{newinfromation!}");
            cmd.Parameters.AddWithValue("id", product.Id);
            await using var reader = await cmd.ExecuteReaderAsync();
        }
        return product?.Id ?? -1 ;
        //await db.CloseAsync();
    }

    public static async Task DeleteProduct(int id)
    {
        await using var db = new NpgsqlConnection(_connString);
        const string sqlQuery = @"DELETE FROM public.products WHERE id = @id";
        await db.ExecuteAsync(sqlQuery, new { id });
    }

    public static async Task<Product[]> GetProductFromDB()
    {
        await using var db = new NpgsqlConnection(_connString);
        const string sqlQuery = @"SELECT * FROM public.products";
        return (await db.QueryAsync<Product>(sqlQuery)).ToArray();
    }
    /*SELECT * FROM public.products, user_products WHERE products.id = user_products.product_id*/
    
    private static async Task<Product> GetProductFromReader(DbDataReader reader)
    {
        while (await reader.ReadAsync())
        {
            Product product = new(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetDecimal(3),
                reader.GetDecimal(4));
                return product;
        }

        return null!;
    }
}