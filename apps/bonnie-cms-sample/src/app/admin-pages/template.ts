import type { Bone } from "@candy-kingdom/bonnie";
import type { IBoneTemplate } from "@candy-kingdom/bonnie-cms";

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
