import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { APP_BASE_HREF } from '@angular/common';
import { Observable } from 'rxjs';

import { SettingGroup } from "@candy-kingdom/bonnie-cms";

@Injectable()
export class AdminDataService {
  constructor(private readonly http: HttpClient, @Inject(APP_BASE_HREF) private baseHref: string) {
    console.log('baseHref: ' + baseHref);
  }
  public getSettingGroups(): Observable<SettingGroup[]> {
    const pageOb = this.http.get<SettingGroup[]>(`${this.baseHref}api/admin/settings`);
    return pageOb;
  }
}

