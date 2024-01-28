import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { AdminControlsComponent } from './admin-controls/admin-controls.component';
import { EditableDirective } from './core-components/editable.directive';
import { FormsModule } from '@angular/forms';
import { TextFormComponent } from './text-form';
import { EditableGroupComponent } from './core-components';
import { FormControlsComponent } from './form-controls';
import { UnknownFormComponent } from './unknown-form';

const components = [
  AdminControlsComponent,
  EditableGroupComponent,
  FormControlsComponent,
  TextFormComponent,
  UnknownFormComponent
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
    ...standaloneDirectives
  ],
  exports: [
    CommonModule,
    FormsModule,
    ...components,
    ...standaloneDirectives
  ]
})
export class BonnieCmsModule { }
