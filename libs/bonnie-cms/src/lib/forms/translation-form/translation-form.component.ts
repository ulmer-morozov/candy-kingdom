import { Component, input } from "@angular/core";

import type { LocalizedString } from "@candy-kingdom/bonnie";

import { TextEditorField } from "../../core";
import { EditableDirective, FormBaseComponent } from "../../core-components";
import { FormControlsComponent } from "../../form-controls/form-controls.component";
import { TranslationInputComponent } from "../../translation-input/translation-input.component";
import { TranslationTextareaComponent } from "../../translation-textarea/translation-textarea.component";

@Component({
	selector: "bonc-translation-form",

	imports: [FormControlsComponent, TranslationInputComponent, TranslationTextareaComponent],
	templateUrl: "./translation-form.component.html",
	styleUrl: "./translation-form.component.scss",
	hostDirectives: [EditableDirective],
})
export class TranslationFormComponent extends FormBaseComponent<LocalizedString> {
	public readonly TextEditorField = TextEditorField;

	public readonly field = input.required<TextEditorField>();

	public readonly label = input<string | undefined>();

	constructor() {
		super();

		this.editable.externalSaveCall.subscribe(() => {
			this.editable.save();
		});
	}
}
