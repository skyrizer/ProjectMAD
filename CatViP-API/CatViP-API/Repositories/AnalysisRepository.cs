using CatViP_API.Data;
using CatViP_API.Repositories.Interfaces;

namespace CatViP_API.Repositories
{
    public class AnalysisRepository : IAnalysisRepository
    {
        private readonly CatViPContext _context;

        public AnalysisRepository(CatViPContext context)
        {
            this._context = context;
        }

        public int GetTodayTipsCount()
        {
            return _context.Posts.Where(x => x.PostTypeId == 2 && x.DateTime.Date >= DateTime.Today.Date && x.Status).Count();
        }

        public int GetOneWeekTipsCount()
        {
            return _context.Posts.Where(x => x.PostTypeId == 2 && x.DateTime.Date >= DateTime.Today.AddDays(-7).Date && x.Status).Count();
        }

        public int GetOneMonthTipsCount()
        {
            return _context.Posts.Where(x => x.PostTypeId == 2 && x.DateTime.Date >= DateTime.Today.AddMonths(-1).Date && x.Status).Count();
        }

        public int GetThreeMonthsTipsCount()
        {
            return _context.Posts.Where(x => x.PostTypeId == 2 && x.DateTime.Date >= DateTime.Today.AddMonths(-3).Date && x.Status).Count();
        }

        public int GetCatOwnerCount()
        {
            return _context.Users.Where(x => x.RoleId == 2).Count();
        }

        public int GetCatExpertCount()
        {
            return _context.Users.Where(x => x.RoleId == 3).Count();
        }

        public int GetMissingCatCount(DateTime date)
        {
            return _context.CatCaseReports.Where(x => x.DateTime.Date == date.Date).Count();
        }

        public int GetMissingCatCount(DateTime startDate, DateTime endDate)
        {
            return _context.CatCaseReports.Where(x => x.DateTime.Date >= startDate.Date && x.DateTime.Date <= endDate).Count();
        }

        public int GetProductsCount()
        {
            return _context.Products.Where(x => x.Status).Count();
        }

        public int GetNewUsersCount()
        {
            return _context.Users.Where(x => x.CreatedTime >= DateTime.Now.Date.AddDays(-30) 
                        && x.CreatedTime < DateTime.Now.Date 
                        && (x.RoleId == 2 || x.RoleId == 3)).Count();
        }

        public int GetTodayPostsCount()
        {
            return _context.Posts.Where(x => x.PostTypeId == 1 && x.DateTime.Date >= DateTime.Today.Date && x.Status).Count();
        }

        public int GetThreeMonthsPostsCount()
        {
            return _context.Posts.Where(x => x.PostTypeId == 1 && x.DateTime.Date >= DateTime.Today.AddMonths(-3).Date && x.Status).Count();
        }

        public int GetOneMonthPostsCount()
        {
            return _context.Posts.Where(x => x.PostTypeId == 1 && x.DateTime.Date >= DateTime.Today.AddMonths(-1).Date && x.Status).Count();
        }

        public int GetOneWeekPostsCount()
        {
            return _context.Posts.Where(x => x.PostTypeId == 1 && x.DateTime.Date >= DateTime.Today.AddDays(-7).Date && x.Status).Count();
        }
    }
}
