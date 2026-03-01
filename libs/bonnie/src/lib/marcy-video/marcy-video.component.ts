import {
	Component,
	input,
	output,
	ElementRef,
	ViewChild,
	AfterViewInit,
	inject,
	signal,
	effect,
} from "@angular/core";
import { CommonModule } from "@angular/common";

import * as MCore from "../generated";

import { MediaStatus } from "../core/MediaStatus";
import { MediaObjectFit } from "../core/MediaObjectFit";
import { DeviceServiceBase } from "../core/device.service.base";
import { VideoSrcDirective } from "./vidsrc.directive";
import { matchesMediaQuery, descendingT } from "../core/utils";
import { IntersectionComponent } from "../core/intersection.component";

function isWebM(src: MCore.FileSrc<MCore.ImageMeta>): boolean {
	return src.mimeType === "video/webm";
}

@Component({
	selector: "bon-video",
	standalone: true,
	imports: [CommonModule, IntersectionComponent],
	templateUrl: "./marcy-video.component.html",
	styleUrls: ["./marcy-video.component.scss"],
})
export class MarcyVideoComponent implements AfterViewInit {
	public readonly MediaStatus = MediaStatus;
	public readonly MarcyObjectFit = MediaObjectFit;

	@ViewChild("video")
	public readonly videoRef!: ElementRef<HTMLVideoElement>;

	public readonly isLoaded = output<void>();

	public readonly $status = signal<MediaStatus>(MediaStatus.NotSet);
	public readonly src: VideoSrcDirective;

	public readonly source = signal<MCore.FileSrc<MCore.VideoMeta> | undefined>(undefined);

	public readonly objectFit = input<MediaObjectFit>(MediaObjectFit.Original);

	public readonly device = inject(DeviceServiceBase);
	private readonly _srcDir = inject(VideoSrcDirective, { optional: true });

	constructor() {
		console.log("MarcyVideoComponent ctor");

		if (this._srcDir === undefined || this._srcDir === null)
			throw new Error(
				`${MarcyVideoComponent.name} should have [vidsrc] directive as source object`,
			);

		this.src = this._srcDir;

		effect(() => {
			if (this.$status() === MediaStatus.Loaded) {
				this.isLoaded.emit();
			}
		});
	}

	public ngAfterViewInit() {
		console.log("MarcyVideoComponent ngAfterViewInit");

		// bind src changes
		this.src.srcChange.subscribe((val) => {
			console.log("MarcyVideoComponent onSrcChange", val);

			this.updateSources();

			// resubscribe because its updated with src
			this.subscribeToMediaQueryChange();
		});

		this.updateSources();

		// initial, for src added before init
		this.subscribeToMediaQueryChange();
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

	private findMoreSuitableSource(): MCore.FileSrc<MCore.VideoMeta> | undefined {
		if (this.videoRef === undefined) {
			console.log("skipping findMoreSuitableSource. videoRef is empty still");
			return;
		}

		const videoSources = this.src.data()?.sources ?? [];

		const currentVideoWidth = this.videoRef.nativeElement.clientWidth;
		const realPixelsVideoWidth = this.device.devicePixelRatio * currentVideoWidth;

		console.log(`MarcyVideoComponent currentVideoWidth ${currentVideoWidth}`);
		console.log(`MarcyVideoComponent realPixelsVideoWidth ${realPixelsVideoWidth}`);

		for (let i = 0; i < videoSources.length; i++) {
			const videoSource = videoSources[i];

			if (!matchesMediaQuery(videoSource.mediaQuery)) continue;

			// SSR
			if (typeof this.videoRef.nativeElement?.canPlayType !== "function") {
				// return first mp4, because all players can play them
				const mp4Srcs = videoSource.srcSet
					.filter((x) => x.mimeType === "video/mp4")
					.sort(descendingT((x) => x.meta.width)); // bigest video

				// console.log(`ssr found video ${mp4Srcs[0].url}`)

				// element or undefined
				return mp4Srcs[0];
			}

			const fileSrcs = videoSource.srcSet
				.filter((x) => this.videoRef.nativeElement.canPlayType(x.mimeType))
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
