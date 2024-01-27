using CatViP_API.DTOs.PostDTOs;
using CatViP_API.Models;

namespace CatViP_API.Repositories.Interfaces
{
    public interface IPostRepository
    {
        ICollection<PostType> GetPostTypes();
        ICollection<Post> GetPosts(long authId);
        Task<bool> StorePost(Post post);
        int GetPostDisLikeCount(long postId);
        int GetPostLikeCount(long postId);
        int GetCurrentUserStatusOnPost(long userId, long postId);
        ICollection<Comment> GetPostComments(long postId);
        int GetPostCommentCount(long postId);
        ICollection<PostImage> GetPostImages(long postId);
        Task<bool> ActPost(long userId, PostActionRequestDTO postActionDTO);
        Task<bool> CommentPost(long userId, CommentRequestDTO commentRequestDTO);
        ICollection<Post> GetPostsByAuthId(long authId);
        ICollection<Post> GetPostsByCatId(long authId, long catId);
        bool CheckIfPostExist(long userId, long postId);
        Task<bool> DeletePost(long postId);
        Task<bool> EditPost(long postId, EditPostRequestDTO editPostRequestDTO);
        Task<bool> DeleteActPost(long userId, long postId);
        Task<bool> CreateReportPost(PostReportRequestDTO reportPostDTO, long authId);
        bool AuthHasPostReported(long authId, long postId);
        bool HasReportEqualOrMoreThan10(long postId);
        ICollection<Post> GetPostsByUserId(long authId, long userId);
        ICollection<Post> GetReportedPost();
        ICollection<PostReport> GetReportedPostDetails(long postId);
        bool CheckCommentIsFromCurrentUser(long autId, long id);
        Task<bool> DeleteComment(long id);
        Post? GetPostById(long id);
        Product? GetRandomProduct();
    }
}
