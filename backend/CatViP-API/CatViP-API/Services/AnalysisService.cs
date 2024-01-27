using CatViP_API.Repositories.Interfaces;
using CatViP_API.Services.Interfaces;

namespace CatViP_API.Services
{
    public class AnalysisService : IAnalysisService
    {
        private readonly IAnalysisRepository _analysisRepository;

        public AnalysisService(IAnalysisRepository analysisRepository)
        {
            _analysisRepository = analysisRepository;
        }

        public ResponseResult<Dictionary<string, int>> GetMissingCatsCount(string query, DateTime? startDate, DateTime? endDate)
        {
            var res = new ResponseResult<Dictionary<string, int>>();

            if (startDate != null && endDate != null && startDate > endDate)
            {
                res.IsSuccessful = false;
                res.ErrorMessage = "start date could not greater than end date";
                return res;
            }

            if (query == "7days")
            {
                var dicts = new Dictionary<string, int>();

                for (var i = DateTime.Today.AddDays(-6).Date; i <= DateTime.Today.Date; i = i.AddDays(1))
                {
                    dicts.Add(i.DayOfWeek.ToString(), _analysisRepository.GetMissingCatCount(i));
                }

                res.Result = dicts;
            }
            else if (query == "weeks")
            {
                var dicts = new Dictionary<string, int>();
                var startOfPeriod = startDate ?? DateTime.Today.AddDays(-27);
                var endOfPeriod = endDate ?? DateTime.Today;

                for (var i = startOfPeriod; i <= endOfPeriod; i = i.AddDays(7))
                {
                    var endOfWeek = i.AddDays(6) > endOfPeriod ? endOfPeriod : i.AddDays(6);
                    dicts.Add($"Week of {i:MMMM dd}", _analysisRepository.GetMissingCatCount(i, endOfWeek));
                }

                res.Result = dicts;
            }
            else if (query == "months")
            {
                var dicts = new Dictionary<string, int>();

                var startOfYear = startDate ?? DateTime.Today.AddMonths(-11);
                var endOfYear = endDate ?? DateTime.Today;

                for (var i = startOfYear; i <= endOfYear; i = i.AddMonths(1))
                {
                    var endOfMonth = i.AddMonths(1).AddDays(-1) > endOfYear ? endOfYear : i.AddMonths(1).AddDays(-1);
                    dicts.Add(i.ToString("MMMM yyyy"), _analysisRepository.GetMissingCatCount(i, endOfMonth));
                }

                res.Result = dicts;
            }
            else
            {
                res.IsSuccessful = false;
                res.ErrorMessage = "Invalid query indentifier";
            }

            return res;
        }

        public Dictionary<string, int> GetUsersAndProductsCount()
        {
            return new Dictionary<string, int>
            {
                { "Cat Owners", _analysisRepository.GetCatOwnerCount() },
                { "Cat Experts", _analysisRepository.GetCatExpertCount() },
                { "New Users", _analysisRepository.GetNewUsersCount() },
                { "Products", _analysisRepository.GetProductsCount() },
            };
        }

        public ResponseResult<Dictionary<string, int>> GetPostsAndExpertTipsCount(string query)
        {
            Dictionary<string, Dictionary<string, int>> queryValues = new Dictionary<string, Dictionary<string, int>>()
            {
                { "today", new Dictionary<string, int>(){
                    { "Posts", _analysisRepository.GetTodayPostsCount() },
                    { "Expert Tips", _analysisRepository.GetTodayTipsCount() }
                } },
                { "lastWeek", new Dictionary<string, int>(){
                    { "Posts", _analysisRepository.GetOneWeekPostsCount() },
                    { "Expert Tips", _analysisRepository.GetOneWeekTipsCount() }
                } },
                { "lastMonth", new Dictionary<string, int>(){
                    { "Posts", _analysisRepository.GetOneMonthPostsCount() },
                    { "Expert Tips", _analysisRepository.GetOneMonthTipsCount() }
                } },
                { "last3Months", new Dictionary<string, int>(){
                    { "Posts", _analysisRepository.GetThreeMonthsPostsCount() },
                    { "Expert Tips", _analysisRepository.GetThreeMonthsTipsCount() }
                } },
            };

            var res = new ResponseResult<Dictionary<string, int>>();

            if (queryValues.ContainsKey(query))
            {
                res.Result = queryValues[query];
            }
            else
            {
                res.IsSuccessful = false;
                res.ErrorMessage = "Invalid query indentifier";
            }

            return res;
        }
    }
}
