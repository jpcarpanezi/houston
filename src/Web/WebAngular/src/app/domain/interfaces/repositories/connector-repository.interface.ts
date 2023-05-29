import { Observable } from "rxjs";
import { ConnectorViewModel } from "../../view-models/connector.view-model";
import { PaginatedItemsViewModel } from "../../view-models/paginated-items.view-model";
import { CreateConnectorCommand } from "../../commands/connector-commands/create-connector.command";
import { UpdateConnectorCommand } from "../../commands/connector-commands/update-connector.command";

export abstract class ConnectorRepositoryInterface {
	abstract getAll(pageSize: number, pageIndex: number): Observable<PaginatedItemsViewModel<ConnectorViewModel>>;
	abstract create(body: CreateConnectorCommand): Observable<ConnectorViewModel>;
	abstract delete(id: string): Observable<any>;
	abstract update(body: UpdateConnectorCommand): Observable<ConnectorViewModel>;
}
