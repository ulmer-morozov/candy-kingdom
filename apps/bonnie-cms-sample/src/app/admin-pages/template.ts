import { IBoneTemplate } from "@candy-kingdom/bonnie-cms";
import { Bone } from "@candy-kingdom/bonnie";

export function template<T extends Bone = Bone>(
	title: string,
	type: string,
	dataEtalon: T,
): IBoneTemplate {
	return {
		title,
		boneFactory: () => JSON.parse(JSON.stringify(dataEtalon)) as T,
	};
}
