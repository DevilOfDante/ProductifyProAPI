using AutoMapper;
using LiteDB;
using ProductifyProAPI.DTO;
using ProductifyProAPI.Models;
using ProductifyProAPI.DAL;

public class ProductController
{
    [Fact]
    public void GetAllProducts_Should_Return_All_Products()
    {
        using (var db = new LiteDatabase(new MemoryStream()))
        {
            var products = db.GetCollection<Product>("products");
            products.InsertBulk(new[]
            {
                new Product { Id = 1, Name = "Product 1" },
                new Product { Id = 2, Name = "Product 2" },
            });

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>();
            });
            IMapper mapper = mapperConfig.CreateMapper();

            var productDAL = new ProductDAL(db, mapper);

            var result = productDAL.GetAllProducts();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }

    [Fact]
    public void GetProductById_Should_Return_Single_Product()
    {
        using (var db = new LiteDatabase(new MemoryStream()))
        {
            var products = db.GetCollection<Product>("products");
            products.InsertBulk(new[]
            {
                new Product { Id = 1, Name = "Product 1" },
                new Product { Id = 2, Name = "Product 2" },
            });

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>();
            });
            IMapper mapper = mapperConfig.CreateMapper();

            var productDAL = new ProductDAL(db, mapper);

            var result = productDAL.GetProductById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Product 1", result.Name);

            var nonExistentResult = productDAL.GetProductById(99);

            Assert.Null(nonExistentResult);
        }
    }

    [Fact]
    public void AddProduct_Should_Add_New_Product()
    {
        using (var db = new LiteDatabase(new MemoryStream()))
        {
            var products = db.GetCollection<Product>("products");

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>();
                cfg.CreateMap<ProductDTO, Product>();
            });
            IMapper mapper = mapperConfig.CreateMapper();

            var productDAL = new ProductDAL(db, mapper);

            var newProduct = new ProductDTO { Id = 1, Name = "New Product" };
            productDAL.AddProduct(newProduct);

            var addedProduct = products.FindById(1);
            Assert.NotNull(addedProduct);
            Assert.Equal("New Product", addedProduct.Name);

            var existingProduct = new ProductDTO { Id = 1, Name = "Existing Product" };
            Assert.Throws<ArgumentException>(() => productDAL.AddProduct(existingProduct));
        }
    }

    [Fact]
    public void UpdateProduct_Should_Update_Existing_Product()
    {
        using (var db = new LiteDatabase(new MemoryStream()))
        {
            var products = db.GetCollection<Product>("products");
            products.Insert(new Product { Id = 1, Name = "Original Name" });

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>();
                cfg.CreateMap<ProductDTO, Product>();
            });
            IMapper mapper = mapperConfig.CreateMapper();

            var productDAL = new ProductDAL(db, mapper);

            var updatedProduct = new ProductDTO { Id = 1, Name = "Updated Name" };
            productDAL.UpdateProduct(updatedProduct);

            var product = products.FindById(1);
            Assert.NotNull(product);
            Assert.Equal("Updated Name", product.Name);

            var nonExistentProduct = new ProductDTO { Id = 99, Name = "Non Existent" };
            Assert.Throws<ArgumentException>(() => productDAL.UpdateProduct(nonExistentProduct));
        }
    }

    [Fact]
    public void DeleteProduct_Should_Delete_Product()
    {
        using (var db = new LiteDatabase(new MemoryStream()))
        {
            var products = db.GetCollection<Product>("products");
            products.Insert(new Product { Id = 1, Name = "Product to Delete" });

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>();
                cfg.CreateMap<ProductDTO, Product>();
            });
            IMapper mapper = mapperConfig.CreateMapper();

            var productDAL = new ProductDAL(db, mapper);

            productDAL.DeleteProduct(1);

            var deletedProduct = products.FindById(1);
            Assert.Null(deletedProduct);

            try
            {
                productDAL.DeleteProduct(99);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected exception: {ex.Message}");
            }
        }
    }
}
