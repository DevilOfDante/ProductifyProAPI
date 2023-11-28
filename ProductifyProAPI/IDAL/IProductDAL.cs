using ProductifyProAPI.DTO;
using ProductifyProAPI.Models;

namespace ProductifyProAPI.IDAL
{
    public interface IProductDAL
    {
        IEnumerable<ProductDTO> GetAllProducts();
        ProductDTO GetProductById(int id);
        void AddProduct(ProductDTO product);
        void UpdateProduct(ProductDTO product);
        void DeleteProduct(int id);
    }
}
