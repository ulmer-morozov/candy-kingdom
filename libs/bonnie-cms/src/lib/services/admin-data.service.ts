import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";

import type { Observable } from "rxjs";

import type { PageBase } from "@candy-kingdom/bonnie";

import type { SettingBase, SettingGroup } from "../generated";
import { API_BASE_URL } from "./API_BASE_URL";

@Injectable()
export class AdminDataService {
	private readonly http = inject(HttpClient);
	private readonly baseHref = inject(API_BASE_URL);

	constructor() {
		console.log(`baseHref: ${this.baseHref}`);
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
