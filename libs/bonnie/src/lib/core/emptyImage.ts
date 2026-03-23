import type { Image } from "../generated";

export function emptyImage(): Image {
	return {
		$type: "image", // todo: remove
		type: "image",
		sources: [],
	};
}
