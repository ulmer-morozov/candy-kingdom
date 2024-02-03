import { emptyBone, emptyLocalizedString } from "@candy-kingdom/bonnie";
import { PageListBone } from "../generated";


export function emptyPageListBone(): PageListBone {
  return {
    $type: 'page-list',// todo: remove
    ...emptyBone(),
    type: 'page-list',
    title: emptyLocalizedString(),
    dataRoute: '~',
    data: []
  };
}
