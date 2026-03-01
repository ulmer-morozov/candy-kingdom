import { Component, computed, ElementRef, input, output, signal, ViewChild, inject } from '@angular/core';
import { CommonModule, DecimalPipe } from '@angular/common';
import { DomSanitizer, SafeStyle } from '@angular/platform-browser';
import { HttpClient, HttpEvent, HttpEventType, HttpRequest } from '@angular/common/http';
import { catchError, last, map, Observable } from 'rxjs';

import { FileMeta, FileSrc } from '@candy-kingdom/bonnie';

@Component({
  selector: 'bonc-file-uploader',
  standalone: true,
  imports: [CommonModule, DecimalPipe],
  templateUrl: './file-uploader.component.html',
  styleUrls: ['./file-uploader.component.scss']
})
export class FileUploaderComponent {
  @ViewChild('fileInput', { static: true })
  public fileInput!: ElementRef<HTMLInputElement>;

  public readonly srcChange = output<FileSrc<FileMeta>>();

  public readonly uploadUrlMap = input.required<ReadonlyMap<string, string>>();

  public readonly src = input<FileSrc<FileMeta> | undefined>();

  public readonly uploadTypes = input<string[]>([]);

  public readonly fileTypeMask = computed(() => {
    const types = this.uploadTypes();
    return types.length === 0 ? undefined : types.join(',');
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

    const uploadUrl = this.uploadUrlMap().get(file.type) ?? this.uploadUrlMap().get("");

    if (uploadUrl === undefined || uploadUrl === null) {
      console.error(`upload map doesn't have url for type '${file.type}'`);
      return;
    }

    const formData: FormData = new FormData();

    formData.append('file', file);

    this.progress.set(0);
    this.isUploading.set(true);

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
        () => { this.isUploading.set(false); }
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
        this.progress.set(event.total === undefined ? 0.5 : event.loaded / event.total);
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
