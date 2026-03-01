import { CommonModule } from "@angular/common";
import { Component, effect, inject, input, output, signal } from "@angular/core";

import { DeviceServiceBase } from "../core/device.service.base";
import { IntersectionComponent } from "../core/intersection.component";
import { MediaObjectFit } from "../core/MediaObjectFit";
import { MediaStatus } from "../core/MediaStatus";
import type * as M_CORE from "../generated";
import { getDefaultSrc } from "./getDefaultSrc";
import type { IHtmlPictureSource } from "./IHtmlPictureSource";
import { ImageSrcDirective } from "./imgsrc.directive";
import { toHtmlPictureSources } from "./toHtmlSources";

@Component({
	selector: "bon-image",
	standalone: true,
	imports: [CommonModule, IntersectionComponent],
	templateUrl: "./marcy-image.component.html",
	styleUrls: ["./marcy-image.component.scss"],
})
export class MarcyImageComponent {
	public readonly MediaStatus = MediaStatus;
	public readonly MarcyObjectFit = MediaObjectFit;

	public readonly isLoaded = output<void>();
	public readonly sources = signal<IHtmlPictureSource[]>([]);

	public readonly status = signal<MediaStatus>(MediaStatus.NotSet);

	public readonly defaultSrc = signal("");

	public readonly objectFit = input<MediaObjectFit>(MediaObjectFit.Original);

	public readonly src: ImageSrcDirective;

	public readonly device = inject(DeviceServiceBase);
	private readonly _srcDir = inject(ImageSrcDirective, { optional: true });

	constructor() {
		if (this._srcDir === undefined || this._srcDir === null)
			throw new Error(
				`${MarcyImageComponent.name} should have [imgsrc] directive as source object`,
			);

		this.src = this._srcDir;

		this.src.srcChange.subscribe(this.onSrcChange.bind(this));

		effect(() => {
			if (this.status() === MediaStatus.Loaded) {
				this.isLoaded.emit();
			}
		});
	}

	private onSrcChange(val: M_CORE.Image | undefined) {
		this.defaultSrc.set(getDefaultSrc(val)?.url ?? "");

		if (val === undefined || val === null || val.sources.length === 0) {
			this.sources.set([]);
			this.status.set(MediaStatus.NotSet);
			return;
		}

		const newSources = val.sources.flatMap(toHtmlPictureSources);
		this.sources.set(newSources);
		this.status.set(MediaStatus.NotLoaded);
	}

	public onLoad() {
		this.status.set(MediaStatus.Loaded);
	}
}
