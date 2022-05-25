using AutoMapper;
using RssFeedAggregator.DAL.Entities;
using RssFeedAggregator.Responses.RssFeedRequests;
using System.ServiceModel.Syndication;

namespace RssFeedAggregator.DAL
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<SyndicationFeed, FeedSourceEntity>()
                .ForMember("Description", x => x.MapFrom(src => src.Description.Text))
                .ForMember("Title", x => x.MapFrom(src => src.Title.Text));

            CreateMap<SyndicationItem, PostEntity>()
                .ForMember(x => x.Description, x => x.MapFrom(src => src.Summary.Text))
                .ForMember(x => x.Title, x => x.MapFrom(src => src.Title.Text))
                .ForMember(x => x.PublishedAt, x => x.MapFrom(src => src.PublishDate.DateTime))
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<PostEntity, Post>();
        }
    }
}
