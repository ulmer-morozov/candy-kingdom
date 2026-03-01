import { CommonModule } from "@angular/common";
import { Component, input, output } from "@angular/core";

import { MediaObjectFit } from "../core/MediaObjectFit";
import type * as M_CORE from "../generated";
import { ImageSrcDirective } from "../marcy-image";
import { MarcyImageComponent } from "../marcy-image/marcy-image.component";
import { VideoSrcDirective } from "../marcy-video";
import { MarcyVideoComponent } from "../marcy-video/marcy-video.component";

@Component({
	selector: "bon-media",
	standalone: true,
	imports: [
		CommonModule,
		MarcyImageComponent,
		MarcyVideoComponent,
		VideoSrcDirective,
		ImageSrcDirective,
	],
	templateUrl: "./marcy-media.component.html",
	styleUrls: ["./marcy-media.component.scss"],
})
export class MarcyMediaComponent {
	public readonly MarcyObjectFit = MediaObjectFit;

	public readonly isLoaded = output<void>();

	public readonly src = input<M_CORE.Video | M_CORE.Image | undefined>();

	public readonly objectFit = input<MediaObjectFit>(MediaObjectFit.Original);

	public onLoad() {
		this.isLoaded.emit();
	}
}
