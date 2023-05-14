import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UpdateFirstAccessPasswordCommand } from 'src/app/domain/commands/user-commands/update-first-access-password.command';
import { UserRepositoryInterface } from 'src/app/domain/interfaces/repositories/user-repository.interface';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserRepositoryService implements UserRepositoryInterface {
	constructor(private http: HttpClient) { }

	firstAccess(body: UpdateFirstAccessPasswordCommand): Observable<any> {
		return this.http.patch<any>(`${environment.apiUrl}/user/firstAccess`, body);
	}
}
