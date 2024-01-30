import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[boncSkeletonEditorAnchor]'
})
export class SkeletonEditorAnchorDirective {

  constructor(public viewContainerRef: ViewContainerRef) { }
}
