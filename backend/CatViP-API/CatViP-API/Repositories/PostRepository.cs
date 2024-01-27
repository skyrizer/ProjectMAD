using CatViP_API.Data;
using CatViP_API.DTOs.PostDTOs;
using CatViP_API.Models;
using CatViP_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace CatViP_API.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly CatViPContext _context;
        public PostRepository(CatViPContext context)
        {
            this._context = context;
        }

        public ICollection<Post> GetPosts(long authId)
        {
            return _context.Posts.Where(x => x.Status && !x.PostReports.Any(y => y.UserId == authId)).Include(x => x.User).Include(x => x.MentionedCats).ThenInclude(x => x.Cat).OrderByDescending(x => x.DateTime).ToList();
        }

        public int GetPostLikeCount(long postId)
        {
            return _context.Posts.Include(x => x.UserActions).FirstOrDefault(x => x.Id == postId)!.UserActions.Count(x => x.ActionTypeId == 1);
        }

        public int GetPostDisLikeCount(long postId)
        {
            return _context.Posts.Include(x => x.UserActions).FirstOrDefault(x => x.Id == postId)!.UserActions.Count(x => x.ActionTypeId == 2);
        }

        public ICollection<PostType> GetPostTypes()
        {
            return _context.PostTypes.ToList();
        }

        public async Task<bool> StorePost(Post post)
        {
            try
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ICollection<Comment> GetPostComments(long postId)
        {
            return _context.Comments.Where(x => x.PostId == postId).Include(x => x.User).OrderByDescending(x => x.DateTime).ToList();
        }

        public ICollection<PostImage> GetPostImages(long postId)
        {
            return _context.PostImages.Where(x => x.PostId == postId).ToList();
        }

        public int GetCurrentUserStatusOnPost(long userId, long postId)
        {
            var post = _context.Posts.Include(x => x.UserActions).FirstOrDefault(x => x.Id == postId);

            if (post!.UserActions.Any(x => x.UserId == userId))
            {
                var userAction = post!.UserActions.FirstOrDefault(x => x.UserId == userId);
                return (int)userAction!.ActionTypeId;
            }

            return 0;
        }

        public int GetPostCommentCount(long postId)
        {
            return _context.Comments.Where(x => x.PostId == postId).Count();
        }

        public async Task<bool> ActPost(long userId, PostActionRequestDTO postActionDTO)
        {
            var postAction = await _context.UserActions.FirstOrDefaultAsync(x => x.UserId == userId && x.PostId == postActionDTO.PostId);

            try
            {
                if (postAction != null)
                {
                    postAction.ActionTypeId = postActionDTO.ActionTypeId;
                    _context.Update(postAction);
                }
                else
                {
                    var newPostAction = new UserAction
                    {
                        UserId = userId,
                        PostId = postActionDTO.PostId,
                        ActionTypeId = postActionDTO.ActionTypeId,
                    };

                    _context.Add(newPostAction);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CommentPost(long userId, CommentRequestDTO commentRequestDTO)
        {
            try
            {
                var comment = new Comment
                {
                    UserId = userId, 
                    PostId = commentRequestDTO.PostId,
                    DateTime = DateTime.Now,
                    Description = commentRequestDTO.Description
                };

                _context.Add(comment);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public ICollection<Post> GetPostsByAuthId(long authId)
        {
            return _context.Posts.Where(x => x.UserId == authId && x.Status).Include(x => x.MentionedCats).ThenInclude(x => x.Cat).ToList();
        }

        public ICollection<Post> GetPostsByCatId(long authId, long catId)
        {
            return _context.Posts.Where(x => x.MentionedCats.Any(y => y.CatId == catId) && x.Status && !x.PostReports.Any(y => y.UserId == authId)).Include(x => x.MentionedCats).ThenInclude(x => x.Cat).ToList();
        }

        public bool CheckIfPostExist(long userId, long postId)
        {
            return _context.Posts.Any(x => x.UserId == userId && x.Id == postId && x.Status);
        }

        public async Task<bool> DeletePost(long postId)
        {
            try
            {
                var post = _context.Posts.FirstOrDefault(x => x.Id == postId);
                post!.Status = false;
                _context.Update(post);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EditPost(long postId, EditPostRequestDTO editPostRequestDTO)
        {
            try
            {
                var post = _context.Posts.FirstOrDefault(x => x.Id == postId);
                post!.Description = editPostRequestDTO.Description;
                _context.Update(post);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteActPost(long userId, long postId)
        {
            try
            {
                var postAction = await _context.UserActions.FirstOrDefaultAsync(x => x.UserId == userId && x.PostId == postId);
                _context.UserActions.Remove(postAction!);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CreateReportPost(PostReportRequestDTO reportPostDTO, long authId)
        {
            try
            {
                var reportPost = new PostReport()
                {
                    DateTime = DateTime.Now,
                    PostId = reportPostDTO.PostId,
                    Description = reportPostDTO.Description,
                    UserId = authId,
                };

                _context.Add(reportPost);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AuthHasPostReported(long authId, long postId)
        {
            return _context.PostReports.Any(x => x.UserId == authId && x.PostId == postId);
        }

        public bool HasReportEqualOrMoreThan10(long postId)
        {
            return _context.PostReports.Where(x => x.PostId == postId).Count() >= 10;
        }

        public ICollection<Post> GetPostsByUserId(long authId, long userId)
        {
            return _context.Posts.Where(x => x.UserId == userId && x.Status).Include(x => x.MentionedCats).ThenInclude(x => x.Cat).ToList();
        }

        public ICollection<Post> GetReportedPost()
        {
            return _context.Posts.Where(x => x.Status && x.PostReports.Any()).Include(x => x.User).Include(x => x.MentionedCats).ThenInclude(x => x.Cat).ToList();
        }

        public ICollection<PostReport> GetReportedPostDetails(long postId)
        {
            return _context.PostReports.Where(x => x.Post.Status && x.PostId == postId).Include(x => x.User).ToList();
        }

        public bool CheckCommentIsFromCurrentUser(long authId, long commentId)
        {
            return _context.Comments.Any(x => x.Id == commentId && x.UserId == authId);
        }

        public async Task<bool> DeleteComment(long id)
        {
            try
            {
                var comment = _context.Comments.FirstOrDefault(x => x.Id == id);
                _context.Comments.Remove(comment!);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Post? GetPostById(long id)
        {
            return _context.Posts.Include(x => x.User).Include(x => x.MentionedCats).ThenInclude(x => x.Cat).FirstOrDefault(x => x.Id == id);
        }

        public Product? GetRandomProduct()
        {
            var count = _context.Products.Count();

            if (count == 0)
            {
                return null;
            }

            var randomIndex = new Random().Next(count);

            return _context.Products.Where(x => x.Status).Include(x => x.Seller).ToList().ElementAt(randomIndex);
        }
    }
}
