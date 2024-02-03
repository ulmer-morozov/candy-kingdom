import { emptyBone, emptyImage, emptyLocalizedString } from "@candy-kingdom/bonnie";
import { MediaBone } from "../generated";

export function emptyMediaBone(): MediaBone {
  return {
    ...emptyBone(),
    type: 'media',
    media: emptyImage(),
    title: emptyLocalizedString(),
    text: emptyLocalizedString(),
    link: emptyLocalizedString(),
    alt: emptyLocalizedString()
  };
}


