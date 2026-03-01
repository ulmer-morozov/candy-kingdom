import { CommonModule } from "@angular/common";
import { Component, input, type OnInit } from "@angular/core";
import { FormsModule } from "@angular/forms";

import { EditableDirective } from "../../core-components";
import { FormBaseComponent } from "../../core-components/form-base.component";
import { FormControlsComponent } from "../../form-controls/form-controls.component";
import { TextInputStyle } from "./TextInputStyle";

@Component({
	selector: "bonc-text-form",
	standalone: true,
	imports: [CommonModule, FormsModule, FormControlsComponent],
	templateUrl: "./text-form.component.html",
	styleUrls: ["./text-form.component.scss"],
	hostDirectives: [EditableDirective],
})
export class TextFormComponent extends FormBaseComponent<string> implements OnInit {
	public readonly TextInputStyle = TextInputStyle;

	public readonly label = input("");

	public readonly type = input(TextInputStyle.SingleLine);

	ngOnInit(): void {
		this.editable.externalSaveCall.subscribe(() => {
			this.editable.save();
		});
	}
}
