import { Directive, Input, OnInit } from '@angular/core';
import * as MCore from '../generated';
import { SrcBaseDirective } from '../core/src.directive';

@Directive({
  standalone: true,
  selector: '[imgsrc]',
})
export class ImageSrcDirective
  extends SrcBaseDirective<MCore.Image>
  implements OnInit
{
  @Input()
  public set imgsrc(value: MCore.Image | undefined) {
    console.log('set imgsrc', value);
    this.data = value;
  }
}
