import { ApplicationConfig } from '@angular/core';
import { Router, provideRouter, withComponentInputBinding } from '@angular/router';
import { APP_Routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, provideHttpClient } from '@angular/common/http';
import { AuthInterceptor } from './interceptor';
import { AuthGuard } from './guard';
import { AuthService } from './service';
import { APP_BASE_HREF } from '@angular/common';

function getBaseHref() {
  return document.getElementsByTagName('base')[0].href;
}

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

    { provide: APP_BASE_HREF, useFactory: getBaseHref, deps: [] },

    AuthGuard,
    AuthService,
  ],
};
