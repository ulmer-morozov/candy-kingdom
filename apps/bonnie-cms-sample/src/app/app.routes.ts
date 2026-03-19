import { inject } from "@angular/core";
import { type ActivatedRouteSnapshot, mapToCanActivate, type Route } from "@angular/router";

import { DataService } from "@candy-kingdom/bonnie-cms";

import { CmsSampleSettingIds } from "./generated";
import { AuthGuard } from "./guard";

export const appRoutes: Route[] = [
	{
		path: "signin",
		loadComponent: () => import("./signin.component"),
	},
	{
		path: "register",
		loadComponent: () => import("./register.component"),
	},
	{
		path: "admin",
		loadComponent: () => import("./admin/admin.component"),
		loadChildren: () => import("./AdminRoutes").then((x) => x.ADMIN_ROUTES),
		canActivateChild: mapToCanActivate([AuthGuard]),
	},
	{
		path: "**",
		loadComponent: () => import("./face/face.component"),
		providers: [DataService],
		resolve: {
			page: (route: ActivatedRouteSnapshot) => inject(DataService).getPage(route.url.join("/")),
			faceView: () => inject(DataService).getView("face"),
			settings: () =>
				inject(DataService).getSettings([
					CmsSampleSettingIds.email,
					CmsSampleSettingIds.company,
					CmsSampleSettingIds.description,
					CmsSampleSettingIds.faviconIco,
					CmsSampleSettingIds.faviconSvg,
					CmsSampleSettingIds.faviconAppleTouch180,
				]),
		},
	},
];
