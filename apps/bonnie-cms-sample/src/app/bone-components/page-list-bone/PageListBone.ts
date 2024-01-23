import { Bone, LocalizedString, IHaveDataRouteWithData, PageBase } from '@candy-kingdom/bonnie';

export interface PageListBone extends Bone, IHaveDataRouteWithData<PageBase[]> {
    title: LocalizedString;
}


