import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ChangeSecretPipelineTriggerCommand } from 'src/app/domain/commands/pipeline-trigger-commands/change-secret-pipeline-trigger.command';
import { CreatePipelineTriggerCommand } from 'src/app/domain/commands/pipeline-trigger-commands/create-pipeline-trigger.command';
import { UpdatePipelineTriggerCommand } from 'src/app/domain/commands/pipeline-trigger-commands/update-pipeline-trigger.command';
import { PipelineTriggerRepositoryInterface } from 'src/app/domain/interfaces/repositories/pipeline-trigger-repository.interface';
import { PipelineTriggerKeysViewModel } from 'src/app/domain/view-models/pipeline-trigger-keys.view-model';
import { PipelineTriggerViewModel } from 'src/app/domain/view-models/pipeline-trigger.view-model';
import { environment } from 'src/environments/environment';

@Injectable({
	providedIn: 'root'
})
export class PipelineTriggerRepositoryService implements PipelineTriggerRepositoryInterface {
	constructor(
		private http: HttpClient
	) { }

	updateDeployKeys(pipelineId: string): Observable<any> {
		return this.http.patch<any>(`${environment.apiUrl}/pipelineTrigger/deployKeys/${pipelineId}`, null);
	}

	revealKeys(pipelineId: string): Observable<PipelineTriggerKeysViewModel> {
		return this.http.get<PipelineTriggerKeysViewModel>(`${environment.apiUrl}/pipelineTrigger/deployKeys/${pipelineId}`);
	}

	create(body: CreatePipelineTriggerCommand): Observable<PipelineTriggerViewModel> {
		return this.http.post<PipelineTriggerViewModel>(`${environment.apiUrl}/pipelineTrigger`, body);
	}

	update(body: UpdatePipelineTriggerCommand): Observable<PipelineTriggerViewModel> {
		return this.http.put<PipelineTriggerViewModel>(`${environment.apiUrl}/pipelineTrigger`, body);
	}

	changeSecret(body: ChangeSecretPipelineTriggerCommand): Observable<any> {
		return this.http.patch<any>(`${environment.apiUrl}/pipelineTrigger/changeSecret`, body);
	}

	delete(pipelineTriggerId: string): Observable<any> {
		return this.http.delete<any>(`${environment.apiUrl}/pipelineTrigger/${pipelineTriggerId}`);
	}

	get(pipelineId: string): Observable<PipelineTriggerViewModel> {
		return this.http.get<PipelineTriggerViewModel>(`${environment.apiUrl}/pipelineTrigger/${pipelineId}`);
	}
}
