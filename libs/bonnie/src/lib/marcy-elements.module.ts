import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { MarcyImageComponent } from './marcy-image/marcy-image.component';
import { IntersectionComponent } from './core/intersection.component';
import { UnsubscriberService } from './core/unsubscribe.service';
import { SrcBaseDirective } from './core/src.directive';
import { ImageSrcDirective } from './marcy-image/imgsrc.directive';
import { VideoSrcDirective } from './marcy-video/vidsrc.directive';
import { DeviceServiceBase } from './core/device.service.base';
import { DeviceService } from './core/device.service';
import { MarcyVideoComponent } from './marcy-video/marcy-video.component';
import { SkeletonAnchorDirective } from './skeleton/skeleton-anchor.directive';
import { SkeletonComponent } from './skeleton/skeleton.component';
import { LocalizationIsNotEmptyPipe, LocalizePipe } from './localization/localize.pipe';
import { UnknownBoneComponent } from './skeleton/unknown-bone.component';
import { MarcyMediaComponent } from './marcy-media/marcy-media.component';
import { BoneDirective } from './skeleton/bone.directive';

const components = [
  MarcyImageComponent,
  MarcyVideoComponent,
  MarcyMediaComponent,
  IntersectionComponent,
  //
  SkeletonComponent,
  UnknownBoneComponent
];

const directives = [
  SrcBaseDirective,
  ImageSrcDirective,
  VideoSrcDirective,
];

const standAloneDirectives = [
  SkeletonAnchorDirective,
  BoneDirective
];

const pipes = [
  LocalizePipe,
  LocalizationIsNotEmptyPipe
]

@NgModule({
  declarations: [
    ...components,
    ...directives,
    ...pipes,
  ],
  providers:
    [
      ...pipes,
      UnsubscriberService,
      DeviceService,
      { provide: DeviceServiceBase, useExisting: DeviceService },
    ],
  imports: [
    CommonModule,
    ...standAloneDirectives
  ],
  exports: [
    CommonModule,
    ...pipes,
    ...components,
    ...directives,
    ...standAloneDirectives
  ]
})
export class MarcyElementsModule { }
