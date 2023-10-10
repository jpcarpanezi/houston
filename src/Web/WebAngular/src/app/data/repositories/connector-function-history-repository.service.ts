import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateConnectorFunctionHistoryCommand } from 'src/app/domain/commands/connector-function-history-commands/create-connector-function-history.command';
import { UpdateConnectorFunctionHistoryCommand } from 'src/app/domain/commands/connector-function-history-commands/update-connector-function-history.command';
import { ConnectorFunctionHistoryRepositoryInterface } from 'src/app/domain/interfaces/repositories/connector-function-history-repository.interface';
import { ConnectorFunctionHistoryDetailViewModel } from 'src/app/domain/view-models/connector-function-history-detail.view-model';
import { environment } from 'src/environments/environment';

@Injectable({
	providedIn: 'root'
})
export class ConnectorFunctionHistoryRepositoryService implements ConnectorFunctionHistoryRepositoryInterface {
	constructor(
		private http: HttpClient
	) { }

	create(body: CreateConnectorFunctionHistoryCommand): Observable<ConnectorFunctionHistoryDetailViewModel> {
		return this.http.post<ConnectorFunctionHistoryDetailViewModel>(`${environment.apiUrl}/v1/connectorFunctionHistory`, body);
	}

	update(body: UpdateConnectorFunctionHistoryCommand): Observable<ConnectorFunctionHistoryDetailViewModel> {
		return this.http.put<ConnectorFunctionHistoryDetailViewModel>(`${environment.apiUrl}/v1/connectorFunctionHistory`, body);
	}

	delete(id: string): Observable<any> {
		return this.http.delete<any>(`${environment.apiUrl}/v1/connectorFunctionHistory/${id}`);
	}

	get(id: string): Observable<ConnectorFunctionHistoryDetailViewModel> {
		return this.http.get<ConnectorFunctionHistoryDetailViewModel>(`${environment.apiUrl}/v1/connectorFunctionHistory/item/${id}`);
	}
}
