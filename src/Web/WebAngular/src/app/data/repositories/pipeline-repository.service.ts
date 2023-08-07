import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreatePipelineCommand } from 'src/app/domain/commands/pipeline-commands/create-pipeline.command';
import { RunPipelineCommand } from 'src/app/domain/commands/pipeline-commands/run-pipeline.command';
import { UpdatePipelineCommand } from 'src/app/domain/commands/pipeline-commands/update-pipeline.command';
import { PipelineRepositoryInterface } from 'src/app/domain/interfaces/repositories/pipeline-repository.interface';
import { PaginatedItemsViewModel } from 'src/app/domain/view-models/paginated-items.view-model';
import { PipelineViewModel } from 'src/app/domain/view-models/pipeline.view-model';
import { environment } from 'src/environments/environment';

@Injectable({
	providedIn: 'root'
})
export class PipelineRepositoryService implements PipelineRepositoryInterface {
	constructor(
		private http: HttpClient
	) { }

	create(body: CreatePipelineCommand): Observable<PipelineViewModel> {
		return this.http.post<PipelineViewModel>(`${environment.apiUrl}/v1/pipeline`, body);
	}

	update(body: UpdatePipelineCommand): Observable<PipelineViewModel> {
		return this.http.put<PipelineViewModel>(`${environment.apiUrl}/v1/pipeline`, body);
	}

	get(id: string): Observable<PipelineViewModel> {
		return this.http.get<PipelineViewModel>(`${environment.apiUrl}/v1/pipeline/${id}`);
	}

	getAll(pageSize: number, pageIndex: number): Observable<PaginatedItemsViewModel<PipelineViewModel>> {
		return this.http.get<PaginatedItemsViewModel<PipelineViewModel>>(`${environment.apiUrl}/v1/pipeline?pageSize=${pageSize}&pageIndex=${pageIndex}`);
	}

	delete(id: string): Observable<any> {
		return this.http.delete(`${environment.apiUrl}/v1/pipeline/${id}`);
	}

	toggle(id: string): Observable<any> {
		return this.http.patch<any>(`${environment.apiUrl}/v1/pipeline/toggle/${id}`, {});
	}

	run(body: RunPipelineCommand): Observable<any> {
		return this.http.post<any>(`${environment.apiUrl}/v1/pipeline/run`, body);
	}
}
