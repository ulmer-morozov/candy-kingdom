import { Component, type OnInit, input } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormBaseComponent } from "../../core-components/form-base.component";
import { EditableDirective } from "../../core-components/editable.directive";

@Component({
	selector: "bonc-unknown-form",
	standalone: true,
	imports: [CommonModule],
	templateUrl: "./unknown-form.component.html",
	styleUrls: ["./unknown-form.component.scss"],
	hostDirectives: [EditableDirective],
})
export class UnknownFormComponent extends FormBaseComponent<string> implements OnInit {
	public readonly label = input("");

	ngOnInit(): void {
		this.editable.externalSaveCall.subscribe(() => {
			this.editable.save();
		});
	}
}
