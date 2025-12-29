import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { BoneDirective } from '@candy-kingdom/bonnie';

import { TextBone } from '../../generated';

@Component({
  selector: 'app-text-bone',
  templateUrl: './text-bone.component.html',
  styleUrls: ['./text-bone.component.scss'],
  hostDirectives: [BoneDirective]
})
export class TextBoneComponent implements OnInit {
  private readonly cd = inject(ChangeDetectorRef);
  public readonly bd = inject(BoneDirective<TextBone>, { host: true });

  constructor() {
    this.cd.detach();
  }

  public ngOnInit(): void {
    this.cd.detectChanges();
  }
}
