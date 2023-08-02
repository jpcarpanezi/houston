namespace Houston.Application.ViewModel {
	public class MapProfile : Profile {
		public MapProfile() {
			CreateMap<User, UserViewModel>();
			CreateMap<Connector, ConnectorViewModel>()
				.ForMember(dest => dest.CreatedBy, m => m.MapFrom(src => src.CreatedByNavigation.Name))
				.ForMember(dest => dest.UpdatedBy, m => m.MapFrom(src => src.UpdatedByNavigation.Name));
			CreateMap<ConnectorFunctionInput, ConnectorFunctionInputViewModel>()
				.ForMember(dest => dest.InputType, m => m.MapFrom(src => src.Type))
				.ForMember(dest => dest.CreatedBy, m => m.MapFrom(src => src.CreatedByNavigation.Name))
				.ForMember(dest => dest.UpdatedBy, m => m.MapFrom(src => src.UpdatedByNavigation.Name));
			CreateMap<ConnectorFunction, ConnectorFunctionViewModel>()
				.ForMember(dest => dest.Inputs, m => m.MapFrom(src => src.ConnectorFunctionInputs))
				.ForMember(dest => dest.CreatedBy, m => m.MapFrom(src => src.CreatedByNavigation.Name))
				.ForMember(dest => dest.UpdatedBy, m => m.MapFrom(src => src.UpdatedByNavigation.Name));
			CreateMap<Pipeline, PipelineViewModel>()
				.ForMember(dest => dest.CreatedBy, m => m.MapFrom(src => src.CreatedByNavigation.Name))
				.ForMember(dest => dest.UpdatedBy, m => m.MapFrom(src => src.UpdatedByNavigation.Name));
			CreateMap<PipelineTriggerEvent, PipelineTriggerEventViewModel>()
				.ForMember(dest => dest.TriggerEvent, m => m.MapFrom(src => src.TriggerEvent.Value));
			CreateMap<PipelineTriggerFilter, PipelineTriggerFilterViewModel>()
				.ForMember(dest => dest.TriggerFilter, m => m.MapFrom(src => src.TriggerFilter.Value));
			CreateMap<PipelineTrigger, PipelineTriggerViewModel>();
			CreateMap<PipelineInstructionInput, PipelineInstructionInputViewModel>();
			CreateMap<PipelineInstruction, PipelineInstructionViewModel>();
			CreateMap<PipelineLog, PipelineLogViewModel>()
				.ForMember(dest => dest.InstructionWithErrorId, m => m.MapFrom(src => src.InstructionWithError))
				.ForMember(dest => dest.InstructionWithError, m => m.MapFrom(src => src.PipelineInstruction.ConnectorFunction.Name))
				.ForMember(dest => dest.TriggeredById, m => m.MapFrom(src => src.TriggeredBy))
				.ForMember(dest => dest.TriggeredBy, m => m.MapFrom(src => src.TriggeredByNavigation.Name));
			CreateMap<PipelineTrigger, PipelineTriggerKeysViewModel>();
		}
	}
}
