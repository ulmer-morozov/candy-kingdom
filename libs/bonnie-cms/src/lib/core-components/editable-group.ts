import {
	QueryList,
	ContentChildren,
	AfterContentInit,
	OnDestroy,
	Component,
	output,
} from "@angular/core";
import { Subject, Unsubscribable, debounceTime } from "rxjs";

import { EditableDirective } from "./editable.directive";

@Component({
	selector: "bonc-editable-group",
	standalone: true,
	template: "<ng-content></ng-content>",
})
export class EditableGroupComponent implements AfterContentInit, OnDestroy {
	public readonly editModeChange = output<boolean>();

	public readonly saved = output<void>();

	public readonly requestEditorClose = output<boolean>();

	@ContentChildren(EditableDirective, { descendants: true })
	public editables!: QueryList<EditableDirective>;

	private readonly subscriptions: Unsubscribable[] = [];
	private _inEditMode = false;

	private readonly saveSubject = new Subject<void>();

	public ngAfterContentInit(): void {
		this.saveSubject
			.asObservable()
			.pipe(debounceTime(100))
			.subscribe(() => this.saved.emit(undefined));

		this.updateSubscriptions();

		this.editables.changes.subscribe(() => {
			this.clearSubscribtions();
			this.updateSubscriptions();
		});
	}

	public get inEditMode(): boolean {
		return this._inEditMode;
	}

	public saveAll(): void {
		this.editables.forEach((x) => x.requestSave());
	}

	public cancelAll(): void {
		this.editables.forEach((x) => x.cancel());
	}

	public closeAll(): void {
		this.requestEditorClose.emit(true);
		this.editables.forEach((x) => x.close());
	}

	private clearSubscribtions(): void {
		this.subscriptions.forEach((x) => x.unsubscribe());
		this.subscriptions.splice(0, this.subscriptions.length);
	}

	private updateSubscriptions(): void {
		this.editables.forEach((editable) => {
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
		const newEditMode = this.editables.filter((x) => x.inEditMode).length > 0;

		if (newEditMode === this._inEditMode) return;

		this._inEditMode = newEditMode;

		this.editModeChange.emit(newEditMode);
	}

	public ngOnDestroy(): void {
		this.clearSubscribtions();
	}
}
