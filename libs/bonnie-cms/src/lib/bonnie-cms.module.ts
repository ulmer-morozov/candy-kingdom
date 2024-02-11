import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CdkTextareaAutosize } from '@angular/cdk/text-field';
import { LottieComponent } from 'ngx-lottie';


import { BonnieModule } from '@candy-kingdom/bonnie';

import { AdminControlsComponent } from './admin-controls/';
import { EditableDirective, LinkPopupComponent } from './core-components';
import { TextFormComponent } from './forms/text-form';
import { EditableGroupComponent } from './core-components';
import { FormControlsComponent } from './form-controls';
import { SeoFormComponent } from './forms/seo-form';
import { TranslationTextareaComponent } from './translation-textarea';
import { TranslationInputComponent } from './translation-input';
import { BoneEditorContainerComponent, SkeletonEditorAnchorDirective, SkeletonEditorComponent } from './skeleton-editor';
import { UnknownBoneEditorComponent } from './bone-editors/unknown-bone-editor/unknown-bone-editor.component';
import { MediaUploaderComponent } from './media-uploader/media-uploader.component';
import { DataService } from './services/data.service';
import { AdminDataService } from './services/admin-data.service';
import { TranslationFormComponent } from './forms/translation-form';
import { SvgFormComponent } from './forms/svg-form';
import { UnknownFormComponent } from './forms/unknown-form/unknown-form.component';
import { FileUploaderComponent } from './file-uploader';
import { FileFormComponent } from './forms/file-form';
import { LottieFormComponent } from './forms/lottie-form/lottie-form.component';

const formComponents = [
  TranslationFormComponent,
  TextFormComponent,
  SeoFormComponent,
  SvgFormComponent,
  FileFormComponent,
  LottieFormComponent,
  UnknownFormComponent
];

const components = [
  ...formComponents,
  TranslationInputComponent,
  TranslationTextareaComponent,
  AdminControlsComponent,
  EditableGroupComponent,
  FormControlsComponent,
  SkeletonEditorComponent,
  SkeletonEditorAnchorDirective,
  BoneEditorContainerComponent,
  UnknownBoneEditorComponent,
  MediaUploaderComponent,
  FileUploaderComponent,
  LinkPopupComponent
];

const standaloneDirectives = [
  EditableDirective
];

@NgModule({
  declarations: [
    ...components,
  ],
  providers:
    [
      DataService,
      AdminDataService
    ],
  imports: [
    CommonModule,
    FormsModule,
    CdkTextareaAutosize,
    BonnieModule,
    LottieComponent,
    ...standaloneDirectives
  ],
  exports: [
    CommonModule,
    FormsModule,
    BonnieModule,
    LottieFormComponent,
    ...components,
    ...standaloneDirectives
  ]
})
export class BonnieCmsModule { } // todo: move to standalone
