using AutoMapper;
using Houston.Application.ViewModel.UserViewModels;
using Houston.Core.Entities.Postgres;

namespace Houston.API.Setups {
	public class MapProfileSetup : Profile {
		public MapProfileSetup() {
			CreateMap<User, UserViewModel>();
		}
	}
}
