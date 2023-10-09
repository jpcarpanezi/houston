import { Observable } from "rxjs";
import { ConnectorFunctionHistoryDetailViewModel } from "../../view-models/connector-function-history-detail.view-model";
import { CreateConnectorFunctionHistoryCommand } from "../../commands/connector-function-history-commands/create-connector-function-history.command";
import { UpdateConnectorFunctionHistoryCommand } from "../../commands/connector-function-history-commands/update-connector-function-history.command";

export abstract class ConnectorFunctionHistoryRepositoryInterface {
	abstract create(body: CreateConnectorFunctionHistoryCommand): Observable<ConnectorFunctionHistoryDetailViewModel>;
	abstract update(body: UpdateConnectorFunctionHistoryCommand): Observable<ConnectorFunctionHistoryDetailViewModel>;
	abstract delete(id: string) : Observable<any>;
	abstract get(id: string): Observable<ConnectorFunctionHistoryDetailViewModel>;
}
