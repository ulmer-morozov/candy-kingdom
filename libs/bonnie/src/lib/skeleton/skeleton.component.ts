import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnInit, Type, ViewChild } from '@angular/core';
import { SkeletonAnchorDirective } from './skeleton-anchor.directive';
import { IBoneComponent } from "./IBoneComponent";
import { Bone } from '../generated';
import { UnknownBoneComponent } from './unknown-bone.component';

@Component({
  selector: 'bon-skeleton',
  templateUrl: './skeleton.component.html',
  styleUrls: ['./skeleton.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SkeletonComponent implements OnInit {
  @ViewChild(SkeletonAnchorDirective, { static: true })
  public skeletonAnchor!: SkeletonAnchorDirective;

  @Input({ required: true })
  public map?: Map<string, Type<IBoneComponent>>;

  private readonly _bones: Bone[] = [];

  private iniailized = false;

  constructor(private cd: ChangeDetectorRef) {
  }

  ngOnInit(): void {
    this.iniailized = true;
    this.fillComponentFromBones();
  }

  @Input()
  public set bones(newValue: Bone[] | undefined) {
    this._bones.splice(0, this._bones.length);
    this._bones.push(...newValue ?? []);

    this.fillComponentFromBones();
  }

  private fillComponentFromBones(): void {
    if (this.iniailized === false)
      return;

    const viewContainerRef = this.skeletonAnchor.viewContainerRef;
    viewContainerRef.clear();

    if (this.map === undefined || this.map === null)
      throw new Error('add type map with input: [map]="..."');

    for (const bone of this._bones) {
      let componentType = this.map.get(bone.type);

      if (componentType === undefined || componentType === null) {
        console.warn(`Mapping type for ${bone.type} not found`);
        componentType = UnknownBoneComponent;
      }

      const boneComponentRef = viewContainerRef.createComponent(componentType);

      boneComponentRef.instance.bd.bone = bone;
    }
  }

}
