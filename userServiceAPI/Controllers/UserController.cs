using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Microsoft.AspNetCore;
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

        //Vise, hvilken ip adresse denne service er på.
        var hostName = System.Net.Dns.GetHostName();
        var ips = System.Net.Dns.GetHostAddresses(hostName);
        var _ipaddr = ips.First().MapToIPv4().ToString();
        _logger.LogInformation(1, $"user-service responding from {_ipaddr}");

        //Få forbindelse til Mongodb
        database = configuration["database"] ?? string.Empty;
        userCol = configuration["userCol"] ?? string.Empty;
        var client = new MongoClient($"{configuration["connectionString"]}");
        _database = client.GetDatabase(database);
    }

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



    [HttpGet("getUser")]
    public IActionResult GetUser([FromQuery] LoginModel loginModel)
    {
        var collection = _database.GetCollection<User>(userCol);
        User user = collection.Find(x => x.UserName == loginModel.UserName && x.UserPassword == loginModel.UserPassword).FirstOrDefault();

        if (user != null)
        {
            _logger.LogInformation("User er fundet");
            return Ok(user);
        }else{
            return NotFound();
        }
    }

    [HttpPost("postUser")]
    public IActionResult PostUser(User user)
    {
        var collection = _database.GetCollection<User>(userCol);
        var highestAuction = collection.Find(_ => true)
            .SortByDescending(a => a.UserID)
            .FirstOrDefault();

        //Være sikker på der ikke er det samme id to gange.
        int nextUserID = 1;
        if (highestAuction != null)
        {
            nextUserID = highestAuction.UserID + 1;
        }
        // Opret den nye auktion med det næste auktions-ID
        user.UserID = nextUserID;

        // Indsæt auktionen i MongoDB
        _database.GetCollection<User>(userCol).InsertOne(user);
        _logger.LogInformation("User er tilføjet");
        return Ok("User er tilføjet");
    }
}
