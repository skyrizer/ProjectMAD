using AutoMapper;
using CatViP_API.DTOs.AuthDTOs;
using CatViP_API.DTOs.CaseReportDTOs;
using CatViP_API.DTOs.CatDTOs;
using CatViP_API.DTOs.ChatDTOs;
using CatViP_API.DTOs.ExpertDTOs;
using CatViP_API.DTOs.PostDTOs;
using CatViP_API.DTOs.ProductDTOs;
using CatViP_API.DTOs.UserDTOs;
using CatViP_API.Models;

namespace CatViP_API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, AuthInfoDTO>();

            CreateMap<PostType, PostTypeDTO>();

            CreateMap<Post, PostDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.User.ProfileImage));

            CreateMap<Post, ReportedPostDTO>()
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
               .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.FullName))
               .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.User.ProfileImage));

            CreateMap<PostReport, PostReportDTO>()
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
               .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
               .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.User.ProfileImage));

            CreateMap<PostImage, PostImageDTO>();

            CreateMap<Comment, CommentDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.User.ProfileImage));

            CreateMap<CatCaseReportComment, CatCaseReportCommentDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.User.ProfileImage));

            CreateMap<MentionedCat, MentionedCatDTO>()
                .ForMember(dest => dest.CatName, opt => opt.MapFrom(src => src.Cat.Name));

            CreateMap<Cat, CatDTO>();

            CreateMap<ExpertApplication, ExpertApplicationDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name))
                .ForMember(dest => dest.CatOnwerId, opt => opt.MapFrom(src => src.UserId));

            CreateMap<User, SearchUserDTO>();
            CreateMap<User, SerachUserInfoDTO>();
            CreateMap<User, UserInfoDTO>();

            CreateMap<ProductType, ProductTypeDTO>();

            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType.Name));

            CreateMap<CatCaseReport, OwnCaseReportDTO>()
                .ForMember(dest => dest.CatName, opt => opt.MapFrom(src => src.Cat == null ? null : src.Cat.Name));

            CreateMap<CatCaseReportImage, CaseReportImageDTO>();

            CreateMap<CatCaseReport, NearByCaseReportDTO>()
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
               .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.User.ProfileImage))
               .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username));

            CreateMap<User, ChatUserDTO>();
        }
    }
}
