import { Observable } from "rxjs";
import { PaginatedItemsViewModel } from "../../view-models/paginated-items.view-model";
import { ConnectorViewModel } from "../../view-models/connector.view-model";
import { CreateConnectorCommand } from "../../commands/connector-commands/create-connector.command";

export abstract class ConnectorUseCaseInterface {
	abstract getAll(pageSize: number, pageIndex: number): Observable<PaginatedItemsViewModel<ConnectorViewModel>>;
	abstract create(body: CreateConnectorCommand): Observable<ConnectorViewModel>;
	abstract delete(id: string): Observable<any>;
}
