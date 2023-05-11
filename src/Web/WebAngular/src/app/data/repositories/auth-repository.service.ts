import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GeneralSignInCommand } from 'src/app/domain/commands/general-sign-in.command';
import { AuthRepositoryInterface } from 'src/app/domain/interfaces/repositories/auth-repository.interface';
import { BearerTokenViewModel } from 'src/app/domain/view-models/bearer-token.view-model';
import { environment } from 'src/environments/environment';

@Injectable({
	providedIn: 'root'
})
export class AuthRepositoryService implements AuthRepositoryInterface {
	constructor(private http: HttpClient) { }

	refreshToken(refreshToken: string): Observable<BearerTokenViewModel> {
		return this.http.get<BearerTokenViewModel>(`${environment.apiUrl}/auth/${refreshToken}`);
	}

	signIn(body: GeneralSignInCommand): Observable<BearerTokenViewModel> {
		return this.http.post<BearerTokenViewModel>(`${environment.apiUrl}/auth`, body);
	}
}
