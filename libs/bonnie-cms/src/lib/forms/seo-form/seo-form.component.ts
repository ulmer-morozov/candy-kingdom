import { Component, OnInit, Input } from '@angular/core';
import { FileMeta, FileSrc, ImageMeta, OpenGraphData } from '@candy-kingdom/bonnie';

import { EditableDirective, FormBaseComponent } from '../../core-components';

const uploadMap = new Map<string, string>();
uploadMap.set('', `/api/admin/upload/image?width=${1200}&height=${630}&format=image/jpeg`);

@Component({
  selector: 'bonc-seo-form',
  templateUrl: './seo-form.component.html',
  styleUrls: ['./seo-form.component.scss'],
  hostDirectives: [EditableDirective]
})
export class SeoFormComponent extends FormBaseComponent<OpenGraphData> implements OnInit {
  public readonly uploadMap = uploadMap;

  private _pageId = '';
  public ogImageUploadUrl = '';

  @Input()
  public label = '';

  public ngOnInit(): void {
    this.editable.externalSaveCall.subscribe(() => {
      this.editable.save();
    });
  }

  @Input()
  public set pageId(value: string) {
    this._pageId = value;
    this.ogImageUploadUrl = `/api/admin/page/Og-Image?pageId=${this.pageId}`; // todo: replace with link to single image api
  }

  public get pageId(): string {
    return this._pageId;
  }

  public ResToSrc(res: { url: string }): string {
    return res.url;
  }

  public replaceImage($event: FileSrc<FileMeta>): void {
    this.editable.startEditing();

    if (this.editable.value !== undefined) {
      this.editable.value.image[this.locale] = $event as FileSrc<ImageMeta>;
    }

    this.editable.updateDirty();
  }
}
