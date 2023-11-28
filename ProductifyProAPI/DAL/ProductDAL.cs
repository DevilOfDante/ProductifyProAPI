using AutoMapper;
using LiteDB;
using Microsoft.EntityFrameworkCore;
using ProductifyProAPI.Context;
using ProductifyProAPI.DTO;
using ProductifyProAPI.IDAL;
using ProductifyProAPI.Models;
using System.Reflection.Emit;

namespace ProductifyProAPI.DAL
{
    public class ProductDAL : IProductDAL
    {
        private readonly LiteDatabase _database;
        private readonly IMapper _mapper;

        public ProductDAL(LiteDatabase database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public IEnumerable<ProductDTO> GetAllProducts()
        {
            var products = _database.GetCollection<Product>("products").FindAll();
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public ProductDTO GetProductById(int id)
        {
            var product = _database.GetCollection<Product>("products").FindById(id);
            return _mapper.Map<ProductDTO>(product);
        }

        public void AddProduct(ProductDTO productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            if (GetProductById(productDto.Id) != null)
            {
                throw new ArgumentException("Product alrealdy exist");
            }
            _database.GetCollection<Product>("products").Insert(product);
        }

        public void UpdateProduct(ProductDTO productDto)
        {
            var existingProduct = _database.GetCollection<Product>("products").FindById(productDto.Id);
            if (existingProduct != null)
            {
                var updatedProduct = _mapper.Map(productDto, existingProduct);
                _database.GetCollection<Product>("products").Update(updatedProduct);
            }
            else
            {
                throw new ArgumentException("Product not found");
            }
        }

        public void DeleteProduct(int id)
        {
            _database.GetCollection<Product>("products").Delete(id);
        }

    }
}
