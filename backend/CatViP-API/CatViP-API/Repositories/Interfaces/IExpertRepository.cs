using CatViP_API.DTOs.ExpertDTOs;
using CatViP_API.Models;

namespace CatViP_API.Repositories.Interfaces
{
    public interface IExpertRepository
    {
        Task<bool> StoreApplication(long userId, ExpertApplicationRequestDTO requestDTO);
        bool HasPendingApplication(long userId);
        bool CheckIfPendingApplicationExist(long application);
        bool CheckIfPendingApplicationExist(long userId, long applicationId);
        ExpertApplication? GetExpertLastestApplication(long userId);
        ICollection<ExpertApplication> GetPendingApplications();
        Task<bool> UpdateApplicationStatus(ExpertApplicationActionRequestDTO expertApplicationActionRequestDTO);
        Task<bool> RevokeApplicaton(long Id);
        ICollection<ExpertApplication> GetApplications();
        ExpertApplication GetApplicationById(long Id);
    }
}
