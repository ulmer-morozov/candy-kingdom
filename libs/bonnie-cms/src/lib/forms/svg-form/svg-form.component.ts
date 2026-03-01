import { Component, computed, OnInit, input } from "@angular/core";
import { CommonModule } from "@angular/common";

import { FileMeta, FileSrc, SvgMeta } from "@candy-kingdom/bonnie";

import { FormBaseComponent } from "../../core-components/form-base.component";
import { EditableDirective } from "../../core-components/editable.directive";
import { FormControlsComponent } from "../../form-controls/form-controls.component";
import { FileUploaderComponent } from "../../file-uploader/file-uploader.component";

@Component({
	selector: "bonc-svg-form",
	standalone: true,
	imports: [CommonModule, FormControlsComponent, FileUploaderComponent],
	templateUrl: "./svg-form.component.html",
	styleUrls: ["./svg-form.component.scss"],
	hostDirectives: [EditableDirective],
})
export class SvgFormComponent extends FormBaseComponent<FileSrc<SvgMeta>> implements OnInit {
	public readonly SvgMime = "image/svg+xml";

	public readonly label = input("");

	public readonly uploadUrl = input("/api/admin/upload/image/svg");

	public readonly uploadMap = computed(() => {
		const m = new Map<string, string>();
		m.set(this.SvgMime, this.uploadUrl());
		return m;
	});

	public ngOnInit(): void {
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
