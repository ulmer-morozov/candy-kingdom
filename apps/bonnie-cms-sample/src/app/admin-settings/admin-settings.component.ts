import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminDataService } from '../admin-data.service';

import { BonnieCmsModule, SettingGroup, TextInputStyle } from "@candy-kingdom/bonnie-cms";
import { HttpClient } from '@angular/common/http';

@Component({
  standalone: true,
  imports: [CommonModule, BonnieCmsModule],
  providers: [AdminDataService],
  selector: 'app-admin-settings',
  templateUrl: './admin-settings.component.html',
})
export class AdminSettingsComponent implements OnInit {
  public readonly TextInputStyle = TextInputStyle;
  private readonly _dataService = inject(AdminDataService);
  private readonly _http = inject(HttpClient);

  public settingGroups: SettingGroup[] = [];

  public ngOnInit(): void {
    this._dataService.getSettingGroups().subscribe(x => {
      this.settingGroups = x;
    });
  }


  public save(): void {

    this._http.post('Api/Admin/SiteData', this.settingGroups)
      .subscribe
      (
        result => {
          console.log('успешно сохранили site data');
        },
        error => {
          alert('site data send error!');
        }
      );
  }
}
