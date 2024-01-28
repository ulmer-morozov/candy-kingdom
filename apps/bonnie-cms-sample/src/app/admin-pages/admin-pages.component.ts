import { Component, Input, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminDataService } from '../admin-data.service';

import { BonnieCmsModule } from "@candy-kingdom/bonnie-cms";
import { HttpClient } from '@angular/common/http';
import { BonnieModule, Page, PageData } from '@candy-kingdom/bonnie';
import { RouterLink } from '@angular/router';

@Component({
  standalone: true,
  imports: [CommonModule, RouterLink, BonnieModule, BonnieCmsModule],
  providers: [AdminDataService],
  selector: 'app-admin-pages',
  templateUrl: './admin-pages.component.html',
})
export default class AdminPagesComponent implements OnInit {
  private readonly _dataService = inject(AdminDataService);
  private readonly _http = inject(HttpClient);

  @Input({ required: true })
  public page!: Page<PageData>;

  public ngOnInit(): void {

  }

  public save(): void {

    // this._http.post('Api/Admin/SiteData', this.settingGroups)
    //   .subscribe
    //   (
    //     result => {
    // console.log('successfully stored data');
    //     },
    //     error => {
    //       alert('site data send error!');
    //     }
    //   );
  }
}
