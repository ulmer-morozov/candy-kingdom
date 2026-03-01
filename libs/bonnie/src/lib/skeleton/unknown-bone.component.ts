import { Component, inject } from '@angular/core';
import { CommonModule, JsonPipe } from '@angular/common';
import { IBoneComponent } from "./IBoneComponent";
import { Bone } from '../generated';
import { BoneDirective } from './bone.directive';

@Component({
  selector: 'bon-unknown-bone',
  standalone: true,
  imports: [CommonModule, JsonPipe],
  hostDirectives: [{ directive: BoneDirective, inputs: ['bone'], outputs: ['boneChange'] }],
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
    }`
  ]
})
export class UnknownBoneComponent implements IBoneComponent {
  public readonly bd = inject(BoneDirective<Bone>, { host: true });
}
