using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace projektarbeit_Pizzashop
{
    internal class Bestellung
    {
        [BsonId]
        [BsonElement("_id")]
        public ObjectId _Id { get; set; }
        [BsonElement("bestellnummer")]
        public int Bestellnummer { get; set; }
        [BsonElement("bestelldatum")]
        public DateTime Bestelldatum { get; set; }

        [BsonElement("kundenId")]
        public ObjectId KundenId { get; set; }
        [BsonIgnore]
        public Kunde Kunde { get; set; }
        [BsonElement("bestellpositionen")]
        public List<Bestellposition> Bestellpositionen { get; set; }
        [BsonElement("total")]
        public double Total { get; set; }
        [BsonElement("lieferAdresse")]
        public Adresse LieferAdresse { get; set; }

        public void GenerateId()
        {
            _Id = ObjectId.GenerateNewId();
        }
    }
}
