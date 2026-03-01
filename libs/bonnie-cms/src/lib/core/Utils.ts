// tslint:disable-next-line: max-line-length
export const regExpIsMobile = new RegExp(
	/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/,
	"i",
);

export function isLocalUrlString(url: string): boolean {
	if (url === undefined || url === null || url.trim().length === 0) return true;

	if (url.startsWith("//")) return false;

	if (url.startsWith("./") || url.startsWith("/")) return true;

	return false;
}

export function hasFlag<T extends number>(value: T, flag: T): boolean {
	return (value & flag) === flag;
}

export function setOrRemoveFlag<T extends number>(value: T, flag: T, enabled: boolean): T {
	return (enabled ? value | flag : value & ~flag) as T;
}
