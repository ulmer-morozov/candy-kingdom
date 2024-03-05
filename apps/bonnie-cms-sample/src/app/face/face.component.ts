import { Component, Input, inject } from '@angular/core';

import { FaceModule } from '../face.module';
import { FaceBoneMap } from '../FaceBoneMap';
import { LocalizeServiceBase, PageBase, View } from '@candy-kingdom/bonnie';
import { RouterLocalizeService } from '../router-localize.service';
import { RouterLink } from '@angular/router';
import { FileSettingData, LocalizedTextSettingData, OneImageSettingData, SettingDataDict, SvgSettingData } from '@candy-kingdom/bonnie-cms';
import { Meta, Title } from '@angular/platform-browser';
import { CmsSampleSettingIds } from '../generated/cms-sample-setting-ids';
import { DOCUMENT, APP_BASE_HREF } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-face',
  templateUrl: './face.component.html',
  styleUrl: './face.component.scss',
  imports: [FaceModule, RouterLink],
  providers:
    [
      { provide: LocalizeServiceBase, useClass: RouterLocalizeService },
    ]
})
export default class FaceComponent {
  public readonly FaceBoneMap = FaceBoneMap;

  public readonly doc = inject(DOCUMENT);
  public readonly metaService = inject(Meta);
  public readonly titleService = inject(Title);
  private readonly baseHref = inject(APP_BASE_HREF);
  public readonly localizeService = inject(LocalizeServiceBase);

  private _page!: PageBase;
  private _settings!: SettingDataDict;

  @Input({ required: true })
  public faceView!: View;


  @Input({ required: true })
  public set page(newPage: PageBase) {
    this._page = newPage;

    const title = this.localizeService.getLocalizedText(newPage.title);
    this.titleService.setTitle(title);

    const localizedOgImage = this.localizeService.getLocalized(newPage.openGraph.image, undefined);

    if (localizedOgImage !== undefined) {
      const ogImage = localizedOgImage.url.startsWith('http') || localizedOgImage.url.startsWith('//')
        ? localizedOgImage.url
        : `${this.baseHref}${localizedOgImage.url}`;

      this.metaService.updateTag({ property: 'og:image', content: ogImage });
    }

    const localizedOgTitle = this.localizeService.getLocalizedText(newPage.openGraph.title);

    if (localizedOgTitle.length > 0) {
      this.metaService.updateTag({ property: 'og:title', content: localizedOgTitle });
    }

    const localizedOgDescription = this.localizeService.getLocalizedText(newPage.openGraph.description);

    if (localizedOgDescription.length > 0) {
      this.metaService.updateTag({ property: 'og:description', content: localizedOgDescription });
    }

  }

  public get page(): PageBase {
    return this._page;
  }

  @Input({ required: true })
  public set settings(newSettings: SettingDataDict) {

    if (newSettings[CmsSampleSettingIds.faviconIco]) {
      this.appendIconLink("icon", "image/x-icon", (newSettings[CmsSampleSettingIds.faviconIco] as FileSettingData).src.url)
    }

    if (newSettings[CmsSampleSettingIds.faviconSvg]) {
      this.appendIconLink("icon", "image/svg+xml", (newSettings[CmsSampleSettingIds.faviconSvg] as SvgSettingData).src.url)
    }

    if (newSettings[CmsSampleSettingIds.faviconAppleTouch180]) {
      this.appendIconLink("apple-touch-icon", "image/png", (newSettings[CmsSampleSettingIds.faviconAppleTouch180] as OneImageSettingData).src.url)
    }

    if (newSettings[CmsSampleSettingIds.description]) {
      const localizedDescription = this.localizeService.getLocalizedText((newSettings[CmsSampleSettingIds.description] as LocalizedTextSettingData).text);
      this.metaService.updateTag({ property: 'description', content: localizedDescription });
    }
  }

  private appendIconLink(rel: string, type: string, href: string, sizes: string = ''): void {
    const link: HTMLLinkElement = this.doc.createElement('link');

    link.setAttribute('rel', rel);
    link.setAttribute('type', type);
    link.setAttribute('href', href);

    if (sizes !== undefined && sizes !== null && sizes.length > 0)
      link.setAttribute('sizes', sizes);

    this.doc.head.appendChild(link);
  }

}
