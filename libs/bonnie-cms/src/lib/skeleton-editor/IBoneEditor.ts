import { OutputEmitterRef, ModelSignal, Signal, InputSignal } from "@angular/core";
import { Bone } from "@candy-kingdom/bonnie";
import { DeviceType } from "../core";
import { ContentPreset } from "./ContentPreset";

// todo: may be those shouldnt be generic?
export interface IBoneEditor<TBone extends Bone = Bone> {
	boneEtalon: ModelSignal<TBone>;

	locale: InputSignal<string>;
	device: InputSignal<DeviceType>;

	currentPreset: Signal<ContentPreset<TBone> | undefined>;
	noPresets: Signal<boolean>;

	readonly saved: OutputEmitterRef<Bone>;
	readonly removed: OutputEmitterRef<void>;
	readonly editing: OutputEmitterRef<boolean>;

	readonly isDirty: Signal<boolean>;
	readonly isEditing: Signal<boolean>;

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
