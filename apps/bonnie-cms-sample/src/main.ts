import { APP_BASE_HREF } from "@angular/common";
import { type ApplicationConfig, mergeApplicationConfig } from "@angular/core";
import { bootstrapApplication } from "@angular/platform-browser";

import { DeviceService, DeviceServiceBase } from "@candy-kingdom/bonnie";
import { API_BASE_URL } from "@candy-kingdom/bonnie-cms";

import { App } from "./app/app";
import { appConfig } from "./app/app.config";

function getBaseHref() {
	return document.getElementsByTagName("base")[0].href;
}

const browserConfig: ApplicationConfig = {
	providers: [
		{ provide: APP_BASE_HREF, useFactory: getBaseHref, deps: [] },
		{ provide: API_BASE_URL, useFactory: getBaseHref, deps: [] },
		{ provide: DeviceServiceBase, useClass: DeviceService },
	],
};

export const config = mergeApplicationConfig(appConfig, browserConfig);

bootstrapApplication(App, config).catch((err) => console.error(err));
