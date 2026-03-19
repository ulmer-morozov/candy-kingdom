import { CdkTextareaAutosize } from "@angular/cdk/text-field";
import { Component, effect, input, output, viewChildren } from "@angular/core";
import { FormsModule } from "@angular/forms";

import type { LocalizedString } from "@candy-kingdom/bonnie";

import { DeviceType } from "../core";

@Component({
	selector: "bonc-translation-textarea",

	imports: [FormsModule, CdkTextareaAutosize],
	templateUrl: "./translation-textarea.component.html",
	styleUrl: "./translation-textarea.component.scss",
})
export class TranslationTextareaComponent {
	public readonly autosizeList = viewChildren(CdkTextareaAutosize);

	public readonly text = input.required<LocalizedString>();
	public readonly locale = input.required<string>();

	public readonly minRows = input<number>();
	public readonly maxRows = input<number>();
	public readonly device = input<DeviceType>(DeviceType.NotSet);

	public readonly startEditing = output<void>();
	public readonly changed = output<void>();
	public readonly blurred = output<void>();

	constructor() {
		// todo: check if it still necessary
		effect((onCleanup) => {
			this.text();
			this.locale();
			this.minRows();
			this.maxRows();
			const timer = setTimeout(() => this.triggerResize(), 500);

			onCleanup(() => {
				clearTimeout(timer);
			});
		});
	}

	public onClick() {
		this.startEditing.emit();
	}

	public onKeyPress() {
		this.changed.emit();
	}

	public onBlur() {
		this.blurred.emit();
	}

	private triggerResize() {
		this.autosizeList().forEach((x) => x.resizeToFitContent(true));
	}
}
