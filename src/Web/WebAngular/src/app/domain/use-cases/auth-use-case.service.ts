import { Injectable } from '@angular/core';
import { AuthRepositoryInterface } from '../interfaces/repositories/auth-repository.interface';
import { AuthUseCaseInterface } from '../interfaces/use-cases/auth-use-case.interface';
import { GeneralSignInCommand } from '../commands/auth-commands/general-sign-in.command';
import { BearerTokenViewModel } from '../view-models/bearer-token.view-model';
import { Observable } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class AuthUseCaseService implements AuthUseCaseInterface {
	constructor(private authRepository: AuthRepositoryInterface) { }

	refreshToken(refreshToken: string): Observable<BearerTokenViewModel> {
		return this.authRepository.refreshToken(refreshToken);
	}

	signIn(body: GeneralSignInCommand): Observable<BearerTokenViewModel> {
		return this.authRepository.signIn(body);
	}
}
