import { Injectable } from '@angular/core';
import { PipelineUseCaseInterface } from '../interfaces/use-cases/pipeline-use-case.interface';
import { Observable } from 'rxjs';
import { CreatePipelineCommand } from '../commands/pipeline-commands/create-pipeline.command';
import { UpdatePipelineCommand } from '../commands/pipeline-commands/update-pipeline.command';
import { PaginatedItemsViewModel } from '../view-models/paginated-items.view-model';
import { PipelineViewModel } from '../view-models/pipeline.view-model';
import { PipelineRepositoryInterface } from '../interfaces/repositories/pipeline-repository.interface';
import { RunPipelineCommand } from '../commands/pipeline-commands/run-pipeline.command';

@Injectable({
	providedIn: 'root'
})
export class PipelineUseCaseService implements PipelineUseCaseInterface {
	constructor(
		private pipelineRepository: PipelineRepositoryInterface
	) { }

	create(body: CreatePipelineCommand): Observable<PipelineViewModel> {
		return this.pipelineRepository.create(body);
	}

	update(body: UpdatePipelineCommand): Observable<PipelineViewModel> {
		return this.pipelineRepository.update(body);
	}

	get(id: string): Observable<PipelineViewModel> {
		return this.pipelineRepository.get(id);
	}

	getAll(pageSize: number, pageIndex: number): Observable<PaginatedItemsViewModel<PipelineViewModel>> {
		return this.pipelineRepository.getAll(pageSize, pageIndex);
	}

	delete(id: string): Observable<any> {
		return this.pipelineRepository.delete(id);
	}

	toggle(id: string): Observable<any> {
		return this.pipelineRepository.toggle(id);
	}

	run(body: RunPipelineCommand): Observable<any> {
		return this.pipelineRepository.run(body);
	}
}
