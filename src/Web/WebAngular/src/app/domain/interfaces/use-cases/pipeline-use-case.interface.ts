import { Observable } from "rxjs";
import { CreatePipelineCommand } from "../../commands/pipeline-commands/create-pipeline.command";
import { UpdatePipelineCommand } from "../../commands/pipeline-commands/update-pipeline.command";
import { PaginatedItemsViewModel } from "../../view-models/paginated-items.view-model";
import { PipelineViewModel } from "../../view-models/pipeline.view-model";
import { RunPipelineCommand } from "../../commands/pipeline-commands/run-pipeline.command";

export abstract class PipelineUseCaseInterface {
	abstract create(body: CreatePipelineCommand): Observable<PipelineViewModel>;
	abstract update(body: UpdatePipelineCommand): Observable<PipelineViewModel>;
	abstract get(id: string): Observable<PipelineViewModel>;
	abstract getAll(pageSize: number, pageIndex: number): Observable<PaginatedItemsViewModel<PipelineViewModel>>;
	abstract delete(id: string): Observable<any>;
	abstract toggle(id: string): Observable<any>;
	abstract run(body: RunPipelineCommand): Observable<any>;
}
