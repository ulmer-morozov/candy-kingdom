import { ChangeDetectorRef, Component, Input, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FileMeta, FileSrc, SvgMeta } from '@candy-kingdom/bonnie';

import { FormBaseComponent } from '../../core-components/form-base.component';
import { EditableDirective } from '../../core-components/editable.directive';
import { FormControlsComponent } from '../../form-controls/form-controls.component';
import { FileUploaderComponent } from '../../file-uploader/file-uploader.component';

@Component({
  selector: 'bonc-svg-form',
  standalone: true,
  imports: [CommonModule, FormControlsComponent, FileUploaderComponent],
  templateUrl: './svg-form.component.html',
  styleUrls: ['./svg-form.component.scss'],
  hostDirectives: [EditableDirective]
})
export class SvgFormComponent extends FormBaseComponent<FileSrc<SvgMeta>> implements OnInit {
  private readonly cd = inject(ChangeDetectorRef);

  public readonly SvgMime = 'image/svg+xml';

  @Input()
  public label = '';

  public uploadMap = new Map<string, string>();

  private _uploadUrl = '/api/admin/upload/image/svg';

  constructor() {
    super()

    this.uploadMap.set(this.SvgMime, this._uploadUrl);
  }

  public ngOnInit(): void {
    this.editable.externalSaveCall.subscribe(() => {
      this.editable.save();
    });
  }

  @Input()
  public set uploadUrl(newValue: string) {
    this._uploadUrl = newValue;

    this.uploadMap.set(this.SvgMime, this._uploadUrl);

    this.cd.detectChanges();
  }

  public onFileUploaded(fileSrc: FileSrc<FileMeta>): void {
    this.editable.startEditing();

    const svgSrc = fileSrc as FileSrc<SvgMeta>;

    this.editable.value = svgSrc;

    this.editable.updateDirty();
  }

}
