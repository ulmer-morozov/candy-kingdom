import { ChangeDetectorRef, Component, HostBinding, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BoneDirective, MarcyMediaComponent, LocalizePipe, LocalizationIsNotEmptyPipe } from '@candy-kingdom/bonnie';
import { MediaBone, MediaBoneStyle } from '../../generated';

@Component({
  selector: 'app-media-bone',
  standalone: true,
  imports: [CommonModule, MarcyMediaComponent, LocalizePipe, LocalizationIsNotEmptyPipe],
  templateUrl: './media-bone.component.html',
  styleUrls: ['./media-bone.component.scss'],
  hostDirectives: [BoneDirective]
})
export class MediaBoneComponent implements OnInit {
  public readonly MediaBoneStyle = MediaBoneStyle;

  private readonly cd = inject(ChangeDetectorRef);
  public readonly bd = inject(BoneDirective<MediaBone>, { host: true });

  constructor() {
    this.cd.detach();
  }

  public ngOnInit(): void {
    this.cd.detectChanges();
  }

  @HostBinding('class')
  public get hostStyle(): string {
    return this.bd.bone.style;
  }
}
