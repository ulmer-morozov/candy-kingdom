import { Directive, effect, input } from "@angular/core";

import type * as M_CORE from "../generated";

import { SrcBaseDirective } from "../core/src.directive";

@Directive({
	standalone: true,
	selector: "[vidsrc]",
})
export class VideoSrcDirective extends SrcBaseDirective<M_CORE.Video> {
	public readonly vidsrc = input<M_CORE.Video | undefined>();

	constructor() {
		super();
		effect(() => {
			this.data.set(this.vidsrc());
		});
	}
}
