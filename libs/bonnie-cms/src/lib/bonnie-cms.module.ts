import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CdkTextareaAutosize } from '@angular/cdk/text-field';

import { AdminControlsComponent } from './admin-controls/';
import { EditableDirective, LinkPopupComponent } from './core-components';
import { TextFormComponent } from './text-form';
import { EditableGroupComponent } from './core-components';
import { FormControlsComponent } from './form-controls';
import { UnknownFormComponent } from './unknown-form';
import { SeoFormComponent } from './seo-form';
import { TranslationTextareaComponent } from './translation-textarea';
import { TranslationInputComponent } from './translation-input';
import { TranslationFormComponent } from './translation-form';
import { BoneEditorContainerComponent, SkeletonEditorAnchorDirective, SkeletonEditorComponent } from './skeleton-editor';
import { UnknownBoneEditorComponent } from './bone-editors/unknown-bone-editor/unknown-bone-editor.component';
import { BonnieModule } from '@candy-kingdom/bonnie';
import { MediaUploaderComponent } from './media-uploader/media-uploader.component';
import { DataService } from './services/data.service';
import { AdminDataService } from './services/admin-data.service';

const components = [
  TranslationInputComponent,
  TranslationTextareaComponent,
  AdminControlsComponent,
  EditableGroupComponent,
  FormControlsComponent,
  TranslationFormComponent,
  TextFormComponent,
  SeoFormComponent,
  UnknownFormComponent,
  SkeletonEditorComponent,
  SkeletonEditorAnchorDirective,
  BoneEditorContainerComponent,
  UnknownBoneEditorComponent,
  MediaUploaderComponent,
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
    ...standaloneDirectives
  ],
  exports: [
    CommonModule,
    FormsModule,
    BonnieModule,
    ...components,
    ...standaloneDirectives
  ]
})
export class BonnieCmsModule { } // todo: move to standalone
