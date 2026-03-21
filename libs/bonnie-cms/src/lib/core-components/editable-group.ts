import {
	Component,
	contentChildren,
	DestroyRef,
	effect,
	inject,
	output,
	signal,
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

	private readonly _subscriptions: Unsubscribable[] = [];
	public readonly inEditMode = signal(false);

	private readonly _saveSubject = new Subject<void>(); // todo: use signal

	constructor() {
		const saveSubscription = this._saveSubject
			.asObservable()
			.pipe(debounceTime(100), takeUntilDestroyed())
			.subscribe(() => this.saved.emit(undefined));

		effect(() => {
			this.clearSubscriptions();
			this.updateSubscriptions();
		});

		inject(DestroyRef).onDestroy(() => {
			saveSubscription.unsubscribe();
			this.clearSubscriptions();
		});
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
		this._subscriptions.forEach((x) => x.unsubscribe());
		this._subscriptions.splice(0, this._subscriptions.length);
	}

	private updateSubscriptions(): void {
		this.editables().forEach((editable) => {
			this._subscriptions.push(
				editable.saved.subscribe(this.onSave.bind(this)),
				editable.editModeChange.subscribe(this.updateEditMode.bind(this)),
			);
		});
	}

	private onSave(): void {
		this._saveSubject.next();
	}

	private updateEditMode(): void {
		const newEditMode = this.editables().filter((x) => x.inEditMode()).length > 0;

		if (newEditMode === this.inEditMode()) {
			return;
		}

		this.inEditMode.set(newEditMode);

		this.editModeChange.emit(newEditMode);
	}
}
