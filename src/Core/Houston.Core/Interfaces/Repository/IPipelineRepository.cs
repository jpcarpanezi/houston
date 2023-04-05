﻿using Houston.Core.Entities.Postgres;

namespace Houston.Core.Interfaces.Repository {
	public interface IPipelineRepository : IRepository<Pipeline> {
		Task<Pipeline?> GetActive(Guid id);

		Task<long> CountActives();

		Task<List<Pipeline>> GetAllActives(int pageSize, int pageIndex);
	}
}
