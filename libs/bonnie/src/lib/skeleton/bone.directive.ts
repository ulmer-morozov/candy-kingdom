import { Directive, model, type Type } from "@angular/core";

import type { Bone } from "../generated";

@Directive({
	selector: "[bonBoneDir]",
})
export class BoneDirective<T extends Bone = Bone> {
	public readonly bone = model.required<T>();
}

export function boneHostDirective<T extends Bone = Bone>(): {
	directive: Type<BoneDirective<T>>;
	inputs: string[];
	outputs: string[];
} {
	return {
		directive: BoneDirective<T>,
		inputs: ["bone"],
		outputs: ["boneChange"],
	};
}
