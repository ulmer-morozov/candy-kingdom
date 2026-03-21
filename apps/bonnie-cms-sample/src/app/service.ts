import { HttpClient, type HttpErrorResponse, type HttpResponse } from "@angular/common/http";
import { Injectable, inject, signal } from "@angular/core";

import { catchError, map, type Observable, of } from "rxjs";

import type { UserInfo } from "./dto";

@Injectable({
	providedIn: "root",
})
export class AuthService {
	private readonly http = inject(HttpClient);

	private readonly _authState = signal<boolean>(false);
	public readonly authState = this._authState.asReadonly();

	// cookie-based login
	public signIn(email: string, password: string) {
		return this.http
			.post(
				"/api/login?useCookies=true",
				{
					email,
					password,
				},
				{
					observe: "response",
					responseType: "text",
				},
			)
			.pipe<boolean>(
				map((res: HttpResponse<string>) => {
					this._authState.set(res.ok);
					return res.ok;
				}),
			);
	}

	// register new user
	public register(email: string, password: string) {
		return this.http
			.post(
				"/api/register",
				{
					email,
					password,
				},
				{
					observe: "response",
					responseType: "text",
				},
			)
			.pipe<boolean>(map((res: HttpResponse<string>) => res.ok));
	}

	// sign out
	public signOut() {
		return this.http
			.post(
				"/api/logout",
				{},
				{
					withCredentials: true,
					observe: "response",
					responseType: "text",
				},
			)
			.pipe<boolean>(
				map((res: HttpResponse<string>) => {
					if (res.ok) {
						this._authState.set(false);
					}
					return res.ok;
				}),
			);
	}

	// check if the user is authenticated. the endpoint is protected so 401 if not.
	public user() {
		return this.http
			.get<UserInfo>("/api/manage/info", {
				withCredentials: true,
			})
			.pipe(catchError((_: HttpErrorResponse, __: Observable<UserInfo>) => of({} as UserInfo)));
	}

	// is signed in when the call completes without error and the user has an email
	public isSignedIn(): Observable<boolean> {
		return this.user().pipe(
			map((userInfo) => {
				const valid = !!(userInfo && userInfo.email && userInfo.email.length > 0);
				return valid;
			}),
			catchError((_) => of(false)),
		);
	}
}
