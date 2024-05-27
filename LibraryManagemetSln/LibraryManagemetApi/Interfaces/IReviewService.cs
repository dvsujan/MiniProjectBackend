using LibraryManagemetApi.Models.DTO;

namespace LibraryManagemetApi.Interfaces
{
    public interface IReviewService
    {
        public Task<IEnumerable<ReturnReviewDTO>> GetReviwsByBookId(int bookId);
        public Task<IEnumerable<ReturnReviewDTO>> GetReviewByUserId(int UserId);
        public Task<ReturnReviewDTO> AddReview(AddReviewDTO review);
        public Task<ReturnReviewDTO> DeleteReview(int reviewId);

    }
}
