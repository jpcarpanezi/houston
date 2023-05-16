import { Observable } from "rxjs";
import { UpdateFirstAccessPasswordCommand } from "../../commands/user-commands/update-first-access-password.command";
import { CreateFirstSetupCommand } from "../../commands/user-commands/create-first-setup.command";
import { UserViewModel } from "../../view-models/user.view-model";

export abstract class UserRepositoryInterface {
	abstract firstAccess(body: UpdateFirstAccessPasswordCommand): Observable<any>;
	abstract firstSetup(body: CreateFirstSetupCommand): Observable<UserViewModel>;
	abstract isFirstSetup(): Observable<any>;
}
