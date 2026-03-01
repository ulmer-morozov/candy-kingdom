import { Component, type OnInit, input } from "@angular/core";
import { CommonModule } from "@angular/common";
import type { LocalizedString } from "@candy-kingdom/bonnie";

import { EditableDirective, FormBaseComponent } from "../../core-components";
import { TextEditorField } from "../../core";
import { FormControlsComponent } from "../../form-controls/form-controls.component";
import { TranslationInputComponent } from "../../translation-input/translation-input.component";
import { TranslationTextareaComponent } from "../../translation-textarea/translation-textarea.component";

@Component({
	selector: "bonc-translation-form",
	standalone: true,
	imports: [
		CommonModule,
		FormControlsComponent,
		TranslationInputComponent,
		TranslationTextareaComponent,
	],
	templateUrl: "./translation-form.component.html",
	styleUrls: ["./translation-form.component.scss"],
	hostDirectives: [EditableDirective],
})
export class TranslationFormComponent extends FormBaseComponent<LocalizedString> implements OnInit {
	public readonly TextEditorField = TextEditorField;

	public readonly field = input.required<TextEditorField>();

	public readonly label = input<string | undefined>();

	ngOnInit(): void {
		this.editable.externalSaveCall.subscribe(() => {
			this.editable.save();
		});
	}
}
