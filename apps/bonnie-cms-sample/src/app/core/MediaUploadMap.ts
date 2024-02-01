import { MediaType } from '@candy-kingdom/bonnie-cms';

const mediaUploadMap = new Map<MediaType, string>();
mediaUploadMap.set('image', '/Api/Admin/Upload/Image/Complex');
mediaUploadMap.set('video', '/Api/Admin/Upload/Image/Complex');

export const MediaUploadMap: ReadonlyMap<MediaType, string> = mediaUploadMap;

