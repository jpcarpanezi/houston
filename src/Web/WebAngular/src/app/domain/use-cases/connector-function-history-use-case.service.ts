import { Injectable } from '@angular/core';
import { ConnectorFunctionHistoryUseCaseInterface } from '../interfaces/use-cases/connector-function-history-use-case.interface';
import { Observable } from 'rxjs';
import { CreateConnectorFunctionHistoryCommand } from '../commands/connector-function-history-commands/create-connector-function-history.command';
import { UpdateConnectorFunctionHistoryCommand } from '../commands/connector-function-history-commands/update-connector-function-history.command';
import { ConnectorFunctionHistoryDetailViewModel } from '../view-models/connector-function-history-detail.view-model';
import { ConnectorFunctionHistoryRepositoryInterface } from '../interfaces/repositories/connector-function-history-repository.interface';

@Injectable({
	providedIn: 'root'
})
export class ConnectorFunctionHistoryUseCaseService implements ConnectorFunctionHistoryUseCaseInterface {
	constructor(
		private connectorFunctionHistoryRepository: ConnectorFunctionHistoryRepositoryInterface
	) { }

	create(body: CreateConnectorFunctionHistoryCommand): Observable<ConnectorFunctionHistoryDetailViewModel> {
		return this.connectorFunctionHistoryRepository.create(body);
	}

	update(body: UpdateConnectorFunctionHistoryCommand): Observable<ConnectorFunctionHistoryDetailViewModel> {
		return this.connectorFunctionHistoryRepository.update(body);
	}

	delete(id: string): Observable<any> {
		return this.connectorFunctionHistoryRepository.delete(id);
	}

	get(id: string): Observable<ConnectorFunctionHistoryDetailViewModel> {
		return this.connectorFunctionHistoryRepository.get(id);
	}
}
