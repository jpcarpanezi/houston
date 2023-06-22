import { Observable } from "rxjs";
import { PipelineTriggerViewModel } from "../../view-models/pipeline-trigger.view-model";
import { CreatePipelineTriggerCommand } from "../../commands/pipeline-trigger-commands/create-pipeline-trigger.command";
import { UpdatePipelineTriggerCommand } from "../../commands/pipeline-trigger-commands/update-pipeline-trigger.command";
import { ChangeSecretPipelineTriggerCommand } from "../../commands/pipeline-trigger-commands/change-secret-pipeline-trigger.command";

export abstract class PipelineTriggerRepositoryInterface {
	abstract create(body: CreatePipelineTriggerCommand): Observable<PipelineTriggerViewModel>;
	abstract update(body: UpdatePipelineTriggerCommand): Observable<PipelineTriggerViewModel>;
	abstract changeSecret(body: ChangeSecretPipelineTriggerCommand): Observable<any>;
	abstract delete(pipelineTriggerId: string): Observable<any>;
	abstract get(pipelineId: string): Observable<PipelineTriggerViewModel>;
}
