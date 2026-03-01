import { Bone } from "@candy-kingdom/bonnie";

export interface ContentPreset<out TBone extends Bone> {
	title: string;
	isActive(data: TBone): boolean;
	clean(data: TBone): void;
	transformer(bone: TBone): void;
}

export function createPreset<TBone extends Bone>(params: {
	title: string;
	style: string;
}): ContentPreset<TBone> {
	return {
		title: params.title,
		isActive: (x) => x.style === params.style,
		transformer: (bone) => (bone.style = params.style),
		clean: (bone) => (bone.style = params.style),
	};
}
