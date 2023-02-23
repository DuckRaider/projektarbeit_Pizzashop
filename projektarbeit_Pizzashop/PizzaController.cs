using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace projektarbeit_Pizzashop
{
    internal class PizzaController : Controller
    {
        public void AddPizza(Pizza pizza)
        {
            List<Pizza> pizzas = modell.Pizzas;
            pizzas.Add(pizza);
            modell.Pizzas = pizzas;

            bsonDocument = pizza.ToBsonDocument();
            modell.PizzasCollection.InsertOne(bsonDocument);
        }
        public void UpdatePizza(Pizza pizza)
        {
            List<Pizza> pizzas = modell.Pizzas;
            int index = pizzas.FindIndex(p => p._Id == pizza._Id);
            if (index != -1)
            {
                pizzas[index] = pizza;
            }
            modell.Pizzas = pizzas;

            bsonDocument = pizza.ToBsonDocument();
            modell.PizzasCollection.ReplaceOne(doc => doc["_id"] == pizza._Id, bsonDocument);
        }
        public List<Pizza> ReadPizza()
        {
            modell.Pizzas = modell.Pizzas.OrderBy(o => o.Name).ToList();
            return modell.Pizzas;
        }
        public void DeletePizza(ObjectId id)
        {
            bool exists = false;

            List<Pizza> pizzas = modell.Pizzas;
            List<Bestellung> bestellungen = modell.Bestellungen;
            foreach (Bestellung bestellung in bestellungen)
            {
                foreach (Bestellposition bestellposition in bestellung.Bestellpositionen)
                {
                    if (bestellposition.PizzaId == id)
                    {
                        exists = true;
                    }
                }
            }
            if (!exists)
            {
                int index = pizzas.FindIndex(p => p._Id == id);
                pizzas.RemoveAt(index);
                modell.Pizzas = pizzas;

                var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
                modell.PizzasCollection.DeleteOne(filter);
            }
            else
            {
                MessageBox.Show("Pizza wird in einer Bestellung verwendet!");
            }
        }
    }
}
