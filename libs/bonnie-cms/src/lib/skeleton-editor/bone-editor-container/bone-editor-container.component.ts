import { Component, ComponentFactoryResolver, EventEmitter, Input, OnChanges, Output, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';

import { Bone } from '@candy-kingdom/bonnie';

import { SkeletonEditorAnchorDirective } from '../skeleton-editor-anchor.directive';
import { DeviceType } from '../../core';
import { IBoneEditor } from '../IBoneEditor';
import { BoneEditorMap } from '../BoneEditorMap';

// todo: rename class
@Component({
  selector: 'bonc-bone-editor-container',
  templateUrl: './bone-editor-container.component.html',
  styleUrls: ['./bone-editor-container.component.scss']
})
export class BoneEditorContainerComponent implements OnChanges {
  @ViewChild(SkeletonEditorAnchorDirective, { static: true })
  public anchor!: SkeletonEditorAnchorDirective;

  @Output()
  public removed: EventEmitter<void> = new EventEmitter<void>();

  @Output()
  public saved: EventEmitter<Bone> = new EventEmitter<Bone>();

  @Output()
  public editing: EventEmitter<boolean> = new EventEmitter<boolean>();

  public DeviceType = DeviceType;

  public editor!: IBoneEditor;

  public themePopupIsShown = false;

  private _bone!: Bone;

  private removeSubscription?: Subscription;
  private saveSubscription?: Subscription;
  private changedSubscription?: Subscription;

  @Input({ required: true })
  public locale!: string;

  @Input()
  public device = DeviceType.NotSet;

  constructor(private readonly componentFactoryResolver: ComponentFactoryResolver) {
  }

  ngOnChanges(): void {
    if (this.editor === undefined || this.editor === null)
      return;

    this.editor.locale = this.locale;
    this.editor.device = this.device;
  }

  @Input({ required: true })
  public map!: BoneEditorMap

  public get bone(): Bone {
    return this._bone;
  }

  @Input({ required: true })
  public set bone(newBone: Bone) {
    this._bone = newBone;

    if (this.removeSubscription) {
      this.removeSubscription.unsubscribe();
      this.removeSubscription = undefined;
    }

    if (this.saveSubscription) {
      this.saveSubscription.unsubscribe();
      this.saveSubscription = undefined;
    }

    if (this.changedSubscription) {
      this.changedSubscription.unsubscribe();
      this.changedSubscription = undefined;
    }

    const viewContainerRef = this.anchor.viewContainerRef;

    viewContainerRef.clear();

    const componentType = this.map.getRequired(newBone.type);

    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(componentType);
    const boneEditorRef = viewContainerRef.createComponent(componentFactory);

    this.editor = boneEditorRef.instance;
    this.editor.bone = newBone;

    this.removeSubscription = this.editor.removed.subscribe(() => {
      this.removed.next();
    });

    this.changedSubscription = this.editor.editing.subscribe
      (
        (isEditing: boolean) => {
          this.editing.next(isEditing);
        }
      );

    this.saveSubscription = this.editor.saved.subscribe(
      (newBoneValue: Bone) => {
        this.saved.next(newBoneValue);
      }
    );

    this.ngOnChanges();
  }

  public nextPreset = (): void => {
    if (this.editor === undefined || this.editor === null)
      return;

    this.editor.nextPreset();
  }

  // todo: add or remove visibility feature

  // public setDisabled = (disabled: boolean): void => {
  //   if (this.editor === undefined || this.editor === null)
  //     return;

  //   this.editor.startEditing();

  //   if (this.device === DeviceType.Desktop)
  //     this.editor.bone.visibility = setOrRemoveFlag(this.editor.bone.visibility, DeviceVisibility.Desktop, !disabled);
  //   else if (this.device === DeviceType.Tablet)
  //     this.editor.bone.visibility = setOrRemoveFlag(this.editor.bone.visibility, DeviceVisibility.Tablet, !disabled);
  //   else if (this.device === DeviceType.Mobile)
  //     this.editor.bone.visibility = setOrRemoveFlag(this.editor.bone.visibility, DeviceVisibility.Mobile, !disabled);

  //   this.editor.updateDirty();
  // }

  public get disabled(): boolean {
    if (this.editor === undefined || this.editor === null)
      throw new Error('editor should have been set');

    return false;
    // todo: add or remove visibility feature
    // const visibility = this.editor.bone.visibility;

    // if (this.device === DeviceType.Desktop && hasFlag(visibility, DeviceVisibility.Desktop))
    //   return false;

    // if (this.device === DeviceType.Tablet && hasFlag(visibility, DeviceVisibility.Tablet))
    //   return false;

    // if (this.device === DeviceType.Mobile && hasFlag(visibility, DeviceVisibility.Mobile))
    //   return false;

    // return true;
  }
}
