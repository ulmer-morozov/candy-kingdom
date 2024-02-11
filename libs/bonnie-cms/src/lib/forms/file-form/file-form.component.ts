import { Component, Input, OnInit } from '@angular/core';

import { FormBaseComponent } from '../../core-components/form-base.component';
import { EditableDirective } from '../../core-components';

import { FileMeta, FileSrc, SvgMeta } from '@candy-kingdom/bonnie';

@Component({
  selector: 'bonc-file-form',
  templateUrl: './file-form.component.html',
  styleUrls: ['./file-form.component.scss'],
  hostDirectives: [EditableDirective]
})
export class FileFormComponent extends FormBaseComponent<FileSrc<FileMeta>> implements OnInit {
  @Input()
  public label = '';

  public ngOnInit(): void {
    this.editable.externalSaveCall.subscribe(() => {
      this.editable.save();
    });
  }

  @Input({ required: true })
  public uploadTypes: string[] = [];

  @Input({ required: true })
  public uploadMap = new Map<string, string>();;

  public onFileUploaded(fileSrc: FileSrc<FileMeta>): void {
    this.editable.startEditing();

    const svgSrc = fileSrc as FileSrc<SvgMeta>;

    this.editable.value = svgSrc;

    this.editable.updateDirty();
  }

}
