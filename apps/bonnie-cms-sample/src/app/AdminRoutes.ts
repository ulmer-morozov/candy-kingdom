import { Route, ActivatedRouteSnapshot } from "@angular/router";
import { inject } from "@angular/core";

import { AdminDataService } from "@candy-kingdom/bonnie-cms";

export const ADMIN_ROUTES: Route[] = [
	{
		path: "",
		pathMatch: "full",
		loadComponent: () => import("./admin-home/admin-home.component"),
	},
	{
		path: "settings",
		loadComponent: () => import("./admin-settings/admin-settings.component"),
	},
	{
		path: "pages/:pageUrl",
		loadComponent: () => import("./admin-pages/admin-pages.component"),
		providers: [AdminDataService],
		resolve: {
			page: (route: ActivatedRouteSnapshot) =>
				inject(AdminDataService).getPage(route.paramMap.get("pageUrl") ?? "~"),
		},
	},
];
