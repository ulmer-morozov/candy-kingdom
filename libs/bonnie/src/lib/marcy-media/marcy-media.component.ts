import { Component, Input, Output, EventEmitter, ChangeDetectorRef, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';

import * as MCore from '../generated';

import { UnsubscriberService } from '../core/unsubscribe.service';
import { MediaObjectFit } from '../core/MediaObjectFit';
import { MarcyImageComponent } from '../marcy-image/marcy-image.component';
import { MarcyVideoComponent } from '../marcy-video/marcy-video.component';
import { VideoSrcDirective } from '../marcy-video';
import { ImageSrcDirective } from '../marcy-image';

@Component({
    selector: 'bon-media',
    standalone: true,
    imports: [CommonModule, MarcyImageComponent, MarcyVideoComponent, VideoSrcDirective, ImageSrcDirective],
    templateUrl: './marcy-media.component.html',
    styleUrls: ['./marcy-media.component.scss'],
    providers: [UnsubscriberService]
})
export class MarcyMediaComponent implements OnInit {
    private readonly cd = inject(ChangeDetectorRef);

    public readonly MarcyObjectFit = MediaObjectFit;

    @Output()
    public readonly isLoaded: EventEmitter<void> = new EventEmitter();

    private _objectFit = MediaObjectFit.Original;
    private _src?: MCore.Video | MCore.Image;

    constructor
        () {
        const cd = this.cd;


        console.log('MarcyMediaComponent ctor');

        cd.detach();
    }


    public ngOnInit(): void {
        console.log('MarcyMediaComponent ngOnInit');

        this.cd.detectChanges();
    }


    public get src(): MCore.Video | MCore.Image | undefined {
        return this._src;
    }

    @Input()
    public set src(val: MCore.Video | MCore.Image | undefined) {
        console.log('set data', val);

        this._src = val;

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
        this.isLoaded.next();
    }

}
