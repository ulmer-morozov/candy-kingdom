import { Component, Input } from '@angular/core';

import { FaceModule } from './face.module';
import { FaceBoneMap } from './FaceBoneMap';
import { PageBase } from '@candy-kingdom/bonnie';

@Component({
  standalone: true,
  selector: 'app-face',
  templateUrl: './face.component.html',
  imports: [FaceModule]
})
export default class FaceComponent {
  public readonly FaceBoneMap = FaceBoneMap;

  @Input({ required: true })
  public page!: PageBase;
}
