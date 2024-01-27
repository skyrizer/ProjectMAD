using CatViP_API.DTOs.CaseReportDTOs;
using CatViP_API.Models;
using System.Threading.Tasks;

namespace CatViP_API.Repositories.Interfaces
{
    public interface ICaseReportRepository
    {
        bool CheckIsReportExist(long authId, long id);
        ICollection<CatCaseReportImage> GetCaseReportImages(long id);
        ICollection<CatCaseReport> GetCaseReportsMoreThan7Days();
        ICollection<CatCaseReport> GetOwnCaseReports(long autId);
        CatCaseReport? GetOwnCaseReport(long Id, long userId);
        ICollection<CatCaseReport> GetNotAuthCaseReports(long authId);
        CatCaseReport? GetNotAuthCaseReport(long Id, long userId);
        Task<bool> RevokeCaseReport(long id);
        Task<bool> SettleCaseReport(long id);
        Task<bool> StoreCaseReport(CatCaseReport catCaseReport);
        object GetCaseReportComments(int caseReportId);
        bool CheckCommentIsFromCurrentUser(long authId, long id);
        Task<bool> CommentPost(long userId, CatCaseReportCommentRequestDTO commentRequestDTO);
        Task<bool> DeleteComment(long id);
        bool CheckIfCaseReportExist(long userId, long id);
    }
}
