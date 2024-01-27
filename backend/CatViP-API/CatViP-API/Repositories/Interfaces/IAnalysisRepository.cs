
namespace CatViP_API.Repositories.Interfaces
{
    public interface IAnalysisRepository
    {
        int GetOneMonthTipsCount();
        int GetOneWeekTipsCount();
        int GetThreeMonthsTipsCount();
        int GetTodayTipsCount();
        int GetCatOwnerCount();
        int GetCatExpertCount();
        int GetMissingCatCount(DateTime date);
        int GetMissingCatCount(DateTime startDate, DateTime endDate);
        int GetProductsCount();
        int GetNewUsersCount();
        int GetTodayPostsCount();
        int GetThreeMonthsPostsCount();
        int GetOneMonthPostsCount();
        int GetOneWeekPostsCount();
    }
}
