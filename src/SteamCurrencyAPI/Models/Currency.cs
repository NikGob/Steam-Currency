using MongoDB.Bson;
namespace SteamCurrencyAPI.Models
{
    public class Currency
    {
        public ObjectId Id { get; set; }
        public required string CurrencyCode { get; set; }
        public List<CurrencyData> CurrencyDatas { get; set; } = new List<CurrencyData>();
        public DateTime CreatedAtUTC { get; set; }
    }
}
