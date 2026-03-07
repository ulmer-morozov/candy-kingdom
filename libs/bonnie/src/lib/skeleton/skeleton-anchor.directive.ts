import { Directive, inject, ViewContainerRef } from "@angular/core";

@Directive({
	selector: "[bonSkeletonAnchor]",
})
export class SkeletonAnchorDirective {
	public readonly viewContainerRef = inject(ViewContainerRef);
}
