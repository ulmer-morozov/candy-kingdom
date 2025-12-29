import { ChangeDetectorRef, Component, inject } from '@angular/core';
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
  private readonly cd = inject(ChangeDetectorRef);
  public readonly bd = inject(BoneDirective<Bone>, { host: true });

  constructor() {
    console.log('BoneDirective', this.bd);
    this.cd.detach();
  }

  ngOnInit(): void {
    this.cd.detectChanges();
  }
}
