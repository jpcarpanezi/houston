import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateConnectorCommand } from 'src/app/domain/commands/connector-commands/create-connector.command';
import { UpdateConnectorCommand } from 'src/app/domain/commands/connector-commands/update-connector.command';
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

	update(body: UpdateConnectorCommand): Observable<ConnectorViewModel> {
		return this.http.put<ConnectorViewModel>(`${environment.apiUrl}/v1/connector`, body);
	}

	delete(id: string): Observable<any> {
		return this.http.delete<any>(`${environment.apiUrl}/v1/connector/${id}`);
	}

	create(body: CreateConnectorCommand): Observable<ConnectorViewModel> {
		return this.http.post<ConnectorViewModel>(`${environment.apiUrl}/v1/connector`, body);
	}

	getAll(pageSize: number, pageIndex: number): Observable<PaginatedItemsViewModel<ConnectorViewModel>> {
		return this.http.get<PaginatedItemsViewModel<ConnectorViewModel>>(`${environment.apiUrl}/v1/connector?pageSize=${pageSize}&pageIndex=${pageIndex}`);
	}
}
