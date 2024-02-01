import { ChangeDetectorRef, Component, HostBinding, OnInit } from '@angular/core';

import { BoneDirective } from '@candy-kingdom/bonnie';
import { MediaBone, MediaBoneStyle } from '../../generated';

@Component({
  selector: 'app-media-bone',
  templateUrl: './media-bone.component.html',
  styleUrls: ['./media-bone.component.scss'],
  hostDirectives: [BoneDirective]
})
export class MediaBoneComponent implements OnInit {
  public readonly MediaBoneStyle = MediaBoneStyle;

  constructor(private readonly cd: ChangeDetectorRef, public readonly bd: BoneDirective<MediaBone>) {
    cd.detach();
  }

  public ngOnInit(): void {
    this.cd.detectChanges();
  }

  @HostBinding('class')
  public get hostStyle(): string {
    return this.bd.bone.style;
  }
}
