import { ApplicationConfig } from '@angular/core';
import { Router, provideRouter, withComponentInputBinding } from '@angular/router';
import { APP_Routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, provideHttpClient } from '@angular/common/http';
import { AuthInterceptor } from './interceptor';
import { AuthGuard } from './guard';
import { AuthService } from './service';

import { provideLottieOptions } from 'ngx-lottie';

export const appConfig: ApplicationConfig = {
  providers: [
    provideClientHydration(),
    provideHttpClient(),
    provideRouter(APP_Routes, withComponentInputBinding()),
    {
      provide: HTTP_INTERCEPTORS,
      useFactory: (router: Router) => {
        return new AuthInterceptor(router);
      },
      multi: true,
      deps: [Router],
    },

    provideLottieOptions({
      player: () => import('lottie-web'),
    }),

    AuthGuard,
    AuthService,
  ],
};
