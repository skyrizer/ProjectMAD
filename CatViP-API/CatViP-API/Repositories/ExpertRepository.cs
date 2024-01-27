using CatViP_API.Data;
using CatViP_API.DTOs.ExpertDTOs;
using CatViP_API.Models;
using CatViP_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatViP_API.Repositories
{
    public class ExpertRepository : IExpertRepository
    {
        private readonly CatViPContext _context;

        public ExpertRepository(CatViPContext catViPContext)
        {
            this._context = catViPContext;
        }

        public bool CheckIfPendingApplicationExist(long applicationId)
        {
            return _context.ExpertApplications.Any(x => x.Id == applicationId && x.StatusId == 2);
        }

        public bool CheckIfPendingApplicationExist(long userId, long applicationId)
        {
            return _context.ExpertApplications.Any(x => x.Id == applicationId && x.UserId == userId && x.StatusId == 2);
        }

        public ExpertApplication? GetExpertLastestApplication(long userId)
        {
            return _context.ExpertApplications.Include(x => x.Status).Where(x => x.UserId == userId).OrderBy(x => x.DateTime).LastOrDefault();
        }

        public ICollection<ExpertApplication> GetPendingApplications()
        {
            return _context.ExpertApplications.Include(x => x.Status).Where(x => x.StatusId == 2).OrderByDescending(x => x.DateTime).ToList();
        }

        public bool HasPendingApplication(long userId)
        {
            return _context.ExpertApplications.Any(x => x.UserId == userId && x.StatusId == 2);
        }

        public async Task<bool> StoreApplication(long userId, ExpertApplicationRequestDTO requestDTO)
        {
            try
            {
                var expertApplication = new ExpertApplication();
                expertApplication.StatusId = 2;
                expertApplication.Documentation = requestDTO.Documentation;
                expertApplication.Description = requestDTO.Description;
                expertApplication.DateTime = DateTime.Now;
                expertApplication.UserId = userId;

                _context.Add(expertApplication);
                await _context.SaveChangesAsync();
                
                return true;    
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateApplicationStatus(ExpertApplicationActionRequestDTO expertApplicationActionRequestDTO)
        {
            try
            {
                var application = _context.ExpertApplications.FirstOrDefault(x => x.Id == expertApplicationActionRequestDTO.ApplictionId);
                application!.StatusId = expertApplicationActionRequestDTO.StatusId;
                application.DateTimeUpdated = DateTime.Now;

                if (expertApplicationActionRequestDTO.StatusId == 3)
                {
                    application!.RejectedReason = expertApplicationActionRequestDTO.RejectedReason;
                }

                _context.Update(application);

                if (expertApplicationActionRequestDTO.StatusId == 1)
                {
                    var user = _context.Users.FirstOrDefault(x => x.Id == application.UserId);
                    user!.RoleId = 3;
                    _context.Update(user);
                }

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RevokeApplicaton(long id)
        {
            try
            {
                var application = _context.ExpertApplications.FirstOrDefault(x => x.Id == id);
                application!.StatusId = 4;
                application.DateTimeUpdated = DateTime.Now;

                _context.Update(application);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ICollection<ExpertApplication> GetApplications()
        {
            return _context.ExpertApplications.Where(x => x.StatusId == 1 || x.StatusId == 3).Include(x => x.Status).OrderByDescending(x => x.DateTime).ToList();
        }

        public ExpertApplication GetApplicationById(long Id)
        {
            return _context.ExpertApplications.Include(x => x.Status).FirstOrDefault(x => x.Id == Id)!;
        }
    }
}
