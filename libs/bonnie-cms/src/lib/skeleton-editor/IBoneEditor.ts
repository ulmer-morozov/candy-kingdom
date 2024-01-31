import { EventEmitter } from '@angular/core';
import { Bone } from '@candy-kingdom/bonnie';
import { DeviceType } from '../core';
import { ContentPreset } from './ContentPreset';

// todo: may be those shouldnt be generic?
export interface IBoneEditor<out TBone extends Bone = Bone> {
  bone: TBone;
  locale: string;
  device: DeviceType;

  readonly saved: EventEmitter<Bone>;
  readonly removed: EventEmitter<void>;
  readonly editing: EventEmitter<boolean>;

  readonly isDirty: boolean;
  readonly isEditing: boolean;

  currentPreset?: ContentPreset<TBone>;
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
