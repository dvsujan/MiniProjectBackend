using LibraryManagemetApi.Models.DTO;

namespace LibraryManagemetApi.Interfaces
{
    public interface IReviewService
    {
        public Task<IEnumerable<ReturnReviewDTO>> GetReviwsByBookId(int bookId);
        public Task<Task<ReturnReviewDTO>> GetReviewByUserId(int UserId);
        public Task<Task<ReturnReviewDTO>> AddReview(AddReviewDTO review);
        public Task<Task<ReturnReviewDTO>> DeleteReview(int reviewId);

    }
}
