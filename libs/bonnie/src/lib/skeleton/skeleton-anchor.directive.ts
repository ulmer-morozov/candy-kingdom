import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[bonSkeletonAnchor]',
  standalone: true
})
export class SkeletonAnchorDirective {
  constructor(public viewContainerRef: ViewContainerRef) { }
}
