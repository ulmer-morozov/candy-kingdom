import { Bone, Video, Image, LocalizedString } from '@candy-kingdom/bonnie';

export interface MediaBone extends Bone {
    media: Video | Image;
    title: LocalizedString;
    text: LocalizedString;
    alt: LocalizedString;
    link: LocalizedString;
}
