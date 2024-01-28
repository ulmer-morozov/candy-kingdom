import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { AdminControlsComponent } from './admin-controls/admin-controls.component';
import { EditableDirective } from './core-components/editable.directive';
import { FormsModule } from '@angular/forms';
import { TextFormComponent } from './text-form';
import { EditableGroupComponent } from './core-components';
import { FormControlsComponent } from './form-controls';

const components = [
  AdminControlsComponent,
  EditableGroupComponent,
  TextFormComponent,
  FormControlsComponent
];

const directives = [
  EditableDirective
];

@NgModule({
  declarations: [
    ...components,
    ...directives
  ],
  providers:
    [
    ],
  imports: [
    CommonModule,
    FormsModule,
  ],
  exports: [
    CommonModule,
    FormsModule,
    ...components,
    ...directives
  ]
})
export class BonnieCmsModule { }
