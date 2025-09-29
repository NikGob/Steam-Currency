using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SteamCurrencyAPI.Models;

public class CurrencyData
{
    [BsonIgnoreIfDefault]
    [BsonId]
    public ObjectId Id { get; set; }
    public DateTime DateAtUtc { get; set; }
    public decimal CurrencyPrice { get; set; }
}