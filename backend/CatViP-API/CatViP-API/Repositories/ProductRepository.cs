using CatViP_API.Data;
using CatViP_API.DTOs.ProductDTOs;
using CatViP_API.Models;
using CatViP_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatViP_API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatViPContext _context;

        public ProductRepository(CatViPContext context)
        {
            this._context = context;
        }

        public bool CheckProductExist(long authId, long productId)
        {
            return _context.Products.Any(x => x.SellerId == authId && x.Id == productId && x.Status);
        }

        public Product GetProduct(long id)
        {
            return _context.Products.Include(x => x.ProductType).FirstOrDefault(x => x.Id == id && x.Status)!;
        }

        public ICollection<Product> GetProducts(long authId)
        {
            return _context.Products.Where(x => x.SellerId == authId && x.Status).Include(x => x.ProductType).ToList();
        }

        public ICollection<ProductType> GetProductTypes()
        {
            return _context.ProductTypes.OrderBy(x => x.Name).ToList();
        }

        public async Task<bool> RemoveProduct(long id)
        {
            try
            {
                var prod = _context.Products.FirstOrDefault(x => x.Id == id)!;
                prod.Status = false;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> StoreProduct(Product product)
        {
            try
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateProduct(long id, ProductEditRequestDTO productRequestDTO)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(x => x.Id == id);
                product!.Description = productRequestDTO.Description;
                product!.Name = productRequestDTO.Name;
                product!.Price = productRequestDTO.Price;
                product!.ProductTypeId = productRequestDTO.ProductTypeId;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
