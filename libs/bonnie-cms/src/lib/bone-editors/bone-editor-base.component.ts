import {
	Component,
	computed,
	effect,
	HostBinding,
	input,
	model,
	output,
	signal,
} from "@angular/core";

import type { Bone } from "@candy-kingdom/bonnie";

import { DeviceType } from "../core";
import type { ContentPreset } from "../skeleton-editor/ContentPreset";
import type { IBoneEditor } from "../skeleton-editor/IBoneEditor";

@Component({ template: "" })
export abstract class BoneEditorBaseComponent<TBone extends Bone> implements IBoneEditor<TBone> {
	public readonly editing = output<boolean>();
	public readonly saved = output<Bone>();
	public readonly removed = output<void>();

	public readonly boneEtalon = model.required<TBone>();

	public readonly locale = input.required<string>();
	public readonly device = input<DeviceType>(DeviceType.NotSet);

	private readonly _presets = signal<ContentPreset<TBone>[]>([]);
	public readonly presets = this._presets.asReadonly();

	private readonly _currentPreset = signal<ContentPreset<TBone> | undefined>(undefined);
	public readonly currentPreset = this._currentPreset.asReadonly();

	private readonly _isDirty = signal(false);
	public readonly isDirty = this._isDirty.asReadonly();

	private readonly _isEditing = signal(false);
	public readonly isEditing = this._isEditing.asReadonly();

	public readonly noPresets = computed(() => this.presets().length === 0);

	// effect gonna fix this undefined value
	// eslint-disable-next-line @typescript-eslint/no-non-null-assertion
	protected readonly bone = signal<TBone>(undefined!);

	private _storedBoneJson = "";

	public abstract onReset(): void;
	public abstract onFinishEditing(): void;

	protected abstract getPresets(): ContentPreset<TBone>[];

	constructor() {
		effect(() => {
			const newData = this.boneEtalon();
			this._isDirty.set(false);
			this._storedBoneJson = JSON.stringify(newData);
			this.bone.set(JSON.parse(this._storedBoneJson));
			this.updatePresetByData();
		});

		const presets = this.getPresets();

		if (presets.length === 0) {
			presets.push({
				title: "default",
				isActive: () => true,
				transformer: (bone) => bone,
				clean: (bone) => bone,
			});
		}

		// todo: remove this, use just regular presets
		this._presets.set(presets);
	}

	@HostBinding("class.mobile") get isMobile(): boolean {
		return this.device() === DeviceType.Mobile;
	}

	@HostBinding("class.tablet") get isTablet(): boolean {
		return this.device() === DeviceType.Tablet;
	}

	@HostBinding("class.desktop") get isDesktop(): boolean {
		return this.device() === DeviceType.Desktop;
	}

	public resetData(): void {
		this._isEditing.set(false);

		this.boneEtalon.set(JSON.parse(this._storedBoneJson));
		this.updatePresetByData();

		if (this.onReset !== undefined) this.onReset();

		this.editing.emit(false);
	}

	public updateDirty(): void {
		this._isDirty.set(JSON.stringify(this.bone()) !== this._storedBoneJson);
	}

	public markAsDirty(): void {
		this._isDirty.set(true);
	}

	public remove(): void {
		this.removed.emit();
	}

	public save(): void {
		if (!this.isDirty()) return;

		this._isDirty.set(false);

		this.finishEditing();

		this._storedBoneJson = JSON.stringify(this.bone());

		const clonedData: TBone = JSON.parse(this._storedBoneJson);
		this.saved.emit(clonedData);
	}

	public startEditing(): void {
		if (this.isEditing()) {
			this.updateDirty();
			return;
		}

		this._isEditing.set(true);
		this.updateDirty();

		this.editing.emit(true);
	}

	public finishEditing(): void {
		if (this._isDirty())
			throw new Error(
				"Нельзя закрывать редактирование когда есть изменения. Надо сохранить либо зарезетить.",
			);

		if (this.onFinishEditing !== undefined) this.onFinishEditing();

		this._isEditing.set(false);
	}

	public nextPreset(): void {
		const currentPreset = this.currentPreset();
		const newIndex = currentPreset === undefined ? 0 : this.presets().indexOf(currentPreset) + 1;

		this.applyPresetAtIndex(newIndex);
	}

	private applyPresetAtIndex = (newIndex: number): void => {
		newIndex = newIndex < 0 ? 0 : newIndex % this.presets().length;

		const currentPreset = this.currentPreset();

		const currentIndex = currentPreset === undefined ? -1 : this.presets().indexOf(currentPreset);
		if (currentIndex === newIndex) return;

		const newPreset = this.presets()[newIndex];

		this._currentPreset.set(newPreset);
		newPreset.transformer(this.bone());

		this.updateDirty();
	};

	private updatePresetByData = (): void => {
		const countOfActive = this.presets()
			.map((p) => p.isActive(this.bone()))
			.filter((p) => p).length;

		if (countOfActive !== 1)
			throw new Error(
				`active preset count should be equal 1, but it was: ${countOfActive}. ${this.constructor.name}`,
			);

		for (let i = 0; i < this.presets().length; i++) {
			const preset = this.presets()[i];

			if (preset.isActive(this.bone())) {
				this.applyPresetAtIndex(i);
				break;
			}
		}
	};
}
