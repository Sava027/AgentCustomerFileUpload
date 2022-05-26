using AgentCustomer.Files;
using AutoMapper;


namespace AgentCustomer.FileDataAccess
{
    public class AutoMapperFileInfoProfile : Profile
    {

        public AutoMapperFileInfoProfile()
        {
            CreateMap<FileInfo, CustomerFile>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(x => x.FileInfoId))
                .ForMember(dest => dest.CustomerId, opts => opts.MapFrom(x => x.CustomerId))
                .ForMember(dest => dest.AgentId, opts => opts.MapFrom(x => x.AgentId))
                .ForMember(dest => dest.Path, opts => opts.MapFrom(x => x.Path))
                .ForMember(dest => dest.TrackingId, opts => opts.MapFrom(x => x.FileTrackingId))
                .ForMember(dest => dest.UploadedDate, opts => opts.MapFrom(x => x.DateCreated))
                .ForMember(dest => dest.FileType, opts => opts.MapFrom((s, d) =>
                {
                    return EnumExtension.GetValueFromDescription<CustomerFileType>(s.FileType);

                }));

            CreateMap<CustomerFile, FileInfo>()
               .ForMember(dest => dest.FileInfoId, opts => opts.MapFrom(x => x.Id))
            .ForMember(dest => dest.CustomerId, opts => opts.MapFrom(x => x.CustomerId))
             .ForMember(dest => dest.AgentId, opts => opts.MapFrom(x => x.AgentId))
            .ForMember(dest => dest.Path, opts => opts.MapFrom(x => x.Path))
            .ForMember(dest => dest.FileTrackingId, opts => opts.MapFrom(x => x.TrackingId))
            .ForMember(dest => dest.FileType, opts => opts.MapFrom((s, d) =>
            {
                return EnumExtension.GetDescription(s.FileType);

            }))
            .ForAllMembers(opts => opts.Ignore());
        }

    }
}
