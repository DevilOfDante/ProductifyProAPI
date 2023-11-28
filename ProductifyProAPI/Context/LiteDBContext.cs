using LiteDB;
using ProductifyProAPI.Models;

namespace ProductifyProAPI.Context
{
    public class LiteDbContext
    {
        private readonly LiteDatabase _db;

        public LiteDbContext(string dbPath)
        {
            _db = new LiteDatabase(dbPath);
        }

        public LiteCollection<Product> ProductsCollection { get; set; }
        public LiteCollection<Category> CategoriesCollection { get; set; }

    }
}
