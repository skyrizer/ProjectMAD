using CatViP_API.DTOs.PostDTOs;
using CatViP_API.Models;

namespace CatViP_API.Services.Interfaces
{
    public interface IPostService
    {
        ICollection<PostDTO> GetPosts(User currentUser);
        ICollection<PostDTO> GetOwnPosts(User currentUser);
        ICollection<PostDTO> GetPostsByCatId(long currentUserId, long catId);
        ICollection<PostTypeDTO> GetPostTypes(User user);
        Task<ResponseResult> CreatePost(User user, PostRequestDTO createPostDTO);
        Task<ResponseResult> ActPost(User user, PostActionRequestDTO postActionDTO);
        Task<ResponseResult> CommentPost(User user, CommentRequestDTO commentRequestDTO);
        ICollection<CommentDTO> GetPostComments(long authId, long postId);
        Task<ResponseResult> DeletePost(long id);
        Task<ResponseResult> EditPost(long id, EditPostRequestDTO editPostRequestDTO);
        ResponseResult CheckIfPostExist(long userId, long postId);
        Task<ResponseResult> DeleteActPost(long userId, long postId);
        ICollection<PostDTO> GetPostsByUserId(long authId, long userId);
        Task<ResponseResult> ReportPost(PostReportRequestDTO reportPostDTO, long authId);
        ICollection<ReportedPostDTO> GetReportedPosts();
        ICollection<PostReportDTO> GetReportedPostDetails(long id);
        Task<ResponseResult> DeleteComment(long id);
        ResponseResult CheckIfCommentExist(long authId, long commentId);
        ResponseResult<PostDTO> GetPost(long authId, long id);
    }
}
