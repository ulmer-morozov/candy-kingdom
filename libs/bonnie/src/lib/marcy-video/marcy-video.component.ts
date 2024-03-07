import { Component, Input, Output, EventEmitter, ChangeDetectorRef, OnInit, OnDestroy, Optional, ElementRef, ViewChild, AfterViewInit } from '@angular/core';
import { BehaviorSubject, filter } from 'rxjs';

import * as MCore from '../generated';

import { UnsubscriberService } from '../core/unsubscribe.service';
import { MediaStatus } from '../core/MediaStatus';
import { MediaObjectFit } from '../core/MediaObjectFit';
import { DeviceServiceBase } from '../core/device.service.base';
import { VideoSrcDirective } from './vidsrc.directive';
import { ascendingT, matchesMediaQuery, descendingT } from '../core/utils';

@Component({
    selector: 'bon-video',
    templateUrl: './marcy-video.component.html',
    styleUrls: ['./marcy-video.component.scss'],
    providers: [UnsubscriberService]
})
export class MarcyVideoComponent implements OnInit, AfterViewInit {
    public readonly MediaStatus = MediaStatus;
    public readonly MarcyObjectFit = MediaObjectFit;

    @ViewChild('video')
    public readonly videoRef!: ElementRef<HTMLVideoElement>;

    @Output()
    public readonly isLoaded: EventEmitter<void> = new EventEmitter();

    public readonly $status = new BehaviorSubject<MediaStatus>(MediaStatus.NotSet);
    public readonly src: VideoSrcDirective;

    public source?: MCore.FileSrc<MCore.VideoMeta>;

    private _objectFit = MediaObjectFit.Original;

    constructor
        (
            public readonly device: DeviceServiceBase,
            public readonly cd: ChangeDetectorRef,
            private readonly _u: UnsubscriberService,
            @Optional() srcDir?: VideoSrcDirective
        ) {

        console.log('MarcyVideoComponent ctor');

        if (srcDir === undefined || srcDir === null)
            throw new Error(`${MarcyVideoComponent.name} should have [vidsrc] directive as source object`);

        this.src = srcDir;

        cd.detach();
    }

    public ngOnInit(): void {
        console.log('MarcyVideoComponent ngOnInit');

        // bind loaded event
        this.$status
            .pipe
            (
                this._u.takeUntilDestroy,
                filter(status => status === MediaStatus.Loaded)
            )
            .subscribe(() => this.isLoaded.next());

        this.cd.detectChanges();
    }

    public ngAfterViewInit() {
        console.log('MarcyVideoComponent ngAfterViewInit');

        // bind src changes
        this.src.srcChange
            .pipe(this._u.takeUntilDestroy)
            .subscribe((val) => {
                console.log('MarcyVideoComponent onSrcChange', val);

                this.updateSources();

                // resubscribe because its updated with src
                this.subscribeToMediaQueryChange();
            })

        this.updateSources();

        // initial, for src added before init
        this.subscribeToMediaQueryChange();
    }

    private subscribeToMediaQueryChange(): void {
        this.src
            .watchMediaQueries()
            .subscribe(() => {
                console.log('MarcyVideoComponent watchMediaQueries');
                this.updateSources();
            });
    }

    private updateSources() {
        console.log('MarcyVideoComponent updateSources');

        this.source = this.findMoreSuitableSource();

        console.log('MarcyVideoComponent new source', this.source);

        if (this.$status.value === MediaStatus.NotSet && this.source === undefined) {
            return;
        }

        if (this.source === undefined) {
            this.$status.next(MediaStatus.NotSet);

            this.cd.detectChanges();
            return;
        }

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

    private findMoreSuitableSource(): MCore.FileSrc<MCore.VideoMeta> | undefined {
        if (this.videoRef === undefined) {
            console.log('skipping findMoreSuitableSource. videoRef is empty still');
            return;
        }

        const videoSources = this.src.data?.sources ?? [];

        const currentVideoWidth = this.videoRef.nativeElement.clientWidth;
        const realPixelsVideoWidth = this.device.devicePixelRatio * currentVideoWidth;

        console.log(`MarcyVideoComponent currentVideoWidth ${currentVideoWidth}`);
        console.log(`MarcyVideoComponent realPixelsVideoWidth ${realPixelsVideoWidth}`);

        for (let i = 0; i < videoSources.length; i++) {
            const videoSource = videoSources[i];

            if (!matchesMediaQuery(videoSource.mediaQuery))
                continue;

            // SSR
            if (typeof this.videoRef.nativeElement?.canPlayType !== 'function') {
                // return first mp4, because all players can play them
                const mp4Srcs = videoSource
                    .srcSet
                    .filter(x => x.mimeType === 'video/mp4')
                    .sort(descendingT(x => x.meta.width)); // bigest video

                console.log(`ssr found video ${mp4Srcs[0].url}`)

                // element or undefined
                return mp4Srcs[0];
            }

            const fileSrcs = videoSource
                .srcSet
                .filter(x => this.videoRef.nativeElement.canPlayType(x.mimeType))
                .sort(ascendingT(x => x.meta.width)) // smallest video
                .sort(x => x.mimeType.includes('webm') ? 1 : -1);// webm first

            if (fileSrcs.length === 0)
                continue;

            let bestSrc = fileSrcs[0];

            for (let i = 1; i < fileSrcs.length; i++) {
                const fileSrc = fileSrcs[i];

                const currentDiff = fileSrc.meta.width - realPixelsVideoWidth;

                console.log(`browser video currentDiff ${currentDiff}`)

                // too big video source width
                if (currentDiff > 0)
                    break;

                bestSrc = fileSrc;
            }

            console.log(`browser found suitable video ${bestSrc.url}`)

            return bestSrc;
        }

        return undefined;
    }
}
