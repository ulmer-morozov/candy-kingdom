import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CdkTextareaAutosize } from '@angular/cdk/text-field';

import { AdminControlsComponent } from './admin-controls/';
import { EditableDirective } from './core-components';
import { TextFormComponent } from './text-form';
import { EditableGroupComponent } from './core-components';
import { FormControlsComponent } from './form-controls';
import { UnknownFormComponent } from './unknown-form';
import { SeoFormComponent } from './seo-form';
import { TranslationTextareaComponent } from './translation-textarea';
import { TranslationInputComponent } from './translation-input';
import { TranslationFormComponent } from './translation-form';
import { BoneEditorContainerComponent, SkeletonEditorAnchorDirective, SkeletonEditorComponent } from './skeleton-editor';

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
  BoneEditorContainerComponent
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
    ],
  imports: [
    CommonModule,
    FormsModule,
    CdkTextareaAutosize,
    ...standaloneDirectives
  ],
  exports: [
    CommonModule,
    FormsModule,
    ...components,
    ...standaloneDirectives
  ]
})
export class BonnieCmsModule { } // todo: move to standalone
