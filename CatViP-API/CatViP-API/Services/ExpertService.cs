using AutoMapper;
using CatViP_API.DTOs.ExpertDTOs;
using CatViP_API.Models;
using CatViP_API.Repositories;
using CatViP_API.Repositories.Interfaces;
using CatViP_API.Services.Interfaces;
using Microsoft.VisualBasic;

namespace CatViP_API.Services
{
    public class ExpertService : IExpertService
    {
        private readonly IConfiguration _configuration;
        private readonly IExpertRepository _expertRepository;
        private readonly IMapper _mapper;

        public ExpertService(IConfiguration configuration, IExpertRepository expertRepository, IMapper mapper)
        {
            this._configuration = configuration;
            this._expertRepository = expertRepository;
            _mapper = mapper;
        }

        public async Task<ResponseResult> ApplyAsExpert(long userId, ExpertApplicationRequestDTO expertApplicationRequestDTO)
        {
            var res = new ResponseResult();

            if (_expertRepository.HasPendingApplication(userId))
            {
                res.IsSuccessful = false;
                res.ErrorMessage = "the previous application is still pending";
                return res;
            }

            res.IsSuccessful = await _expertRepository.StoreApplication(userId, expertApplicationRequestDTO);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "fail to store the application";
            }

            return res;
        }

        public ExpertApplicationDTO? GetLastestApplication(long userId)
        {
            var lastestApplication = _expertRepository.GetExpertLastestApplication(userId);

            if (lastestApplication == null)
            {
                return null;
            }

            return _mapper.Map<ExpertApplicationDTO>(lastestApplication);
        }

        public ICollection<ExpertApplicationDTO> GetPendingApplications()
        {
            return _mapper.Map<ICollection<ExpertApplicationDTO>>(_expertRepository.GetPendingApplications());
        }

        public async Task<ResponseResult> UpdateApplicationStatus(ExpertApplicationActionRequestDTO expertApplicationActionRequestDTO)
        {
            var res = new ResponseResult();

            if (expertApplicationActionRequestDTO.StatusId != 1 && expertApplicationActionRequestDTO.StatusId != 3)
            {
                res.IsSuccessful = false;
                res.ErrorMessage = "Invalid Status Id.";
                return res;
            }

            res.IsSuccessful = await _expertRepository.UpdateApplicationStatus(expertApplicationActionRequestDTO);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "fail to udpate expert application status.";
            }

            return res;
        }

        public async Task<ResponseResult> RevokeApplication(long Id)
        {
            var res = new ResponseResult();

            res.IsSuccessful = await _expertRepository.RevokeApplicaton(Id);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "fail to revoke expert application status.";
            }

            return res;
        }

        public ResponseResult CheckIfPendingApplicationExist(long userId, long applicationId)
        {
            var res = new ResponseResult();

            res.IsSuccessful = _expertRepository.CheckIfPendingApplicationExist(userId, applicationId);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "application is not exist or may not belong to this user.";
            }

            return res;
        }

        public ResponseResult CheckIfPendingApplicationExist(long applicationId)
        {
            var res = new ResponseResult();

            res.IsSuccessful = _expertRepository.CheckIfPendingApplicationExist(applicationId);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "application is not exist.";
            }

            return res;
        }

        public ICollection<ExpertApplicationDTO> GetApplications()
        {
            return _mapper.Map<ICollection<ExpertApplicationDTO>>(_expertRepository.GetApplications());
        }

        public ExpertApplicationDTO GetApplicationById(long id)
        {
            return _mapper.Map<ExpertApplicationDTO>(_expertRepository.GetApplicationById(id));
        }
    }
}
