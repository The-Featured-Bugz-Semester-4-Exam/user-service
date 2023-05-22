using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using userServiceAPI.Models;
using System.Security.Permissions;
namespace userServiceAPI.Controllers;

[ApiController]
[Route("api")]
public class UserController : ControllerBase
{

    private readonly ILogger<UserController> _logger;

    private readonly IMongoDatabase _database;

    //Collection i selve databasen
    private string userCol = string.Empty;
    private string database = string.Empty;
    public UserController(ILogger<UserController> logger, IConfiguration configuration)
    {
        _logger = logger;
        
        database = configuration["database"] ?? string.Empty;
        userCol = configuration["userCol"] ?? string.Empty;
        
        var client = new MongoClient($"mongodb://{configuration["server"] ?? string.Empty}:{configuration["port"] ?? string.Empty}/");
        _database = client.GetDatabase(database);
    }
    [HttpGet("GetUser")]
    public User GetUser([FromQuery] LoginModel loginModel){
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
