import { CommonModule } from "@angular/common";
import { Component, input } from "@angular/core";

import type { EditableDirective } from "../core-components";

@Component({
	selector: "bonc-form-controls",
	standalone: true,
	imports: [CommonModule],
	templateUrl: "./form-controls.component.html",
	styleUrls: ["./form-controls.component.scss"],
})
export class FormControlsComponent {
	public readonly editable = input.required<EditableDirective<any>>();
}
