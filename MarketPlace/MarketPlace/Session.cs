using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Security.Cryptography;

namespace MarketPlace
{
    public static class Session
    {
        const double _sessionTime = 20;
        /*private readonly User _user;
        private readonly int _id;
        private readonly string? _name;
        private readonly string? _password;
        private readonly long _balance;
        private string? _sessionId;*/

        /*public Session(User user)
        {
            if (user is not null)
            {
                _name = user.Name!;
                _password = user.Password!;
                _id = user.Id;
                _balance = user.Balance;
                _sessionId = BCrypt.Net.BCrypt.HashString(user.Id + "mysite");
            }
        }*/
        /*public override string ToString()
        {
            return JsonSerializer.Serialize(new User(_id, _name, _password, _balance));
        }*/
        public static async Task SetSession(this User user, HttpListenerContext context)
        {
            var randomBytes = new byte[32];
            new Random().NextBytes(randomBytes);
            var sessionId = Convert.ToBase64String(randomBytes);
            if (user != null)
            {
                await RedisStore.RedisCashe.StringSetAsync(sessionId + (user.Id * 10 - 3),user.Id, TimeSpan.FromMinutes(_sessionTime));
                context.Response.Cookies.Add(new Cookie("sessionId", sessionId + (user.Id * 10 - 3))
                {
                    Expires = DateTime.UtcNow.AddMinutes(_sessionTime),
                    Path = "/"
                });
            }
        }
        
        public static void RemoveSession( this HttpListenerContext context)
        {
            context.Response.Cookies.Add(new Cookie("sessionId", "delete")
            {
                Expires = DateTime.UtcNow.AddMinutes(-1),
                Path = "/"
            });
            Console.WriteLine(context.Request.Cookies["sessionId"]);
        }

        public static async Task<string> GetCookieInformation(this HttpListenerContext context)
        {
            var cookieValue = context.Request.Cookies["sessionId"]?.Value;
            if (cookieValue is null)
            {
                return null!;
            }
            var resultOfGettingRedis = await RedisStore.RedisCashe.StringGetAsync(cookieValue);
            if (!resultOfGettingRedis.HasValue)
            {
                return null!;
            }
            
            return resultOfGettingRedis.ToString();
        }

        public static async Task<int> GetUserId(this HttpListenerContext context)
        {
            var id = await GetCookieInformation(context);
            if (id is not null && int.TryParse(id, out var result))
                return result;
            return -1;
        }
    }
}
