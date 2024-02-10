import { Component, OnInit, Input } from '@angular/core';
import { OpenGraphData } from '@candy-kingdom/bonnie';

import { EditableDirective, FormBaseComponent } from '../../core-components';

@Component({
  selector: 'bonc-seo-form',
  templateUrl: './seo-form.component.html',
  styleUrls: ['./seo-form.component.scss'],
  hostDirectives: [EditableDirective]
})
export class SeoFormComponent extends FormBaseComponent<OpenGraphData> implements OnInit {
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
}
