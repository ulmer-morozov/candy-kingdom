import * as MCore from '../generated';

// https://stackoverflow.com/questions/42136098/array-groupby-in-typescript

export const groupBy = <T, K extends keyof any>(arr: T[], key: (i: T) => K) =>
    arr.reduce((groups, item) => {
        (groups[key(item)] ||= []).push(item);
        return groups;
    }, {} as Record<K, T[]>);

export function distinct<T>(value: T, index: number, array: T[]) {
    return array.indexOf(value) === index;
}

export function ascending(a: number, b: number) {
    return a - b;
}

export function ascendingT<T>(sel: (x: T) => number): (a: T, b: T) => number {
    return function (a: T, b: T) { return ascending(sel(a), sel(b)) };
}

export function descending(a: number, b: number) {
    return b - a;
}

export function descendingT<T>(sel: (x: T) => number): (a: T, b: T) => number {
    return function (a: T, b: T) { return descending(sel(a), sel(b)) };
}

export function generateSizesString(sizes: MCore.SizesItem[]): string {
    const sizesString = sizes
        .sort(x => x.width)
        .map(x => `${x.mediaQuery} ${x.width}${x.unit}`)
        .join(',');

    return sizesString;
}

export function matchesMediaQuery(mediaQuery?: string): boolean {
    const isEmptyQuery = mediaQuery === null || mediaQuery === undefined || mediaQuery.trim().length === 0;

    // work for SSR and browser
    if (isEmptyQuery)
        return true;

    // other queries disallowed in SSR
    if (typeof window === 'undefined')
        return false;

    const mediaQueryList = window.matchMedia(mediaQuery);

    return mediaQueryList.matches;
}

export function isLocalUrlString(url: string): boolean {
    if (url === undefined || url === null || url.trim().length === 0)
        return true;

    if (url.startsWith('//'))
        return false;

    if (url.startsWith('./') || url.startsWith('/'))
        return true;

    return false;
}
