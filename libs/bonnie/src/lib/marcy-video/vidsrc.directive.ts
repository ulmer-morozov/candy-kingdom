import { Directive, model } from "@angular/core";

import { SrcBaseDirective } from "../core/src.directive";
import type * as M_CORE from "../generated";

@Directive({
	selector: "[vidsrc]",
})
export class VideoSrcDirective extends SrcBaseDirective<M_CORE.Video> {
	public readonly data = model<M_CORE.Video | undefined>(undefined, { alias: "vidsrc" });
}
