import { Directive, ViewContainerRef, inject } from '@angular/core';

@Directive({
  selector: '[boncSkeletonEditorAnchor]',
  standalone: true
})
export class SkeletonEditorAnchorDirective {
  public readonly viewContainerRef = inject(ViewContainerRef);
}
