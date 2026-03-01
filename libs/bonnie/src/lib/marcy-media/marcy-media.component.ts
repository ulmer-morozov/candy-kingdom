import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';

import * as MCore from '../generated';

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
})
export class MarcyMediaComponent {
    public readonly MarcyObjectFit = MediaObjectFit;

    public readonly isLoaded = output<void>();

    public readonly src = input<MCore.Video | MCore.Image | undefined>();

    public readonly objectFit = input<MediaObjectFit>(MediaObjectFit.Original);

    public onLoad() {
        this.isLoaded.emit();
    }
}
