import { Directive, model, } from "@angular/core";

import { SrcBaseDirective } from "../core/src.directive";
import type * as M_CORE from "../generated";

@Directive({
	selector: "[imgsrc]",
})
export class ImageSrcDirective extends SrcBaseDirective<M_CORE.Image> {
	public readonly data = model<M_CORE.Image | undefined>(undefined, { alias: "imgsrc" });
}
