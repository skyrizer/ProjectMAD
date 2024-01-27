using AutoMapper;
using CatViP_API.DTOs.ProductDTOs;
using CatViP_API.Models;
using CatViP_API.Repositories;
using CatViP_API.Repositories.Interfaces;
using CatViP_API.Services.Interfaces;
using Microsoft.CodeAnalysis;

namespace CatViP_API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public ResponseResult CheckProductExist(long authId, long productId)
        {
            var res = new ResponseResult();

            res.IsSuccessful = _productRepository.CheckProductExist(authId, productId);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "product is not exist.";
            }

            return res;
        }

        public async Task<ResponseResult> DeleteProduct(long id)
        {
            var res = new ResponseResult();

            res.IsSuccessful = await _productRepository.RemoveProduct(id);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "fail to delete product";
            }

            return res;
        }

        public async Task<ResponseResult> EditProduct(long id, ProductEditRequestDTO productRequestDTO)
        {
            var res = new ResponseResult();

            res.IsSuccessful = await _productRepository.UpdateProduct(id, productRequestDTO);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "fail to store the product.";
            }

            return res;
        }

        public ProductDTO GetProductById(long id)
        {
            return _mapper.Map<ProductDTO>(_productRepository.GetProduct(id));
        }

        public ICollection<ProductDTO> GetProducts(long authId)
        {
            return _mapper.Map<ICollection<ProductDTO>>(_productRepository.GetProducts(authId));
        }

        public ICollection<ProductTypeDTO> GetProductTypes()
        {
            return _mapper.Map<ICollection<ProductTypeDTO>>(_productRepository.GetProductTypes());
        }

        public async Task<ResponseResult> StoreProduct(long authId, ProductRequestDTO productRequestDTO)
        {
            var res = new ResponseResult();

            var product = new Product()
            {
                Name = productRequestDTO.Name,
                Description = productRequestDTO.Description,
                Price = productRequestDTO.Price,
                SellerId = authId,
                Status = true,
                URL = productRequestDTO.URL,
                ProductTypeId = productRequestDTO.ProductTypeId,
                Image = productRequestDTO.Image,
            };

            res.IsSuccessful = await _productRepository.StoreProduct(product);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "fail to store the product.";
            }

            return res;
        }
    }
}
