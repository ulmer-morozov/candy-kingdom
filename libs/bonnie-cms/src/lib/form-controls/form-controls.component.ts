import { Component, input } from "@angular/core";

import type { EditableDirective } from "../core-components";

@Component({
	selector: "bonc-form-controls",

	templateUrl: "./form-controls.component.html",
	styleUrl: "./form-controls.component.scss",
})
export class FormControlsComponent {
	public readonly editable = input.required<EditableDirective<any>>();
}
