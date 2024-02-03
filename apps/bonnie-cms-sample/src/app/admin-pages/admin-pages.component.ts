import { Component, Input, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminDataService } from '../admin-data.service';

import { BonnieCmsModule, IBoneTemplate, TextEditorField } from "@candy-kingdom/bonnie-cms";
import { HttpClient } from '@angular/common/http';
import { BonnieModule, Page, PageData } from '@candy-kingdom/bonnie';
import { RouterLink } from '@angular/router';

import { AdminBoneMap } from '../AdminBoneMap';
import { MediaBoneEditorComponent, TextBoneEditorComponent } from '../bone-editors';
import { emptyMediaBone } from '../core/emptyMediaBone';
import { emptyTextBone } from '../core';
import { emptyPageListBone } from '../core/emptyPageListBone';

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
  styleUrl: './admin-pages.component.scss'
})
export default class AdminPagesComponent {
  public readonly TextEditorField = TextEditorField;
  public readonly AdminBoneMap = AdminBoneMap;

  public readonly templates: IBoneTemplate[] = [
    { title: 'media', boneFactory: emptyMediaBone },
    { title: 'text', boneFactory: emptyTextBone },
    { title: 'page-list', boneFactory: emptyPageListBone },
  ];

  private readonly _dataService = inject(AdminDataService);
  private readonly _http = inject(HttpClient);

  @Input({ required: true })
  public page!: Page<PageData>;

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
