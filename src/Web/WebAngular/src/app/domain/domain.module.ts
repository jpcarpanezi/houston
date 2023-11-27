import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthUseCaseInterface } from './interfaces/use-cases/auth-use-case.interface';
import { AuthUseCaseService } from './use-cases/auth-use-case.service';
import { UserUseCaseInterface } from './interfaces/use-cases/user-use-case.interface';
import { UserUseCaseService } from './use-cases/user-use-case.service';
import { ConnectorUseCaseInterface } from './interfaces/use-cases/connector-use-case.interface';
import { ConnectorUseCaseService } from './use-cases/connector-use-case.service';
import { ConnectorFunctionUseCaseInterface } from './interfaces/use-cases/connector-function-use-case.interface';
import { ConnectorFunctionUseCaseService } from './use-cases/connector-function-use-case.service';
import { PipelineUseCaseInterface } from './interfaces/use-cases/pipeline-use-case.interface';
import { PipelineUseCaseService } from './use-cases/pipeline-use-case.service';
import { PipelineTriggerUseCaseInterface } from './interfaces/use-cases/pipeline-trigger-use-case.interface';
import { PipelineTriggerUseCaseService } from './use-cases/pipeline-trigger-use-case.service';
import { PipelineLogUseCaseInterface } from './interfaces/use-cases/pipeline-log-use-case.interface';
import { PipelineLogUseCaseService } from './use-cases/pipeline-log-use-case.service';
import { ConnectorFunctionHistoryUseCaseInterface } from './interfaces/use-cases/connector-function-history-use-case.interface';
import { ConnectorFunctionHistoryUseCaseService } from './use-cases/connector-function-history-use-case.service';
import { PipelineInstructionUseCaseInterface } from './interfaces/use-cases/pipeline-instruction-use-case.interface';
import { PipelineInstructionUseCaseService } from './use-cases/pipeline-instruction-use-case.service';



@NgModule({
	declarations: [],
	imports: [
		CommonModule
	],
	providers: [
		{ provide: AuthUseCaseInterface, useClass: AuthUseCaseService },
		{ provide: UserUseCaseInterface, useClass: UserUseCaseService },
		{ provide: ConnectorUseCaseInterface, useClass: ConnectorUseCaseService },
		{ provide: ConnectorFunctionUseCaseInterface, useClass: ConnectorFunctionUseCaseService },
		{ provide: ConnectorFunctionHistoryUseCaseInterface, useClass: ConnectorFunctionHistoryUseCaseService },
		{ provide: PipelineInstructionUseCaseInterface, useClass: PipelineInstructionUseCaseService },
		{ provide: PipelineUseCaseInterface, useClass: PipelineUseCaseService },
		{ provide: PipelineTriggerUseCaseInterface, useClass: PipelineTriggerUseCaseService },
		{ provide: PipelineLogUseCaseInterface, useClass: PipelineLogUseCaseService }
	]
})
export class DomainModule { }
