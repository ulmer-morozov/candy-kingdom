import { Component, Input, OnInit } from '@angular/core';

import { FileMeta, FileSrc, SvgMeta } from '@candy-kingdom/bonnie';

import { FormBaseComponent } from '../../core-components/form-base.component';
import { EditableDirective } from '../../core-components/editable.directive';
import { AnimationOptions } from 'ngx-lottie';

@Component({
  selector: 'bonc-lottie-form',
  templateUrl: './lottie-form.component.html',
  styleUrls: ['./lottie-form.component.scss'],
  hostDirectives: [EditableDirective]
})
export class LottieFormComponent extends FormBaseComponent<FileSrc<FileMeta>> implements OnInit {
  public readonly LottieMimeType = 'application/json';

  public animOptions?: AnimationOptions;

  @Input()
  public label = '';

  public ngOnInit(): void {
    this.editable.externalSaveCall.subscribe(() => {
      this.editable.save();
    });

    this.editable.valueChange.subscribe(x => {
      const url = x?.url ?? '';
      this.animOptions = url.length === 0 ? undefined : { path: url };
    })
  }

  @Input({ required: true })
  public uploadMap = new Map<string, string>();;

  public onFileUploaded(fileSrc: FileSrc<FileMeta>): void {
    this.editable.startEditing();

    const svgSrc = fileSrc as FileSrc<SvgMeta>;

    this.editable.value = svgSrc;

    this.editable.updateDirty();
  }

}
