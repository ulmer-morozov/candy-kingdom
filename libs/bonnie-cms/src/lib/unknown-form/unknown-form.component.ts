import { Component, Input, OnInit } from '@angular/core';
import { FormBaseComponent } from '../core-components/form-base.component';

@Component({
  selector: 'bonc-unknown-form',
  templateUrl: './unknown-form.component.html',
  styleUrls: ['./unknown-form.component.scss'],
  inputs: FormBaseComponent.inputs
})
export class UnknownFormComponent extends FormBaseComponent<string> implements OnInit {
  @Input()
  public label = '';

  ngOnInit(): void {
    this.editable.externalSaveCall.subscribe(() => {
      this.editable.save();
    });
  }
}
