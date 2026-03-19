import { CommonModule } from "@angular/common";
import { Component, effect, inject } from "@angular/core";

import { AuthService } from "../service";

@Component({
	imports: [CommonModule],
	selector: "app-admin-home",
	templateUrl: "./admin-home.component.html",
})
export default class AdminHomeComponent {
	public isSignedIn: boolean = false;

	private readonly authService = inject(AuthService);

	constructor() {
		effect(() => {
			this.isSignedIn = this.authService.authState();
		});
	}
}
