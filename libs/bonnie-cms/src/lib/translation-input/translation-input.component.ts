import { Component, input, output } from "@angular/core";
import { FormsModule } from "@angular/forms";

import type { LocalizedString } from "@candy-kingdom/bonnie";

import { DeviceType } from "../core";

@Component({
	selector: "bonc-translation-input",

	imports: [FormsModule],
	templateUrl: "./translation-input.component.html",
	styleUrl: "./translation-input.component.scss",
})
export class TranslationInputComponent {
	public readonly text = input.required<LocalizedString>();

	public readonly locale = input.required<string>();

	public readonly device = input<DeviceType>(DeviceType.NotSet);

	public readonly startEditing = output<void>();

	public readonly changed = output<void>();

	public readonly blurred = output<void>();

	public onClick() {
		this.startEditing.emit();
	}

	public onKeyPress() {
		this.changed.emit();
	}

	public onBlur() {
		this.blurred.emit();
	}
}
