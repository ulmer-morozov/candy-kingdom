import { Component, OnInit, Input } from '@angular/core';
import { OpenGraphData } from '@candy-kingdom/bonnie';

import { EditableDirective, FormBaseComponent } from '../core-components';
import { TranslationInputStyle } from '../core/TranslationInputStyle';
import { MediaType } from '../core/MediaType';

@Component({
  selector: 'bonc-seo-form',
  templateUrl: './seo-form.component.html',
  styleUrls: ['./seo-form.component.scss'],
  hostDirectives: [EditableDirective]
})
export class SeoFormComponent extends FormBaseComponent<OpenGraphData> implements OnInit {
  public readonly TranslationInputStyle = TranslationInputStyle;
  public readonly MediaType = MediaType;

  private _pageId = '';
  public ogImageUploadUrl = '';

  @Input()
  public label = '';

  ngOnInit(): void {
    this.editable.externalSaveCall.subscribe(() => {
      this.editable.save();
    });
  }

  @Input()
  public set pageId(value: string) {
    this._pageId = value;
    this.ogImageUploadUrl = `/Api/Admin/Page/Og-Image?pageId=${this.pageId}`; // todo: replace with link to single image api
  }

  public get pageId(): string {
    return this._pageId;
  }

  public ResToSrc(res: { url: string }): string {
    return res.url;
  }
}
