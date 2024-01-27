using CatViP_API.DTOs.ExpertDTOs;

namespace CatViP_API.Services.Interfaces
{
    public interface IExpertService
    {
        Task<ResponseResult> ApplyAsExpert(long userId, ExpertApplicationRequestDTO expertApplicationRequestDTO);
        ExpertApplicationDTO? GetLastestApplication(long userId);
        ICollection<ExpertApplicationDTO> GetPendingApplications();
        Task<ResponseResult> UpdateApplicationStatus(ExpertApplicationActionRequestDTO expertApplicationActionRequestDTO);
        ResponseResult CheckIfPendingApplicationExist(long applicationId);
        ResponseResult CheckIfPendingApplicationExist(long userId, long applicationId);
        Task<ResponseResult> RevokeApplication(long Id);
        ICollection<ExpertApplicationDTO> GetApplications();
        ExpertApplicationDTO GetApplicationById(long id);
    }
}
