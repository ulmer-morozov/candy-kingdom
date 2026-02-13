import { Component, OnInit, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TextInputStyle } from './TextInputStyle';
import { FormBaseComponent } from '../../core-components/form-base.component';
import { EditableDirective } from '../../core-components';
import { FormControlsComponent } from '../../form-controls/form-controls.component';

@Component({
  selector: 'bonc-text-form',
  standalone: true,
  imports: [CommonModule, FormsModule, FormControlsComponent],
  templateUrl: './text-form.component.html',
  styleUrls: ['./text-form.component.scss'],
  hostDirectives: [EditableDirective]
})
export class TextFormComponent extends FormBaseComponent<string> implements OnInit {
  public readonly TextInputStyle = TextInputStyle;

  public readonly label = input('');

  public readonly type = input(TextInputStyle.SingleLine);

  ngOnInit(): void {
    this.editable.externalSaveCall.subscribe(() => {
      this.editable.save();
    });
  }
}
