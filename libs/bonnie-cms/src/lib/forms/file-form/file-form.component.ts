import { Component, input } from "@angular/core";

import type { FileMeta, FileSrc, SvgMeta } from "@candy-kingdom/bonnie";

import { EditableDirective } from "../../core-components";
import { FormBaseComponent } from "../../core-components/form-base.component";
import { FileUploaderComponent } from "../../file-uploader/file-uploader.component";
import { FormControlsComponent } from "../../form-controls/form-controls.component";

@Component({
	selector: "bonc-file-form",

	imports: [FormControlsComponent, FileUploaderComponent],
	templateUrl: "./file-form.component.html",
	styleUrl: "./file-form.component.scss",
	hostDirectives: [EditableDirective],
})
export class FileFormComponent extends FormBaseComponent<FileSrc<FileMeta>> {
	public readonly label = input("");

	public readonly uploadTypes = input.required<string[]>();

	public readonly uploadMap = input.required<Map<string, string>>();

	constructor() {
		super();

		this.editable.externalSaveCall.subscribe(() => {
			this.editable.save();
		});
	}

	public onFileUploaded(fileSrc: FileSrc<FileMeta>): void {
		this.editable.startEditing();

		const svgSrc = fileSrc as FileSrc<SvgMeta>;

		this.editable.value = svgSrc;

		this.editable.updateDirty();
	}
}
