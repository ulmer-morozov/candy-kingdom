import { Directive, effect, input } from '@angular/core';

import * as MCore from '../generated';

import { SrcBaseDirective } from '../core/src.directive';

@Directive({
  standalone: true,
  selector: '[vidsrc]',
})
export class VideoSrcDirective
  extends SrcBaseDirective<MCore.Video>
{
  public readonly vidsrc = input<MCore.Video | undefined>();

  constructor() {
    super();
    effect(() => {
      this.data.set(this.vidsrc());
    });
  }
}
