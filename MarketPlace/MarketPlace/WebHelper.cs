using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Channels;
using System.Threading.Tasks;
using Azure;
using static System.Int32;

namespace MarketPlace
{
    public static class WebHelper
    {
        public static byte[] GetBytes(this string convertingString)
        {
            return Encoding.UTF8.GetBytes(convertingString);
        }

        public static Tuple<string, string> GetTupleFromArray(this string[] strArr)
        {
            return new Tuple<string, string>(strArr[0], strArr[1]);
        }

        public static async Task<Dictionary<string, string>> GetFormInfo(this HttpListenerContext context)
        {
            await using var inputStream = context.Request.InputStream;
            using var reader = new StreamReader(inputStream);
            var content = await reader.ReadToEndAsync();
            Console.WriteLine(content);
            if (content is not null)
            {
                return content.Split("&").ToDictionary(x =>x.Split("=")[0], x => x.Split("=")[1]);
            }

            return null!;
        }
        
        public static async Task Home(HttpListenerContext context)
        {
            await context.Response.ShowFile("WWW/html/mainpage.html");
        }

        public static async Task Products(HttpListenerContext context)
        {
            await context.Response.ShowFile("WWW/html/products.html");
        }

        public static async Task GetMyProducts(HttpListenerContext context)
        {
            await context.Response.ShowFile("WWW/html/myProducts.html");
        }

        public static async Task SetBalacePage(HttpListenerContext context)
        {
            await context.Response.ShowFile("WWW/html/updBalance.html");
        }

        public static async Task ProductsNotRegistered(HttpListenerContext context)
        {
            await context.Response.ShowFile("WWW/html/productsNotRegistered.html");
        }
    
        public static async Task NotFound(HttpListenerContext context)
        {
            await context.Response.ShowFile("WWW/html/notFound.html");
        }
        
        public static async Task UpdateUserData(HttpListenerContext context)
        {
            await context.Response.ShowFile("WWW/html/settings.html");
        }
        
        public static async Task ShowBuyProductsPage(HttpListenerContext context)
        {
            await context.Response.ShowFile("WWW/html/buyProducts.html");
        }
        
        public static async Task ShowFindPage(HttpListenerContext context)
        {
            await context.Response.ShowFile("WWW/html/filter.html");
        }
        
        public static async Task ShowReviewPage(HttpListenerContext context)
        {
            /*await using var inputStream = context.Request.InputStream;
            using var reader = new StreamReader(inputStream);
            var content = await reader.ReadToEndAsync();
            Console.WriteLine(content);
            var id = JsonSerializer.Deserialize<int>(content);
            Console.WriteLine(id);
            Console.WriteLine("Отзывы");
            context.Response.Headers.Add("id", id.ToString());*/
            await context.Response.ShowFile("WWW/html/review.html");
        }
        public static async Task ShowFilteredProducts(HttpListenerContext context)
        {
            var checkRating = false;
            var checkPrice = false;
            var checkDes = false;
            var content = await context.GetFormInfo();
            Console.WriteLine(content);
            var searchName = content["searchName"];
            if (content.TryGetValue("checkRating", out var isRating) && isRating == "on")
            {
                checkPrice = true;
            }
            
            if (content.TryGetValue("checkPrice", out var isPrice) && isPrice == "on")
            {
                checkPrice = true;
            }
            
            if (content["checkDes"] == "on")
            {
                checkDes = true;
            }
            
        }

        public static async Task GetUserProductById(HttpListenerContext context)
        {
            await using var inputStream = context.Request.InputStream;
            using var reader = new StreamReader(inputStream);
            var content = await reader.ReadToEndAsync();
            TryParse(content.Replace(@"""", ""), out var productId);
            Console.WriteLine(productId + "  ProductId");
            await context.Response.OutputStream.WriteAsync(JsonSerializer.Serialize(await ProductRepository.GetProduct(productId)).GetBytes());
        }

        public static async Task DeleteReview(HttpListenerContext context)
        {
            await using var inputStream = context.Request.InputStream;
            using var reader = new StreamReader(inputStream);
            var content = await reader.ReadToEndAsync();
            Console.WriteLine(content);
            var deletingReview = JsonSerializer.Deserialize<DeletingReview>(content);
            await DeleteReviewHelper.DeleteReview(deletingReview, context.GetUserId().Result);
            context.Response.StatusCode = 200;
        }
        
        public static async Task GetFilteredProducts(HttpListenerContext context)
        {
            await using var inputStream = context.Request.InputStream;
            using var reader = new StreamReader(inputStream);
            var content = await reader.ReadToEndAsync();
            var productParams = JsonSerializer.Deserialize<ProductParams>(content);
            var a = await ProductRepositoryWithCount.GetFilteredProducts(productParams!, context.GetUserId().Result);
            Console.WriteLine(a[0].Name);
            await context.Response.OutputStream.WriteAsync(
                JsonSerializer
                    .Serialize(a)
                    .GetBytes());
        }
        
        /*public static async Task ShowFilteredPage(HttpListenerContext context)
        {
            /*var personInfo = await context.GetFormInfo();
            if (personInfo is null)
            {
                return;
            }
            foreach (var info in personInfo)
            {
                context.Response.Headers.Add(info.Key,info.Value);
            }#1#
            //var content = await context.GetFormInfo();
            /*await using var inputStream = context.Request.InputStream;
            using var reader = new StreamReader(inputStream);
            var content = await reader.ReadToEndAsync();
            Console.WriteLine("content");
            Console.WriteLine(content);#1#
            await context.Response.ShowFile("WWW/html/filteredProducts.html");
            //context.Response.OutputStream.WriteAsync(@$"div id='infoFilter' class='{content}'></div>".GetBytes());
        }*/
        
        public static async Task LeaveAccount(HttpListenerContext context)
        {
            context.Response.StatusCode = 300;
            Session.RemoveSession(context);
            await context.Response.ShowFile("WWW/html/mainpage.html");
            context.Response.Close();
        }
        public static async Task BuyAllProducts(HttpListenerContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = await ProductRepositoryWithCount.BuyAllProducts(context.GetUserId().Result) > 0 ? 200 : 418;
            context.Response.OutputStream.Close();
        }
        
        public static void IncorrectSession(HttpListenerContext context)
        {
            context.RemoveSession();
            context.Response.StatusCode = 300;
            context.Response.OutputStream.WriteAsync(null);
        }
        
        public static async Task GetUser(HttpListenerContext context)
        {
            await using var stream = context.Response.OutputStream;
            var user = await UserRepository.GetUser(await context.GetUserId());
            if (user is not null)
            {
                context.Response.StatusCode = 200;
                await stream.WriteAsync((await user.ToJSON()).GetBytes());
            }
            else
            {
                context.Response.StatusCode = 418;
            }
        }
    
        public static async Task AddBalance(HttpListenerContext context)
        {
            await Task.Delay(5000);
            try
            {
                await using var inputStream = context.Request.InputStream;
                using var reader = new StreamReader(inputStream);
                var content = await reader.ReadToEndAsync();
                var count = JsonSerializer.Deserialize<long>(content);
                int delay = 0;
                if (count == 10)
                {
                    delay = 5;
                }

                if (count == 100)
                {
                    delay = 55;
                }

                await Task.Delay(delay * 1000);
                UserRepository.UpdateUserBalance(context.GetUserId().Result, count);
                context.Response.StatusCode = 200;
            }
            catch (Exception)
            {
                context.Response.StatusCode = 400;
            }
        }
        
        public static async Task GetReviews(HttpListenerContext context)
        {
            await using var inputStream = context.Request.InputStream;
            using var reader = new StreamReader(inputStream);
            var content = await reader.ReadToEndAsync();
            Console.WriteLine(content);
            var updNameInfo = JsonSerializer.Deserialize<UpdatingNameDTO>(content);
            var userId = await context.GetUserId();
            var user = UserRepository.GetUser(userId).Result;
            if (updNameInfo is not null && user is not null)
            {
                Console.WriteLine(user.Password);
                Console.WriteLine(updNameInfo.Password);
                if (BCrypt.Net.BCrypt.Verify(updNameInfo.Password, user.Password))
                {
                    context.Response.StatusCode = UserRepository.UpdateUserName(user.Password, user.Name, updNameInfo.Name).Result != -1 ? 200 : 412;
                    context.Response.Close();
                }
            }
            context.Response.StatusCode = 412;
            context.Response.Close();
        }
        public static async Task UpdateUserName(HttpListenerContext context)
        {
            await using var inputStream = context.Request.InputStream;
            using var reader = new StreamReader(inputStream);
            var content = await reader.ReadToEndAsync();
            Console.WriteLine(content);
            var updNameInfo = JsonSerializer.Deserialize<UpdatingNameDTO>(content);
            var userId = await context.GetUserId();
            var user = UserRepository.GetUser(userId).Result;
            if (updNameInfo is not null && user is not null)
            {
                Console.WriteLine(user.Password);
                Console.WriteLine(updNameInfo.Password);
                if (BCrypt.Net.BCrypt.Verify(updNameInfo.Password, user.Password))
                {
                    context.Response.StatusCode = UserRepository.UpdateUserName(user.Password, user.Name, updNameInfo.Name).Result != -1 ? 200 : 412;
                    context.Response.Close();
                }
            }
            context.Response.StatusCode = 412;
            context.Response.Close();
        }
        
        public static async Task GetUserProductList(HttpListenerContext context)
        {
            Console.WriteLine("@@  " + context.GetUserId().Result);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            var products = await ProductRepositoryWithCount.GetUserProductList(context.GetUserId().Result);
            var price = products.Sum(x => x.Price * x.Count);
            Console.WriteLine("@@!!@@");
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var product in products)
            {
                stringBuilder.Append($"{product.Name}: {product.Count}pcs - {product.Count * product.Price}⚡\n");
            }
            stringBuilder.Append($"\nSum: {price}⚡");
            Console.WriteLine(stringBuilder + " " + price);
            await context.Response.OutputStream.WriteAsync(
                JsonSerializer.Serialize(new {ShoppingList = stringBuilder.ToString(), Price = price}).GetBytes());
        }
        
        public static async Task UpdateUserPass(HttpListenerContext context)
        {
            await using var inputStream = context.Request.InputStream;
            using var reader = new StreamReader(inputStream);
            var content = await reader.ReadToEndAsync();
            Console.WriteLine(content);
            Console.WriteLine("cont");
            var updNameInfo = JsonSerializer.Deserialize<UpdatingPasswordDTO>(content);
            Console.WriteLine(0);
            var userId = await context.GetUserId();
            var user = UserRepository.GetUser(userId).Result;
            Console.WriteLine(1);
            if (updNameInfo is not null && user is not null)
            {
                Console.WriteLine(2);
                if (BCrypt.Net.BCrypt.Verify(updNameInfo.LastPassword, user.Password))
                {
                    Console.WriteLine(3);
                    var a = UserRepository.UpdateUserPassword(user.Password, user.Name, updNameInfo.NewPassword).Result;
                    context.Response.StatusCode = a != -1 ? 200 : 412;
                    context.Response.Close();
                }
                context.Response.StatusCode = 418;
                context.Response.Close();
            }
            context.Response.StatusCode = 412;
            context.Response.Close();
        }

        public static async Task DeleteUserProduct(HttpListenerContext context)
        {
            await using var inputStream = context.Request.InputStream;
            using var reader = new StreamReader(inputStream);
            var content = await reader.ReadToEndAsync();
            var userProduct = JsonSerializer.Deserialize<UserProductDTO>(content);
            var userId = await context.GetUserId();
            if (userProduct is not null)
            {
                await UserProductRepository.DeleteUserProduct(userId, userProduct.ProductId);
            }

            context.Response.StatusCode = 418;
            await context.Response.OutputStream.WriteAsync(
                Encoding.UTF8.GetBytes(
                    JsonSerializer.Serialize(
                        new ProductCountUserBalanceDTO
                        {
                            ProductCount = 0,
                            Balance = UserRepository.GetUser(userId).Result.Balance
                        })));
            context.Response.OutputStream.Close();
        }

        public static async Task AddProducts(HttpListenerContext context)
        {
            await using var inputStream = context.Request.InputStream;
            using var reader = new StreamReader(inputStream);
            var content = await reader.ReadToEndAsync();
            Console.WriteLine(content);
            var userId = await context.GetUserId();
            var userProducts = JsonSerializer.Deserialize<UserProductDTO[]>(content);
            if (userProducts != null)
            {
                Dictionary<int, long> products = new();
                    //Enumerable.Repeat(new ProductIDCountDTO {ProductId = -1 ,ProductCount = 0},userProducts.Length).ToArray();
                Console.WriteLine(userProducts.Length);
                foreach (var userProduct in userProducts)
                {
                    if (products.TryGetValue(userProduct.ProductId, out _))
                        {
                            products[userProduct.ProductId] += userProduct.ProductCount;
                        }
                        if (!products.TryGetValue(userProduct.ProductId, out _))
                        {
                            products[userProduct.ProductId] = userProduct.ProductCount;
                        }
                }
                
                foreach (var product in products)
                {
                    AddProductToDB(new ProductIDCountDTO() { ProductId = product.Key, ProductCount = product.Value }, userId);
                }
            }
            
            /*var userBalance = UserRepository.GetUserBalance(userId).Result;
            var productsCount = UserProductRepository.GetAllUserProductsWithBalance(userId).Result;
            context.Response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(new ProductIDCountBalanceDTO(){ Products = productsCount, Balance = userBalance})));
            Console.WriteLine(JsonSerializer.Serialize(new ProductIDCountBalanceDTO(){ Products = productsCount, Balance = userBalance}));*/
            context.Response.StatusCode = 200;
            context.Response.Close();
        }
        
        
        public static async Task AddReviews(HttpListenerContext context)
        {
            await using var inputStream = context.Request.InputStream;
            using var reader = new StreamReader(inputStream);
            var content = await reader.ReadToEndAsync();
            Console.WriteLine(content);
            var userId = await context.GetUserId();
            Console.WriteLine("VEsELO");
            var userReviews = JsonSerializer.Deserialize<UserReviewDTO[]>(content);
            Console.WriteLine("Кайф");
            Console.WriteLine(userReviews[0].Rating + " RAT");
            if (userReviews != null)
            {
                try
                {
                    foreach (var review in userReviews)
                    {
                        await ReviewRepository.AddReview(new Review(-1, userId, review.ProductId, review.Rating,
                            review.Message));
                        context.Response.StatusCode = 200;
                    }
                }
                catch (Exception)
                {
                    context.Response.StatusCode = 400;
                }
            }
            
            /*var userBalance = UserRepository.GetUserBalance(userId).Result;
            var productsCount = UserProductRepository.GetAllUserProductsWithBalance(userId).Result;
            context.Response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(new ProductIDCountBalanceDTO(){ Products = productsCount, Balance = userBalance})));
            Console.WriteLine(JsonSerializer.Serialize(new ProductIDCountBalanceDTO(){ Products = productsCount, Balance = userBalance}));*/
            context.Response.StatusCode = 200;
            context.Response.Close();
        }
        

        public static async Task AddProductToDB(ProductIDCountDTO userProduct, int userId)
        {
            if (userProduct != null)
            {
                var userProductFromDB = UserProductRepository.GetUserProduct(userId, userProduct.ProductId).Result;
                await UserProductRepository.UpdateUserProducts(userId, userProduct.ProductId,
                        userProduct.ProductCount, userProductFromDB);
            }
        }
        
        
        public static async Task AddProductCount(HttpListenerContext context)
        {
            await using var inputStream = context.Request.InputStream;
            using var reader = new StreamReader(inputStream);
            var content = await reader.ReadToEndAsync();
            var userProduct = JsonSerializer.Deserialize<UserProductDTO>(content);
            var userId = await context.GetUserId();
            var userProductFromDB = UserProductRepository.GetUserProduct(userId, userProduct.ProductId).Result;
            var resultOfUpdatingDB =
                UserProductRepository.UpdateUserProductWithStatusCodes(userId, userProduct.ProductId,
                    userProduct.ProductCount, userProductFromDB).Result;
            if (userId is not -1 && userProduct is not null)
            {
                if (resultOfUpdatingDB == -205)
                {
                    context.Response.StatusCode = 201;
                    await context.Response.OutputStream.WriteAsync(
                        Encoding.UTF8.GetBytes(JsonSerializer.Serialize(
                            new ProductCountUserBalanceDTO
                            {
                                ProductCount = 0,
                                Balance = UserRepository.GetUser(userId).Result.Balance
                            })));
                }
                else if (resultOfUpdatingDB != -1)
                {
                    context.Response.StatusCode = 200;
                    await context.Response.OutputStream.WriteAsync(
                        Encoding.UTF8.GetBytes(
                            JsonSerializer.Serialize(
                                new ProductCountUserBalanceDTO
                                {
                                    ProductCount = UserProductRepository.GetUserProduct(userId, userProduct.ProductId)
                                        .Result.ProductCount,
                                    Balance = UserRepository.GetUser(userId).Result.Balance
                                })));
                }
                else
                {
                    context.Response.StatusCode = 418;
                }
            }
            else
            {
                context.Response.StatusCode = 418;
            }

            context.Response.OutputStream.Close();
        }

        public static async Task<int> Register(HttpListenerContext context)
        {
            await using (var inputStream = context.Request.InputStream)
            {
                using var reader = new StreamReader(inputStream);
                var content = await reader.ReadToEndAsync();
                var user = JsonSerializer.Deserialize<User>(content);
                await using var stream = context.Response.OutputStream;
                if (UserRepository.AddUser(user).Result != -1)
                {
                    var succsessOperation = Encoding.ASCII.GetBytes("All done!");
                    context.Response.StatusCode = 201;
                    await Session.SetSession(user, context);
                    await context.Response.OutputStream.WriteAsync(succsessOperation);
                    Console.WriteLine("allDone");
                    return user.Id;
                }
                else
                {
                    var succsessOperation = Encoding.ASCII.GetBytes("Error");
                    context.Response.StatusCode = 208;
                    await context.Response.OutputStream.WriteAsync(succsessOperation);
                    return -1;
                }
            }
        }

        public static async Task SignIn(HttpListenerContext context)
        {
            await using (var inputStream = context.Request.InputStream)
            {
                using var reader = new StreamReader(inputStream);
                var content = await reader.ReadToEndAsync();
                var user = JsonSerializer.Deserialize<User>(content);
                await using var stream = context.Response.OutputStream;
                user = UserRepository.GetUser(user?.Name, user?.Password).Result;
                if (user != null)
                {
                    var succsessOperation = Encoding.ASCII.GetBytes("All done!");
                    await Session.SetSession(user, context);
                    context.Response.StatusCode = 201;
                    await context.Response.OutputStream.WriteAsync(succsessOperation);
                }
                else
                {
                    var succsessOperation = Encoding.ASCII.GetBytes("Error");
                    context.Response.StatusCode = 208;
                    await context.Response.OutputStream.WriteAsync(succsessOperation);
                }
            }
        }

        public static async Task GetProductsFromDB(HttpListenerContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            await context.Response.OutputStream.WriteAsync(
                JsonSerializer
                    .Serialize(await ProductRepositoryWithCount.GetAllProductFromDB(await context.GetUserId()))
                    .GetBytes());
        }

        public static async Task GetUserProducts(HttpListenerContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            await context.Response.OutputStream.WriteAsync(
                JsonSerializer.Serialize(await ProductRepositoryWithCount.GetProductFromDB(await context.GetUserId()))
                    .GetBytes());
        }
        
        public static async Task GetUserReviews(HttpListenerContext context)
        {
            await using var inputStream = context.Request.InputStream;
            using var reader = new StreamReader(inputStream);
            var content = await reader.ReadToEndAsync();
            Console.WriteLine(content);
            Console.WriteLine();
            var productId = -1;
            TryParse(content.Replace(@"""", ""), out productId);
            Console.WriteLine(productId);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            await context.Response.OutputStream.WriteAsync(
                JsonSerializer.Serialize(await UserReviewRepository.GetReviewsFromDB(productId, await context.GetUserId()))
                    .GetBytes());
        }
        
        public static async Task GetUserReviewsCanEdit(HttpListenerContext context)
        {
            await using var inputStream = context.Request.InputStream;
            using var reader = new StreamReader(inputStream);
            var content = await reader.ReadToEndAsync();
            Console.WriteLine(content);
            Console.WriteLine();
            var productId = -1;
            TryParse(content.Replace(@"""", ""), out productId);
            Console.WriteLine(productId);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            await context.Response.OutputStream.WriteAsync(
                JsonSerializer.Serialize(await UserReviewRepository.GetUsersReviewsFromDB(productId, await context.GetUserId()))
                    .GetBytes());
        }
    }
}