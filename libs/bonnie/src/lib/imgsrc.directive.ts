import { Directive, Input, OnInit } from '@angular/core';
import * as MCore from './generated';
import { SrcBaseDirective } from './src.directive';

@Directive({
    selector: '[imgsrc]'
})
export class ImageSrcDirective extends SrcBaseDirective<MCore.Image> implements OnInit {

    @Input()
    public set imgsrc(value: MCore.Image | undefined) {
        console.log('set imgsrc', value);
        this.data = value;
    }
}
