import { Directive, model } from "@angular/core";

import type { Bone } from "../generated";

@Directive({
	selector: "[bonBoneDir]",
})
export class BoneDirective<T extends Bone = Bone> {
	public readonly bone = model.required<T>();
}

export const BONE_DIRECTIVE_WITH_INPUTS_AND_OUTPUTS = {
	directive: BoneDirective,
	inputs: ["bone"],
	outputs: ["boneChange"],
};
