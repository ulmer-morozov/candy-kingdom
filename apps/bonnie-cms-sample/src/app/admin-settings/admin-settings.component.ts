import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';

import * as BONC from '@candy-kingdom/bonnie-cms';

const genericFileUploadMap = new Map<string, string>();

genericFileUploadMap.set("", "/api/admin/upload/file")

@Component({
  standalone: true,
  imports: [CommonModule, BONC.BonnieCmsModule],
  selector: 'app-admin-settings',
  templateUrl: './admin-settings.component.html',
})
export default class AdminSettingsComponent implements OnInit {
  public readonly TextInputStyle = BONC.TextInputStyle;
  public readonly TextSettingType = BONC.TextSettingType;
  public readonly TextEditorField = BONC.TextEditorField;

  public readonly genericFileUploadMap = genericFileUploadMap;

  private readonly _dataService = inject(BONC.AdminDataService);

  public settingGroups: BONC.SettingGroup[] = [];

  public ngOnInit(): void {
    this._dataService.getSettingGroups().subscribe(x => {
      this.settingGroups = x;
    });
  }

  public save(): void {
    try {
      const settings = this.settingGroups.flatMap(x => x.records);

      this._dataService.updateSettings(settings)
        .subscribe
        (
          () => {
            console.log('successfully updated settings'); // todo: add toast
          },
        );
    }

    catch (e) {
      console.error(e);
    }
  }
}
