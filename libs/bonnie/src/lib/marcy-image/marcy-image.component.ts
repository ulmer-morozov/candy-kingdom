import { Component, Input, Output, EventEmitter, ChangeDetectorRef, OnInit, OnDestroy, Optional } from '@angular/core';
import { BehaviorSubject, filter } from 'rxjs';

import * as MCore from '../generated';

import { UnsubscriberService } from '../core/unsubscribe.service';
import { MediaStatus } from '../core/MediaStatus';
import { MediaObjectFit } from '../core/MediaObjectFit';
import { ImageSrcDirective } from './imgsrc.directive';
import { IHtmlPictureSource } from './IHtmlPictureSource';
import { toHtmlPictureSources } from './toHtmlSources';
import { getDefaultSrc } from './getDefaultSrc';
import { DeviceServiceBase } from '../core/device.service.base';

@Component({
  selector: 'bon-image',
  templateUrl: './marcy-image.component.html',
  styleUrls: ['./marcy-image.component.scss'],
  providers: [UnsubscriberService]
})
export class MarcyImageComponent implements OnInit {
  public readonly MediaStatus = MediaStatus;
  public readonly MarcyObjectFit = MediaObjectFit;

  @Output()
  public readonly isLoaded: EventEmitter<void> = new EventEmitter();
  public readonly sources: IHtmlPictureSource[] = [];

  public readonly $status = new BehaviorSubject<MediaStatus>(MediaStatus.NotSet);

  public defaultSrc = '';

  private _objectFit = MediaObjectFit.Original;

  public readonly src: ImageSrcDirective;

  constructor
    (
      public readonly device: DeviceServiceBase,
      public readonly cd: ChangeDetectorRef,
      private readonly _u: UnsubscriberService,
      @Optional() srcDir?: ImageSrcDirective
    ) {

    console.log('MarcyImageComponent ctor');

    if (srcDir === undefined || srcDir === null)
      throw new Error(`${MarcyImageComponent.name} should have [imgsrc] directive as source object`);

    this.src = srcDir;

    // bind src changes
    this.src.srcChange
      .pipe(this._u.takeUntilDestroy)
      .subscribe(this.onSrcChange.bind(this))

    cd.detach();
  }

  public ngOnInit(): void {
    // bind loaded event
    this.$status
      .pipe
      (
        this._u.takeUntilDestroy,
        filter(status => status === MediaStatus.Loaded)
      )
      .subscribe(x => this.isLoaded.next());

    this.cd.detectChanges();
  }
  private onSrcChange(val: MCore.Image | undefined) {
    console.log('MarcyImageComponent onSrcChange', val);
    this.defaultSrc = getDefaultSrc(val)?.url ?? '';

    this.sources.splice(0, this.sources.length);

    if (val === undefined || val === null || val.sources.length === 0) {
      this.$status.next(MediaStatus.NotSet);

      this.cd.detectChanges();
      return;
    }

    const newSources = val.sources.flatMap(toHtmlPictureSources);

    console.log('MarcyImageComponent newSources', newSources);

    this.sources.push(...newSources);

    this.$status.next(MediaStatus.NotLoaded);

    this.cd.detectChanges();
  }

  @Input()
  public set objectFit(val: MediaObjectFit | undefined) {
    this._objectFit = val ?? MediaObjectFit.Original;

    this.cd.detectChanges();
  }

  public get objectFit(): MediaObjectFit {
    return this._objectFit;
  }

  public onLoad() {
    this.$status.next(MediaStatus.Loaded);
  }
}
