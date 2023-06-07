import { Injectable } from '@angular/core';
import { PipelineInstructionUseCaseInterface } from '../interfaces/use-cases/pipeline-instruction-use-case.interface';
import { Observable } from 'rxjs';
import { SavePipelineInstructionCommand } from '../commands/pipeline-instruction-commands/save-pipeline-instruction.command';
import { PipelineInstructionViewModel } from '../view-models/pipeline-instruction.view-model';
import { PipelineInstructionRepositoryInterface } from '../interfaces/repositories/pipeline-instruction-repository.interface';

@Injectable({
	providedIn: 'root'
})
export class PipelineInstructionUseCaseService implements PipelineInstructionUseCaseInterface {
	constructor(
		private pipelineInstructionRepository: PipelineInstructionRepositoryInterface
	) { }

	save(body: SavePipelineInstructionCommand): Observable<PipelineInstructionViewModel> {
		return this.pipelineInstructionRepository.save(body);
	}

	get(pipelineId: string): Observable<PipelineInstructionViewModel> {
		return this.pipelineInstructionRepository.get(pipelineId);
	}
}
