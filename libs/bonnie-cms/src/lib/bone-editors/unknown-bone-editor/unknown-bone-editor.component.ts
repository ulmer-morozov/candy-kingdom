import { JsonPipe } from "@angular/common";
import { Component } from "@angular/core";

import type { Bone } from "@candy-kingdom/bonnie";

import type { ContentPreset } from "../../skeleton-editor/ContentPreset";
import { BoneEditorBaseComponent } from "../bone-editor-base.component";

@Component({
	selector: "bonc-unknown-bone-editor",

	imports: [JsonPipe],
	templateUrl: "./unknown-bone-editor.component.html",
	styleUrl: "./unknown-bone-editor.component.scss",
})
export class UnknownBoneEditorComponent extends BoneEditorBaseComponent<Bone> {
	public onFinishEditing(): void {
		// no-op for unknown bone type
	}
	public onReset(): void {
		// no-op for unknown bone type
	}

	protected getPresets(): ContentPreset<Bone>[] {
		return [];
	}
}
