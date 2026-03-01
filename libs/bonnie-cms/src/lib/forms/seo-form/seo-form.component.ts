import { Component, OnInit, input, computed } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FileMeta, FileSrc, ImageMeta, OpenGraphData } from "@candy-kingdom/bonnie";

import { EditableDirective, FormBaseComponent } from "../../core-components";
import { FormControlsComponent } from "../../form-controls/form-controls.component";
import { TranslationInputComponent } from "../../translation-input/translation-input.component";
import { TranslationTextareaComponent } from "../../translation-textarea/translation-textarea.component";
import { FileUploaderComponent } from "../../file-uploader/file-uploader.component";

const defaultUploadMap = new Map<string, string>();
defaultUploadMap.set("", `/api/admin/upload/image?width=${1200}&height=${630}&format=image/jpeg`);

@Component({
	selector: "bonc-seo-form",
	standalone: true,
	imports: [
		CommonModule,
		FormControlsComponent,
		TranslationInputComponent,
		TranslationTextareaComponent,
		FileUploaderComponent,
	],
	templateUrl: "./seo-form.component.html",
	styleUrls: ["./seo-form.component.scss"],
	hostDirectives: [EditableDirective],
})
export class SeoFormComponent extends FormBaseComponent<OpenGraphData> implements OnInit {
	public readonly uploadMap = defaultUploadMap;

	public readonly label = input("");

	public readonly pageId = input("");

	public readonly ogImageUploadUrl = computed(
		() => `/api/admin/page/Og-Image?pageId=${this.pageId()}`,
	);

	public ngOnInit(): void {
		this.editable.externalSaveCall.subscribe(() => {
			this.editable.save();
		});
	}

	public ResToSrc(res: { url: string }): string {
		return res.url;
	}

	public replaceImage($event: FileSrc<FileMeta>): void {
		this.editable.startEditing();

		if (this.editable.value !== undefined) {
			this.editable.value.image[this.locale()] = $event as FileSrc<ImageMeta>;
		}

		this.editable.updateDirty();
	}
}
