using CatViP_API.DTOs.PostDTOs;
using CatViP_API.Models;

namespace CatViP_API.Helpers
{
    public class MapProductToPostAdsHelper
    {
        public static PostDTO MapProductToPostAds(Product product)
        {
            var post = new PostDTO()
            {
                Id = -1,
                FullName = product.Seller.FullName,
                Description = $"{product.Name} - {(product.Description.Length > 80 ? product.Description.Substring(0, 77) + "..." : product.Description)}",
                IsAds = true,
                PostImages = new List<PostImageDTO>() { new PostImageDTO() { Image = product.Image, IsBloodyContent = false } },
                Username = product.Seller.Username,
                ProfileImage = product.Seller.ProfileImage,
                UserId = product.SellerId,
                Price = product.Price,
                AdsUrl = product.URL
            };

            return post;
        }
    }
}
