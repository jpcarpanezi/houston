import { Injectable } from '@angular/core';
import { UserUseCaseInterface } from '../interfaces/use-cases/user-use-case.interface';
import { Observable } from 'rxjs';
import { UpdateFirstAccessPasswordCommand } from '../commands/user-commands/update-first-access-password.command';
import { UserRepositoryInterface } from '../interfaces/repositories/user-repository.interface';

@Injectable({
	providedIn: 'root'
})
export class UserUseCaseService implements UserUseCaseInterface {
	constructor(private userRepository: UserRepositoryInterface) { }

	firstAccess(body: UpdateFirstAccessPasswordCommand): Observable<any> {
		return this.userRepository.firstAccess(body);
	}
}
