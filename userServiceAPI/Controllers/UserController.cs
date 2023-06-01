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

    // Collection name in the database
    private string userCol = string.Empty;
    private string database = string.Empty;

    public UserController(ILogger<UserController> logger, IConfiguration configuration)
    {
        _logger = logger;

        // Get the host name and IP address for logging purposes
        var hostName = System.Net.Dns.GetHostName();
        var ips = System.Net.Dns.GetHostAddresses(hostName);
        var _ipaddr = ips.First().MapToIPv4().ToString();
        _logger.LogInformation(1, $"user-service responding from {_ipaddr}");

        // Get the database and collection names from the configuration
        database = configuration["database"] ?? string.Empty;
        userCol = configuration["userCol"] ?? string.Empty;

        // Create a MongoClient instance and connect to the database
        var client = new MongoClient($"{configuration["connectionString"]}");
        _database = client.GetDatabase(database);
    }

    // Get API endpoint to retrieve assembly version information
    [HttpGet("version")]
    public IEnumerable<string> Get()
    {
        var properties = new List<string>();
        var assembly = typeof(Program).Assembly;
        foreach (var attribute in assembly.GetCustomAttributesData())
        {
            properties.Add($"{attribute.AttributeType.Name} - {attribute.ToString()}");
        }
        return properties;
    }

    // Get user API endpoint
    [HttpGet("getUser")]
    public IActionResult GetUser([FromQuery] LoginModel loginModel)
    {
        var collection = _database.GetCollection<User>(userCol);

        // Find the user based on the provided login credentials
        User user = collection.Find(x => x.UserName == loginModel.UserName && x.UserPassword == loginModel.UserPassword).FirstOrDefault();

        if (user != null)
        {
            _logger.LogInformation("User found");
            return Ok(user);
        }
        else
        {
            // Return a NotFound response if the user is not found
            string feedback = "Can't find user: " + loginModel.UserName;
            _logger.LogInformation(feedback);
            return NotFound(feedback);
        }
    }

    // Post user API endpoint
    [HttpPost("postUser")]
    public IActionResult PostUser(User user)
    {
        var collection = _database.GetCollection<User>(userCol);

        // Get the highest UserID from the collection
        var highestUser = collection.Find(_ => true)
            .SortByDescending(a => a.UserID)
            .FirstOrDefault();

        int nextUserID = 1;
        if (highestUser != null)
        {
            // Set the nextUserID to the highest UserID + 1
            nextUserID = highestUser.UserID + 1;
        }

        user.UserID = nextUserID;

        // Insert the user into the collection
        _database.GetCollection<User>(userCol).InsertOne(user);

        string feedback = "User added";
        _logger.LogInformation(feedback);
        return Ok(feedback);
    }
}
