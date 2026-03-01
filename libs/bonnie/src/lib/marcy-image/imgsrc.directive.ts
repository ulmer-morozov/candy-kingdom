import { Directive, effect, input } from "@angular/core";

import { SrcBaseDirective } from "../core/src.directive";
import type * as M_CORE from "../generated";

@Directive({
	standalone: true,
	selector: "[imgsrc]",
})
export class ImageSrcDirective extends SrcBaseDirective<M_CORE.Image> {
	// todo: do not use effect for passing data
	public readonly imgsrc = input<M_CORE.Image | undefined>();

	constructor() {
		super();
		effect(() => {
			this.data.set(this.imgsrc());
		});
	}
}
