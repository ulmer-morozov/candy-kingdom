import { MediaType } from '@candy-kingdom/bonnie-cms';

const mediaUploadMap = new Map<MediaType, string>();
mediaUploadMap.set('image', '/api/admin/upload/image/complex');
mediaUploadMap.set('video', '/api/admin/upload/video/complex');

export const MediaUploadMap: ReadonlyMap<MediaType, string> = mediaUploadMap;

