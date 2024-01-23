import { NgModule } from '@angular/core';
import { TextBoneComponent } from './bone-components/text-bone/text-bone.component';
import { MediaBoneComponent } from './bone-components/media-bone/media-bone.component';
import { PageListBoneComponent } from './bone-components/page-list-bone/page-list-bone.component';
import { MarcyElementsModule } from '@candy-kingdom/bonnie';
import { RouterModule } from '@angular/router';

const boneComponents = [
  TextBoneComponent,
  MediaBoneComponent,
  PageListBoneComponent
];

@NgModule({
  declarations: [
    ...boneComponents
  ],
  providers: [
  ],
  imports: [
    MarcyElementsModule,
    RouterModule
  ],
  exports: [
    ...boneComponents,
    MarcyElementsModule
  ]
})
export class FaceModule { }
