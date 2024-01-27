using CatViP_API.DTOs.CatDTOs;
using CatViP_API.Models;

namespace CatViP_API.Repositories.Interfaces
{
    public interface ICatRepository
    {
        ICollection<Cat> GetCats(long userId);
        Cat GetCat(long catId);
        Task<bool> StoreCat(long userId, CatRequestDTO catRequestDTO);
        Task<bool> EditCat(long catId, CatRequestDTO catRequestDTO);
        Task<bool> DeleteCat(long catId);
        bool CheckIfCatExist(long userId, long catId);
    }
}

