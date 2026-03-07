import {
	Component,
	contentChildren,
	DestroyRef,
	effect,
	inject,
	output,
} from "@angular/core";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";

import { debounceTime, Subject, type Unsubscribable } from "rxjs";

import { EditableDirective } from "./editable.directive";

@Component({
	selector: "bonc-editable-group",
	template: "<ng-content></ng-content>",
})
export class EditableGroupComponent {
	public readonly editModeChange = output<boolean>();

	public readonly saved = output<void>();

	public readonly requestEditorClose = output<boolean>();

	public readonly editables = contentChildren(EditableDirective, { descendants: true });

	private readonly subscriptions: Unsubscribable[] = [];
	private _inEditMode = false;

	private readonly saveSubject = new Subject<void>(); // todo: use signal

	constructor() {
		this.saveSubject
			.asObservable()
			.pipe(debounceTime(100), takeUntilDestroyed())
			.subscribe(() => this.saved.emit(undefined));

		effect(() => {
			this.clearSubscriptions();
			this.updateSubscriptions();
		});

		inject(DestroyRef).onDestroy(() => this.clearSubscriptions());
	}

	public get inEditMode(): boolean {
		return this._inEditMode;
	}

	public saveAll(): void {
		this.editables().forEach((x) => x.requestSave());
	}

	public cancelAll(): void {
		this.editables().forEach((x) => x.cancel());
	}

	public closeAll(): void {
		this.requestEditorClose.emit(true);
		this.editables().forEach((x) => x.close());
	}

	private clearSubscriptions(): void {
		this.subscriptions.forEach((x) => x.unsubscribe());
		this.subscriptions.splice(0, this.subscriptions.length);
	}

	private updateSubscriptions(): void {
		this.editables().forEach((editable) => {
			this.subscriptions.push(editable.saved.subscribe(() => this.onSave()));

			this.subscriptions.push(
				...editable.subscribe({
					onEditModeChange: this.updateEditMode.bind(this),
				}),
			);
		});
	}

	private onSave(): void {
		this.saveSubject.next();
	}

	private updateEditMode(): void {
		const newEditMode = this.editables().filter((x) => x.inEditMode).length > 0;

		if (newEditMode === this._inEditMode) return;

		this._inEditMode = newEditMode;

		this.editModeChange.emit(newEditMode);
	}
}
