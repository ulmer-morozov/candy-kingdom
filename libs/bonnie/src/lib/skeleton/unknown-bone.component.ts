import { ChangeDetectorRef, Component } from '@angular/core';
import { IBoneComponent } from "./IBoneComponent";
import { Bone } from '../generated';
import { BoneDirective } from './bone.directive';

@Component({
  selector: 'bon-unknown-bone',
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
  constructor(private readonly cd: ChangeDetectorRef, public readonly bd: BoneDirective<Bone>) {
    console.log('BoneDirective', bd);
    cd.detach();
  }

  ngOnInit(): void {
    this.cd.detectChanges();
  }
}
