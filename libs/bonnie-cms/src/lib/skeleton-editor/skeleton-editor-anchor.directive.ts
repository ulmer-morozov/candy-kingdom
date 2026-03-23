import { Directive, inject, ViewContainerRef } from "@angular/core";

@Directive({
	selector: "[boncSkeletonEditorAnchor]",
})
export class SkeletonEditorAnchorDirective {
	public readonly viewContainerRef = inject(ViewContainerRef);
}
