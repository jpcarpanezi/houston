import { Observable } from "rxjs";
import { PaginatedItemsViewModel } from "../../view-models/paginated-items.view-model";
import { ConnectorFunctionViewModel } from "../../view-models/connector-function.view-model";
import { CreateConnectorFunctionCommand } from "../../commands/connector-function-commands/create-connector-function.command";

export abstract class ConnectorFunctionUseCaseInterface {
	abstract getAll(connectorId: string, pageSize: number, pageIndex: number): Observable<PaginatedItemsViewModel<ConnectorFunctionViewModel>>;
	abstract get(connectorFunctionId: string): Observable<ConnectorFunctionViewModel>;
	abstract create(body: CreateConnectorFunctionCommand): Observable<ConnectorFunctionViewModel>;
	abstract delete(connectorFunctionId: string): Observable<any>;
}
