using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace projektarbeit_Pizzashop
{
    internal class PizzaShopModell
    {
        public List<Pizza> Pizzas { get; set; }
        public List<Kunde> Kunden { get; set; }
        public List<Bestellung> Bestellungen { get; set; }
        public Config Config { get; set; }
        public IMongoDatabase Db { get; set; }
        public IMongoCollection<BsonDocument> PizzasCollection { get; set; }
        public IMongoCollection<BsonDocument> KundenCollection { get; set; }
        public IMongoCollection<BsonDocument> BestellungCollection { get; set; }
        private List<BsonDocument> Documents { get; set; }

        public PizzaShopModell()
        {
            Config = new Config();
            Pizzas = new List<Pizza>();
            Kunden = new List<Kunde>();
            Bestellungen = new List<Bestellung>();

            Db = Config.getDb();
            PizzasCollection = Db.GetCollection<BsonDocument>("pizzas");
            KundenCollection = Db.GetCollection<BsonDocument>("kunden");
            BestellungCollection = Db.GetCollection<BsonDocument>("bestellungen");

            //Die Abfragen sollten lieber in den einzelnen Controllern gemacht werden. Längerer Code, dafür effizienter!
            Documents = PizzasCollection.Find(new BsonDocument()).ToList();
            Pizzas = Documents.Select(v => BsonSerializer.Deserialize<Pizza>(v)).ToList();

            Documents = KundenCollection.Find(new BsonDocument()).ToList();
            Kunden = Documents.Select(v => BsonSerializer.Deserialize<Kunde>(v)).ToList();

            Documents = BestellungCollection.Find(new BsonDocument()).ToList();
            Bestellungen = Documents.Select(v => BsonSerializer.Deserialize<Bestellung>(v)).ToList();
            foreach (Bestellung bestellung in Bestellungen)
            {
                bestellung.Kunde = BsonSerializer.Deserialize<Kunde>(KundenCollection.Find(x => x["_id"] == bestellung.KundenId).FirstOrDefault());

                foreach (Bestellposition bestellposition in bestellung.Bestellpositionen)
                {
                    bestellposition.Pizza = BsonSerializer.Deserialize<Pizza>(PizzasCollection.Find(x => x["_id"] == bestellposition.PizzaId).FirstOrDefault());
                }
            }
        }
    }
}
