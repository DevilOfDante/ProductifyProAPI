using ProductifyProAPI.DTO;
using ProductifyProAPI.Models;

namespace ProductifyProAPI.IDAL
{
    public interface ICategoryDAL
    {
        IEnumerable<CategoryDTO> GetAllCategories();
        CategoryDTO GetCategoryById(int id);
        void AddCategory(CategoryDTO category);
        void UpdateCategory(CategoryDTO category);
        void DeleteCategory(int id);
    }
}
