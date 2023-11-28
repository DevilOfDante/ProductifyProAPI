using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ProductifyProAPI.DTO;
using ProductifyProAPI.IDAL;

[EnableCors("_myAllowSpecificOrigins")]
[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductDAL _productService;

    public ProductsController(IProductDAL productService)
    {
        _productService = productService;
    }


    #region Get

    [HttpGet]
    public IActionResult GetAllProducts()
    {
        var products = _productService.GetAllProducts();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
        var product = _productService.GetProductById(id);
        if (product == null)
        {
            return UnprocessableEntity(); 
        }
        return Ok(product);
    }

    #endregion

    #region Post

    [HttpPost("Add")]
    public IActionResult AddProduct([FromBody] ProductDTO productDto)
    {
        if (!ModelState.IsValid)
        {
            // Return bad request if model validation fails
            return BadRequest(ModelState); 
        }

        _productService.AddProduct(productDto);
        return CreatedAtAction(nameof(GetProductById), new { id = productDto.Id }, productDto);
    }

    #endregion

    #region Put

    [HttpPut("UpdateProduct/{id}")]
    public IActionResult UpdateProduct(int id, [FromBody] ProductDTO productDto)
    {
        if (id != productDto.Id)
        {
            return UnprocessableEntity("Product ID mismatch"); // Return bad request if ID in URL doesn't match the one in the body
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Return bad request if model validation fails
        }

        var existingProduct = _productService.GetProductById(id);
        if (existingProduct == null)
        {
            return UnprocessableEntity(); // Return 404 if product is not found
        }

        _productService.UpdateProduct(productDto);
        return NoContent();
    }

    #endregion

    #region Delete

    [HttpDelete("DeleteProduct/{id}")]
    public IActionResult DeleteProduct(int id)
    {
        var existingProduct = _productService.GetProductById(id);
        if (existingProduct == null)
        {
            return UnprocessableEntity();
        }

        _productService.DeleteProduct(id);
        return NoContent();
    }
    #endregion
}