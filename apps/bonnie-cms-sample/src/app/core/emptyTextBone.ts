import { emptyBone, emptyLocalizedString } from "@candy-kingdom/bonnie";
import { TextBone } from "../generated";


export function emptyTextBone(): TextBone {
  return {
    ...emptyBone(),
    type: 'text',
    text: emptyLocalizedString(),
    title: emptyLocalizedString()
  };
}
