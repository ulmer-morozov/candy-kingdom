import {
	type AfterViewInit,
	Component,
	effect,
	input,
	output,
	type QueryList,
	ViewChildren,
} from "@angular/core";
import { FormsModule } from "@angular/forms";
import { CdkTextareaAutosize } from "@angular/cdk/text-field";

import type { LocalizedString } from "@candy-kingdom/bonnie";
import { DeviceType } from "../core";

@Component({
	selector: "bonc-translation-textarea",
	standalone: true,
	imports: [FormsModule, CdkTextareaAutosize],
	templateUrl: "./translation-textarea.component.html",
	styleUrls: ["./translation-textarea.component.scss"],
})
export class TranslationTextareaComponent implements AfterViewInit {
	@ViewChildren(CdkTextareaAutosize)
	public autosizeList!: QueryList<CdkTextareaAutosize>;

	public readonly minRows = input<number>();

	public readonly maxRows = input<number>();

	public readonly text = input.required<LocalizedString>();

	public readonly locale = input.required<string>();

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
			const timer = setTimeout(this.triggerResize.bind(this), 500);

			onCleanup(() => {
				clearTimeout(timer);
			});
		});
	}

	ngAfterViewInit(): void {
		setTimeout(this.triggerResize.bind(this));
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
		// console.log('trigger resize!');
		// todo: investigate is it working or not
		// this.ngZone.onStable.pipe(take(1))
		//   .subscribe(() => {
		//     this.autosizeList.forEach(x => x.resizeToFitContent(true));
		//   });
	}
}
