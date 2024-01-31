import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { BoneDirective } from '@candy-kingdom/bonnie';

import { TextBone } from '../../generated';

@Component({
  selector: 'app-text-bone',
  templateUrl: './text-bone.component.html',
  styleUrls: ['./text-bone.component.scss'],
  hostDirectives: [BoneDirective]
})
export class TextBoneComponent implements OnInit {
  constructor(private readonly cd: ChangeDetectorRef, public readonly bd: BoneDirective<TextBone>) {
    cd.detach();
  }

  public ngOnInit(): void {
    this.cd.detectChanges();
  }
}
