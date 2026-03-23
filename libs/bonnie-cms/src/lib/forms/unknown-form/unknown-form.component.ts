import { Component, input } from "@angular/core";

import { EditableDirective } from "../../core-components/editable.directive";
import { FormBaseComponent } from "../../core-components/form-base.component";

@Component({
	selector: "bonc-unknown-form",

	templateUrl: "./unknown-form.component.html",
	styleUrl: "./unknown-form.component.scss",
	hostDirectives: [EditableDirective],
})
export class UnknownFormComponent extends FormBaseComponent<string> {
	public readonly label = input("");

	constructor() {
		super();
		this.editable.externalSaveCall.subscribe(() => {
			this.editable.save();
		});
	}
}
