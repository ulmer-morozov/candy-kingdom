import { Directive, model } from "@angular/core";
import { Bone } from "../generated";

@Directive({
	selector: "[bonBoneDir]",
	standalone: true,
})
export class BoneDirective<T extends Bone = Bone> {
	public readonly bone = model.required<T>();
}
