import { Directive, inject, ViewContainerRef } from "@angular/core";

@Directive({
	selector: "[boncSkeletonEditorAnchor]",
	standalone: true,
})
export class SkeletonEditorAnchorDirective {
	public readonly viewContainerRef = inject(ViewContainerRef);
}
