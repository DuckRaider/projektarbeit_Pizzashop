using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace projektarbeit_Pizzashop
{
    internal class Bestellposition
    {
        [BsonElement("pizzaId")]
        public ObjectId PizzaId { get; set; }
        [BsonIgnore]
        public Pizza Pizza { get; set; }
        [BsonElement("groesse")]
        public Groesse Groesse { get; set; }
        [BsonElement("extrazutaten")]
        public List<Zutat> Extrazutaten { get; set; }
        [BsonElement("preis")]
        public double Preis { get; set; }
        [BsonElement("stueckzahl")]
        public int Stueckzahl { get; set; }
    }
}
