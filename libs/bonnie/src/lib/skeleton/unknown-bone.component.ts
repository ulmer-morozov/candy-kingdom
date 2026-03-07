import { JsonPipe } from "@angular/common";
import { Component, inject } from "@angular/core";

import type { Bone } from "../generated";
import { BoneDirective } from "./bone.directive";
import type { IBoneComponent } from "./IBoneComponent";

@Component({
	selector: "bon-unknown-bone",

	imports: [JsonPipe],
	hostDirectives: [{ directive: BoneDirective, inputs: ["bone"], outputs: ["boneChange"] }],
	template: `@if (bd.bone(); as bone) {
               <h2>unknown bone {{bone.type}}</h2>
               <div>{{bone | json}}</div>
             } @else{
              bone is undefined
             }`,
	styles: [
		`:host{
        display: block;
        border: 2px solid red;
        box-sizing: border-box;
    }`,
	],
})
export class UnknownBoneComponent implements IBoneComponent {
	public readonly bd = inject(BoneDirective<Bone>, { host: true });
}
