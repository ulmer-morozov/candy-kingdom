import { ChangeDetectorRef, Component, ElementRef, EventEmitter, Input, Output, ViewChild, inject } from '@angular/core';
import { DomSanitizer, SafeStyle } from '@angular/platform-browser';
import { HttpClient, HttpEvent, HttpEventType, HttpRequest } from '@angular/common/http';
import { catchError, last, map, Observable } from 'rxjs';

import { FileMeta, FileSrc } from '@candy-kingdom/bonnie';

@Component({
  selector: 'bonc-file-uploader',
  templateUrl: './file-uploader.component.html',
  styleUrls: ['./file-uploader.component.scss']
})
export class FileUploaderComponent {
  @ViewChild('fileInput', { static: true })
  public fileInput!: ElementRef<HTMLInputElement>;

  @Output()
  public srcChange = new EventEmitter<FileSrc<FileMeta>>();

  @Input({ required: true })
  public uploadUrlMap!: ReadonlyMap<string, string>;

  public progress = 0;
  public isUploading = false;

  public autoplay = true;
  public clipStyle?: SafeStyle;

  public fileTypeMask?: string = undefined;

  private _uploadTypes: string[] = [];
  private _src?: FileSrc<FileMeta>;

  private readonly sanitizer = inject(DomSanitizer);
  private readonly http = inject(HttpClient);
  private readonly cd = inject(ChangeDetectorRef);

  @Input()
  public set src(newSrc: FileSrc<FileMeta> | undefined) {
    if (this._src === newSrc)
      return;

    this._src = newSrc;
  }

  public get src(): FileSrc<FileMeta> | undefined {
    return this._src;
  }

  @Input()
  public set uploadTypes(newUploadType: string[]) {
    this._uploadTypes.splice(0, this._uploadTypes.length);

    this._uploadTypes.push(...newUploadType);

    this.fileTypeMask = this._uploadTypes.length === 0 ? undefined : this._uploadTypes.join(',');

    this.cd.detectChanges();
  }

  public get uploadTypes(): string[] {
    return this._uploadTypes;
  }

  public onFileSelect(fileInput: HTMLInputElement) {
    if (fileInput.files === undefined || fileInput.files === null || fileInput.files.length !== 1)
      return;

    const file = fileInput.files[0];

    const uploadUrl = this.uploadUrlMap.get(file.type) ?? this.uploadUrlMap.get("");

    if (uploadUrl === undefined || uploadUrl === null) {
      console.error(`upload map doesn't have url for type '${file.type}'`);
      return;
    }

    const formData: FormData = new FormData();

    formData.append('file', file);

    this.progress = 0;
    this.isUploading = true;

    this.updateClip();

    const request = new HttpRequest('POST', uploadUrl, formData, {
      reportProgress: true
    });

    this.http
      .request<FileSrc<FileMeta>>(request)
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

  private getEventMessage(event: HttpEvent<FileSrc<FileMeta>>): void {
    switch (event.type) {
      case HttpEventType.Sent:
        break;

      case HttpEventType.UploadProgress:
        this.progress = event.total === undefined ? 0.5 : event.loaded / event.total;
        this.updateClip();
        break;

      case HttpEventType.Response:
        if (event.body === null || event.body === undefined) {
          console.warn(`empty body from uploader`);
          break;
        }

        this.srcChange.emit(event.body);
        break;

      default:
        break;
    }

    this.cd.detectChanges()
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
