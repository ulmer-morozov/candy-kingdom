import { Directive, ViewContainerRef, inject } from '@angular/core';

@Directive({
  selector: '[boncSkeletonEditorAnchor]'
})
export class SkeletonEditorAnchorDirective {
  public readonly viewContainerRef = inject(ViewContainerRef);
}
