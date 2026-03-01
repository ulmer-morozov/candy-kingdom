import { ChangeDetectionStrategy, Component, effect, input, OnInit, Type, ViewChild } from '@angular/core';
import { SkeletonAnchorDirective } from './skeleton-anchor.directive';
import { IBoneComponent } from "./IBoneComponent";
import { Bone } from '../generated';
import { UnknownBoneComponent } from './unknown-bone.component';

@Component({
  selector: 'bon-skeleton',
  standalone: true,
  imports: [SkeletonAnchorDirective],
  templateUrl: './skeleton.component.html',
  styleUrls: ['./skeleton.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SkeletonComponent implements OnInit {
  @ViewChild(SkeletonAnchorDirective, { static: true })
  public skeletonAnchor!: SkeletonAnchorDirective;

  public readonly map = input.required<Map<string, Type<IBoneComponent>>>();

  public readonly bones = input.required<Bone[]>();

  private readonly _bones: Bone[] = [];

  private iniailized = false;

  constructor() {
    effect(() => {
      const newBones = this.bones();
      this._bones.splice(0, this._bones.length);
      this._bones.push(...newBones);
      if (this.iniailized) {
        this.fillComponentFromBones();
      }
    });
  }

  ngOnInit(): void {
    this.iniailized = true;
    this.fillComponentFromBones();
  }

  private fillComponentFromBones(): void {
    if (this.iniailized === false)
      return;

    const viewContainerRef = this.skeletonAnchor.viewContainerRef;
    viewContainerRef.clear();

    const mapVal = this.map();
    if (mapVal === undefined || mapVal === null)
      throw new Error('add type map with input: [map]="..."');

    for (const bone of this._bones) {
      let componentType = mapVal.get(bone.type);

      if (componentType === undefined || componentType === null) {
        console.warn(`Mapping type for ${bone.type} not found`);
        componentType = UnknownBoneComponent;
      }

      viewContainerRef.createComponent(componentType).setInput("bone", bone);
    }
  }

}
