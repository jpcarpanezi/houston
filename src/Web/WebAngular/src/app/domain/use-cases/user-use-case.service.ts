import { Injectable } from '@angular/core';
import { UserUseCaseInterface } from '../interfaces/use-cases/user-use-case.interface';
import { Observable } from 'rxjs';
import { UpdateFirstAccessPasswordCommand } from '../commands/user-commands/update-first-access-password.command';
import { UserRepositoryInterface } from '../interfaces/repositories/user-repository.interface';
import { CreateFirstSetupCommand } from '../commands/user-commands/create-first-setup.command';
import { UserViewModel } from '../view-models/user.view-model';

@Injectable({
	providedIn: 'root'
})
export class UserUseCaseService implements UserUseCaseInterface {
	constructor(private userRepository: UserRepositoryInterface) { }

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
