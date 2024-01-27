using CatViP_API.DTOs.ProductDTOs;
using CatViP_API.Models;

namespace CatViP_API.Repositories.Interfaces
{
    public interface IProductRepository
    {
        bool CheckProductExist(long authId, long productId);
        Product GetProduct(long id);
        ICollection<Product> GetProducts(long authId);
        ICollection<ProductType> GetProductTypes();
        Task<bool> RemoveProduct(long id);
        Task<bool> StoreProduct(Product product);
        Task<bool> UpdateProduct(long id, ProductEditRequestDTO productRequestDTO);
    }
}
