import { RenderMode, type ServerRoute } from "@angular/ssr";

export const serverRoutes: ServerRoute[] = [
	{
		path: "signin",
		renderMode: RenderMode.Client, // todo: change to Server when fixing
	},
	{
		path: "register",
		renderMode: RenderMode.Client, // todo: change to Server when fixing
	},
	{
		path: "admin/**",
		renderMode: RenderMode.Client, // todo: change to Server when fixing
	},
	{
		path: "**",
		renderMode: RenderMode.Client, // todo: change to Server when fixing
	},
];
