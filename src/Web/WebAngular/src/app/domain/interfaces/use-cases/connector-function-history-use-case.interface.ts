import { Observable } from "rxjs";
import { CreateConnectorFunctionHistoryCommand } from "../../commands/connector-function-history-commands/create-connector-function-history.command";
import { UpdateConnectorFunctionHistoryCommand } from "../../commands/connector-function-history-commands/update-connector-function-history.command";
import { ConnectorFunctionHistoryDetailViewModel } from "../../view-models/connector-function-history-detail.view-model";

export abstract class ConnectorFunctionHistoryUseCaseInterface {
	abstract create(body: CreateConnectorFunctionHistoryCommand): Observable<ConnectorFunctionHistoryDetailViewModel>;
	abstract update(body: UpdateConnectorFunctionHistoryCommand): Observable<ConnectorFunctionHistoryDetailViewModel>;
	abstract delete(id: string) : Observable<any>;
	abstract get(id: string): Observable<ConnectorFunctionHistoryDetailViewModel>;
}
