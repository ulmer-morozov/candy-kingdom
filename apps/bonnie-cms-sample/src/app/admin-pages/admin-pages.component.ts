import { Component, Input, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminDataService } from '../admin-data.service';

import { BonnieCmsModule, IBoneTemplate, TextEditorField } from "@candy-kingdom/bonnie-cms";
import { HttpClient } from '@angular/common/http';
import { Bone, BonnieModule, Page, PageData } from '@candy-kingdom/bonnie';
import { RouterLink } from '@angular/router';

import { AdminBoneMap } from '../AdminBoneMap';
import { MediaBoneEditorComponent, TextBoneEditorComponent } from '../bone-editors';


export function template<T extends Bone = Bone>(title: string, type: string, dataEtalon: T): IBoneTemplate {
  return {
    title,
    boneFactory: () => JSON.parse(JSON.stringify(dataEtalon)) as T
  };
}

const boneEditors = [
  TextBoneEditorComponent,
  MediaBoneEditorComponent
];

@Component({
  standalone: true,
  imports: [CommonModule, RouterLink, BonnieModule, BonnieCmsModule, ...boneEditors],
  providers: [AdminDataService],
  selector: 'app-admin-pages',
  templateUrl: './admin-pages.component.html',
})
export default class AdminPagesComponent implements OnInit {
  public readonly TextEditorField = TextEditorField;
  public readonly AdminBoneMap = AdminBoneMap;

  public readonly templates: IBoneTemplate[] = [];

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
