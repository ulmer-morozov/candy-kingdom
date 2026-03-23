import { Component, HostBinding } from "@angular/core";

import {
	BoneEditorBaseComponent,
	type ContentPreset,
	createPreset,
	TranslationTextareaComponent,
} from "@candy-kingdom/bonnie-cms";

import { type TextBone, TextBoneStyle } from "../../generated";

@Component({
	selector: "app-text-bone-editor",
	templateUrl: "./text-bone-editor.component.html",
	styleUrls: ["./text-bone-editor.component.scss"],
	imports: [TranslationTextareaComponent],
})
export class TextBoneEditorComponent extends BoneEditorBaseComponent<TextBone> {
	protected getPresets(): ContentPreset<TextBone>[] {
		return [
			createPreset<TextBone>({
				title: "Default",
				style: TextBoneStyle.default,
			}),

			createPreset<TextBone>({
				title: "Decorated",
				style: TextBoneStyle.decorated,
			}),
		];
	}

	@HostBinding("class") get styleFromBone(): string {
		return this.bone().style;
	}

	public onReset(): void {}

	public onFinishEditing(): void {
		// todo: remove?
		// this.data.elements.splice(this.mediaCount, this.data.elements.length - this.mediaCount);
	}
}
