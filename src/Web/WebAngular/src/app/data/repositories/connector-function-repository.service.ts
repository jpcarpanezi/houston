import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateConnectorFunctionCommand } from 'src/app/domain/commands/connector-function-commands/create-connector-function.command';
import { UpdateConnectorFunctionCommand } from 'src/app/domain/commands/connector-function-commands/update-connector-function.command';
import { ConnectorFunctionRepositoryInterface } from 'src/app/domain/interfaces/repositories/connector-function-repository.interface';
import { ConnectorFunctionDetailViewModel } from 'src/app/domain/view-models/connector-function-detail.view-model';
import { ConnectorFunctionGroupedViewModel } from 'src/app/domain/view-models/connector-function-grouped.view-model';
import { PaginatedItemsViewModel } from 'src/app/domain/view-models/paginated-items.view-model';
import { environment } from 'src/environments/environment';

@Injectable({
	providedIn: 'root'
})
export class ConnectorFunctionRepositoryService implements ConnectorFunctionRepositoryInterface {
	constructor(
		private http: HttpClient
	) { }

	update(body: UpdateConnectorFunctionCommand): Observable<ConnectorFunctionDetailViewModel> {
		const formData = new FormData();
		formData.append("specFile", body.specFile);
		formData.append("script", body.script);
		formData.append("package", body.package);

		return this.http.put<ConnectorFunctionDetailViewModel>(`${environment.apiUrl}/v1/connectorFunction/${body.id}`, formData);
	}

	delete(connectorFunctionId: string): Observable<any> {
		return this.http.delete<any>(`${environment.apiUrl}/v1/connectorFunction/${connectorFunctionId}`)
	}

	get(connectorFunctionId: string): Observable<ConnectorFunctionDetailViewModel> {
		return this.http.get<ConnectorFunctionDetailViewModel>(`${environment.apiUrl}/v1/connectorFunction/item/${connectorFunctionId}`);
	}

	create(body: CreateConnectorFunctionCommand): Observable<ConnectorFunctionDetailViewModel> {
		const formData = new FormData();
		formData.append("specFile", body.specFile);
		formData.append("script", body.script);
		formData.append("package", body.package);

		return this.http.post<ConnectorFunctionDetailViewModel>(`${environment.apiUrl}/v1/connectorFunction`, formData);
	}

	getAll(connectorId: string, pageSize: number, pageIndex: number): Observable<PaginatedItemsViewModel<ConnectorFunctionGroupedViewModel>> {
		return this.http.get<PaginatedItemsViewModel<ConnectorFunctionGroupedViewModel>>(`${environment.apiUrl}/v1/connectorFunction/${connectorId}?connectorId=${connectorId}&pageSize=${pageSize}&pageIndex=${pageIndex}`);
	}
}
