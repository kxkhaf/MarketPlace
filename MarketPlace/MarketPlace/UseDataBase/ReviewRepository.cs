using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using Dapper;
using Microsoft.VisualBasic;
using Npgsql;

namespace MarketPlace;

public class ReviewRepository
{
    const string _connString = "Server=localhost;Port=5432;User Id=omr;Password=1234;Database=marketplace";

    public static async Task<int> AddReview(Review review)
    {
        if (review is null)
        {
            return -1;
        }
        await using var db = new NpgsqlConnection(_connString);
        const string sqlQuery = 
            @"Insert Into public.reviews " +
            @"(reviewer_id, product_id, rating, message) Values " +
            @"(@reviewerId, @productId, @rating, @message) RETURNING id";
        review.Id = await db.QuerySingleAsync<int>(sqlQuery, review);
        return review.Id;
    }

    public static async Task<Review> GetReview(int id) 
    {
        await using var db = new NpgsqlConnection(_connString);
        const string sqlQuery = @"SELECT * FROM public.reviews WHERE id = @id";
        return await db.QueryFirstOrDefaultAsync<Review>(sqlQuery, new { id });
    }

    public static async Task<Review[]> GetReviewsByProductId(int productId)
    {
        await using var db = new NpgsqlConnection(_connString);
        const string sqlQuery = @"SELECT * FROM public.reviews WHERE id = @productId";
        return (await db.QueryAsync<Review>(sqlQuery, new { productId })).ToArray();
    } 

    public static async Task<int> UpdateReview(int id, int newRating, string newMessage)
    {
        if (await GetReview(id) is null)
        {
            return -1;
        }
        await using var db = new NpgsqlConnection(_connString);
        const string sqlQuery = @"UPDATE public.reviews SET rating = @newRating, message = @newMessage WHERE id = @id RETURNING id";
        return await db.QueryFirstAsync<int>(sqlQuery, new { @newRating, @newMessage});
    }

    public static async Task DeleteReview(int id)
    {
        await using var db = new NpgsqlConnection(_connString);
        const string sqlQuery = @"DELETE FROM public.reviews WHERE id = @id";
        await db.ExecuteAsync(sqlQuery, new { id });
    }

    public static async Task<Review[]> GetReviewsFromDB()
    {
        await using var db = new NpgsqlConnection(_connString);
        const string sqlQuery = @"SELECT * FROM public.reviews";
        return (await db.QueryAsync<Review>(sqlQuery)).ToArray();
    }
}