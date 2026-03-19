import { CommonModule } from "@angular/common";
import { Component, effect, inject, ViewEncapsulation } from "@angular/core";
import { Router, RouterModule } from "@angular/router";

import { LocalizeServiceBase } from "@candy-kingdom/bonnie";

import { AuthGuard } from "../guard";
import { RouterLocalizeService } from "../router-localize.service";
import { AuthService } from "../service";

@Component({
	imports: [CommonModule, RouterModule],
	providers: [
		AuthGuard,
		AuthService,
		{ provide: LocalizeServiceBase, useClass: RouterLocalizeService },
	],
	selector: "app-admin",
	templateUrl: "./admin.component.html",
	styleUrl: "./admin.component.scss",
	encapsulation: ViewEncapsulation.None,
	styles: `
    :host{
      --bg-color: white;
      --text-color: black;
    }
  `,
})
export default class AdminComponent {
	public isSignedIn: boolean = false;

	private readonly auth = inject(AuthService);
	private readonly router = inject(Router);

	constructor() {
		effect(() => {
			this.isSignedIn = this.auth.authState();
		});
	}

	signOut() {
		if (this.isSignedIn) {
			this.auth.signOut().forEach((response) => {
				if (response) {
					this.router.navigateByUrl("");
				}
			});
		}
	}
}
