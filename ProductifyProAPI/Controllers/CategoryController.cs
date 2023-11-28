using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ProductifyProAPI.DTO;
using ProductifyProAPI.IDAL;

[EnableCors("_myAllowSpecificOrigins")]
[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryDAL  _category;

    public CategoryController(ICategoryDAL categoryService)
    {
        _category = categoryService;
    }

    #region Get

    [HttpGet]
    public IActionResult GetAllCategories()
    {
        var categories = _category.GetAllCategories();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public IActionResult GetCategoryById(int id)
    {
       var category = _category.GetCategoryById(id);
        if (category == null)
        {
            return UnprocessableEntity();
        }
        return Ok(category);
    }

    #endregion

    #region Post

    [HttpPost("Add")]
    public IActionResult AddCategory([FromBody] CategoryDTO categoryDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); 
        }

        _category.AddCategory(categoryDto);
        return CreatedAtAction(nameof(GetCategoryById), new { id = categoryDto.Id }, categoryDto);
    }

    #endregion

    #region Put

    [HttpPut("UpdateCategory/{id}")]
    public IActionResult UpdateCategory(int id, [FromBody] CategoryDTO categoryDto)
    {
        if (id != categoryDto.Id)
        {
            return UnprocessableEntity("Category ID mismatch");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); 
        }

        var existingCategory = _category.GetCategoryById(id);
        if (existingCategory == null)
        {
            return UnprocessableEntity(); 
        }

        _category.UpdateCategory(categoryDto);
        return Ok();
    }

    #endregion

    #region Delete
    [HttpDelete("DeleteCategory/{id}")]
    public IActionResult DeleteCategory(int id)
    {
        var existingCategory = _category.GetCategoryById(id);
        if (existingCategory == null)
        {
            return UnprocessableEntity();
        }

        _category.DeleteCategory(id);
        return Ok();
    }

    #endregion
}
