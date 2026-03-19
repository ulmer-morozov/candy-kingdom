import { CommonModule } from "@angular/common";
import { Component, HostBinding, inject } from "@angular/core";

import {
	BoneDirective,
	LocalizationIsNotEmptyPipe,
	LocalizePipe,
	MarcyMediaComponent,
} from "@candy-kingdom/bonnie";

import { type MediaBone, MediaBoneStyle } from "../../generated";

@Component({
	selector: "app-media-bone",

	imports: [CommonModule, MarcyMediaComponent, LocalizePipe, LocalizationIsNotEmptyPipe],
	templateUrl: "./media-bone.component.html",
	styleUrls: ["./media-bone.component.scss"],
	hostDirectives: [{ directive: BoneDirective, inputs: ["bone"], outputs: ["boneChange"] }],
})
export class MediaBoneComponent {
	public readonly MediaBoneStyle = MediaBoneStyle;

	public readonly bd = inject(BoneDirective<MediaBone>, { host: true });

	@HostBinding("class")
	public get hostStyle(): string {
		return this.bd.bone().style;
	}
}
