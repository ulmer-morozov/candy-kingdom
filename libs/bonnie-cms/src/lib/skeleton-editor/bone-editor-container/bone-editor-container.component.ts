import {
	afterNextRender,
	Component,
	type ComponentRef,
	effect,
	input,
	output,
	signal,
	viewChild,
} from "@angular/core";

import type { Unsubscribable } from "rxjs";

import type { Bone } from "@candy-kingdom/bonnie";

import { UnknownBoneEditorComponent } from "../../bone-editors/unknown-bone-editor/unknown-bone-editor.component";
import { DeviceType } from "../../core";
import type { BoneEditorMap } from "../BoneEditorMap";
import type { IBoneEditor } from "../IBoneEditor";
import { SkeletonEditorAnchorDirective } from "../skeleton-editor-anchor.directive";

@Component({
	selector: "bonc-bone-editor-container",

	imports: [SkeletonEditorAnchorDirective],
	templateUrl: "./bone-editor-container.component.html",
	styleUrl: "./bone-editor-container.component.scss",
})
export class BoneEditorContainerComponent {
	public readonly anchor = viewChild.required(SkeletonEditorAnchorDirective);

	public readonly removed = output<void>();
	public readonly saved = output<Bone>();
	public readonly editing = output<boolean>();

	public DeviceType = DeviceType;

	public editor!: IBoneEditor;

	public themePopupIsShown = false;

	public readonly bone = input.required<Bone>();
	public readonly locale = input.required<string>();
	public readonly device = input(DeviceType.NotSet);
	public readonly map = input.required<BoneEditorMap>();

	private boneEditorRef?: ComponentRef<IBoneEditor<Bone>>;
	private removeSubscription?: Unsubscribable;
	private saveSubscription?: Unsubscribable;
	private changedSubscription?: Unsubscribable;

	private readonly viewReady = signal(false);

	constructor() {
		afterNextRender(() => this.viewReady.set(true));

		effect(() => {
			if (!this.viewReady()) {
				return;
			}

			const newBone = this.bone();
			const editorMap = this.map();

			if (this.removeSubscription) {
				this.removeSubscription.unsubscribe();
				this.removeSubscription = undefined;
			}
			if (this.saveSubscription) {
				this.saveSubscription.unsubscribe();
				this.saveSubscription = undefined;
			}
			if (this.changedSubscription) {
				this.changedSubscription.unsubscribe();
				this.changedSubscription = undefined;
			}

			const viewContainerRef = this.anchor().viewContainerRef;
			viewContainerRef.clear();

			const componentType = editorMap.get(newBone.type) ?? UnknownBoneEditorComponent;
			this.boneEditorRef = viewContainerRef.createComponent(componentType);

			this.editor = this.boneEditorRef.instance;

			this.boneEditorRef.setInput("boneEtalon", newBone);
			this.boneEditorRef.setInput("locale", this.locale());
			this.boneEditorRef.setInput("device", this.device());

			this.removeSubscription = this.editor.removed.subscribe(() => {
				this.removed.emit();
			});
			this.changedSubscription = this.editor.editing.subscribe((isEditing: boolean) =>
				this.editing.emit(isEditing),
			);
			this.saveSubscription = this.editor.saved.subscribe((newBoneValue: Bone) =>
				this.saved.emit(newBoneValue),
			);
		});

		effect(() => {
			const locale = this.locale();
			const device = this.device();
			if (this.boneEditorRef) {
				this.boneEditorRef.setInput("locale", locale);
				this.boneEditorRef.setInput("device", device);
			}
		});
	}

	public nextPreset = (): void => {
		if (this.editor === undefined || this.editor === null) {
			return;
		}

		this.editor.nextPreset();
	};

	// todo: add or remove visibility feature

	// public setDisabled = (disabled: boolean): void => {
	//   if (this.editor === undefined || this.editor === null)
	//     return;

	//   this.editor.startEditing();

	//   if (this.device === DeviceType.Desktop)
	//     this.editor.bone.visibility = setOrRemoveFlag(this.editor.bone.visibility, DeviceVisibility.Desktop, !disabled);
	//   else if (this.device === DeviceType.Tablet)
	//     this.editor.bone.visibility = setOrRemoveFlag(this.editor.bone.visibility, DeviceVisibility.Tablet, !disabled);
	//   else if (this.device === DeviceType.Mobile)
	//     this.editor.bone.visibility = setOrRemoveFlag(this.editor.bone.visibility, DeviceVisibility.Mobile, !disabled);

	//   this.editor.updateDirty();
	// }

	// todo: use this feature
	// its not working now
	public get disabled(): boolean {
		if (this.editor === undefined || this.editor === null) {
			console.warn("editor should have been set before disabled is called");
			return false;
		}

		return false;
		// todo: add or remove visibility feature
		// const visibility = this.editor.bone.visibility;

		// if (this.device === DeviceType.Desktop && hasFlag(visibility, DeviceVisibility.Desktop))
		//   return false;

		// if (this.device === DeviceType.Tablet && hasFlag(visibility, DeviceVisibility.Tablet))
		//   return false;

		// if (this.device === DeviceType.Mobile && hasFlag(visibility, DeviceVisibility.Mobile))
		//   return false;

		// return true;
	}
}
