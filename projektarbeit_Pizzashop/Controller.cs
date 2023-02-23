using MongoDB.Bson;

namespace projektarbeit_Pizzashop
{
    internal abstract class Controller
    {
        protected PizzaShopModell modell = new PizzaShopModell();
        protected BsonDocument bsonDocument;
    }
}
