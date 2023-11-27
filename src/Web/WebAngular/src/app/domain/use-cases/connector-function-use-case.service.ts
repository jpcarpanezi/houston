import { Injectable } from '@angular/core';
import { ConnectorFunctionUseCaseInterface } from '../interfaces/use-cases/connector-function-use-case.interface';
import { Observable } from 'rxjs';
import { PaginatedItemsViewModel } from '../view-models/paginated-items.view-model';
import { ConnectorFunctionRepositoryInterface } from '../interfaces/repositories/connector-function-repository.interface';
import { CreateConnectorFunctionCommand } from '../commands/connector-function-commands/create-connector-function.command';
import { UpdateConnectorFunctionCommand } from '../commands/connector-function-commands/update-connector-function.command';
import { ConnectorFunctionDetailViewModel } from '../view-models/connector-function-detail.view-model';
import { ConnectorFunctionGroupedViewModel } from '../view-models/connector-function-grouped.view-model';

@Injectable({
	providedIn: 'root'
})
export class ConnectorFunctionUseCaseService implements ConnectorFunctionUseCaseInterface {
	constructor(
		private connectorFunctionRepository: ConnectorFunctionRepositoryInterface
	) { }

	update(body: UpdateConnectorFunctionCommand): Observable<ConnectorFunctionDetailViewModel> {
		return this.connectorFunctionRepository.update(body);
	}

	delete(connectorFunctionId: string): Observable<any> {
		return this.connectorFunctionRepository.delete(connectorFunctionId);
	}

	get(connectorFunctionId: string): Observable<ConnectorFunctionDetailViewModel> {
		return this.connectorFunctionRepository.get(connectorFunctionId);
	}

	create(body: CreateConnectorFunctionCommand): Observable<ConnectorFunctionDetailViewModel> {
		return this.connectorFunctionRepository.create(body);
	}

	getAll(connectorId: string, pageSize: number, pageIndex: number): Observable<PaginatedItemsViewModel<ConnectorFunctionGroupedViewModel>> {
		return this.connectorFunctionRepository.getAll(connectorId, pageSize, pageIndex);
	}
}
