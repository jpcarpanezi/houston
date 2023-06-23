import { Observable } from "rxjs";
import { PipelineLogViewModel } from "../../view-models/pipeline-log.view-model";
import { PaginatedItemsViewModel } from "../../view-models/paginated-items.view-model";

export abstract class PipelineLogUseCaseInterface {
	abstract get(id: string): Observable<PipelineLogViewModel>;
	abstract getAll(pipelineId: string, pageSize: number, pageIndex: number): Observable<PaginatedItemsViewModel<PipelineLogViewModel>>;
}
