import { Component, inject, input } from "@angular/core";
import { CommonModule } from "@angular/common";

import { RouterLink } from "@angular/router";

import {
	Page,
	PageData,
	LocalizePipe,
	EncodeURIComponentPipe,
	DeviceServiceBase,
	DeviceService,
} from "@candy-kingdom/bonnie";
import {
	AdminDataService,
	IBoneTemplate,
	TextEditorField,
	SeoFormComponent,
	TranslationFormComponent,
	AdminControlsComponent,
	EditableGroupComponent,
	SkeletonEditorComponent,
} from "@candy-kingdom/bonnie-cms";

import { AdminBoneMap } from "../AdminBoneMap";
import { emptyMediaBone } from "../core/emptyMediaBone";
import { emptyTextBone } from "../core";
import { emptyPageListBone } from "../core/emptyPageListBone";
import { FormsModule } from "@angular/forms";

@Component({
	standalone: true,
	imports: [
		FormsModule,
		CommonModule,
		RouterLink,
		AdminControlsComponent,
		EditableGroupComponent,
		SeoFormComponent,
		TranslationFormComponent,
		SkeletonEditorComponent,
		LocalizePipe,
		EncodeURIComponentPipe,
	],
	providers: [AdminDataService, { provide: DeviceServiceBase, useClass: DeviceService }],
	selector: "app-admin-pages",
	templateUrl: "./admin-pages.component.html",
	styleUrl: "./admin-pages.component.scss",
})
export default class AdminPagesComponent {
	public readonly TextEditorField = TextEditorField;
	public readonly AdminBoneMap = AdminBoneMap;

	public readonly templates: IBoneTemplate[] = [
		{ title: "media", boneFactory: emptyMediaBone },
		{ title: "text", boneFactory: emptyTextBone },
		{ title: "page-list", boneFactory: emptyPageListBone },
	];

	private readonly _dataService = inject(AdminDataService);

	public readonly page = input.required<Page<PageData>>();

	public save(): void {
		try {
			this._dataService.storePage(this.page()).subscribe(() => {
				console.log("successfully stored data"); // todo: add toast
			});
		} catch (e: unknown) {
			console.error(e);
			const msg = e instanceof Error ? e.message : String(e);
			alert(`Error: ${msg}`);
		}
	}
}
