import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { tap } from 'rxjs';
import { AuthService } from './auth.service';

export const authGuard: CanActivateFn = (route, state) => {
	const router = inject(Router);
	const authService = inject(AuthService);

	return authService.isAuthenticated().pipe(
		tap((isAuthenticated) => {
			if (!isAuthenticated) {
				return router.navigate(["/login"]);
			} else {
				const requiredRoles: string[] = route.data["roles"];
				const userRoles: string = authService.userInfo!.role;

				if (!requiredRoles) {
					return true;
				}

				const hasRequiredRoles = requiredRoles.includes(userRoles);
				if (!hasRequiredRoles) {
					return router.navigate(["/home"]);
				}

				return true;
			}
		})
	);
};
