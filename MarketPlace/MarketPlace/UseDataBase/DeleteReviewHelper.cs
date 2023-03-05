using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using Dapper;
using Microsoft.VisualBasic;
using Npgsql;

namespace MarketPlace;

public class DeleteReviewHelper
{
    const string _connString = "Server=localhost;Port=5432;User Id=omr;Password=1234;Database=marketplace";

    public static async Task DeleteReview(DeletingReview deletingReview, int user_id)
    {
        await using var db = new NpgsqlConnection(_connString);
        var sqlQuery = @$"DELETE FROM public.reviews WHERE message = '{deletingReview.Message}' and rating = {deletingReview.Rating} and product_id = {deletingReview.ProductId} and reviewer_id = ";
        sqlQuery += $"{user_id}";
        Console.WriteLine("111");
        Console.WriteLine(sqlQuery);
        await db.ExecuteAsync(sqlQuery, deletingReview);
    }
}