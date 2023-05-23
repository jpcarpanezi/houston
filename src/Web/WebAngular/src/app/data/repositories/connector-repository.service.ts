import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateConnectorCommand } from 'src/app/domain/commands/connector-commands/create-connector.command';
import { ConnectorRepositoryInterface } from 'src/app/domain/interfaces/repositories/connector-repository.interface';
import { ConnectorViewModel } from 'src/app/domain/view-models/connector.view-model';
import { PaginatedItemsViewModel } from 'src/app/domain/view-models/paginated-items.view-model';
import { environment } from 'src/environments/environment';

@Injectable({
	providedIn: 'root'
})
export class ConnectorRepositoryService implements ConnectorRepositoryInterface {
	constructor(
		private http: HttpClient
	) { }

	delete(id: string): Observable<any> {
		return this.http.delete<any>(`${environment.apiUrl}/connector/${id}`);
	}

	create(body: CreateConnectorCommand): Observable<ConnectorViewModel> {
		return this.http.post<ConnectorViewModel>(`${environment.apiUrl}/connector`, body);
	}

	getAll(pageSize: number, pageIndex: number): Observable<PaginatedItemsViewModel<ConnectorViewModel>> {
		return this.http.get<PaginatedItemsViewModel<ConnectorViewModel>>(`${environment.apiUrl}/connector?pageSize=${pageSize}&pageIndex=${pageIndex}`);
	}
}
