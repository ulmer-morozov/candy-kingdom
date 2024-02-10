import { Component, Input, OnInit } from '@angular/core';
import { LocalizedString } from '@candy-kingdom/bonnie';

import { EditableDirective, FormBaseComponent } from '../../core-components';
import { TextEditorField } from '../../core';

@Component({
  selector: 'bonc-translation-form',
  templateUrl: './translation-form.component.html',
  styleUrls: ['./translation-form.component.scss'],
  inputs: FormBaseComponent.inputs,
  hostDirectives: [EditableDirective]
})
export class TranslationFormComponent extends FormBaseComponent<LocalizedString> implements OnInit {
  public readonly TextEditorField = TextEditorField;

  @Input({ required: true })
  public field!: TextEditorField;

  @Input() label?: string;


  ngOnInit(): void {
    this.editable.externalSaveCall.subscribe(() => {
      this.editable.save();
    });
  }
}
