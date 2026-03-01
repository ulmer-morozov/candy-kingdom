import { Component, inject } from '@angular/core';
import { CommonModule, JsonPipe } from '@angular/common';
import { IBoneComponent } from "./IBoneComponent";
import { Bone } from '../generated';
import { BoneDirective } from './bone.directive';

@Component({
  selector: 'bon-unknown-bone',
  standalone: true,
  imports: [CommonModule, JsonPipe],
  hostDirectives: [BoneDirective],
  template: `<h2>unknown bone {{bd.bone.type}}</h2>
             <div>{{bd.bone | json}}</div>`,
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
