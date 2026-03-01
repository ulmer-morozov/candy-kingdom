import { Component, OnInit, inject } from "@angular/core";
import { CommonModule } from "@angular/common";

import {
	AdminDataService,
	SettingGroup,
	TextEditorField,
	TextInputStyle,
	TextSettingType,
	TextFormComponent,
	TranslationFormComponent,
	SvgFormComponent,
	OneImageFormComponent,
	FileFormComponent,
	LottieFormComponent,
	AdminControlsComponent,
	EditableGroupComponent,
	UnknownFormComponent,
} from "@candy-kingdom/bonnie-cms";
import { FormsModule } from "@angular/forms";

const genericFileUploadMap = new Map<string, string>();

genericFileUploadMap.set("", "/api/admin/upload/file");

@Component({
	standalone: true,
	imports: [
		FormsModule,
		CommonModule,
		AdminControlsComponent,
		TextFormComponent,
		TranslationFormComponent,
		SvgFormComponent,
		OneImageFormComponent,
		FileFormComponent,
		LottieFormComponent,
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

	public settingGroups: SettingGroup[] = [];

	public ngOnInit(): void {
		this._dataService.getSettingGroups().subscribe((x) => {
			this.settingGroups = x;
		});
	}

	public save(): void {
		try {
			const settings = this.settingGroups.flatMap((x) => x.records);

			this._dataService.updateSettings(settings).subscribe(() => {
				console.log("successfully updated settings"); // todo: add toast
			});
		} catch (e) {
			console.error(e);
		}
	}
}
