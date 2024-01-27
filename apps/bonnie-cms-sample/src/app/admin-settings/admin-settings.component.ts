import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminDataService } from '../admin-data.service';

import { SettingGroup } from "@candy-kingdom/bonnie-cms";

@Component({
  standalone: true,
  imports: [CommonModule],
  providers: [AdminDataService],
  selector: 'app-admin-settings',
  templateUrl: './admin-settings.component.html',
})
export class AdminSettingsComponent implements OnInit {
  private readonly _dataService = inject(AdminDataService);

  public settingGroups: SettingGroup[] = [];

  public ngOnInit(): void {
    this._dataService.getSettingGroups().subscribe(x => {
      this.settingGroups = x;
    });
  }
}
