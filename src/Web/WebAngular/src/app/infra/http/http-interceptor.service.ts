import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService } from '../auth/auth.service';
import { AuthUseCaseInterface } from 'src/app/domain/interfaces/use-cases/auth-use-case.interface';
import { BearerTokenViewModel } from 'src/app/domain/view-models/bearer-token.view-model';

@Injectable({
	providedIn: 'root'
})
export class HttpInterceptorService implements HttpInterceptor {
	constructor(
		private router: Router,
		private authService: AuthService,
		private authUseCase: AuthUseCaseInterface
	) { }

	intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		if (this.authService.accessToken) {
			req = req.clone({
				setHeaders: {
					Authorization: `Bearer ${this.authService.accessToken}`
				}
			});
		}

		return next.handle(req).pipe(catchError(error => this.errorHandler(error, req, next)));
	}

	private errorHandler(response: HttpErrorResponse, req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		let errs: any[] = [];

		switch(response.status) {
			case 401:
				this.handleUnhautorized(req, next);
				break;
			default:
				errs.push(response);
				break;
		}

		return throwError(() => errs);
	}

	private handleUnhautorized(req: HttpRequest<any>, next: HttpHandler): void {
		if (!this.authService.refreshToken) {
			this.router.navigateByUrl("/login");
			return;
		}

		this.authUseCase.refreshToken(this.authService.refreshToken).subscribe({
			next: (token: BearerTokenViewModel) => {
				this.authService.setSession(token);

				req = req.clone({
					setHeaders: {
						Authorization: `Bearer ${this.authService.accessToken}`
					}
				});

				next.handle(req);
			},
			error: () => this.router.navigateByUrl("/login")
		});
	}
}
