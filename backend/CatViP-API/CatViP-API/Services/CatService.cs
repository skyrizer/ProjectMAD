using AutoMapper;
using CatViP_API.DTOs.CatDTOs;
using CatViP_API.Helpers;
using CatViP_API.Models;
using CatViP_API.Repositories.Interfaces;
using CatViP_API.Services.Interfaces;

namespace CatViP_API.Services
{
    public class CatService : ICatService
    {
        private readonly ICatRepository _catRepository;
        private readonly IMapper _mapper;

        public CatService(ICatRepository catRepository, IMapper mapper)
        {
            _catRepository = catRepository;
            _mapper = mapper;
        }

        public CatDTO GetCat(long catId)
        {
            return _mapper.Map<CatDTO>(_catRepository.GetCat(catId));
        }

        public ICollection<CatDTO> GetCats(long userId)
        {
            return _mapper.Map<ICollection<CatDTO>>(_catRepository.GetCats(userId));
        }

        public async Task<ResponseResult> StoreCat(long userId, CatRequestDTO createCatRequestDTO)
        {
            var res = new ResponseResult();

            res.IsSuccessful = await CatDetectionHelper.CheckIfPhotoContainCat(createCatRequestDTO.ProfileImage!);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "image may not contain cat.";
                return res;
            }

            res.IsSuccessful = await _catRepository.StoreCat(userId, createCatRequestDTO);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "Fail to store the cat.";
            }

            return res;
        }

        public async Task<ResponseResult> EditCat(long catId, CatRequestDTO editCatRequestDTO)
        {
            var res = new ResponseResult();

            res.IsSuccessful = await CatDetectionHelper.CheckIfPhotoContainCat(editCatRequestDTO.ProfileImage!);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "image may not contain cat.";
                return res;
            }

            res.IsSuccessful = await _catRepository.EditCat(catId, editCatRequestDTO);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "Fail to edit the cat.";
            }

            return res;
        }

        public async Task<ResponseResult> DeleteCat(long catId)
        {
            var res = new ResponseResult();

            res.IsSuccessful = await _catRepository.DeleteCat(catId);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "Fail to delete the cat.";
            }

            return res;
        }

        public ResponseResult CheckIfCatExist(long userId, long catId)
        {
            var res = new ResponseResult();

            res.IsSuccessful = _catRepository.CheckIfCatExist(userId, catId);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "Cat is not exits.";
            }

            return res;
        }
    }
}
