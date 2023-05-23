import { Injectable } from '@angular/core';
import { ConnectorUseCaseInterface } from '../interfaces/use-cases/connector-use-case.interface';
import { ConnectorRepositoryInterface } from '../interfaces/repositories/connector-repository.interface';
import { Observable } from 'rxjs';
import { ConnectorViewModel } from '../view-models/connector.view-model';
import { PaginatedItemsViewModel } from '../view-models/paginated-items.view-model';
import { CreateConnectorCommand } from '../commands/connector-commands/create-connector.command';
import { UpdateConnectorCommand } from '../commands/connector-commands/update-connector.command';

@Injectable({
	providedIn: 'root'
})
export class ConnectorUseCaseService implements ConnectorUseCaseInterface {
	constructor(
		private connectorRepository: ConnectorRepositoryInterface
	) { }

	update(body: UpdateConnectorCommand): Observable<ConnectorViewModel> {
		return this.connectorRepository.update(body);
	}

	delete(id: string): Observable<any> {
		return this.connectorRepository.delete(id);
	}

	create(body: CreateConnectorCommand): Observable<ConnectorViewModel> {
		return this.connectorRepository.create(body);
	}

	getAll(pageSize: number, pageIndex: number): Observable<PaginatedItemsViewModel<ConnectorViewModel>> {
		return this.connectorRepository.getAll(pageSize, pageIndex);
	}
}
