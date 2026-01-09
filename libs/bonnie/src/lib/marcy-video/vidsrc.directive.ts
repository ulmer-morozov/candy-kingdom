import { Directive, Input, OnInit } from '@angular/core';

import * as MCore from '../generated';

import { SrcBaseDirective } from '../core/src.directive';

@Directive({
  standalone: true,
  selector: '[vidsrc]',
})
export class VideoSrcDirective
  extends SrcBaseDirective<MCore.Video>
  implements OnInit
{
  @Input()
  public set vidsrc(value: MCore.Video | undefined) {
    this.data = value;

    console.log('set vidsrc', value);
  }
}
