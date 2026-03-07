import { Component, computed, DestroyRef, inject, input } from "@angular/core";

import type { FileMeta, FileSrc, ImageMeta } from "@candy-kingdom/bonnie";

import { EditableDirective } from "../../core-components/editable.directive";
import { FormBaseComponent } from "../../core-components/form-base.component";
import { FileUploaderComponent } from "../../file-uploader/file-uploader.component";
import { FormControlsComponent } from "../../form-controls/form-controls.component";

const DefaultImageMimeTypes = ["image/png", "image/jpeg"];

@Component({
	selector: "bonc-one-image-form",

	imports: [FormControlsComponent, FileUploaderComponent],
	templateUrl: "./one-image-form.component.html",
	styleUrl: "./one-image-form.component.scss",
	hostDirectives: [EditableDirective],
})
export class OneImageFormComponent extends FormBaseComponent<FileSrc<ImageMeta>> {
	public readonly label = input("");

	public readonly uploadUrl = input("/api/admin/upload/image");

	public readonly mimeTypes = input(DefaultImageMimeTypes);

	public readonly uploadMap = computed(() => {
		const m = new Map<string, string>();
		m.set("", this.uploadUrl());
		return m;
	});

	private readonly destroyRef = inject(DestroyRef);

	constructor() {
		super();

		const sub = this.editable.externalSaveCall.subscribe(() => {
			this.editable.save();
		});
		this.destroyRef.onDestroy(() => sub.unsubscribe());
	}

	public onFileUploaded(fileSrc: FileSrc<FileMeta>): void {
		this.editable.startEditing();

		const svgSrc = fileSrc as FileSrc<ImageMeta>;

		this.editable.value = svgSrc;

		this.editable.updateDirty();
	}
}
