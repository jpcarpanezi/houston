import { Observable } from "rxjs";
import { UpdateFirstAccessPasswordCommand } from "../../commands/user-commands/update-first-access-password.command";
import { UserViewModel } from "../../view-models/user.view-model";
import { CreateFirstSetupCommand } from "../../commands/user-commands/create-first-setup.command";
import { CreateUserCommand } from "../../commands/user-commands/create-user.command";
import { UpdatePasswordCommand } from "../../commands/user-commands/update-password.command";
import { PaginatedItemsViewModel } from "../../view-models/paginated-items.view-model";

export abstract class UserUseCaseInterface {
	abstract firstAccess(body: UpdateFirstAccessPasswordCommand): Observable<any>;
	abstract firstSetup(body: CreateFirstSetupCommand): Observable<UserViewModel>;
	abstract isFirstSetup(): Observable<any>;
	abstract getAll(pageSize: number, pageIndex: number): Observable<PaginatedItemsViewModel<UserViewModel>>;
	abstract create(body: CreateUserCommand): Observable<UserViewModel>;
	abstract toggleStatus(id: string): Observable<any>;
	abstract changePassword(body: UpdatePasswordCommand): Observable<any>;
}
