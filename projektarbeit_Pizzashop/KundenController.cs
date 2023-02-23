using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace projektarbeit_Pizzashop
{
    internal class KundenController : Controller
    {
        public void AddKunde(Kunde kunde)
        {
            List<Kunde> kunden = modell.Kunden;
            kunden.Add(kunde);
            modell.Kunden = kunden;

            bsonDocument = kunde.ToBsonDocument();
            modell.KundenCollection.InsertOne(bsonDocument);
        }
        public void UpdateKunde(Kunde kunde)
        {
            List<Kunde> kunden = modell.Kunden;
            int index = kunden.FindIndex(p => p._Id == kunde._Id);
            if (index != -1)
            {
                kunden[index] = kunde;
            }
            modell.Kunden = kunden;

            bsonDocument = kunde.ToBsonDocument();
            modell.KundenCollection.ReplaceOne(doc => doc["_id"] == kunde._Id, bsonDocument);
        }
        public List<Kunde> ReadKunde()
        {
            modell.Kunden = modell.Kunden.OrderBy(o => o.Nachname).ToList();
            return modell.Kunden;
        }
        public void DeleteKunde(ObjectId id)
        {
            bool exists = false;

            List<Kunde> kunden = modell.Kunden;
            List<Bestellung> bestellungen = modell.Bestellungen;
            foreach (Bestellung bestellung in bestellungen)
            {
                if (bestellung.KundenId == id)
                {
                    exists = true;
                }
            }
            if (!exists)
            {
                int index = kunden.FindIndex(k => k._Id == id);
                kunden.RemoveAt(index);
                modell.Kunden = kunden;

                var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
                modell.KundenCollection.DeleteOne(filter);
            }
            else
            {
                MessageBox.Show("Kunde wird in einer Bestellung verwendet!");
            }
        }
    }
}
