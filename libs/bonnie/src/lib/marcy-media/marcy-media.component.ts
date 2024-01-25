import { Component, Input, Output, EventEmitter, ChangeDetectorRef, OnInit } from '@angular/core';

import * as MCore from '../generated';

import { UnsubscriberService } from '../core/unsubscribe.service';
import { MediaObjectFit } from '../core/MediaObjectFit';
import { DeviceServiceBase } from '../core/device.service.base';

@Component({
    selector: 'bon-media',
    templateUrl: './marcy-media.component.html',
    styleUrls: ['./marcy-media.component.scss'],
    providers: [UnsubscriberService]
})
export class MarcyMediaComponent implements OnInit {
    public readonly MarcyObjectFit = MediaObjectFit;

    @Output()
    public readonly isLoaded: EventEmitter<void> = new EventEmitter();

    private _objectFit = MediaObjectFit.Original;
    private _src?: MCore.Video | MCore.Image;

    constructor
        (
            public readonly device: DeviceServiceBase,
            public readonly cd: ChangeDetectorRef,
            private readonly _u: UnsubscriberService
        ) {

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
