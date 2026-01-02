import { Component, Input, inject } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RouterLink } from '@angular/router';

import { BonnieModule, Page, PageData } from '@candy-kingdom/bonnie';
import {
  AdminDataService,
  BonnieCmsModule,
  IBoneTemplate,
  TextEditorField,
} from '@candy-kingdom/bonnie-cms';

import { AdminBoneMap } from '../AdminBoneMap';
import {
  MediaBoneEditorComponent,
  TextBoneEditorComponent,
} from '../bone-editors';
import { emptyMediaBone } from '../core/emptyMediaBone';
import { emptyTextBone } from '../core';
import { emptyPageListBone } from '../core/emptyPageListBone';

const boneEditors = [TextBoneEditorComponent, MediaBoneEditorComponent];

@Component({
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    BonnieModule,
    BonnieCmsModule,
    ...boneEditors,
  ],
  selector: 'app-admin-pages',
  templateUrl: './admin-pages.component.html',
  styleUrl: './admin-pages.component.scss',
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

  @Input({ required: true })
  public page!: Page<PageData>;

  public save(): void {
    try {
      this._dataService.storePage(this.page).subscribe(() => {
        console.log('successfully stored data'); // todo: add toast
      });
    } catch (e: unknown) {
      console.error(e);
      const msg = e instanceof Error ? e.message : String(e);
      alert(`Error: ${msg}`);
    }
  }
}
