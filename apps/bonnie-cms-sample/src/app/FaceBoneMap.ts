import { BoneMap } from "@candy-kingdom/bonnie";

import { MediaBoneComponent } from "./bone-components/media-bone/media-bone.component";
import { PageListBoneComponent } from "./bone-components/page-list-bone/page-list-bone.component";
import { TextBoneComponent } from "./bone-components/text-bone/text-bone.component";

const fullMap = new BoneMap();

fullMap.set("text", TextBoneComponent);
fullMap.set("media", MediaBoneComponent);
fullMap.set("page-list", PageListBoneComponent);

export const FaceBoneMap: Readonly<BoneMap> = fullMap;
