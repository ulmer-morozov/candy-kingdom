import { Component } from '@angular/core';

import { FaceModule } from './face.module';
import { FaceBoneMap } from './FaceBoneMap';

@Component({
  standalone: true,
  selector: 'app-face',
  templateUrl: './face.component.html',
  imports: [FaceModule]
})
export default class FaceComponent {
  public readonly FaceBoneMap = FaceBoneMap;
}
