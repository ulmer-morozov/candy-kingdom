import { Component, HostBinding, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { BoneDirective, LocalizePipe } from '@candy-kingdom/bonnie';
import { PageListBone, PageListBoneStyle } from '../../generated';

@Component({
  selector: 'app-page-list',
  standalone: true,
  imports: [CommonModule, RouterLink, LocalizePipe],
  templateUrl: './page-list-bone.component.html',
  styleUrls: ['./page-list-bone.component.scss'],
  hostDirectives: [{ directive: BoneDirective, inputs: ['bone'], outputs: ['boneChange'] }]
})
export class PageListBoneComponent {
  public readonly PageListBoneStyle = PageListBoneStyle;

  public readonly bd = inject(BoneDirective<PageListBone>, { host: true });

  @HostBinding('class')
  public get hostStyle(): string {
    return this.bd.bone().style;
  }
}
