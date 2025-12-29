import { Directive, ViewContainerRef, inject } from '@angular/core';

@Directive({
  selector: '[bonSkeletonAnchor]',
  standalone: true
})
export class SkeletonAnchorDirective {
  public readonly viewContainerRef = inject(ViewContainerRef);
}
