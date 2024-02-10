import { Component, Input, OnInit } from '@angular/core';
import { TextInputStyle } from './TextInputStyle';
import { FormBaseComponent } from '../../core-components/form-base.component';
import { EditableDirective } from '../../core-components';

@Component({
  selector: 'bonc-text-form',
  templateUrl: './text-form.component.html',
  styleUrls: ['./text-form.component.scss'],
  inputs: FormBaseComponent.inputs,
  hostDirectives: [EditableDirective]
})
export class TextFormComponent extends FormBaseComponent<string> implements OnInit {
  public readonly TextInputStyle = TextInputStyle;

  @Input()
  public label = '';

  @Input()
  public type = TextInputStyle.SingleLine;

  ngOnInit(): void {
    this.editable.externalSaveCall.subscribe(() => {
      this.editable.save();
    });
  }
}
