import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './auth.service';
import { AuthRepositoryService } from 'src/app/data/repositories/auth-repository.service';
import { BearerTokenViewModel } from 'src/app/domain/view-models/bearer-token.view-model';

export const authGuard: CanActivateFn = (route, state) => {
	const router = inject(Router);
	const authService = inject(AuthService);
	const authRepository = inject(AuthRepositoryService);

	if (authService.accessToken) {
		const requiredRoles: string[] = route.data["roles"];
		const userRoles: string = authService.userInfo!.role;

		if (!requiredRoles) {
			return true;
		}

		const hasRequiredRoles = requiredRoles.includes(userRoles);
		if (!hasRequiredRoles) {
			return router.navigate(["/home"]);
		}
	}

	if (authService.accessToken == null) {
		const refreshToken = authService.refreshToken;

		if (refreshToken == null) {
			router.navigate(["/login"], { queryParams: { returnUrl: state.url } });
			return false;
		}

		return new Promise((resolve, reject) => {
			authRepository.refreshToken(refreshToken).subscribe({
				next: (response: BearerTokenViewModel) => {
					authService.setSession(response);
					resolve(true);
				},
				error: () => {
					router.navigate(["/login"], { queryParams: { returnUrl: state.url } });
					reject(false);
				}
			})
		});
	}

	return true;
};
