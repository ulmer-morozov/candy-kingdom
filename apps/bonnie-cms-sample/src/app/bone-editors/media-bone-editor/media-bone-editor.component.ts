import { Component } from '@angular/core';
import * as BONC from '@candy-kingdom/bonnie-cms';

import { MediaBone, MediaBoneStyle } from '../../generated';
import { MediaUploadMap } from '../../core';

@Component({
  standalone: true,
  selector: 'app-media-bone-editor',
  templateUrl: './media-bone-editor.component.html',
  styleUrls: ['./media-bone-editor.component.scss'],
  imports: [BONC.BonnieCmsModule]
})
export class MediaBoneEditorComponent extends BONC.BoneEditorBaseComponent<MediaBone> {
  public readonly MediaBoneStyle = MediaBoneStyle;
  public readonly MediaUploadMap = MediaUploadMap;

  public LinkPopupField = BONC.TextEditorField;

  private currentOpenLinkPopup: BONC.LinkPopupComponent | undefined;

  protected getPresets(): BONC.ContentPreset<MediaBone>[] {
    return [
      BONC.createPreset<MediaBone>({
        title: 'Default',
        style: MediaBoneStyle.default
      }),
      BONC.createPreset<MediaBone>({
        title: 'Full width',
        style: MediaBoneStyle.fullWidth
      })
    ];
  }

  public onReset(): void {
    this.closePopUpIfExists();
  }

  public onFinishEditing(): void {
    this.closePopUpIfExists();
  }

  public updateClosed(): void {
    this.currentOpenLinkPopup = undefined;
  }

  public updateOpen(newLinkPopup: BONC.LinkPopupComponent): void {
    this.closePopUpIfExists();
    this.currentOpenLinkPopup = newLinkPopup;
  }

  private closePopUpIfExists(): void {
    if (this.currentOpenLinkPopup === undefined)
      return;

    this.currentOpenLinkPopup.hidePopup();
    this.currentOpenLinkPopup = undefined;
  }

  public onUpdated() {
  }
}
