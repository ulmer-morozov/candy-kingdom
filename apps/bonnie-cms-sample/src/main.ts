import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { ApplicationConfig, mergeApplicationConfig } from '@angular/core';
import { APP_BASE_HREF } from '@angular/common';
import { API_BASE_URL } from '@candy-kingdom/bonnie-cms';

function getBaseHref() {
  return document.getElementsByTagName('base')[0].href;
}

const browserConfig: ApplicationConfig = {
  providers: [
    { provide: APP_BASE_HREF, useFactory: getBaseHref, deps: [] },
    { provide: API_BASE_URL, useFactory: getBaseHref, deps: [] },
  ],
};

export const config = mergeApplicationConfig(appConfig, browserConfig);

bootstrapApplication(AppComponent, config).catch((err) =>
  console.error(err)
);
