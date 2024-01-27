using CatViP_API.Services.Interfaces;
using Quartz;

namespace CatViP_API.Jobs
{
    public class RevokeCaseReportsMoreThan7DaysJob : IJob
    {
        private readonly ICaseReportService _caseReportService;

        public RevokeCaseReportsMoreThan7DaysJob(ICaseReportService caseReportService)
        {
            _caseReportService = caseReportService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _caseReportService.RevokeCaseReportsMoreThan7Days();
        }
    }
}
