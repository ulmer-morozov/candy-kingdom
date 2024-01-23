import { ChangeDetectorRef, Component, HostBinding, OnInit } from '@angular/core';
import { BoneDirective } from '@candy-kingdom/bonnie';
import { PageListBone } from './PageListBone';
import { PageListBoneStyle } from './PageListBoneStyle';

@Component({
  selector: 'app-page-list',
  templateUrl: './page-list-bone.component.html',
  styleUrls: ['./page-list-bone.component.scss'],
  hostDirectives: [BoneDirective]
})
export class PageListBoneComponent implements OnInit {
  public readonly PageListBoneStyle = PageListBoneStyle;

  constructor(private readonly cd: ChangeDetectorRef, public readonly bd: BoneDirective<PageListBone>) {
    cd.detach();
  }

  @HostBinding('class')
  public get hostStyle(): string {
    return this.bd.bone.style;
  }


  public ngOnInit(): void {
    this.cd.detectChanges();
  }
}
