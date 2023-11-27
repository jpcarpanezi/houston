import { Observable } from "rxjs";
import { PaginatedItemsViewModel } from "../../view-models/paginated-items.view-model";
import { CreateConnectorFunctionCommand } from "../../commands/connector-function-commands/create-connector-function.command";
import { UpdateConnectorFunctionCommand } from "../../commands/connector-function-commands/update-connector-function.command";
import { ConnectorFunctionGroupedViewModel } from "../../view-models/connector-function-grouped.view-model";
import { ConnectorFunctionDetailViewModel } from "../../view-models/connector-function-detail.view-model";

export abstract class ConnectorFunctionUseCaseInterface {
	abstract getAll(connectorId: string, pageSize: number, pageIndex: number): Observable<PaginatedItemsViewModel<ConnectorFunctionGroupedViewModel>>;
	abstract get(connectorFunctionId: string): Observable<ConnectorFunctionDetailViewModel>;
	abstract create(body: CreateConnectorFunctionCommand): Observable<ConnectorFunctionDetailViewModel>;
	abstract delete(connectorFunctionId: string): Observable<any>;
	abstract update(body: UpdateConnectorFunctionCommand): Observable<ConnectorFunctionDetailViewModel>;
}
