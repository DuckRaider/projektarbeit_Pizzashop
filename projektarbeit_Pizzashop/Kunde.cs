using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace projektarbeit_Pizzashop
{
    internal class Kunde
    {
        [BsonId]
        [BsonElement("_id")]
        public ObjectId _Id { get; set; }
        [BsonElement("nachname")]
        public string Nachname { get; set; }
        [BsonElement("vorname")]
        public string Vorname { get; set; }
        [BsonElement("adressen")]
        public List<Adresse> Adressen { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("geburtsdatum")]
        public DateTime Geburtsdatum { get; set; }
        [BsonElement("telefon")]
        public string Telefon { get; set; }

        public void GenerateId()
        {
            _Id = ObjectId.GenerateNewId();
        }
    }
}
