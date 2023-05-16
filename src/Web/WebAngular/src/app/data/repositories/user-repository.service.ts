import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateFirstSetupCommand } from 'src/app/domain/commands/user-commands/create-first-setup.command';
import { UpdateFirstAccessPasswordCommand } from 'src/app/domain/commands/user-commands/update-first-access-password.command';
import { UserRepositoryInterface } from 'src/app/domain/interfaces/repositories/user-repository.interface';
import { UserViewModel } from 'src/app/domain/view-models/user.view-model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserRepositoryService implements UserRepositoryInterface {
	constructor(private http: HttpClient) { }

	isFirstSetup(): Observable<any> {
		return this.http.get<any>(`${environment.apiUrl}/user/isFirstSetup`);
	}

	firstSetup(body: CreateFirstSetupCommand): Observable<UserViewModel> {
		return this.http.post<UserViewModel>(`${environment.apiUrl}/user/firstSetup`, body);
	}

	firstAccess(body: UpdateFirstAccessPasswordCommand): Observable<any> {
		return this.http.patch<any>(`${environment.apiUrl}/user/firstAccess`, body);
	}
}
