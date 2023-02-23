using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace projektarbeit_Pizzashop
{
    internal class Pizza
    {
        [BsonId]
        [BsonElement("_id")]
        public ObjectId _Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("zutaten")]
        public List<Zutat> Zutaten { get; set; }
        [BsonElement("einzelpreis")]
        public double Einzelpreis { get; set; }
        [BsonElement("groessen")]
        public List<Groesse> Groessen { get; set; }


        public Pizza()
        {
            Zutaten = new List<Zutat>();
            Groessen = new List<Groesse>();
        }

        public void AddZutat(Zutat zutat)
        {
            Zutaten.Add(zutat);
        }
        public void AddGroesse(Groesse groesse)
        {
            Groessen.Add(groesse);
        }
        public void GenerateId()
        {
            _Id = ObjectId.GenerateNewId();
        }
    }
}
