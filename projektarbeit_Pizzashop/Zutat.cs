using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace projektarbeit_Pizzashop
{
    internal class Zutat
    {
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("aufpreis")]
        public double Aufpreis { get; set; }

        public Zutat(string name, double aufpreis)
        {
            Name = name;
            Aufpreis = aufpreis;
        }
    }
}
