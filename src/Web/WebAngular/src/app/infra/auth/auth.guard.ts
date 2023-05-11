import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { tap } from 'rxjs';
import { AuthService } from './auth.service';

export const authGuard: CanActivateFn = (route, state) => {
	var router = inject(Router);

	return inject(AuthService).isAuthenticated().pipe(
		tap((isAuthenticated) => {
			return !isAuthenticated ? router.navigate(["/login"]) : true;
		})
	);
};
