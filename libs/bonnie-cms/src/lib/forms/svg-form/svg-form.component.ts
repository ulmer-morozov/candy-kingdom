import { Component, Input, OnInit } from '@angular/core';

import { FormBaseComponent } from '../../core-components/form-base.component';
import { EditableDirective } from '../../core-components';
import { FileSrc, ImageMeta } from '@candy-kingdom/bonnie';

@Component({
  selector: 'bonc-svg-form',
  templateUrl: './svg-form.component.html',
  styleUrls: ['./svg-form.component.scss'],
  inputs: FormBaseComponent.inputs, // todo: remove
  hostDirectives: [EditableDirective]
})
export class SvgFormComponent extends FormBaseComponent<FileSrc<ImageMeta>> implements OnInit {
  @Input()
  public label = '';

  ngOnInit(): void {
    this.editable.externalSaveCall.subscribe(() => {
      this.editable.save();
    });
  }
}
