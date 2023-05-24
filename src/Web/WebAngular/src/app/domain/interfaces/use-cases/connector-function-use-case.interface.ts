import { Observable } from "rxjs";
import { PaginatedItemsViewModel } from "../../view-models/paginated-items.view-model";
import { ConnectorFunctionViewModel } from "../../view-models/connector-function.view-model";

export abstract class ConnectorFunctionUseCaseInterface {
	abstract getAll(connectorId: string, pageSize: number, pageIndex: number): Observable<PaginatedItemsViewModel<ConnectorFunctionViewModel>>;
}
