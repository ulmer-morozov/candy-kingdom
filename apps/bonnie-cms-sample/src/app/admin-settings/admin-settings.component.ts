import { CommonModule } from "@angular/common";
import { Component, inject, type OnInit, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";

import {
	AdminControlsComponent,
	AdminDataService,
	EditableGroupComponent,
	FileFormComponent,
	OneImageFormComponent,
	type SettingGroup,
	SvgFormComponent,
	TextEditorField,
	TextFormComponent,
	TextInputStyle,
	TextSettingType,
	TranslationFormComponent,
	UnknownFormComponent,
} from "@candy-kingdom/bonnie-cms";

const genericFileUploadMap = new Map<string, string>();

genericFileUploadMap.set("", "/api/admin/upload/file");

@Component({
	imports: [
		FormsModule,
		CommonModule,
		AdminControlsComponent,
		TextFormComponent,
		TranslationFormComponent,
		SvgFormComponent,
		OneImageFormComponent,
		FileFormComponent,
		EditableGroupComponent,
		UnknownFormComponent,
	],
	providers: [AdminDataService],
	selector: "app-admin-settings",
	templateUrl: "./admin-settings.component.html",
})
export default class AdminSettingsComponent implements OnInit {
	public readonly TextInputStyle = TextInputStyle;
	public readonly TextSettingType = TextSettingType;
	public readonly TextEditorField = TextEditorField;

	public readonly genericFileUploadMap = genericFileUploadMap;

	private readonly _dataService = inject(AdminDataService);

	public settingGroups = signal<SettingGroup[]>([]);

	public ngOnInit(): void {
		this._dataService.getSettingGroups().subscribe((x) => {
			this.settingGroups.set(x);
		});
	}

	public save(): void {
		try {
			const settings = this.settingGroups().flatMap((x) => x.records);

			this._dataService.updateSettings(settings).subscribe(() => {
				console.log("successfully updated settings"); // todo: add toast
			});
		} catch (e) {
			console.error(e);
		}
	}
}
