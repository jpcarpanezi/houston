import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SavePipelineInstructionCommand } from 'src/app/domain/commands/pipeline-instruction-commands/save-pipeline-instruction.command';
import { PipelineInstructionRepositoryInterface } from 'src/app/domain/interfaces/repositories/pipeline-instruction-repository.interface';
import { PipelineInstructionViewModel } from 'src/app/domain/view-models/pipeline-instruction.view-model';
import { environment } from 'src/environments/environment';

@Injectable({
	providedIn: 'root'
})
export class PipelineInstructionRepositoryService implements PipelineInstructionRepositoryInterface {
	constructor(
		private http: HttpClient
	) { }

	save(body: SavePipelineInstructionCommand): Observable<PipelineInstructionViewModel> {
		return this.http.post<PipelineInstructionViewModel>(`${environment.apiUrl}/v1/pipelineInstruction`, body);
	}

	get(pipelineId: string): Observable<PipelineInstructionViewModel[]> {
		return this.http.get<PipelineInstructionViewModel[]>(`${environment.apiUrl}/v1/pipelineInstruction/${pipelineId}`);
	}
}
