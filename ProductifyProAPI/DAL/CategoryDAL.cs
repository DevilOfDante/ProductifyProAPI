using AutoMapper;
using LiteDB;
using ProductifyProAPI.DTO;
using ProductifyProAPI.IDAL;
using ProductifyProAPI.Models;

public class CategoryDAL : ICategoryDAL
{
    private readonly LiteDatabase _database;
    private readonly IMapper _mapper;

    public CategoryDAL(LiteDatabase database, IMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }

    public IEnumerable<CategoryDTO> GetAllCategories()
    {
        var categories = _database.GetCollection<Category>("categories").FindAll();
        return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
    }

    public CategoryDTO GetCategoryById(int id)
    {
        var category = _database.GetCollection<Category>("categories").FindById(id);
        return _mapper.Map<CategoryDTO>(category);
    }

    public void AddCategory(CategoryDTO categoryDto)
    {
        var category = _mapper.Map<Category>(categoryDto);
        if(GetCategoryById(categoryDto.Id) != null)
        {
            throw new ArgumentException("Category alrealdy exist");
        }

        _database.GetCollection<Category>("categories").Insert(category);
    }

    public void UpdateCategory(CategoryDTO categoryDto)
    {
        var existingCategory = _database.GetCollection<Category>("categories").FindById(categoryDto.Id);
        if (existingCategory != null)
        {
            var updatedCategory = _mapper.Map(categoryDto, existingCategory);
            _database.GetCollection<Category>("categories").Update(updatedCategory);
        }
        else
        {
            throw new ArgumentException("Category not found");
        }
    }

    public void DeleteCategory(int id)
    {
        _database.GetCollection<Category>("categories").Delete(id);
    }
}
