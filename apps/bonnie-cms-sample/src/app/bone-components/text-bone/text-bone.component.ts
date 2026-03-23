import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";

import { BoneDirective, LocalizationIsNotEmptyPipe, LocalizePipe } from "@candy-kingdom/bonnie";

import type { TextBone } from "../../generated";

@Component({
	selector: "app-text-bone",

	imports: [CommonModule, LocalizePipe, LocalizationIsNotEmptyPipe],
	templateUrl: "./text-bone.component.html",
	styleUrls: ["./text-bone.component.scss"],
	hostDirectives: [{ directive: BoneDirective, inputs: ["bone"], outputs: ["boneChange"] }],
})
export class TextBoneComponent {
	public readonly bd = inject(BoneDirective<TextBone>, { host: true });
}
