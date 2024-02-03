import { emptyBone, emptyLocalizedString } from "@candy-kingdom/bonnie";
import { TextBone } from "../generated";


export function emptyTextBone(): TextBone {
  return {
    $type: 'text',// todo: remove
    ...emptyBone(),
    type: 'text',
    text: emptyLocalizedString(),
    title: emptyLocalizedString()
  };
}
