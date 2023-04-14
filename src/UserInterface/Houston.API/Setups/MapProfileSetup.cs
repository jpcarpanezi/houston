using AutoMapper;
using Houston.Application.ViewModel.ConnectorFunctionInputViewModels;
using Houston.Application.ViewModel.ConnectorFunctionViewModels;
using Houston.Application.ViewModel.ConnectorViewModels;
using Houston.Application.ViewModel.PipelineInstructionInputViewModels;
using Houston.Application.ViewModel.PipelineInstructionViewModels;
using Houston.Application.ViewModel.PipelineTriggerEventViewModels;
using Houston.Application.ViewModel.PipelineTriggerFilterViewModels;
using Houston.Application.ViewModel.PipelineTriggerViewModels;
using Houston.Application.ViewModel.PipelineViewModels;
using Houston.Application.ViewModel.UserViewModels;
using Houston.Core.Entities.Postgres;

namespace Houston.API.Setups {
	public class MapProfileSetup : Profile {
		public MapProfileSetup() {
			CreateMap<User, UserViewModel>();
			CreateMap<Connector, ConnectorViewModel>()
				.ForMember(dest => dest.CreatedBy, m => m.MapFrom(src => src.CreatedByNavigation.Name))
				.ForMember(dest => dest.UpdatedBy, m => m.MapFrom(src => src.UpdatedByNavigation.Name));
			CreateMap<ConnectorFunctionInput, ConnectorFunctionInputViewModel>()
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
		}
	}
}
