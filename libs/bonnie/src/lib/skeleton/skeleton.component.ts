import {
	ChangeDetectionStrategy,
	Component,
	effect,
	input,
	type Type,
	viewChild,
} from "@angular/core";

import type { Bone } from "../generated";
import type { IBoneComponent } from "./IBoneComponent";
import { SkeletonAnchorDirective } from "./skeleton-anchor.directive";
import { UnknownBoneComponent } from "./unknown-bone.component";

@Component({
	selector: "bon-skeleton",
	imports: [SkeletonAnchorDirective],
	templateUrl: "./skeleton.component.html",
	styleUrl: "./skeleton.component.scss",
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SkeletonComponent {
	public readonly skeletonAnchor = viewChild.required(SkeletonAnchorDirective);

	public readonly map = input.required<Map<string, Type<IBoneComponent>>>();

	public readonly bones = input.required<Bone[]>();

	constructor() {
		effect(() => {
			const anchor = this.skeletonAnchor();
			const bones = this.bones();
			const mapVal = this.map();

			const viewContainerRef = anchor.viewContainerRef;
			viewContainerRef.clear();

			if (mapVal === undefined || mapVal === null)
				throw new Error('add type map with input: [map]="..."');

			for (const bone of bones) {
				let componentType = mapVal.get(bone.type);

				if (componentType === undefined || componentType === null) {
					console.warn(`Mapping type for ${bone.type} not found`);
					componentType = UnknownBoneComponent;
				}

				viewContainerRef.createComponent(componentType).setInput("bone", bone);
			}
		});
	}
}
