import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from 'rxjs';

import { PageBase } from "@candy-kingdom/bonnie";

import { SettingBase, SettingGroup } from "../generated";
import { API_BASE_URL } from "./API_BASE_URL";

@Injectable()
export class AdminDataService {
  constructor(private readonly http: HttpClient, @Inject(API_BASE_URL) private baseHref: string) {
    console.log('baseHref: ' + baseHref);
  }

  public getSettingGroups(): Observable<SettingGroup[]> {
    const pageOb = this.http.get<SettingGroup[]>(`${this.baseHref}api/admin/settings`);
    return pageOb;
  }

  public getPage(url: string): Observable<SettingGroup[]> {
    const pageOb = this.http.get<SettingGroup[]>(`${this.baseHref}api/admin/pages/${url}`);
    return pageOb;
  }

  public storePage(page: PageBase): Observable<void> {
    const ob = this.http.post<void>(`${this.baseHref}api/admin/pages`, page);
    return ob;
  }

  public updateSettings(settings: SettingBase[]): Observable<void> {
    const ob = this.http.post<void>(`${this.baseHref}api/admin/settings`, settings);
    return ob;
  }
}

