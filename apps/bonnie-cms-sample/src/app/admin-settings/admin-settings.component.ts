import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminDataService } from '../admin-data.service';

import { BonnieCmsModule, SettingGroup, TextInputStyle, TextEditorField, TextSettingType } from '@candy-kingdom/bonnie-cms';
import { HttpClient } from '@angular/common/http';

@Component({
  standalone: true,
  imports: [CommonModule, BonnieCmsModule],
  providers: [AdminDataService],
  selector: 'app-admin-settings',
  templateUrl: './admin-settings.component.html',
})
export default class AdminSettingsComponent implements OnInit {
  public readonly TextInputStyle = TextInputStyle;
  public readonly TextSettingType = TextSettingType;
  public readonly TextEditorField = TextEditorField;

  private readonly _dataService = inject(AdminDataService);
  private readonly _http = inject(HttpClient);

  public settingGroups: SettingGroup[] = [];

  public ngOnInit(): void {
    this._dataService.getSettingGroups().subscribe(x => {
      this.settingGroups = x;
    });
  }

  public save(): void {

    // try {
    //   this._http.post('api/Admin/Pages', this.settingGroups)
    //     .subscribe
    //     (
    //       () => {
    //         debugger;
    //         console.log('successfully stored data'); // todo: add toast
    //       },
    //     );
    // }

    // catch (e) {
    //   console.error(e);
    // }


  }
}
