import { Observable } from "rxjs";
import { UpdateFirstAccessPasswordCommand } from "../../commands/user-commands/update-first-access-password.command";
import { UserViewModel } from "../../view-models/user.view-model";
import { CreateFirstSetupCommand } from "../../commands/user-commands/create-first-setup.command";

export abstract class UserUseCaseInterface {
	abstract firstAccess(body: UpdateFirstAccessPasswordCommand): Observable<any>;
	abstract firstSetup(body: CreateFirstSetupCommand): Observable<UserViewModel>;
	abstract isFirstSetup(): Observable<any>;
}
