using AgentCustomer.Files;
using AutoMapper;


namespace AgentCustomer.FileDataAccess
{
    public class AutoMapperFileTrackingProfile : Profile
    {
        public AutoMapperFileTrackingProfile()
        {
            CreateMap<FileTracking, TrackingInfo>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(x => x.FileTrackingId))
                .ForMember(dest => dest.PulicTrackingId, opts => opts.MapFrom(x => x.PulicTrackingId))
                .ForMember(dest => dest.FileCount, opts => opts.MapFrom(x => x.FileCount))
                .ForMember(dest => dest.TempLocationPath, opts => opts.MapFrom(x => x.TempLocationPath))                
                .ForMember(dest => dest.Status, opts => opts.MapFrom((s, d) =>
                {
                    return EnumExtension.GetValueFromDescription<FileTrackingStatus>(s.Status);

                }));

            CreateMap<TrackingInfo, FileTracking>()
             .ForMember(dest => dest.FileTrackingId, opts => opts.MapFrom(x => x.Id))
             .ForMember(dest => dest.PulicTrackingId, opts => opts.MapFrom(x => x.PulicTrackingId))
             .ForMember(dest => dest.FileCount, opts => opts.MapFrom(x => x.FileCount))
             .ForMember(dest => dest.TempLocationPath, opts => opts.MapFrom(x => x.TempLocationPath))
             .ForMember(dest => dest.Status, opts => opts.MapFrom((s, d) =>
                {
                    return EnumExtension.GetDescription(s.Status);

                }));
             
        }

    }
}
