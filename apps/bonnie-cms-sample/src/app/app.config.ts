import {
	ApplicationConfig,
	provideBrowserGlobalErrorListeners,
	provideZonelessChangeDetection,
} from "@angular/core";
import { provideRouter, withComponentInputBinding } from "@angular/router";
import { appRoutes } from "./app.routes";
import { provideClientHydration, withEventReplay } from "@angular/platform-browser";

import { provideLottieOptions } from "ngx-lottie";
import { HTTP_INTERCEPTORS, provideHttpClient } from "@angular/common/http";

import { AuthGuard } from "./guard";
import { AuthService } from "./service";
import { AuthInterceptor } from "./interceptor";

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
