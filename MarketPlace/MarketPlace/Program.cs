using System.Diagnostics;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using MarketPlace;
using StackExchange.Redis;

HttpClient client = new();
HttpListener listener = new();
client.DefaultRequestHeaders.UserAgent.ParseAdd(
    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36 Edg/106.0.1370.34");
listener.Prefixes.Add("http://localhost:1111/");
listener.Start();
SqlConnection connection = new(@"Data source= LAPTOP-QHM9MDKR;Initial Catalog=MyDataBase; Integrated Security=True");
var userBalancesInUpdate = new Dictionary<int, bool>();

var usersIds = await UserRepository.GetUsersId();
foreach (var id in usersIds)
{
    userBalancesInUpdate.Add(id, false);
}
while (listener.IsListening)
{
    try
    {
        var context = await listener.GetContextAsync();
        var request = context.Request;
        var isUsingShowStatic = true;
        _ = Task.Run(async () =>
        {
            Console.WriteLine(request.Url?.LocalPath);
            switch (request.Url?.LocalPath)
            {
                //Pages
                case "/":
                    await WebHelper.Home(context);
                    break;
                case "/productsNotRegistered":
                    await WebHelper.ProductsNotRegistered(context);
                    break;
                //Actions
                case "/register":
                    var userId = await WebHelper.Register(context);
                    if (userId != -1)
                    {
                        userBalancesInUpdate.Add(userId, false);
                    }
                    break;
                case "/signIn":
                    await WebHelper.SignIn(context);
                    break;
                case "/getPersonInfo":
                    await WebHelper.GetUser(context);
                    isUsingShowStatic = false;
                    break;
                case "/getProductsFromDB":
                    await WebHelper.GetProductsFromDB(context);
                    isUsingShowStatic = false;
                    break;
                default:
                    break;
            }
            
            if (context.Request.Cookies["sessionId"] is not null)
            {
                var enteredUserId = context.GetCookieInformation().Result;
                if (int.TryParse(enteredUserId, out var intUserId))
                {
                    var enteredUser = await UserRepository.GetUser(intUserId);
                    if (enteredUser != null)
                    {
                        await Session.SetSession(enteredUser, context);
                        
                        switch (request.Url?.LocalPath)
                        {
                            //Pages
                            case "/products":
                                await WebHelper.Products(context);
                                break;
                            case "/myProducts":
                                await WebHelper.GetMyProducts(context);
                                break;
                            case "/balancePage":
                                await WebHelper.SetBalacePage(context);
                                break;
                            case "/settings":
                                await WebHelper.UpdateUserData(context);
                                break;
                            case "/buyProductsPage":
                                await WebHelper.ShowBuyProductsPage(context);
                                break;
                            case "/filter":
                                await WebHelper.ShowFindPage(context);
                                break;
                            case "/reviews":
                                await WebHelper.ShowReviewPage(context);
                                break;
                            case "/getReviews":
                                await WebHelper.GetReviews(context);
                                break;
                            /*case "/getFilteredProducts":
                                await WebHelper.ShowFilteredProducts(context);
                                break;*/
                            //Actions
                            case "/getUserProducts":
                                await WebHelper.GetUserProducts(context);
                                isUsingShowStatic = false;
                                break;
                            case "/getUserReviews":
                                await WebHelper.GetUserReviews(context);
                                isUsingShowStatic = false; 
                                break;
                            case "/getUserReviewsCanEdit":
                                await WebHelper.GetUserReviewsCanEdit(context);
                                isUsingShowStatic = false;
                                break;
                            case "/getUserProductsList":
                                await WebHelper.GetUserProductList(context);
                                isUsingShowStatic = false;
                                break;
                            case "/addProductCount":
                                await WebHelper.AddProductCount(context);
                                isUsingShowStatic = false;
                                break;
                            case "/deleteUserProduct":
                                await WebHelper.DeleteUserProduct(context);
                                isUsingShowStatic = false;
                                break;
                            case "/addProducts":
                                await WebHelper.AddProducts(context);
                                isUsingShowStatic = false;
                                break;
                            case "/addReviews":
                                await WebHelper.AddReviews(context);
                                isUsingShowStatic = false;
                                break;
                            case "/updBalance":
                                if (!userBalancesInUpdate[await context.GetUserId()])
                                {
                                    userBalancesInUpdate[intUserId] = true;
                                    await WebHelper.AddBalance(context);
                                    userBalancesInUpdate[await context.GetUserId()] = false;
                                    isUsingShowStatic = false;
                                }
                                break; 
                            case "/updName":
                                await WebHelper.UpdateUserName(context);
                                isUsingShowStatic = false;
                                break;
                            case "/getUserProductById":
                                await WebHelper.GetUserProductById(context);
                                isUsingShowStatic = false;
                                break;
                            case "/updPass":
                                await WebHelper.UpdateUserPass(context);
                                isUsingShowStatic = false;
                                break;
                            case "/leaveAccount":
                                await WebHelper.LeaveAccount(context);
                                isUsingShowStatic = false;
                                break;
                            case "/buyAllProducts":
                                await WebHelper.BuyAllProducts(context);
                                isUsingShowStatic = false;
                                break;
                            case "/getFilteredProducts":
                                await WebHelper.GetFilteredProducts(context);
                                isUsingShowStatic = false;
                                break;
                            case "/deleteReview":
                                await WebHelper.DeleteReview(context);
                                isUsingShowStatic = false;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        WebHelper.IncorrectSession(context);
                    }
                }
                else
                {
                    WebHelper.IncorrectSession(context);
                }
            }
            else
            {
                switch (request.Url?.LocalPath)
                {
                    //Pages
                    case "/buyProductsPage":
                    case "/products":
                    case "/myProducts":
                    case "/addProductCount":
                    case "/deleteUserProduct":
                    case "/balancePage":
                    case "/settings":
                    case "/filter":
                        //case"/notFound": //Addings
                        await WebHelper.NotFound(context);
                        break;
                    //Actions

                    default:
                        break;
                }
            }

            if (isUsingShowStatic)
            {
                await context.ShowStatic();
            }
            else
            {
                context.Response.Close();
            }
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine("Ошибка!");
        Console.WriteLine(ex.Message);
    }
}

listener.Stop();