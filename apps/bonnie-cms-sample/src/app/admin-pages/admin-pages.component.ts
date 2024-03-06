import { Component, Input, inject } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RouterLink } from '@angular/router';

import * as BON from '@candy-kingdom/bonnie';
import * as BONC from "@candy-kingdom/bonnie-cms";

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
  imports: [CommonModule, RouterLink, BON.BonnieModule, BONC.BonnieCmsModule, ...boneEditors],
  selector: 'app-admin-pages',
  templateUrl: './admin-pages.component.html',
  styleUrl: './admin-pages.component.scss'
})
export default class AdminPagesComponent {
  public readonly TextEditorField = BONC.TextEditorField;
  public readonly AdminBoneMap = AdminBoneMap;

  public readonly templates: BONC.IBoneTemplate[] = [
    { title: 'media', boneFactory: emptyMediaBone },
    { title: 'text', boneFactory: emptyTextBone },
    { title: 'page-list', boneFactory: emptyPageListBone },
  ];

  private readonly _dataService = inject(BONC.AdminDataService);

  @Input({ required: true })
  public page!: BON.Page<BON.PageData>;

  public save(): void {
    try {
      this._dataService.storePage(this.page)
        .subscribe
        (
          () => {
            console.log('successfully stored data'); // todo: add toast
          },
        );
    }

    catch (e) {
      console.error(e);
      alert(`Error: ${JSON.stringify(e)}`);
    }
  }
}
