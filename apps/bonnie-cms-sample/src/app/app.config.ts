import { HTTP_INTERCEPTORS, provideHttpClient } from "@angular/common/http";
import {
	type ApplicationConfig,
	provideBrowserGlobalErrorListeners,
	provideZonelessChangeDetection,
} from "@angular/core";
import { provideClientHydration, withEventReplay } from "@angular/platform-browser";
import { provideRouter, withComponentInputBinding } from "@angular/router";

import { provideLottieOptions } from "ngx-lottie";

import { appRoutes } from "./app.routes";
import { AuthGuard } from "./guard";
import { AuthInterceptor } from "./interceptor";
import { AuthService } from "./service";

export const appConfig: ApplicationConfig = {
	providers: [
		provideZonelessChangeDetection(),
		provideHttpClient(),
		provideClientHydration(withEventReplay()),
		provideBrowserGlobalErrorListeners(),
		provideRouter(appRoutes, withComponentInputBinding()),

		{
			provide: HTTP_INTERCEPTORS,
			useClass: AuthInterceptor,
			multi: true,
		},

		provideLottieOptions({
			player: () => import("lottie-web"),
		}),

		AuthGuard,
		AuthService,
	],
};
