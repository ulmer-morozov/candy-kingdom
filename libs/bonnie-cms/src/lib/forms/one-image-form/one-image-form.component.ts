import { ChangeDetectorRef, Component, Input, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FileMeta, FileSrc, ImageMeta } from '@candy-kingdom/bonnie';

import { FormBaseComponent } from '../../core-components/form-base.component';
import { EditableDirective } from '../../core-components/editable.directive';
import { FormControlsComponent } from '../../form-controls/form-controls.component';
import { FileUploaderComponent } from '../../file-uploader/file-uploader.component';

const DefaultImageMimeTypes = ['image/png', 'image/jpeg'];

@Component({
  selector: 'bonc-one-image-form',
  standalone: true,
  imports: [CommonModule, FormControlsComponent, FileUploaderComponent],
  templateUrl: './one-image-form.component.html',
  styleUrls: ['./one-image-form.component.scss'],
  hostDirectives: [EditableDirective]
})
export class OneImageFormComponent extends FormBaseComponent<FileSrc<ImageMeta>> implements OnInit {
  private readonly cd = inject(ChangeDetectorRef);

  public uploadMap = new Map<string, string>();

  private _mimeTypes = DefaultImageMimeTypes;

  private _uploadUrl = '/api/admin/upload/image';

  private _label = ''

  constructor() {
    super()

    this.uploadMap.set('', this._uploadUrl);
  }

  public ngOnInit(): void {
    this.editable.externalSaveCall.subscribe(() => {
      this.editable.save();
    });
  }

  @Input()
  public set label(newValue: string) {
    this._label = newValue;
  }

  public get label(): string {
    return this._label;
  }

  @Input()
  public set uploadUrl(newValue: string) {
    this._uploadUrl = newValue;

    this.uploadMap.set('', this._uploadUrl);

    this.cd.detectChanges();
  }

  @Input()
  public set mimeTypes(newValue: string[]) {
    this._mimeTypes = newValue.length === 0 ? DefaultImageMimeTypes : newValue;
    this.cd.detectChanges();
  }

  public get mimeTypes(): string[] {
    return this._mimeTypes;
  }

  public onFileUploaded(fileSrc: FileSrc<FileMeta>): void {
    this.editable.startEditing();

    const svgSrc = fileSrc as FileSrc<ImageMeta>;

    this.editable.value = svgSrc;

    this.editable.updateDirty();
  }

}
