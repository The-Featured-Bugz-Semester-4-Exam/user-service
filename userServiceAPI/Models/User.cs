using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace userServiceAPI.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public int UserID { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserPassword { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string UserAddress { get; set; } = string.Empty;
}