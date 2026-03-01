import { CommonModule } from "@angular/common";
import { Component, input } from "@angular/core";

import { DeviceType } from "../core";
import type { EditableGroupComponent } from "../core-components";

@Component({
	selector: "bonc-admin-controls",
	standalone: true,
	imports: [CommonModule],
	templateUrl: "./admin-controls.component.html",
	styleUrls: ["./admin-controls.component.scss"],
})
export class AdminControlsComponent {
	public readonly DeviceType = DeviceType;

	public readonly editableGroup = input.required<EditableGroupComponent>();

	public readonly deviceControls = input(false);

	public locale = "en";
	public device = this.DeviceType.Desktop;

	public changeLocale(): void {
		this.locale = this.locale === "en" ? "ru" : "en";
	}

	public changeDevice(): void {
		switch (this.device) {
			case DeviceType.Desktop:
				this.device = DeviceType.Tablet;
				return;

			case DeviceType.Tablet:
				this.device = DeviceType.Mobile;
				return;

			case DeviceType.Mobile:
				this.device = DeviceType.Desktop;
				return;

			default:
				this.device = DeviceType.Desktop;
				return;
		}
	}
}
