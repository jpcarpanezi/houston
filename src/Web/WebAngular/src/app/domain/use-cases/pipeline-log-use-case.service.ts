import { Injectable } from '@angular/core';
import { PipelineLogUseCaseInterface } from '../interfaces/use-cases/pipeline-log-use-case.interface';
import { Observable } from 'rxjs';
import { PaginatedItemsViewModel } from '../view-models/paginated-items.view-model';
import { PipelineLogViewModel } from '../view-models/pipeline-log.view-model';
import { PipelineLogRepositoryInterface } from '../interfaces/repositories/pipeline-log-repository.interface';

@Injectable({
	providedIn: 'root'
})
export class PipelineLogUseCaseService implements PipelineLogUseCaseInterface {
	constructor(
		private pipelineLogRepository: PipelineLogRepositoryInterface
	) { }

	get(id: string): Observable<PipelineLogViewModel> {
		return this.pipelineLogRepository.get(id);
	}

	getAll(pipelineId: string, pageSize: number, pageIndex: number): Observable<PaginatedItemsViewModel<PipelineLogViewModel>> {
		return this.pipelineLogRepository.getAll(pipelineId, pageSize, pageIndex);
	}
}
