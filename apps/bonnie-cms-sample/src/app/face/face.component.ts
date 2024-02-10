import { Component, Input } from '@angular/core';

import { FaceModule } from '../face.module';
import { FaceBoneMap } from '../FaceBoneMap';
import { LocalizeServiceBase, PageBase, View } from '@candy-kingdom/bonnie';
import { RouterLocalizeService } from '../router-localize.service';
import { RouterLink } from '@angular/router';
import { SettingDataDict } from '@candy-kingdom/bonnie-cms';

@Component({
  standalone: true,
  selector: 'app-face',
  templateUrl: './face.component.html',
  styleUrl: './face.component.scss',
  imports: [FaceModule, RouterLink],
  providers:
    [
      { provide: LocalizeServiceBase, useClass: RouterLocalizeService },
    ]
})
export default class FaceComponent {
  public readonly FaceBoneMap = FaceBoneMap;

  @Input({ required: true })
  public page!: PageBase;

  @Input({ required: true })
  public faceView!: View;

  @Input({ required: true })
  public settings!: SettingDataDict;
}
