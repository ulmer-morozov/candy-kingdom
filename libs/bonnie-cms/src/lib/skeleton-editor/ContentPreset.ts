import { Bone } from '@candy-kingdom/bonnie';

export interface ContentPresetParams<TBone extends Bone> {
  title: string;
  isActive: (data: TBone) => boolean;
  clean?: (data: TBone) => void;
  transformer?: (bone: TBone) => void;
}

export class ContentPreset<TBone extends Bone> {
  public readonly title: string;
  public readonly isActive: (data: TBone) => boolean;

  public readonly clean: (data: TBone) => void;
  public readonly transformer: (bone: TBone) => void;

  constructor(params: ContentPresetParams<TBone>) {

    this.title = params.title ?? 'default preset title';
    this.isActive = params.isActive;

    this.clean = params.clean ?? (() => { });
    this.transformer = params.transformer ?? (() => { });
  }
}

export function createPreset<TBone extends Bone>(params: { title: string, style: string }): ContentPreset<TBone> {
  return new ContentPreset<TBone>({
    title: `[${params.style}] \n${params.title}`,
    isActive: x => x.style === params.style,
    transformer: bone => bone.style = params.style
  });
}
