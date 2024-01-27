using CatViP_API.DTOs.CaseReportDTOs;
using CatViP_API.Models;

namespace CatViP_API.Services.Interfaces
{
    public interface ICaseReportService
    {
        ResponseResult CheckIsReportExist(long authId, long id);
        Task<ResponseResult> CreateCaseReport(long id, CaseReportRequestDTO caseReportRequestDTO);
        ICollection<CatCaseReportCommentDTO> GetCaseReportComments(long authId, int caseReportId);
        ResponseResult<NearByCaseReportDTO> GetNearByCaseReport(long Id, User user);
        ICollection<NearByCaseReportDTO> GetNearByCaseReports(User user);
        ResponseResult<OwnCaseReportDTO> GetOwnCaseReport(long Id, User user);
        ICollection<OwnCaseReportDTO> GetOwnCaseReports(long id);
        Task<ResponseResult> RevokeCaseReport(long id);
        Task RevokeCaseReportsMoreThan7Days();
        Task<ResponseResult> SettleCaseReport(long id);
        Task<ResponseResult> CommentCaseReport(User user, CatCaseReportCommentRequestDTO commentRequestDTO);
        Task<ResponseResult> DeleteComment(long id);
        ResponseResult CheckIfCaseReportExist(long userId, long postId);
        int GetNearByCaseReportsCount(User user);
    }
}
