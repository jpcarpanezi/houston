import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateConnectorFunctionCommand } from 'src/app/domain/commands/connector-function-commands/create-connector-function.command';
import { UpdateConnectorFunctionCommand } from 'src/app/domain/commands/connector-function-commands/update-connector-function.command';
import { ConnectorFunctionRepositoryInterface } from 'src/app/domain/interfaces/repositories/connector-function-repository.interface';
import { ConnectorFunctionViewModel } from 'src/app/domain/view-models/connector-function.view-model';
import { PaginatedItemsViewModel } from 'src/app/domain/view-models/paginated-items.view-model';
import { environment } from 'src/environments/environment';

@Injectable({
	providedIn: 'root'
})
export class ConnectorFunctionRepositoryService implements ConnectorFunctionRepositoryInterface {
	constructor(
		private http: HttpClient
	) { }

	update(body: UpdateConnectorFunctionCommand): Observable<ConnectorFunctionViewModel> {
		return this.http.put<ConnectorFunctionViewModel>(`${environment.apiUrl}/connectorFunction`, body);
	}

	delete(connectorFunctionId: string): Observable<any> {
		return this.http.delete<any>(`${environment.apiUrl}/connectorFunction/${connectorFunctionId}`)
	}

	get(connectorFunctionId: string): Observable<ConnectorFunctionViewModel> {
		return this.http.get<ConnectorFunctionViewModel>(`${environment.apiUrl}/connectorFunction/item/${connectorFunctionId}`);
	}

	create(body: CreateConnectorFunctionCommand): Observable<ConnectorFunctionViewModel> {
		return this.http.post<ConnectorFunctionViewModel>(`${environment.apiUrl}/connectorFunction`, body);
	}

	getAll(connectorId: string, pageSize: number, pageIndex: number): Observable<PaginatedItemsViewModel<ConnectorFunctionViewModel>> {
		return this.http.get<PaginatedItemsViewModel<ConnectorFunctionViewModel>>(`${environment.apiUrl}/connectorFunction/${connectorId}?pageSize=${pageSize}&pageIndex=${pageIndex}`);
	}
}
