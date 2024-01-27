using AutoMapper;
using Azure;
using CatViP_API.DTOs.PostDTOs;
using CatViP_API.Helpers;
using CatViP_API.Models;
using CatViP_API.Repositories.Interfaces;
using CatViP_API.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace CatViP_API.Services
{
    public class PostService : IPostService
    {
        private readonly IConfiguration _configuration;
        private readonly IPostRepository _postRepository;
        private readonly ICatRepository _catRepository;
        private readonly IMapper _mapper;

        public PostService(IConfiguration configuration, IPostRepository postRepository, ICatRepository catRepository, IMapper mapper)
        {
            _configuration = configuration;
            _postRepository = postRepository;
            _catRepository = catRepository;
            _mapper = mapper;
        }

        public ICollection<PostDTO> GetOwnPosts(User currentUser)
        {
            var posts = _mapper.Map<ICollection<PostDTO>>(_postRepository.GetPostsByAuthId(currentUser.Id));

            foreach (var post in posts)
            {
                post.LikeCount = _postRepository.GetPostLikeCount(post.Id);
                post.DislikeCount = _postRepository.GetPostDisLikeCount(post.Id);
                post.CommentCount = _postRepository.GetPostCommentCount(post.Id);
                post.PostImages = _mapper.Map<ICollection<PostImageDTO>>(_postRepository.GetPostImages(post.Id));
                post.CurrentUserAction = _postRepository.GetCurrentUserStatusOnPost(currentUser.Id, post.Id);
            }

            return posts;
        }

        public ICollection<PostTypeDTO> GetPostTypes(User user)
        {
            var postTypes = _mapper.Map<ICollection<PostTypeDTO>>(_postRepository.GetPostTypes());

            if (user.RoleId != 3)
            {
                var itemToRemove = postTypes.SingleOrDefault(pt => pt.Id == 2);
                postTypes.Remove(itemToRemove!);
            }

            return postTypes;
        }

        public async Task<ResponseResult> CreatePost(User user, PostRequestDTO createPostDTO)
        {
            var storeResult = new ResponseResult();

            if (createPostDTO.PostTypeId == 2 && user.RoleId != 3)
            {
                storeResult.IsSuccessful = false;
                storeResult.ErrorMessage = "user is not a expert";
                return storeResult;
            }

            if (createPostDTO.PostTypeId == 1 && createPostDTO.PostImages.IsNullOrEmpty())
            {
                storeResult.IsSuccessful = false;
                storeResult.ErrorMessage = "must be at least one image.";
                return storeResult;
            }

            if (createPostDTO.PostImages.Count > 5)
            {
                storeResult.IsSuccessful = false;
                storeResult.ErrorMessage = "the maximum images count is 5.";
                return storeResult;
            }

            foreach (var mentionCat in createPostDTO.MentionedCats)
            {
                if (!_catRepository.CheckIfCatExist(user.Id, mentionCat.CatId))
                {
                    storeResult.IsSuccessful = false;
                    storeResult.ErrorMessage = "mentioned cat is not exist.";
                    return storeResult;
                }
            }

            if (createPostDTO.PostTypeId == 1)
            {
                foreach (var postImage in createPostDTO.PostImages)
                {
                    var isContainCat = await CatDetectionHelper.CheckIfPhotoContainCat(postImage.Image);

                    if (!isContainCat)
                    {
                        storeResult.IsSuccessful = false;
                        storeResult.ErrorMessage = "image may not contain cat.";
                        return storeResult;
                    }
                }
            }

            var post = new Post()
            {
                UserId = user.Id,
                PostTypeId = createPostDTO.PostTypeId,
                Description = createPostDTO.Description,
                DateTime = DateTime.Now,
                Status = true,

                PostImages = createPostDTO.PostImages.Select(pi => new PostImage
                {
                    Image = pi.Image,
                    IsBloodyContent = pi.IsBloodyContent
                }).ToList(),

                MentionedCats = createPostDTO.MentionedCats.Select(mc => new MentionedCat
                {
                    CatId = mc.CatId,
                }).ToList()
            };

            storeResult.IsSuccessful = await _postRepository.StorePost(post);

            if (!storeResult.IsSuccessful)
            {
                storeResult.ErrorMessage = "fail to store";
                return storeResult;
            }

            return storeResult;
        }

        public async Task<ResponseResult> ActPost(User user, PostActionRequestDTO postActionDTO)
        {
            var res = new ResponseResult();

            res.IsSuccessful = await _postRepository.ActPost(user.Id, postActionDTO);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "Fail to store.";
            }

            return res;
        }

        public async Task<ResponseResult> CommentPost(User user, CommentRequestDTO commentRequestDTO)
        {
            var res = new ResponseResult();

            res.IsSuccessful = await _postRepository.CommentPost(user.Id, commentRequestDTO);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "Fail to store comment.";
            }

            return res;
        }

        public ICollection<CommentDTO> GetPostComments(long authId, long postId)
        {
            var postComments = _mapper.Map<ICollection<CommentDTO>>(_postRepository.GetPostComments(postId));

            foreach (var comment in postComments)
            {
                comment.IsCurrentLoginUser = _postRepository.CheckCommentIsFromCurrentUser(authId, comment.Id);
            }

            return postComments;
        }

        public ICollection<PostDTO> GetPosts(User currentUser)
        {
            var tempPosts = _postRepository.GetPosts(currentUser.Id);

            var posts = new List<PostDTO>();

            for (int i = 0; i < tempPosts.Count; i++)
            {
                var post = _mapper.Map<PostDTO>(tempPosts.ElementAt(i));

                post.IsCurrentUserPost = (post.UserId == currentUser.Id);
                post.LikeCount = _postRepository.GetPostLikeCount(post.Id);
                post.DislikeCount = _postRepository.GetPostDisLikeCount(post.Id);
                post.CommentCount = _postRepository.GetPostCommentCount(post.Id);
                post.PostImages = _mapper.Map<ICollection<PostImageDTO>>(_postRepository.GetPostImages(post.Id));
                post.CurrentUserAction = _postRepository.GetCurrentUserStatusOnPost(currentUser.Id, post.Id);

                posts.Add(post);

                if ((i + 1) % 10 == 0)
                {
                    var product = _postRepository.GetRandomProduct();

                    if (product != null)
                    {
                        var adsDTO = MapProductToPostAdsHelper.MapProductToPostAds(product);
                        posts.Insert(i + 1, adsDTO);
                    }
                }
            }

            return posts;
        }

        public ICollection<PostDTO> GetPostsByCatId(long currentUserId, long catId)
        {
            var posts = _mapper.Map<ICollection<PostDTO>>(_postRepository.GetPostsByCatId(currentUserId, catId));

            foreach (var post in posts)
            {
                post.LikeCount = _postRepository.GetPostLikeCount(post.Id);
                post.DislikeCount = _postRepository.GetPostDisLikeCount(post.Id);
                post.CommentCount = _postRepository.GetPostCommentCount(post.Id);
                post.PostImages = _mapper.Map<ICollection<PostImageDTO>>(_postRepository.GetPostImages(post.Id));
                post.CurrentUserAction = _postRepository.GetCurrentUserStatusOnPost(currentUserId, post.Id);
            }

            return posts;
        }

        public async Task<ResponseResult> DeletePost(long id)
        {
            var res = new ResponseResult();

            res.IsSuccessful = await _postRepository.DeletePost(id);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "fail to delete the post.";
            }

            return res;
        }

        public async Task<ResponseResult> EditPost(long postId, EditPostRequestDTO editPostRequestDTO)
        {
            var res = new ResponseResult();

            res.IsSuccessful = await _postRepository.EditPost(postId, editPostRequestDTO);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "fail to edit the post.";
            }

            return res;
        }

        public ResponseResult CheckIfPostExist(long userId, long postId)
        {
            var res = new ResponseResult();

            res.IsSuccessful = _postRepository.CheckIfPostExist(userId, postId);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "Post is not exist.";
            }

            return res;
        }

        public async Task<ResponseResult> DeleteActPost(long userId, long postId)
        {
            var res = new ResponseResult();

            res.IsSuccessful = await _postRepository.DeleteActPost(userId, postId);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "Post act is not exist.";
            }

            return res;
        }

        public ICollection<PostDTO> GetPostsByUserId(long authId, long userId)
        {
            var posts = _mapper.Map<ICollection<PostDTO>>(_postRepository.GetPostsByUserId(authId, userId));

            foreach (var post in posts)
            {
                post.LikeCount = _postRepository.GetPostLikeCount(post.Id);
                post.DislikeCount = _postRepository.GetPostDisLikeCount(post.Id);
                post.CommentCount = _postRepository.GetPostCommentCount(post.Id);
                post.PostImages = _mapper.Map<ICollection<PostImageDTO>>(_postRepository.GetPostImages(post.Id));
                post.CurrentUserAction = _postRepository.GetCurrentUserStatusOnPost(authId, post.Id);
            }

            return posts;
        }

        public async Task<ResponseResult> ReportPost(PostReportRequestDTO reportPostDTO, long authId)
        {
            var res = new ResponseResult();

            res.IsSuccessful = !_postRepository.CheckIfPostExist(authId, reportPostDTO.PostId);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "you can't report your own post.";
                return res;
            }

            res.IsSuccessful = !_postRepository.AuthHasPostReported(authId, reportPostDTO.PostId);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "you have already reported the post.";
                return res;
            }

            res.IsSuccessful = await _postRepository.CreateReportPost(reportPostDTO, authId);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "fail to report the post.";
            }

            if (_postRepository.HasReportEqualOrMoreThan10(reportPostDTO.PostId))
            {
                await _postRepository.DeletePost(reportPostDTO.PostId);
            }

            return res;
        }

        public ICollection<ReportedPostDTO> GetReportedPosts()
        {
            var posts =  _mapper.Map<ICollection<ReportedPostDTO>>(_postRepository.GetReportedPost());

            foreach (var post in posts)
            {
                post.PostImages = _mapper.Map<ICollection<PostImageDTO>>(_postRepository.GetPostImages(post.Id));
            }

            return posts;
        }

        public ICollection<PostReportDTO> GetReportedPostDetails(long id)
        {
            return _mapper.Map<ICollection<PostReportDTO>>(_postRepository.GetReportedPostDetails(id));
        }

        public async Task<ResponseResult> DeleteComment(long id)
        {
            var res = new ResponseResult();

            res.IsSuccessful = await _postRepository.DeleteComment(id);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "fail to delete the comment.";
            }

            return res;
        }

        public ResponseResult CheckIfCommentExist(long authId, long commentId)
        {
            var res = new ResponseResult();

            res.IsSuccessful = _postRepository.CheckCommentIsFromCurrentUser(authId, commentId);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "comment is not exist.";
            }

            return res;
        }

        public ResponseResult<PostDTO> GetPost(long authId, long id)
        {
            var res = new ResponseResult<PostDTO>();

            res.Result = _mapper.Map<PostDTO>(_postRepository.GetPostById(id));

            if (res.Result == null)
            {
                res.IsSuccessful = false;
                res.ErrorMessage = "post is not exist";
                return res;
            }

            res.Result!.LikeCount = _postRepository.GetPostLikeCount(id);
            res.Result!.DislikeCount = _postRepository.GetPostDisLikeCount(id);
            res.Result!.CommentCount = _postRepository.GetPostCommentCount(id);
            res.Result!.PostImages = _mapper.Map<ICollection<PostImageDTO>>(_postRepository.GetPostImages(id));
            res.Result!.CurrentUserAction = _postRepository.GetCurrentUserStatusOnPost(authId, id);

            return res;
        }
    }
}
