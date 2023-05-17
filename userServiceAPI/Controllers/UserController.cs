using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using userServiceAPI.Models;
namespace userServiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{

    private readonly ILogger<UserController> _logger;

    private readonly IMongoDatabase _database;

    //Collection i selve databasen
    private string userCol = string.Empty;

    public UserController(ILogger<UserController> logger, IConfiguration configuration)
    {
        var server = configuration["server"] ?? string.Empty;
        var port = configuration["port"] ?? string.Empty;
        var database = configuration["database"] ?? string.Empty;
        userCol = configuration["userCol"] ?? string.Empty;
        


        var connectionString = $"mongodb://{server}:{port}/";

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(database);
        _logger = logger;
    }
    [HttpGet("GetUser")]
    public User GetUser(LoginModel loginModel){
        var collection = _database.GetCollection<User>(userCol);
        User user =  collection.Find(x => x.UserName == loginModel.UserName && x.UserPassword == loginModel.UserPassword).FirstOrDefault();
        return  user;
    }
    [HttpPost("PostUser")]
    public void PostUser(User user){
        var collection = _database.GetCollection<User>(userCol);
        var highestAuction = collection.Find(_ => true)
            .SortByDescending(a => a.UserID)
            .FirstOrDefault();

        int nextUserID = 1;
        if (highestAuction != null)
        {
            nextUserID = highestAuction.UserID + 1;
        }
        // Opret den nye auktion med det næste auktions-ID
        user.UserID = nextUserID;
        // Indsæt auktionen i MongoDB
        _database.GetCollection<User>(userCol).InsertOne(user);
    }
    
}
