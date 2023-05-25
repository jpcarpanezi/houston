import { Injectable } from '@angular/core';
import { ConnectorFunctionUseCaseInterface } from '../interfaces/use-cases/connector-function-use-case.interface';
import { Observable } from 'rxjs';
import { PaginatedItemsViewModel } from '../view-models/paginated-items.view-model';
import { ConnectorFunctionViewModel } from '../view-models/connector-function.view-model';
import { ConnectorFunctionRepositoryInterface } from '../interfaces/repositories/connector-function-repository.interface';
import { CreateConnectorFunctionCommand } from '../commands/connector-function-commands/create-connector-function.command';

@Injectable({
	providedIn: 'root'
})
export class ConnectorFunctionUseCaseService implements ConnectorFunctionUseCaseInterface {
	constructor(
		private connectorFunctionRepository: ConnectorFunctionRepositoryInterface
	) { }

	delete(connectorFunctionId: string): Observable<any> {
		return this.connectorFunctionRepository.delete(connectorFunctionId);
	}

	get(connectorFunctionId: string): Observable<ConnectorFunctionViewModel> {
		return this.connectorFunctionRepository.get(connectorFunctionId);
	}

	create(body: CreateConnectorFunctionCommand): Observable<ConnectorFunctionViewModel> {
		return this.connectorFunctionRepository.create(body);
	}

	getAll(connectorId: string, pageSize: number, pageIndex: number): Observable<PaginatedItemsViewModel<ConnectorFunctionViewModel>> {
		return this.connectorFunctionRepository.getAll(connectorId, pageSize, pageIndex);
	}
}
