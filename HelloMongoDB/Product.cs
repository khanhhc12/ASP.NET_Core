using MongoDB.Bson;

namespace HelloMongoDB
{
    public class Product
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
    }
}