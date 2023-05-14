import { Observable } from "rxjs";
import { UpdateFirstAccessPasswordCommand } from "../../commands/user-commands/update-first-access-password.command";

export abstract class UserRepositoryInterface {
	abstract firstAccess(body: UpdateFirstAccessPasswordCommand): Observable<any>;
}
