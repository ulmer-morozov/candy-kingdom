import * as MCore from '../generated';
import * as utils from '../utils';

import { IHtmlPictureSource } from './IHtmlPictureSource';

export function toHtmlPictureSources(imageSource: MCore.ImageSource): IHtmlPictureSource[] {
  const sizes = utils.generateSizesString(imageSource.sizes);
  const groupedByMime = utils.groupBy(imageSource.srcSet, x => x.mimeType);

  const simpleSources: IHtmlPictureSource[] = [];

  for (const mime in groupedByMime) {
    const fileSrcs = groupedByMime[mime].sort(utils.ascendingT(x => x.meta.width));

    const srcSet = fileSrcs
      .map(x => `${x.url} ${x.meta.width}w`)
      .join(',');

    const source: IHtmlPictureSource = { mime, sizes, srcSet, media: imageSource.mediaQuery };

    simpleSources.push(source);
  }

  return simpleSources;
}

