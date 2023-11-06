import { Injectable } from '@angular/core';
import jwtDecode from 'jwt-decode';
import { CookieService } from 'ngx-cookie-service';
import { BehaviorSubject, Observable, catchError, of, switchMap } from 'rxjs';
import { AuthUseCaseInterface } from 'src/app/domain/interfaces/use-cases/auth-use-case.interface';
import { BearerTokenViewModel } from 'src/app/domain/view-models/bearer-token.view-model';
import { UserSessionViewModel } from 'src/app/domain/view-models/user-session.view-model';

const SESSION_TOKEN = "session_token";
const REFRESH_TOKEN = "refresh_token";
const USER_INFO = "user_info";

@Injectable({
	providedIn: 'root'
})
export class AuthService {
	public userInfoSubject: BehaviorSubject<UserSessionViewModel | null> = new BehaviorSubject<UserSessionViewModel | null>(null);

	private _accessToken: string | null = null;
	private _refreshToken: string | null = null;
	private _userInfo: UserSessionViewModel | null = null;

	constructor(
		private cookieService: CookieService,
		private authUseCase: AuthUseCaseInterface
	) {
		const accessToken = this.cookieService.get(SESSION_TOKEN);
		const refreshToken = this.cookieService.get(REFRESH_TOKEN);
		const userInfo = this.cookieService.get(USER_INFO);

		if (accessToken) this._accessToken = accessToken;
		if (refreshToken) this._refreshToken = refreshToken;
		if (userInfo) this._userInfo = JSON.parse(userInfo);
	}

	isAuthenticated(): Observable<boolean> {
		if (!this.accessToken)
			return of(false);

		if (!this.refreshToken)
			return of(false);

		return this.authUseCase.refreshToken(this.refreshToken).pipe(
			switchMap((token: BearerTokenViewModel) => {
				this.setSession(token);
				return of(true);
			}),
			catchError(() => of(false))
		);
	}

	get accessToken(): string | null {
		return this._accessToken;
	}

	get refreshToken(): string | null {
		return this._refreshToken;
	}

	get userInfo(): UserSessionViewModel | null {
		return this._userInfo;
	}

	setSession(token: BearerTokenViewModel): void {
		const payload: UserSessionViewModel = jwtDecode(token?.accessToken!);
		this._accessToken = token.accessToken;
		this._refreshToken = token.refreshToken;
		this._userInfo = payload;
		this.userInfoSubject.next(payload);

		if (token) {
			this.setRefreshTokenCookie(token.refreshToken);
			this.setSessionTokenCookie(token.accessToken, token.expiresAt);
			this.setUserInfoCookie(payload, token.expiresAt);
		} else {
			this.cookieService.delete(SESSION_TOKEN);
			this.cookieService.delete(REFRESH_TOKEN);
			this.cookieService.delete(USER_INFO);
		}
	}

	removeSession(): void {
		this._accessToken = null;
		this._refreshToken = null;
		this._userInfo = null;

		this.cookieService.delete(SESSION_TOKEN);
		this.cookieService.delete(REFRESH_TOKEN);
		this.cookieService.delete(USER_INFO);
	}

	private setRefreshTokenCookie(refreshToken: string): void {
		const expirationDate: Date = new Date();
		expirationDate.setHours(expirationDate.getHours() + 1);
		this.cookieService.set(REFRESH_TOKEN, refreshToken, { path: "/", expires: expirationDate, sameSite: "Strict", secure: true });
	}

	private setSessionTokenCookie(sessionToken: string, expiresAt: string): void {
		const expirationDate: Date = new Date(expiresAt);
		this.cookieService.set(SESSION_TOKEN, sessionToken, { path: "/", expires: expirationDate, sameSite: "Strict", secure: true });
	}

	private setUserInfoCookie(payload: UserSessionViewModel, expiresAt: string): void {
		const expirationDate: Date = new Date(expiresAt);
		this.cookieService.set(USER_INFO, JSON.stringify(payload), { path: "/", expires: expirationDate, sameSite: "Strict", secure: true });
	}
}
