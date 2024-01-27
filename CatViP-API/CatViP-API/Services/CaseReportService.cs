using AutoMapper;
using CatViP_API.DTOs.CaseReportDTOs;
using CatViP_API.Helpers;
using CatViP_API.Models;
using CatViP_API.Repositories;
using CatViP_API.Repositories.Interfaces;
using CatViP_API.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;

namespace CatViP_API.Services
{
    public class CaseReportService : ICaseReportService
    {
        private readonly ICaseReportRepository _caseReportRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CaseReportService(ICaseReportRepository caseReportRepository, IUserRepository userRepository, IMapper mapper)
        {
            _caseReportRepository = caseReportRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public ResponseResult CheckIsReportExist(long authId, long id)
        {
            var res = new ResponseResult();

            res.IsSuccessful = _caseReportRepository.CheckIsReportExist(authId, id);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "Report is not exist";
            }

            return res;
        }

        public async Task<ResponseResult> CreateCaseReport(long authId, CaseReportRequestDTO caseReportRequestDTO)
        {
            var storeResult = new ResponseResult();

            if (caseReportRequestDTO.CaseReportImages.IsNullOrEmpty())
            {
                storeResult.IsSuccessful = false;
                storeResult.ErrorMessage = "at least one image is required.";
                return storeResult;
            }

            if (caseReportRequestDTO.CaseReportImages.Count > 5)
            {
                storeResult.IsSuccessful = false;
                storeResult.ErrorMessage = "the maximum images count is 5.";
                return storeResult;
            }

            var catCaseReport = new CatCaseReport()
            {
                Address = caseReportRequestDTO.Address,
                CatId = caseReportRequestDTO.CatId,
                CatCaseReportStatusId = 1,
                Description = caseReportRequestDTO.Description,
                Latitude = caseReportRequestDTO.Latitude,
                Longitude = caseReportRequestDTO.Longitude,
                UserId = authId,
                DateTime = DateTime.Now,

                CatCaseReportImages = caseReportRequestDTO.CaseReportImages.Select(pi => new CatCaseReportImage
                {
                    Image = pi.Image,
                    IsBloodyContent = pi.IsBloodyContent
                }).ToList(),
            };

            storeResult.IsSuccessful = await _caseReportRepository.StoreCaseReport(catCaseReport);

            if (!storeResult.IsSuccessful)
            {
                storeResult.ErrorMessage = "fail to store";
                return storeResult;
            }

            // push notification
            var users = _userRepository.GetOtherActiveCatOwnerAndExpert(authId);

            var usernames = new List<string>();

            foreach (var user in users)
            {
                if (CalculateDistanceHelper.CalculateDistance((double)user.Latitude!, (double)user.Longitude!, (double)catCaseReport.Latitude, (double)catCaseReport.Longitude) <= 10)
                {
                    usernames.Add(user.Username!);
                }
            }

            await OneSignalSendNotiHelper.OneSignalSendCaseReportNoti(usernames, "there is a missing cat report nearby you.");

            return storeResult;
        }

        public ICollection<CatCaseReportCommentDTO> GetCaseReportComments(long authId, int caseReportId)
        {
            var comments = _mapper.Map<ICollection<CatCaseReportCommentDTO>>(_caseReportRepository.GetCaseReportComments(caseReportId));

            foreach (var comment in comments)
            {
                comment.IsCurrentLoginUser = _caseReportRepository.CheckCommentIsFromCurrentUser(authId, comment.Id);
            }

            return comments;
        }

        public async Task<ResponseResult> CommentCaseReport(User user, CatCaseReportCommentRequestDTO commentRequestDTO)
        {
            var res = new ResponseResult();

            res.IsSuccessful = await _caseReportRepository.CommentPost(user.Id, commentRequestDTO);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "Fail to store.";
            }

            return res;
        }

        public async Task<ResponseResult> DeleteComment(long id)
        {
            var res = new ResponseResult();

            res.IsSuccessful = await _caseReportRepository.DeleteComment(id);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "fail to delete the comment.";
            }

            return res;
        }

        public ResponseResult<NearByCaseReportDTO> GetNearByCaseReport(long Id, User user)
        {
            var res = new ResponseResult<NearByCaseReportDTO>();

            res.Result = _mapper.Map<NearByCaseReportDTO>(_caseReportRepository.GetNotAuthCaseReport(Id, user.Id));

            if (res.Result == null)
            {
                res.ErrorMessage = "the case report is not exist.";
                return res;
            }

            res.IsSuccessful = (CalculateDistanceHelper.CalculateDistance((double)user.Latitude!, (double)user.Longitude!, (double)res.Result!.Latitude, (double)res.Result!.Longitude) <= 10);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "the case report is not near by you.";
            }

            return res;
        }

        public ICollection<NearByCaseReportDTO> GetNearByCaseReports(User user)
        {
            var tempCases = _mapper.Map<ICollection<NearByCaseReportDTO>>(_caseReportRepository.GetNotAuthCaseReports(user.Id));

            var cases = new List<NearByCaseReportDTO>();

            foreach (var caseReport in tempCases)
            {
                if (CalculateDistanceHelper.CalculateDistance((double)user.Latitude!, (double)user.Longitude!, (double)caseReport.Latitude, (double)caseReport.Longitude) <= 3)
                {
                    cases.Add(caseReport);
                    caseReport.CaseReportImages = _mapper.Map<ICollection<CaseReportImageDTO>>(_caseReportRepository.GetCaseReportImages(caseReport.Id));
                }
            }

            return cases;
        }

        public ResponseResult<OwnCaseReportDTO> GetOwnCaseReport(long Id, User user)
        {
            var res = new ResponseResult<OwnCaseReportDTO>();

            res.Result = _mapper.Map<OwnCaseReportDTO>(_caseReportRepository.GetOwnCaseReport(Id, user.Id));

            if (res.Result == null)
            {
                res.ErrorMessage = "the case report is not exist.";
                return res;
            }

            res.IsSuccessful = (CalculateDistanceHelper.CalculateDistance((double)user.Latitude!, (double)user.Longitude!, (double)res.Result!.Latitude, (double)res.Result!.Longitude) <= 10);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "the case report is not near by you.";
            }

            return res;
        }

        public ICollection<OwnCaseReportDTO> GetOwnCaseReports(long autId)
        {
            var cases = _mapper.Map<ICollection<OwnCaseReportDTO>>(_caseReportRepository.GetOwnCaseReports(autId));

            foreach (var caseReport in cases)
            {
                caseReport.CaseReportImages = _mapper.Map<ICollection<CaseReportImageDTO>>(_caseReportRepository.GetCaseReportImages(caseReport.Id));
            }

            return cases;
        }

        public async Task<ResponseResult> RevokeCaseReport(long id)
        {
            var res = new ResponseResult();

            res.IsSuccessful = await _caseReportRepository.RevokeCaseReport(id);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "Report is not exist";
            }

            return res;
        }

        public async Task RevokeCaseReportsMoreThan7Days()
        {
            var caseReports = _caseReportRepository.GetCaseReportsMoreThan7Days();

            foreach (var caseReport in caseReports)
            {
                await _caseReportRepository.RevokeCaseReport(caseReport.Id);
            }
        }

        public async Task<ResponseResult> SettleCaseReport(long id)
        {
            var res = new ResponseResult();

            res.IsSuccessful = await _caseReportRepository.SettleCaseReport(id);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "Report is not exist";
            }

            return res;
        }

        public ResponseResult CheckIfCaseReportExist(long userId, long postId)
        {
            var res = new ResponseResult();

            res.IsSuccessful = _caseReportRepository.CheckIfCaseReportExist(userId, postId);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "Post is not exist.";
            }

            return res;
        }

        public ResponseResult CheckIfCommentExist(long authId, long commentId)
        {
            var res = new ResponseResult();

            res.IsSuccessful = _caseReportRepository.CheckCommentIsFromCurrentUser(authId, commentId);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "comment is not exist.";
            }

            return res;
        }

        public int GetNearByCaseReportsCount(User user)
        {
            var tempCases = _mapper.Map<ICollection<NearByCaseReportDTO>>(_caseReportRepository.GetNotAuthCaseReports(user.Id));

            int count = 0;

            foreach (var caseReport in tempCases)
            {
                if (CalculateDistanceHelper.CalculateDistance((double)user.Latitude!, (double)user.Longitude!, (double)caseReport.Latitude, (double)caseReport.Longitude) <= 3)
                    count++;
            }

            return count;
        }
    }
}
