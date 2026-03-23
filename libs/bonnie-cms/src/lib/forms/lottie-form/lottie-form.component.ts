import { Component, input } from "@angular/core";

import { type AnimationOptions, LottieComponent } from "ngx-lottie";

import type { FileMeta, FileSrc, SvgMeta } from "@candy-kingdom/bonnie";

import { EditableDirective } from "../../core-components/editable.directive";
import { FormBaseComponent } from "../../core-components/form-base.component";
import { FileUploaderComponent } from "../../file-uploader/file-uploader.component";
import { FormControlsComponent } from "../../form-controls/form-controls.component";

@Component({
	selector: "bonc-lottie-form",

	imports: [FormControlsComponent, FileUploaderComponent, LottieComponent],
	templateUrl: "./lottie-form.component.html",
	styleUrl: "./lottie-form.component.scss",
	hostDirectives: [EditableDirective],
})
export class LottieFormComponent extends FormBaseComponent<FileSrc<FileMeta>> {
	public readonly LottieMimeType = "application/json";

	// todo: convert to signal
	public animOptions?: AnimationOptions;

	public readonly label = input("");

	public readonly uploadMap = input.required<Map<string, string>>();

	constructor() {
		super();

		this.editable.externalSaveCall.subscribe(() => {
			this.editable.save();
		});

		this.editable.valueChange.subscribe((x) => {
			const url = x?.url ?? "";
			this.animOptions = url.length === 0 ? undefined : { path: url };
		});
	}

	public onFileUploaded(fileSrc: FileSrc<FileMeta>): void {
		this.editable.startEditing();

		const svgSrc = fileSrc as FileSrc<SvgMeta>;

		this.editable.value = svgSrc;

		this.editable.updateDirty();
	}
}
