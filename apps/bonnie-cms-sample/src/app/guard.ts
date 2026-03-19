import { Injectable, inject } from "@angular/core";
import { Router } from "@angular/router";

import { map, type Observable } from "rxjs";

import { AuthService } from "./service";

@Injectable({ providedIn: "root" })
// protects routes from unauthenticated users
export class AuthGuard {
	private readonly authService = inject(AuthService);
	private readonly router = inject(Router);

	canActivate() {
		return this.isSignedIn();
	}

	isSignedIn(): Observable<boolean> {
		return this.authService.isSignedIn().pipe(
			map((isSignedIn) => {
				if (!isSignedIn) {
					// redirect to signin page
					this.router.navigate(["signin"]);
					return false;
				}
				return true;
			}),
		);
	}
}
