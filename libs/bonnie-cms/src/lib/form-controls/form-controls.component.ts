import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EditableDirective } from '../core-components';

@Component({
  selector: 'bonc-form-controls',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './form-controls.component.html',
  styleUrls: ['./form-controls.component.scss']
})
export class FormControlsComponent {
  @Input({ required: true })
  public editable!: EditableDirective<any>;
}
