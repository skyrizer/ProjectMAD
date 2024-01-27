using CatViP_API.DTOs.CatDTOs;
using CatViP_API.Models;

namespace CatViP_API.Services.Interfaces
{
    public interface ICatService
    {
        ICollection<CatDTO> GetCats(long userId);
        CatDTO GetCat(long catId);
        Task<ResponseResult> StoreCat(long userId, CatRequestDTO createCatRequestDTO);
        Task<ResponseResult> EditCat(long catId, CatRequestDTO editCatRequestDTO);
        Task<ResponseResult> DeleteCat(long catId);
        ResponseResult CheckIfCatExist(long userId, long catId);
    }
}
