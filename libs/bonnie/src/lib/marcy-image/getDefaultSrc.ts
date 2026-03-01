import * as utils from "../core/utils";
import type * as M_CORE from "../generated";

export function getDefaultSrc(
	image: M_CORE.Image | undefined,
): M_CORE.FileSrc<M_CORE.ImageMeta> | undefined {
	if (image === undefined || image === null) return undefined;

	const files = image.sources.flatMap((x) => x.srcSet).sort(utils.descendingT((x) => x.meta.width));

	const prefferedFiles = files.filter(
		(x) => x.mimeType === "image/jpeg" || x.mimeType === "image/png" || x.mimeType === "image/gif",
	);

	if (prefferedFiles.length > 0) return prefferedFiles[0];

	if (files.length > 0) return files[0];

	return undefined;
}
