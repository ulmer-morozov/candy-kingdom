import { Component } from '@angular/core';
import { Bone } from '@candy-kingdom/bonnie';
import { ContentPreset } from '../../skeleton-editor';
import { BoneEditorBaseComponent } from '../bone-editor-base.component';

@Component({
  selector: 'bonc-unknown-bone-editor',
  templateUrl: './unknown-bone-editor.component.html',
  styleUrls: ['./unknown-bone-editor.component.scss']
})
export class UnknownBoneEditorComponent extends BoneEditorBaseComponent<Bone> {
  public onFinishEditing(): void { }
  public onReset(): void { }

  protected getPresets(): ContentPreset<Bone>[] {
    return [];
  }
}
