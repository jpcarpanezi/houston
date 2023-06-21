import { Observable } from "rxjs";
import { PipelineInstructionViewModel } from "../../view-models/pipeline-instruction.view-model";
import { SavePipelineInstructionCommand } from "../../commands/pipeline-instruction-commands/save-pipeline-instruction.command";

export abstract class PipelineInstructionRepositoryInterface {
	abstract save(body: SavePipelineInstructionCommand): Observable<PipelineInstructionViewModel>;
	abstract get(pipelineId: string): Observable<PipelineInstructionViewModel[]>;
}
