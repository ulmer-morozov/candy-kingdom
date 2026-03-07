import { NgTemplateOutlet } from "@angular/common";
import {
	Component,
	type ElementRef,
	effect,
	inject,
	input,
	output,
	signal,
	viewChild,
} from "@angular/core";

import { DeviceServiceBase } from "../core/device.service.base";
import { IntersectionComponent } from "../core/intersection.component";
import { MediaObjectFit } from "../core/MediaObjectFit";
import { MediaStatus } from "../core/MediaStatus";
import { descendingT, matchesMediaQuery } from "../core/utils";
import type * as M_CORE from "../generated";
import { VideoSrcDirective } from "./vidsrc.directive";

function isWebM(src: M_CORE.FileSrc<M_CORE.ImageMeta>): boolean {
	return src.mimeType === "video/webm";
}

@Component({
	selector: "bon-video",

	imports: [NgTemplateOutlet, IntersectionComponent],
	templateUrl: "./marcy-video.component.html",
	styleUrl: "./marcy-video.component.scss",
})
export class MarcyVideoComponent {
	public readonly MediaStatus = MediaStatus;
	public readonly MarcyObjectFit = MediaObjectFit;

	public readonly videoRef = viewChild<ElementRef<HTMLVideoElement>>("video");

	public readonly isLoaded = output<void>();

	public readonly $status = signal<MediaStatus>(MediaStatus.NotSet);
	public readonly src: VideoSrcDirective;

	public readonly source = signal<M_CORE.FileSrc<M_CORE.VideoMeta> | undefined>(undefined);

	public readonly objectFit = input<MediaObjectFit>(MediaObjectFit.Original);

	public readonly device = inject(DeviceServiceBase);
	private readonly _srcDir = inject(VideoSrcDirective, { optional: true });

	constructor() {
		if (this._srcDir === undefined || this._srcDir === null)
			throw new Error(
				`${MarcyVideoComponent.name} should have [vidsrc] directive as source object`,
			);

		this.src = this._srcDir;

		effect(() => {
			this.src.data();
			this.videoRef();
			this.updateSources();
			this.subscribeToMediaQueryChange();
		});

		effect(() => {
			if (this.$status() === MediaStatus.Loaded) {
				this.isLoaded.emit();
			}
		});
	}

	private subscribeToMediaQueryChange(): void {
		this.src.watchMediaQueries().subscribe(() => {
			console.log("MarcyVideoComponent watchMediaQueries");
			this.updateSources();
		});
	}

	private updateSources() {
		console.log("MarcyVideoComponent updateSources");

		this.source.set(this.findMoreSuitableSource());

		console.log("MarcyVideoComponent new source", this.source());

		const src = this.source();

		if (this.$status() === MediaStatus.NotSet && src === undefined) {
			return;
		}

		if (src === undefined) {
			this.$status.set(MediaStatus.NotSet);
			return;
		}

		this.$status.set(MediaStatus.NotLoaded);
	}

	public onLoad() {
		this.$status.set(MediaStatus.Loaded);
	}

	private findMoreSuitableSource(): M_CORE.FileSrc<M_CORE.VideoMeta> | undefined {
		const ref = this.videoRef();
		if (ref === undefined) {
			console.log("skipping findMoreSuitableSource. videoRef is empty still");
			return;
		}

		const videoSources = this.src.data()?.sources ?? [];

		const currentVideoWidth = ref.nativeElement.clientWidth;
		const realPixelsVideoWidth = this.device.devicePixelRatio * currentVideoWidth;

		console.log(`MarcyVideoComponent currentVideoWidth ${currentVideoWidth}`);
		console.log(`MarcyVideoComponent realPixelsVideoWidth ${realPixelsVideoWidth}`);

		for (let i = 0; i < videoSources.length; i++) {
			const videoSource = videoSources[i];

			if (!matchesMediaQuery(videoSource.mediaQuery)) continue;

			// SSR
			if (typeof ref.nativeElement.canPlayType !== "function") {
				const mp4Srcs = videoSource.srcSet
					.filter((x) => x.mimeType === "video/mp4")
					.sort(descendingT((x) => x.meta.width));

				return mp4Srcs[0];
			}

			const fileSrcs = videoSource.srcSet
				.filter((x) => ref.nativeElement.canPlayType(x.mimeType))
				.sort((a, b) => {
					if (a.meta.width === b.meta.width) {
						return isWebM(a) ? -1 : 1; // if same width prefer webM
					}

					// else prefer smallest
					return a.meta.width <= b.meta.width ? -1 : 1;
				}); // smallest video

			if (fileSrcs.length === 0) continue;

			// console.log('sources ', fileSrcs);

			let bestSrc = fileSrcs[0];

			for (let i = 1; i < fileSrcs.length; i++) {
				const fileSrc = fileSrcs[i];

				const currentDiff = fileSrc.meta.width - realPixelsVideoWidth;

				// console.log(`browser video currentDiff ${currentDiff}`)

				// too big video source width
				if (currentDiff > 0) break;

				bestSrc = fileSrc;
			}

			console.log(`browser found suitable video ${bestSrc.url}`);

			return bestSrc;
		}

		return undefined;
	}
}
