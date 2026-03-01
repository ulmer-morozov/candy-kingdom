import { Directive, effect, input } from "@angular/core";
import * as MCore from "../generated";
import { SrcBaseDirective } from "../core/src.directive";

@Directive({
	standalone: true,
	selector: "[imgsrc]",
})
export class ImageSrcDirective extends SrcBaseDirective<MCore.Image> {
	// todo: do not use effect for passing data
	public readonly imgsrc = input<MCore.Image | undefined>();

	constructor() {
		super();
		effect(() => {
			this.data.set(this.imgsrc());
		});
	}
}
