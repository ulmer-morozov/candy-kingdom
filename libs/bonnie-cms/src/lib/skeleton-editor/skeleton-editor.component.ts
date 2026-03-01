import { CommonModule } from "@angular/common";
import {
	Component,
	effect,
	inject,
	input,
	type OnInit,
	type QueryList,
	ViewChildren,
} from "@angular/core";

import type { Bone } from "@candy-kingdom/bonnie";

import { DeviceType } from "../core";
import { EditableDirective } from "../core-components/editable.directive";
import type { BoneEditorMap } from "./BoneEditorMap";
import { BoneEditorContainerComponent } from "./bone-editor-container/bone-editor-container.component";
import type { IBoneTemplate } from "./IBoneTemplate";

// todo: rename class
@Component({
	selector: "bonc-skeleton-editor",
	standalone: true,
	imports: [CommonModule, BoneEditorContainerComponent],
	templateUrl: "./skeleton-editor.component.html",
	styleUrls: ["./skeleton-editor.component.scss"],
	hostDirectives: [EditableDirective],
})
export class SkeletonEditorComponent implements OnInit {
	@ViewChildren("boneEditorContainer")
	public boneEditorContainerList!: QueryList<BoneEditorContainerComponent>;

	public readonly locale = input.required<string>();

	public readonly device = input(DeviceType.NotSet);

	public readonly map = input.required<BoneEditorMap>();

	public readonly templates = input.required<ReadonlyArray<IBoneTemplate>>();

	public readonly templatesAreShown: boolean[] = [];

	public readonly editable = inject(EditableDirective<Bone[]>, { host: true });

	constructor() {
		effect(() => {
			this.map();
			this.templates();
			this.templatesAreShown.splice(0, this.templatesAreShown.length);
			const bones = this.editable?.value ?? [];

			if (bones === undefined) return;

			bones.forEach(() => this.templatesAreShown.push(false));
			this.templatesAreShown.push(false);
		});
	}

	public ngOnInit(): void {
		this.editable.externalSaveCall.subscribe(() => {
			this.boneEditorContainerList.forEach((editorContainer) => editorContainer.editor?.save());

			this.editable.save();
		});

		this.editable.canceled.subscribe(() => {
			this.boneEditorContainerList.forEach((editorContainer) =>
				editorContainer.editor?.resetData(),
			);

			this.editable.save();
		});
	}

	public get bones(): Bone[] {
		return this.editable?.value ?? [];
	}

	public boneEditHandler(isEditing: boolean): void {
		if (isEditing) this.editable.startEditing();
		else {
			const allClosed =
				this.boneEditorContainerList.filter((x) => x.editor !== undefined && x.editor.isEditing)
					.length === 0;

			this.editable.updateDirty();

			if (!this.editable.isDirty && allClosed) this.editable.close();
		}
	}

	public boneChangeHandler(index: number, newBoneValue: Bone): void {
		if (Array.isArray(this.editable.value)) {
			this.editable.value[index] = newBoneValue;
		}

		this.editable.save();
	}

	public removeBone(boneIndex: number): void {
		if (this.bones === undefined || this.bones === null) return;

		if (!confirm("Вы уверены, что хотите удалить компонент?")) return;

		this.bones.splice(boneIndex, 1);

		if (!this.editable.inEditMode) this.editable.startEditing();

		this.editable.save();
	}

	public moveDown(boneIndex: number): void {
		if (boneIndex >= this.bones.length - 1) return;

		this.swapBones(boneIndex, boneIndex + 1);
	}

	public moveUp(boneIndex: number): void {
		if (boneIndex <= 0) return;

		this.swapBones(boneIndex - 1, boneIndex);
	}

	public createBone(index: number, selectedTemplate: IBoneTemplate): void {
		const newBone = selectedTemplate.boneFactory();

		this.bones.splice(index, 0, newBone);
	}

	private swapBones(index1: number, index2: number): void {
		if (index1 < 0 || index2 < 0 || index1 >= this.bones.length || index2 >= this.bones.length)
			throw new Error(
				`ошибка swapBones. неправильные индексы ${index1} и ${index2}. bone count: ${this.bones.length}`,
			);

		const tempBone = this.bones[index1];
		this.bones[index1] = this.bones[index2];
		this.bones[index2] = tempBone;
	}
}
