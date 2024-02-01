import { Component, Input } from '@angular/core';

import { DeviceType } from '../core';
import { EditableGroupComponent } from '../core-components';

@Component({
  selector: 'bonc-admin-controls',
  templateUrl: './admin-controls.component.html',
  styleUrls: ['./admin-controls.component.scss']
})
export class AdminControlsComponent {
  public readonly DeviceType = DeviceType;

  @Input({ required: true })
  public editableGroup!: EditableGroupComponent;

  @Input()
  public deviceControls = false;

  public locale = 'en';
  public device = this.DeviceType.Desktop;

  public changeLocale(): void {
    this.locale = this.locale === 'en' ? 'ru' : 'en';
  }

  public changeDevice(): void {
    switch (this.device) {
      case DeviceType.Desktop:
        this.device = DeviceType.Tablet;
        return;

      case DeviceType.Tablet:
        this.device = DeviceType.Mobile;
        return;

      case DeviceType.Mobile:
        this.device = DeviceType.Desktop;
        return;

      default:
        this.device = DeviceType.Desktop;
        return;
    }
  }
}
