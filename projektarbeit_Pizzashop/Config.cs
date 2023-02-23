using MongoDB.Driver;

namespace projektarbeit_Pizzashop
{
    internal class Config
    {
        private MongoClient dbClient;
        private IMongoDatabase db;

        public Config()
        {
            dbClient = new MongoClient("mongodb://localhost:27017");
            db = dbClient.GetDatabase("ShopDb");
        }

        public IMongoDatabase getDb()
        {
            return db;
        }
    }
}
