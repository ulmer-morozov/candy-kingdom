import { Component, computed, OnInit, input } from "@angular/core";
import { CommonModule } from "@angular/common";

import { FileMeta, FileSrc, ImageMeta } from "@candy-kingdom/bonnie";

import { FormBaseComponent } from "../../core-components/form-base.component";
import { EditableDirective } from "../../core-components/editable.directive";
import { FormControlsComponent } from "../../form-controls/form-controls.component";
import { FileUploaderComponent } from "../../file-uploader/file-uploader.component";

const DefaultImageMimeTypes = ["image/png", "image/jpeg"];

@Component({
	selector: "bonc-one-image-form",
	standalone: true,
	imports: [CommonModule, FormControlsComponent, FileUploaderComponent],
	templateUrl: "./one-image-form.component.html",
	styleUrls: ["./one-image-form.component.scss"],
	hostDirectives: [EditableDirective],
})
export class OneImageFormComponent extends FormBaseComponent<FileSrc<ImageMeta>> implements OnInit {
	public readonly label = input("");

	public readonly uploadUrl = input("/api/admin/upload/image");

	public readonly mimeTypes = input(DefaultImageMimeTypes);

	public readonly uploadMap = computed(() => {
		const m = new Map<string, string>();
		m.set("", this.uploadUrl());
		return m;
	});

	public ngOnInit(): void {
		this.editable.externalSaveCall.subscribe(() => {
			this.editable.save();
		});
	}

	public onFileUploaded(fileSrc: FileSrc<FileMeta>): void {
		this.editable.startEditing();

		const svgSrc = fileSrc as FileSrc<ImageMeta>;

		this.editable.value = svgSrc;

		this.editable.updateDirty();
	}
}
