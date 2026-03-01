import { Component, OnInit, input } from "@angular/core";
import { CommonModule } from "@angular/common";
import { AnimationOptions, LottieComponent } from "ngx-lottie";

import { FileMeta, FileSrc, SvgMeta } from "@candy-kingdom/bonnie";

import { FormBaseComponent } from "../../core-components/form-base.component";
import { EditableDirective } from "../../core-components/editable.directive";
import { FormControlsComponent } from "../../form-controls/form-controls.component";
import { FileUploaderComponent } from "../../file-uploader/file-uploader.component";

@Component({
	selector: "bonc-lottie-form",
	standalone: true,
	imports: [CommonModule, FormControlsComponent, FileUploaderComponent, LottieComponent],
	templateUrl: "./lottie-form.component.html",
	styleUrls: ["./lottie-form.component.scss"],
	hostDirectives: [EditableDirective],
})
export class LottieFormComponent extends FormBaseComponent<FileSrc<FileMeta>> implements OnInit {
	public readonly LottieMimeType = "application/json";

	public animOptions?: AnimationOptions;

	public readonly label = input("");

	public readonly uploadMap = input.required<Map<string, string>>();

	public ngOnInit(): void {
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
