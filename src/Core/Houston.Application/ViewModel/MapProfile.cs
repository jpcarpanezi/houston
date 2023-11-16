namespace Houston.Application.ViewModel {
	public class MapProfile : Profile {
		public MapProfile() {
			CreateMap<User, UserViewModel>();

			CreateMap<Connector, ConnectorViewModel>()
				.ForMember(dest => dest.CreatedBy, m => m.MapFrom(src => src.CreatedByNavigation.Name))
				.ForMember(dest => dest.UpdatedBy, m => m.MapFrom(src => src.UpdatedByNavigation.Name));
			
			CreateMap<IGrouping<string, ConnectorFunction>, ConnectorFunctionGroupedViewModel>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Key))
				.ForMember(dest => dest.Connector, opt => opt.MapFrom(src => src.First().Connector.Name))
				.ForMember(dest => dest.Versions, opt => opt.MapFrom(src => src.Select(cf => new ConnectorFunctionSummaryViewModel {
					Id = cf.Id,
					Version = cf.Version,
					FriendlyName = cf.FriendlyName,
					BuildStatus = cf.BuildStatus,
					CreatedBy = cf.CreatedByNavigation.Name,
					CreationDate = cf.CreationDate,
					UpdatedBy = cf.UpdatedByNavigation.Name,
					LastUpdate = cf.LastUpdate
				})))
				.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.First().CreatedByNavigation.Name))
				.ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.First().CreationDate))
				.ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.First().UpdatedByNavigation.Name))
				.ForMember(dest => dest.LastUpdate, opt => opt.MapFrom(src => src.First().LastUpdate));
			
			CreateMap<ConnectorFunction, ConnectorFunctionDetailViewModel>()
				.ForMember(dest => dest.Connector, m => m.MapFrom(src => src.Connector.Name))
				.ForMember(dest => dest.CreatedBy, m => m.MapFrom(src => src.CreatedByNavigation.Name))
				.ForMember(dest => dest.UpdatedBy, m => m.MapFrom(src => src.UpdatedByNavigation.Name));
			
			CreateMap<ConnectorFunction, ConnectorFunctionSummaryViewModel>()
				.ForMember(dest => dest.Connector, m => m.MapFrom(src => src.Connector.Name))
				.ForMember(dest => dest.CreatedBy, m => m.MapFrom(src => src.CreatedByNavigation.Name))
				.ForMember(dest => dest.UpdatedBy, m => m.MapFrom(src => src.UpdatedByNavigation.Name));

			CreateMap<Pipeline, PipelineViewModel>()
				.ForMember(dest => dest.CreatedBy, m => m.MapFrom(src => src.CreatedByNavigation.Name))
				.ForMember(dest => dest.UpdatedBy, m => m.MapFrom(src => src.UpdatedByNavigation.Name));

			CreateMap<PipelineTrigger, PipelineTriggerViewModel>();

			CreateMap<PipelineLog, PipelineLogViewModel>()
				.ForMember(dest => dest.TriggeredById, m => m.MapFrom(src => src.TriggeredBy))
				.ForMember(dest => dest.TriggeredBy, m => m.MapFrom(src => src.TriggeredByNavigation!.Name));

			CreateMap<PipelineTrigger, PipelineTriggerKeysViewModel>();
		}
	}
}
