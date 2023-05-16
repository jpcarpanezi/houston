import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
	providedIn: 'root'
})
export class HttpInterceptorService implements HttpInterceptor {
	constructor(private router: Router) { }

	intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		return next.handle(req).pipe(catchError(error => this.errorHandler(error)));
	}

	private errorHandler(response: HttpErrorResponse): Observable<HttpEvent<any>> {
		let errs: any[] = [];

		switch(response.status) {
			case 401:
				this.router.navigateByUrl("/login");
				break;
			default:
				errs.push(response);
				break;
		}

		return throwError(() => errs);
	}
}
