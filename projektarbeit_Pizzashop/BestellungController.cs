using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace projektarbeit_Pizzashop
{
    internal class BestellungController : Controller
    {
        public void AddBestellung(Bestellung bestellung)
        {
            List<Bestellung> bestellungen = modell.Bestellungen;
            bestellungen.Add(bestellung);
            modell.Bestellungen = bestellungen;

            bsonDocument = bestellung.ToBsonDocument();
            modell.BestellungCollection.InsertOne(bsonDocument);
        }
        public void UpdateBestellung(Bestellung bestellung)
        {
            List<Bestellung> bestellungen = modell.Bestellungen;
            int index = bestellungen.FindIndex(p => p._Id == bestellung._Id);
            if (index != -1)
            {
                bestellungen[index] = bestellung;
            }
            modell.Bestellungen = bestellungen;

            bsonDocument = bestellung.ToBsonDocument();
            modell.BestellungCollection.ReplaceOne(doc => doc["_id"] == bestellung._Id, bsonDocument);
        }
        public List<Bestellung> ReadBestellung()
        {
            modell.Bestellungen = modell.Bestellungen.OrderBy(o => o.Bestellnummer).ToList();
            return modell.Bestellungen;
        }
        public void DeleteBestellung(ObjectId id)
        {
            List<Bestellung> bestellungen = modell.Bestellungen;
            int index = bestellungen.FindIndex(b => b._Id == id);
            bestellungen.RemoveAt(index);
            modell.Bestellungen = bestellungen;

            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            modell.BestellungCollection.DeleteOne(filter);
        }
    }
}
