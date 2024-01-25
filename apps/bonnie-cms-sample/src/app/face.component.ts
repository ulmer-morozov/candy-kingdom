import { Component, Input } from '@angular/core';

import { FaceModule } from './face.module';
import { FaceBoneMap } from './FaceBoneMap';
import { LocalizeServiceBase, PageBase } from '@candy-kingdom/bonnie';
import { RouterLocalizeService } from './router-localize.service';

@Component({
  standalone: true,
  selector: 'app-face',
  templateUrl: './face.component.html',
  imports: [FaceModule],
  providers:
    [
      { provide: LocalizeServiceBase, useClass: RouterLocalizeService },
    ]
})
export default class FaceComponent {
  public readonly FaceBoneMap = FaceBoneMap;

  @Input({ required: true })
  public page!: PageBase;
}
