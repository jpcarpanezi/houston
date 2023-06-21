import { Observable } from "rxjs";
import { SavePipelineInstructionCommand } from "../../commands/pipeline-instruction-commands/save-pipeline-instruction.command";
import { PipelineInstructionViewModel } from "../../view-models/pipeline-instruction.view-model";

export abstract class PipelineInstructionUseCaseInterface {
	abstract save(body: SavePipelineInstructionCommand): Observable<PipelineInstructionViewModel>;
	abstract get(pipelineId: string): Observable<PipelineInstructionViewModel[]>;
}
