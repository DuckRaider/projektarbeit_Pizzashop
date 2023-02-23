using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace projektarbeit_Pizzashop
{
    internal class Groesse
    {
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("groesseInCm")]
        public double GroesseInCm { get; set; }
        [BsonElement("aufpreis")]
        public double Aufpreis { get; set; }
        [BsonElement("kcal")]
        public double Kcal { get; set; }

        public Groesse(string name, double groesseInCm, double aufpreis, double kcal)
        {
            Name = name;
            GroesseInCm = groesseInCm;
            Aufpreis = aufpreis;
            Kcal = kcal;
        }

    }
}
