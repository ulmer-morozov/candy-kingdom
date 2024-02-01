import { Component, HostBinding } from '@angular/core';
import { BoneEditorBaseComponent, BonnieCmsModule, ContentPreset, createPreset } from '@candy-kingdom/bonnie-cms';
import { PageListBone, PageListBoneStyle } from '../../generated';

@Component({
  standalone: true,
  selector: 'app-page-list-bone-editor',
  templateUrl: './page-list-bone-editor.component.html',
  styleUrls: ['./page-list-bone-editor.component.scss'],
  imports: [BonnieCmsModule]
})
export class PageListBoneEditorComponent extends BoneEditorBaseComponent<PageListBone> {
  protected getPresets(): ContentPreset<PageListBone>[] {
    return [
      createPreset<PageListBone>({
        title: 'Default',
        style: PageListBoneStyle.default
      }),

      createPreset<PageListBone>({
        title: 'Main nav',
        style: PageListBoneStyle.mainNav
      })
    ];
  }

  @HostBinding('class') get styleFromBone(): string {
    return this.bone.style;
  }

  public onReset(): void {
  }

  public onFinishEditing(): void {
    // todo: remove?
    // this.data.elements.splice(this.mediaCount, this.data.elements.length - this.mediaCount);
  }
}

