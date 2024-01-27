using CatViP_API.DTOs.ProductDTOs;

namespace CatViP_API.Services.Interfaces
{
    public interface IProductService
    {
        ResponseResult CheckProductExist(long authId, long productId);
        Task<ResponseResult> DeleteProduct(long id);
        Task<ResponseResult> EditProduct(long id, ProductEditRequestDTO productRequestDTO);
        ProductDTO GetProductById(long id);
        ICollection<ProductDTO> GetProducts(long authId);
        ICollection<ProductTypeDTO> GetProductTypes();
        Task<ResponseResult> StoreProduct(long authId, ProductRequestDTO productRequestDTO);
    }
}
