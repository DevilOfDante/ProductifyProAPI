using AutoMapper;
using ProductifyProAPI.DTO;
using ProductifyProAPI.Models;

namespace ProductifyProAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        #region constructor
        public AutoMapperProfiles()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
        #endregion
    }
}
