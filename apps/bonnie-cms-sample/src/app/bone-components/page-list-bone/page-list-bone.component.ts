import { ChangeDetectorRef, Component, HostBinding, OnInit, inject } from '@angular/core';
import { BoneDirective } from '@candy-kingdom/bonnie';
import { PageListBone, PageListBoneStyle } from '../../generated';

@Component({
  selector: 'app-page-list',
  templateUrl: './page-list-bone.component.html',
  styleUrls: ['./page-list-bone.component.scss'],
  hostDirectives: [BoneDirective]
})
export class PageListBoneComponent implements OnInit {
  public readonly PageListBoneStyle = PageListBoneStyle;

  private readonly cd = inject(ChangeDetectorRef);
  public readonly bd = inject(BoneDirective<PageListBone>, { host: true });

  constructor() {
    this.cd.detach();
  }

  @HostBinding('class')
  public get hostStyle(): string {
    return this.bd.bone.style;
  }


  public ngOnInit(): void {
    this.cd.detectChanges();
  }
}
