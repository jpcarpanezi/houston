import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PipelineLogRepositoryInterface } from 'src/app/domain/interfaces/repositories/pipeline-log-repository.interface';
import { PaginatedItemsViewModel } from 'src/app/domain/view-models/paginated-items.view-model';
import { PipelineLogViewModel } from 'src/app/domain/view-models/pipeline-log.view-model';
import { environment } from 'src/environments/environment';

@Injectable({
	providedIn: 'root'
})
export class PipelineLogRepositoryService implements PipelineLogRepositoryInterface {
	constructor(
		private http: HttpClient
	) { }

	get(id: string): Observable<PipelineLogViewModel> {
		return this.http.get<PipelineLogViewModel>(`${environment.apiUrl}/pipelineLog/item/${id}`);
	}

	getAll(pipelineId: string, pageSize: number, pageIndex: number): Observable<PaginatedItemsViewModel<PipelineLogViewModel>> {
		return this.http.get<PaginatedItemsViewModel<PipelineLogViewModel>>(`${environment.apiUrl}/pipelineLog/${pipelineId}?pageSize=${pageSize}&pageIndex=${pageIndex}`);
	}
}
