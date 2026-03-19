import {
	HttpErrorResponse,
	type HttpEvent,
	type HttpHandler,
	type HttpInterceptor,
	type HttpRequest,
} from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { Router } from "@angular/router";

import { type Observable, tap } from "rxjs";

// this will intercept all http requests and redirect to signin if the user is not authenticated and
// is trying to access a protected route
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
	private readonly router = inject(Router);

	intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
		return next.handle(req).pipe(
			tap({
				next: (event: HttpEvent<unknown>) => {
					if (event instanceof HttpErrorResponse) {
						if (event.status !== 401 || (event.url && event.url.indexOf("api/manage/info") >= 0)) {
							return;
						}

						this.router.navigate(["signin"]);
					}
				},
			}),
		);
	}
}
