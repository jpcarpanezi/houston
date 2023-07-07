import { Injectable } from '@angular/core';
import { UserUseCaseInterface } from '../interfaces/use-cases/user-use-case.interface';
import { Observable } from 'rxjs';
import { UpdateFirstAccessPasswordCommand } from '../commands/user-commands/update-first-access-password.command';
import { UserRepositoryInterface } from '../interfaces/repositories/user-repository.interface';
import { CreateFirstSetupCommand } from '../commands/user-commands/create-first-setup.command';
import { UserViewModel } from '../view-models/user.view-model';
import { CreateUserCommand } from '../commands/user-commands/create-user.command';
import { UpdatePasswordCommand } from '../commands/user-commands/update-password.command';
import { PaginatedItemsViewModel } from '../view-models/paginated-items.view-model';

@Injectable({
	providedIn: 'root'
})
export class UserUseCaseService implements UserUseCaseInterface {
	constructor(private userRepository: UserRepositoryInterface) { }

	getAll(pageSize: number, pageIndex: number): Observable<PaginatedItemsViewModel<UserViewModel>> {
		return this.userRepository.getAll(pageSize, pageIndex);
	}

	create(body: CreateUserCommand): Observable<UserViewModel> {
		return this.userRepository.create(body);
	}

	toggleStatus(id: string): Observable<any> {
		return this.userRepository.toggleStatus(id);
	}

	changePassword(body: UpdatePasswordCommand): Observable<any> {
		return this.userRepository.changePassword(body);
	}

	isFirstSetup(): Observable<any> {
		return this.userRepository.isFirstSetup();
	}

	firstSetup(body: CreateFirstSetupCommand): Observable<UserViewModel> {
		return this.userRepository.firstSetup(body);
	}

	firstAccess(body: UpdateFirstAccessPasswordCommand): Observable<any> {
		return this.userRepository.firstAccess(body);
	}
}
