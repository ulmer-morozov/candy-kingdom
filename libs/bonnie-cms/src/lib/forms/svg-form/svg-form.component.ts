import { Component, computed, input } from "@angular/core";

import type { FileMeta, FileSrc, SvgMeta } from "@candy-kingdom/bonnie";

import { EditableDirective } from "../../core-components/editable.directive";
import { FormBaseComponent } from "../../core-components/form-base.component";
import { FileUploaderComponent } from "../../file-uploader/file-uploader.component";
import { FormControlsComponent } from "../../form-controls/form-controls.component";

@Component({
	selector: "bonc-svg-form",

	imports: [FormControlsComponent, FileUploaderComponent],
	templateUrl: "./svg-form.component.html",
	styleUrl: "./svg-form.component.scss",
	hostDirectives: [EditableDirective],
})
export class SvgFormComponent extends FormBaseComponent<FileSrc<SvgMeta>> {
	public readonly SvgMime = "image/svg+xml";

	public readonly label = input("");

	public readonly uploadUrl = input("/api/admin/upload/image/svg");

	public readonly uploadMap = computed(() => {
		const m = new Map<string, string>();
		m.set(this.SvgMime, this.uploadUrl());
		return m;
	});

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
