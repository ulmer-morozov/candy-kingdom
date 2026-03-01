import { Bone } from "@candy-kingdom/bonnie";

export interface IBoneTemplate {
	readonly title: string;
	readonly boneFactory: () => Bone;
}
