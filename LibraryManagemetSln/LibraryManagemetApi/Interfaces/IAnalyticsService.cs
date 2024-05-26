using LibraryManagemetApi.Models.DTO;

namespace LibraryManagemetApi.Interfaces
{
    public interface IAnalyticsService
    {
        public Task<IEnumerable<ReturnAnalyticsDTO>> GetAnalytics(AnalyticsDTO dto);
        public Task<IEnumerable<ReturnODAnalyticsDTO>> returnODAnalyticsDTOs(AnalyticsDTO dto);
    }
}
