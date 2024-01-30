import { EventEmitter, HostBinding, Component } from '@angular/core';

import { Bone } from '@candy-kingdom/bonnie';
import { ContentPreset, IBoneEditor } from '../skeleton-editor';
import { DeviceType } from '../core/DeviceType';

@Component({ template: '' })
export abstract class BoneEditorBaseComponent<TBone extends Bone> implements IBoneEditor<TBone> {
  public readonly editing: EventEmitter<boolean> = new EventEmitter<boolean>();
  public readonly saved: EventEmitter<TBone> = new EventEmitter<TBone>();
  public readonly removed: EventEmitter<void> = new EventEmitter<void>();

  public readonly noPresets: boolean;

  public locale: string;
  public device = DeviceType.NotSet;

  private _bone: TBone;
  private _storedData: string;

  private _currentPreset: ContentPreset<TBone>;

  protected readonly presets: ReadonlyArray<ContentPreset<TBone>>;
  private _isDirty = false;
  private _isEditing = false;

  public abstract onReset(): void;
  public abstract onFinishEditing(): void;

  protected abstract getPresets(): ContentPreset<TBone>[];

  constructor() {

    this.presets = this.getPresets();

    if (this.presets === undefined || this.presets === null)
      throw new Error('presets cannot be undefined in ContentPreset constructor');

    if (this.presets.length === 0) {
      this.presets = [
        new ContentPreset<TBone>({ title: 'default', isActive: () => true })
      ];
    }

    this.noPresets = this.presets.length === 1;
  }

  @HostBinding('class.mobile') get isMobile(): boolean {
    return this.device !== undefined && this.device === DeviceType.Mobile;
  }

  @HostBinding('class.tablet') get isTablet(): boolean {
    return this.device !== undefined && this.device === DeviceType.Tablet;
  }

  @HostBinding('class.desktop') get isDesktop(): boolean {
    return this.device !== undefined && this.device === DeviceType.Desktop;
  }

  public get currentPreset(): ContentPreset<TBone> {
    // console.log(`current preset: ${JSON.stringify(this._currentPreset)}`);
    return this._currentPreset;
  }

  public get bone(): TBone {
    return this._bone;
  }

  public get data(): TBone {
    return this._bone?.data;
  }

  public set bone(newData: TBone) {
    this._isDirty = false;
    this._storedData = JSON.stringify(newData);
    this._bone = JSON.parse(this._storedData);
    this.updatePresetByData();
  }

  public get isDirty(): boolean {
    return this._isDirty;
  }

  public get isEditing(): boolean {
    return this._isEditing;
  }

  public resetData(): void {
    this._isEditing = false;
    this.bone = JSON.parse(this._storedData);

    if (this.onReset !== undefined)
      this.onReset();

    this.editing.emit(false);
  }

  public updateDirty(): void {
    this._isDirty = JSON.stringify(this._bone) !== this._storedData;
  }

  public markAsDirty(): void {
    this._isDirty = true;
  }

  public remove(): void {
    this.removed.next();
  }

  public save(): void {
    if (!this.isDirty)
      return;

    this._isDirty = false;

    this.finishEditing();

    this._storedData = JSON.stringify(this._bone);

    const clonedData: TBone = JSON.parse(this._storedData);
    this.saved.emit(clonedData);
  }

  public startEditing(): void {
    if (this._isEditing) {
      this.updateDirty();
      return;
    }

    this._isEditing = true;
    this.updateDirty();

    this.editing.emit(true);
  }

  public finishEditing(): void {
    if (this._isDirty)
      throw new Error('Нельзя закрывать редактирование когда есть изменения. Надо сохранить либо зарезетить.');

    if (this.onFinishEditing !== undefined)
      this.onFinishEditing();

    this._isEditing = false;
  }

  public nextPreset(): void {
    const currentIndex = this.presets.indexOf(this._currentPreset);
    this.applyPresetAtIndex(currentIndex + 1);
    // console.log(`next preset: ${currentIndex + 1}`);
  }

  private applyPresetAtIndex = (newIndex: number): void => {
    newIndex = newIndex < 0 ? 0 : newIndex % this.presets.length;

    const currentIndex = this.presets.indexOf(this._currentPreset);

    if (currentIndex === newIndex)
      return;

    this._currentPreset = this.presets[newIndex];
    this._currentPreset.transformer(this._bone);
    this.updateDirty();
  }

  private updatePresetByData = (): void => {
    if (this.bone === undefined)
      return;

    const countOfActive = this.presets.map(p => p.isActive(this._bone)).filter(p => p).length;

    if (countOfActive !== 1)
      throw new Error(`active preset count should be equal 1, but it was: ${countOfActive}. ${this.constructor.name}`);

    for (let i = 0; this.presets.length; i++) {
      const preset = this.presets[i];

      if (preset.isActive(this._bone)) {
        this.applyPresetAtIndex(i);
        break;
      }
    }
  }
}
