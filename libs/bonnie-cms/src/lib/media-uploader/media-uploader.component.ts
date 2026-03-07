import { DecimalPipe } from "@angular/common";
import { HttpClient, type HttpEvent, HttpEventType, HttpRequest } from "@angular/common/http";
import {
	Component,
	computed,
	type ElementRef,
	inject,
	input,
	output,
	signal,
	viewChild,
} from "@angular/core";
import { DomSanitizer, type SafeStyle } from "@angular/platform-browser";

import { catchError, last, map, Observable } from "rxjs";

import { MarcyMediaComponent, MediaObjectFit, type PixMediaUnion } from "@candy-kingdom/bonnie";

import type { MediaType } from "../core";

const imageMimeTypes = ["image/png", "image/jpeg"];
const videoMimeTypes = ["video/mp4"];

const imageFileTypes = imageMimeTypes.join(",");
const videoFileTypes = videoMimeTypes.join(",");

const allMediaFileTypes = `${imageFileTypes},${videoFileTypes}`;

@Component({
	selector: "bonc-media-uploader",

	imports: [DecimalPipe, MarcyMediaComponent],
	templateUrl: "./media-uploader.component.html",
	styleUrl: "./media-uploader.component.scss",
})
export class MediaUploaderComponent {
	public readonly MediaObjectFit = MediaObjectFit;

	public readonly fileInput = viewChild.required<ElementRef<HTMLInputElement>>("fileInput");

	public readonly srcChange = output<PixMediaUnion>();

	public readonly uploadUrlMap = input.required<ReadonlyMap<MediaType, string>>();

	public readonly forceRatio = input<number>();

	public readonly src = input<PixMediaUnion | undefined>();

	public readonly uploadType = input<MediaType | undefined>();

	public readonly fileTypeMask = computed(() => {
		switch (this.uploadType()) {
			case "image":
				return imageFileTypes;
			case "video":
				return videoFileTypes;
			default:
				return allMediaFileTypes;
		}
	});

	public readonly progress = signal(0);
	public readonly isUploading = signal(false);
	public readonly clipStyle = signal<SafeStyle | undefined>(undefined);

	private readonly sanitizer = inject(DomSanitizer);
	private readonly http = inject(HttpClient);

	public onFileSelect(fileInput: HTMLInputElement) {
		if (fileInput.files === undefined || fileInput.files === null || fileInput.files.length !== 1)
			return;

		const file = fileInput.files[0];

		let uploadingMediaType: MediaType;

		const uploadType = this.uploadType();

		if (uploadType !== undefined && uploadType !== null) {
			uploadingMediaType = uploadType;
		} else if (imageMimeTypes.includes(file.type)) {
			uploadingMediaType = "image";
		} else if (videoMimeTypes.includes(file.type)) {
			uploadingMediaType = "video";
		} else {
			console.error(`unknown media type ${file.type} (${file.name})`);
			return;
		}

		const uploadUrl = this.uploadUrlMap().get(uploadingMediaType);

		if (uploadUrl === undefined || uploadUrl === null) {
			console.error(`upload map doesn't have url for type ${uploadingMediaType}`);
			return;
		}

		const formData: FormData = new FormData();

		formData.append("file", file);
		formData.append("ratio", `${this.forceRatio() ?? 0}`);

		this.progress.set(0);
		this.isUploading.set(true);

		this.updateClip();

		const request = new HttpRequest("POST", uploadUrl, formData, {
			reportProgress: true,
		});

		this.http
			.request<PixMediaUnion>(request)
			.pipe(
				map((event) => this.getEventMessage(event)),
				// tap(message => this.showProgress(message)),
				last(), // return last (completed) message to caller
				catchError(this.handleError(file)),
			)
			.subscribe(() => {
				this.isUploading.set(false);
			});
	}

	public selectFile(event: Event): void {
		if (event.target === this.fileInput().nativeElement) return;

		this.fileInput().nativeElement.click();
	}

	private getEventMessage(event: HttpEvent<PixMediaUnion>): void {
		switch (event.type) {
			case HttpEventType.Sent:
				break;

			case HttpEventType.UploadProgress:
				this.progress.set(event.total === undefined ? 0.5 : event.loaded / event.total);
				this.updateClip();
				break;

			case HttpEventType.Response:
				if (event.body === undefined || event.body === null) {
					console.error("media deserialization error. Response body in undefined");
				} else {
					// remove this
					// needed for C# deserialization
					const pixmedia = {
						$type: event.body.type,
						...(event.body as any),
					};

					this.srcChange.emit(pixmedia);
				}
				break;

			default:
				break;
		}
	}

	private updateClip(): void {
		this.clipStyle.set(this.sanitizer.bypassSecurityTrustStyle(`inset(0px 100% 0px 0%)`));
	}

	private handleError(file: File): (p1: unknown, p2: Observable<unknown>) => Observable<unknown> {
		const func = (error: unknown, p2: Observable<unknown>): Observable<unknown> => {
			const message = `error uploadingFile ${file.name}.`;
			console.error(message, error, p2);
			alert(message);
			return new Observable<unknown>();
		};

		return func;
	}
}
