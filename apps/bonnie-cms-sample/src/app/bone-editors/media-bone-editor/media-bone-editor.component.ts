import { Component } from "@angular/core";

import {
	BoneEditorBaseComponent,
	type ContentPreset,
	createPreset,
	LinkPopupComponent,
	MediaUploaderComponent,
	TextEditorField,
	TranslationInputComponent,
} from "@candy-kingdom/bonnie-cms";

import { MediaUploadMap } from "../../core";
import { type MediaBone, MediaBoneStyle } from "../../generated";

@Component({
	selector: "app-media-bone-editor",
	templateUrl: "./media-bone-editor.component.html",
	styleUrls: ["./media-bone-editor.component.scss"],
	imports: [TranslationInputComponent, MediaUploaderComponent, LinkPopupComponent],
})
export class MediaBoneEditorComponent extends BoneEditorBaseComponent<MediaBone> {
	public readonly MediaBoneStyle = MediaBoneStyle;
	public readonly MediaUploadMap = MediaUploadMap;

	public LinkPopupField = TextEditorField;

	private currentOpenLinkPopup: LinkPopupComponent | undefined;

	protected getPresets(): ContentPreset<MediaBone>[] {
		return [
			createPreset<MediaBone>({
				title: "Default",
				style: MediaBoneStyle.default,
			}),
			createPreset<MediaBone>({
				title: "Full width",
				style: MediaBoneStyle.fullWidth,
			}),
		];
	}

	public onReset(): void {
		this.closePopUpIfExists();
	}

	public onFinishEditing(): void {
		this.closePopUpIfExists();
	}

	public updateClosed(): void {
		this.currentOpenLinkPopup = undefined;
	}

	public updateOpen(newLinkPopup: LinkPopupComponent): void {
		this.closePopUpIfExists();
		this.currentOpenLinkPopup = newLinkPopup;
	}

	private closePopUpIfExists(): void {
		if (this.currentOpenLinkPopup === undefined) return;

		this.currentOpenLinkPopup.hidePopup();
		this.currentOpenLinkPopup = undefined;
	}

	public onUpdated() {}
}
