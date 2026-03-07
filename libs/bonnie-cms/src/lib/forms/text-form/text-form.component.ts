import { Component, input } from "@angular/core";
import { FormsModule } from "@angular/forms";

import { EditableDirective } from "../../core-components";
import { FormBaseComponent } from "../../core-components/form-base.component";
import { FormControlsComponent } from "../../form-controls/form-controls.component";
import { TextInputStyle } from "./TextInputStyle";

@Component({
	selector: "bonc-text-form",

	imports: [FormsModule, FormControlsComponent],
	templateUrl: "./text-form.component.html",
	styleUrl: "./text-form.component.scss",
	hostDirectives: [EditableDirective],
})
export class TextFormComponent extends FormBaseComponent<string> {
	public readonly TextInputStyle = TextInputStyle;

	public readonly label = input("");

	public readonly type = input(TextInputStyle.SingleLine);

	constructor() {
		super();

    this.editable.externalSaveCall.subscribe(() => {
			this.editable.save();
		});
	}
}
