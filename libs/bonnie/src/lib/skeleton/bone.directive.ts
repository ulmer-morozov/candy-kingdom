import { Directive, model } from "@angular/core";
import type { Bone } from "../generated";

@Directive({
	selector: "[bonBoneDir]",
	standalone: true,
})
export class BoneDirective<T extends Bone = Bone> {
	public readonly bone = model.required<T>();
}
