import { Injectable } from '@angular/core';
import { ConnectorFunctionUseCaseInterface } from '../interfaces/use-cases/connector-function-use-case.interface';
import { Observable } from 'rxjs';
import { PaginatedItemsViewModel } from '../view-models/paginated-items.view-model';
import { ConnectorFunctionViewModel } from '../view-models/connector-function.view-model';
import { ConnectorFunctionRepositoryInterface } from '../interfaces/repositories/connector-function-repository.interface';

@Injectable({
	providedIn: 'root'
})
export class ConnectorFunctionUseCaseService implements ConnectorFunctionUseCaseInterface {
	constructor(
		private connectorFunctionRepository: ConnectorFunctionRepositoryInterface
	) { }

	getAll(connectorId: string, pageSize: number, pageIndex: number): Observable<PaginatedItemsViewModel<ConnectorFunctionViewModel>> {
		return this.connectorFunctionRepository.getAll(connectorId, pageSize, pageIndex);
	}
}
