import { Component, inject, input } from "@angular/core";

import { EditableDirective } from "./editable.directive";

@Component({
	template: "",
})
export abstract class FormBaseComponent<TData = unknown> {
	protected name = "";

	public readonly editable = inject(EditableDirective<TData>, { host: true });

	public readonly locale = input("");
}
