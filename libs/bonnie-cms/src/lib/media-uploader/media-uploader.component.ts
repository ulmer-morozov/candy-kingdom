import { ChangeDetectorRef, Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { DomSanitizer, SafeStyle } from '@angular/platform-browser';
import { HttpClient, HttpEvent, HttpEventType, HttpRequest } from '@angular/common/http';
import { catchError, last, map, Observable } from 'rxjs';

import { MediaObjectFit, PixMediaUnion } from '@candy-kingdom/bonnie';
import { MediaType } from '../core';

const imageMimeTypes = ['image/png', 'image/jpeg'];
const videoMimeTypes = ['video/mp4'];

const imageFileTypes = imageMimeTypes.join(',');
const videoFileTypes = videoMimeTypes.join(',');

const allMediaFileTypes = `${imageFileTypes},${videoFileTypes}`;

@Component({
  selector: 'bonc-media-uploader',
  templateUrl: './media-uploader.component.html',
  styleUrls: ['./media-uploader.component.scss']
})
export class MediaUploaderComponent {
  public readonly MediaObjectFit = MediaObjectFit;

  @ViewChild('fileInput', { static: true })
  public fileInput!: ElementRef<HTMLInputElement>;

  @Output()
  public srcChange = new EventEmitter<PixMediaUnion>();

  @Input({ required: true })
  public uploadUrlMap!: ReadonlyMap<MediaType, string>;

  @Input()
  public forceRatio?: number;

  public progress = 0;
  public isUploading = false;

  public autoplay = true;
  public clipStyle?: SafeStyle;
  public fileTypeMask = allMediaFileTypes;

  private _uploadType?: MediaType;
  private _src?: PixMediaUnion;

  constructor(private sanitizer: DomSanitizer, private http: HttpClient, private cd: ChangeDetectorRef) {
  }

  @Input()
  public set src(newSrc: PixMediaUnion | undefined) {
    if (this._src === newSrc)
      return;

    this._src = newSrc;
  }

  public get src(): PixMediaUnion | undefined {
    return this._src;
  }

  @Input()
  public set uploadType(newUploadType: MediaType | undefined) {
    switch (newUploadType) {
      case "image":
        this._uploadType = newUploadType;
        this.fileTypeMask = imageFileTypes;
        break;

      case "video":
        this._uploadType = newUploadType;
        this.fileTypeMask = videoFileTypes;
        break;

      case undefined:
      default:
        this._uploadType = newUploadType;
        this.fileTypeMask = allMediaFileTypes;
        break;
    }
  }

  public get uploadType(): MediaType | undefined {
    return this._uploadType;
  }

  public onFileSelect(fileInput: HTMLInputElement) {
    if (fileInput.files === undefined || fileInput.files === null || fileInput.files.length !== 1)
      return;

    const file = fileInput.files[0];

    let uploadingMediaType: MediaType;

    if (this._uploadType !== undefined && this._uploadType !== null) {
      uploadingMediaType = this._uploadType;
    }
    else if (imageMimeTypes.includes(file.type)) {
      uploadingMediaType = 'image';
    }
    else if (videoMimeTypes.includes(file.type)) {
      uploadingMediaType = 'video';
    }
    else {
      console.error(`unknown media type ${file.type} (${file.name})`);
      return;
    }

    const uploadUrl = this.uploadUrlMap.get(uploadingMediaType);

    if (uploadUrl === undefined || uploadUrl === null) {
      console.error(`upload map doesn't have url for type ${uploadingMediaType}`);
      return;
    }

    const formData: FormData = new FormData();

    formData.append('file', file);
    formData.append('ratio', `${this.forceRatio ?? 0}`);

    this.progress = 0;
    this.isUploading = true;

    this.updateClip();

    const request = new HttpRequest('POST', uploadUrl, formData, {
      reportProgress: true
    });

    this.http
      .request<PixMediaUnion>(request)
      .pipe
      (
        map(event => this.getEventMessage(event)),
        // tap(message => this.showProgress(message)),
        last(), // return last (completed) message to caller
        catchError(this.handleError(file))
      ).subscribe
      (
        () => { this.isUploading = false; }
      );
  }

  public selectFile(event: Event): void {
    // ignore buble click on file picker
    if (event.target === this.fileInput.nativeElement)
      return;

    this.fileInput.nativeElement.click();
  }

  private getEventMessage(event: HttpEvent<PixMediaUnion>): void {
    switch (event.type) {
      case HttpEventType.Sent:
        break;

      case HttpEventType.UploadProgress:
        this.progress = event.total === undefined ? 0.5 : event.loaded / event.total;
        this.updateClip();
        break;

      case HttpEventType.Response:
        if (event.body === undefined || event.body === null) {
          console.error('media deserialization error. Response body in undefined');
        } else {
          // remove this
          // needed for C# deserialization
          const pixmedia = {
            $type: event.body.type,
            ...event.body as any,
          };

          this.srcChange.emit(pixmedia);
        }
        break;

      default:
        break;
    }

    this.cd.detectChanges();
  }

  private updateClip(): void {
    this.clipStyle = this.sanitizer.bypassSecurityTrustStyle(`inset(0px 100% 0px 0%)`);
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
