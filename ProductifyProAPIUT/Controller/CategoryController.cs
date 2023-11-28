using AutoMapper;
using LiteDB;
using ProductifyProAPI.DTO;
using ProductifyProAPI.Models;

public class CategoryController
{
    [Fact]
    public void GetAllCategories_Should_Return_All_Categories()
    {
        // Creazione di un database in memoria
        using (var db = new LiteDatabase(new MemoryStream()))
        {
            // Popolamento del database in memoria con dati di esempio
            var categories = db.GetCollection<Category>("categories");
            categories.InsertBulk(new[]
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" },
            });

            
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryDTO>();
            });
            IMapper mapper = mapperConfig.CreateMapper();

            
            var categoryDAL = new CategoryDAL(db, mapper);

            
            var result = categoryDAL.GetAllCategories();

            
            Assert.NotNull(result);
            Assert.Equal(2, result.Count()); 
        }
    }
    [Fact]
    public void GetCategoryById_Should_Return_Single_Category()
    {
        using (var db = new LiteDatabase(new MemoryStream()))
        {
            // Populating the in-memory database with sample data
            var categories = db.GetCollection<Category>("categories");
            categories.InsertBulk(new[]
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" },
            });

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryDTO>();
            });
            IMapper mapper = mapperConfig.CreateMapper();

            var categoryDAL = new CategoryDAL(db, mapper);

            // Calling the Get CategoryById method with an existing ID
            var result = categoryDAL.GetCategoryById(1);

            // Verify that the method returns a single category with matching ID
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Category 1", result.Name);

            // Calling the Get Category By Id method with a non-existent ID
            var nonExistentResult = categoryDAL.GetCategoryById(99);

            // Verify that the method returns null when the ID does not exist
            Assert.Null(nonExistentResult);
        }
    }

    [Fact]
    public void AddCategory_Should_Add_New_Category()
    {
        using (var db = new LiteDatabase(new MemoryStream()))
        {
            var categories = db.GetCollection<Category>("categories");
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryDTO>();
                cfg.CreateMap<CategoryDTO, Category>();
            });
            IMapper mapper = mapperConfig.CreateMapper();

            var categoryDAL = new CategoryDAL(db, mapper);

            // Calling the AddCategory method with a new category
            var newCategory = new CategoryDTO { Id = 1, Name = "New Category" };
            categoryDAL.AddCategory(newCategory);

            // Check that the category was added correctly
            var addedCategory = categories.FindById(1);
            Assert.NotNull(addedCategory);
            Assert.Equal("New Category", addedCategory.Name);

            //Verify that the method raises an exception if you try to add a category with an existing ID
            var existingCategory = new CategoryDTO { Id = 1, Name = "Existing Category" };
            Assert.Throws<ArgumentException>(() => categoryDAL.AddCategory(existingCategory));
        }
    }

    [Fact]
    public void UpdateCategory_Should_Update_Existing_Category()
    {
        using (var db = new LiteDatabase(new MemoryStream()))
        {
            var categories = db.GetCollection<Category>("categories");
            categories.Insert(new Category { Id = 1, Name = "Original Name" });

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryDTO>();
                cfg.CreateMap<CategoryDTO, Category>();
            });
            IMapper mapper = mapperConfig.CreateMapper();

            var categoryDAL = new CategoryDAL(db, mapper);

            // Calling the UpdateCategory method to update an existing category
            var updatedCategory = new CategoryDTO { Id = 1, Name = "Updated Name" };
            categoryDAL.UpdateCategory(updatedCategory);

            // Check that the category was updated correctly
            var category = categories.FindById(1);
            Assert.NotNull(category);
            Assert.Equal("Updated Name", category.Name);

            // Verify that the method raises an exception if you try to update a non-existing category
            var nonExistentCategory = new CategoryDTO { Id = 99, Name = "Non Existent" };
            Assert.Throws<ArgumentException>(() => categoryDAL.UpdateCategory(nonExistentCategory));
        }
    }

    [Fact]
    public void DeleteCategory_Should_Delete_Category()
    {
        using (var db = new LiteDatabase(new MemoryStream()))
        {
            var categories = db.GetCollection<Category>("categories");
            categories.Insert(new Category { Id = 1, Name = "Category to Delete" });

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryDTO>();
                cfg.CreateMap<CategoryDTO, Category>();
            });
            IMapper mapper = mapperConfig.CreateMapper();

            var categoryDAL = new CategoryDAL(db, mapper);

            // Calling the DeleteCategory method to delete an existing category
            categoryDAL.DeleteCategory(1);

            // Verify that the category was deleted successfully
            var deletedCategory = categories.FindById(1);
            Assert.Null(deletedCategory);

            // Verify that the method does not raise an exception if you try to delete a non-existing category
            try
            {
                categoryDAL.DeleteCategory(99);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected exception: {ex.Message}");
            }
        }
    }
}
