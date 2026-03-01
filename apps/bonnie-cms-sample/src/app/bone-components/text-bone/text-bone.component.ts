import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BoneDirective, LocalizePipe, LocalizationIsNotEmptyPipe } from '@candy-kingdom/bonnie';

import { TextBone } from '../../generated';

@Component({
  selector: 'app-text-bone',
  standalone: true,
  imports: [CommonModule, LocalizePipe, LocalizationIsNotEmptyPipe],
  templateUrl: './text-bone.component.html',
  styleUrls: ['./text-bone.component.scss'],
  hostDirectives: [BoneDirective]
})
export class TextBoneComponent {
  public readonly bd = inject(BoneDirective<TextBone>, { host: true });
}
