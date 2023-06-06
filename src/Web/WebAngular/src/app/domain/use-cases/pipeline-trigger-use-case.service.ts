import { Injectable } from '@angular/core';
import { PipelineTriggerUseCaseInterface } from '../interfaces/use-cases/pipeline-trigger-use-case.interface';
import { Observable } from 'rxjs';
import { ChangeSecretPipelineTriggerCommand } from '../commands/pipeline-trigger-commands/change-secret-pipeline-trigger.command';
import { CreatePipelineTriggerCommand } from '../commands/pipeline-trigger-commands/create-pipeline-trigger.command';
import { UpdatePipelineTriggerCommand } from '../commands/pipeline-trigger-commands/update-pipeline-trigger.command';
import { PipelineTriggerViewModel } from '../view-models/pipeline-trigger.view-model';
import { PipelineTriggerRepositoryInterface } from '../interfaces/repositories/pipeline-trigger-repository.interface';

@Injectable({
	providedIn: 'root'
})
export class PipelineTriggerUseCaseService implements PipelineTriggerUseCaseInterface {
	constructor(
		private pipelineTriggerRepository: PipelineTriggerRepositoryInterface
	) { }

	create(body: CreatePipelineTriggerCommand): Observable<PipelineTriggerViewModel> {
		return this.pipelineTriggerRepository.create(body);
	}

	update(body: UpdatePipelineTriggerCommand): Observable<PipelineTriggerViewModel> {
		return this.pipelineTriggerRepository.update(body);
	}

	changeSecret(body: ChangeSecretPipelineTriggerCommand): Observable<any> {
		return this.pipelineTriggerRepository.changeSecret(body);
	}

	delete(pipelineTriggerId: string): Observable<any> {
		return this.pipelineTriggerRepository.delete(pipelineTriggerId);
	}

	get(pipelineId: string): Observable<PipelineTriggerViewModel> {
		return this.pipelineTriggerRepository.get(pipelineId);
	}
}
