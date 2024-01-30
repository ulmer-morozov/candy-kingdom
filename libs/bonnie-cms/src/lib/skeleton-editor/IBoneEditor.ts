import { EventEmitter } from '@angular/core';
import { Bone } from '@candy-kingdom/bonnie';
import { DeviceType } from '../core';
import { ContentPreset } from './ContentPreset';

export interface IBoneEditor<TBone extends Bone = Bone> {
  bone: TBone;
  locale: string;
  device: DeviceType;

  readonly saved: EventEmitter<TBone>;
  readonly removed: EventEmitter<void>;
  readonly editing: EventEmitter<boolean>;

  readonly isDirty: boolean;
  readonly isEditing: boolean;

  currentPreset: ContentPreset<TBone>;
  noPresets: boolean;

  save(): void;
  remove(): void;
  resetData(): void;

  startEditing(): void;
  finishEditing(): void;

  markAsDirty(): void;
  updateDirty(): void;

  nextPreset(): void;

  onReset(): void;
  onFinishEditing(): void;
}
