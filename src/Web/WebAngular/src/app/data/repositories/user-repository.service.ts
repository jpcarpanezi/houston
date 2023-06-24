import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateFirstSetupCommand } from 'src/app/domain/commands/user-commands/create-first-setup.command';
import { CreateUserCommand } from 'src/app/domain/commands/user-commands/create-user.command';
import { UpdateFirstAccessPasswordCommand } from 'src/app/domain/commands/user-commands/update-first-access-password.command';
import { UpdatePasswordCommand } from 'src/app/domain/commands/user-commands/update-password.command';
import { UserRepositoryInterface } from 'src/app/domain/interfaces/repositories/user-repository.interface';
import { PaginatedItemsViewModel } from 'src/app/domain/view-models/paginated-items.view-model';
import { UserViewModel } from 'src/app/domain/view-models/user.view-model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserRepositoryService implements UserRepositoryInterface {
	constructor(private http: HttpClient) { }

	getAll(pageSize: number, pageIndex: number): Observable<PaginatedItemsViewModel<UserViewModel>> {
		return this.http.get<PaginatedItemsViewModel<UserViewModel>>(`${environment.apiUrl}/user?pageSize=${pageSize}&pageIndex=${pageIndex}`);
	}

	create(body: CreateUserCommand): Observable<UserViewModel> {
		return this.http.post<UserViewModel>(`${environment.apiUrl}/user`, body);
	}

	toggleStatus(id: string): Observable<any> {
		return this.http.patch<any>(`${environment.apiUrl}/user/toggleStatus/${id}`, null);
	}

	changePassword(body: UpdatePasswordCommand): Observable<any> {
		return this.http.patch<any>(`${environment.apiUrl}/user/changePassword`, body);
	}

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
