import { Directive, forwardRef, output } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { Unsubscribable } from "rxjs";

@Directive({
	standalone: true,
	selector: "[boncEditable]",
	providers: [
		{ provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => EditableDirective), multi: true },
	],
})
export class EditableDirective<T = unknown> {
	public readonly saved = output<T>();
	public readonly editModeChange = output<boolean>();
	public readonly externalSaveCall = output<void>();
	public readonly canceled = output<void>();
	public readonly valueChange = output<T | undefined>();

	private propagateChange: (newValue: T) => void = () => {};

	private _inEditMode = false;
	private _isDirty = false;

	private _value?: T;
	private _originalValue?: T;
	private _storedData?: string;

	public get inEditMode(): boolean {
		return this._inEditMode;
	}

	public subscribe(params: {
		readonly onValueChange?: (x: T | undefined) => void;
		readonly onEditModeChange?: (x: boolean) => void;
		readonly onSaveRequest?: () => void;
	}): Unsubscribable[] {
		const subscriptions: Unsubscribable[] = [];

		if (params.onValueChange !== undefined && params.onValueChange !== null) {
			subscriptions.push(
				this.valueChange.subscribe((x) => {
					if (params.onValueChange) params.onValueChange(x);
				}),
			);
		}

		if (params.onEditModeChange !== undefined) {
			subscriptions.push(
				this.editModeChange.subscribe((x) => {
					if (params.onEditModeChange) params.onEditModeChange(x);
				}),
			);
		}

		if (params.onSaveRequest !== undefined) {
			subscriptions.push(
				this.externalSaveCall.subscribe(() => {
					if (params.onSaveRequest) params.onSaveRequest();
				}),
			);
		}

		return subscriptions;
	}

	public requestSave() {
		this.externalSaveCall.emit();
	}

	public startEditing = (): void => {
		if (this._inEditMode) {
			this.updateDirty();
			return;
		}

		this._inEditMode = true;
		this.updateDirty();
		this.editModeChange.emit(true);
	};

	public save(newData?: T): void {
		if (!this._inEditMode) {
			console.warn("save before edit mode"); //todo: fix that
		}

		this.finishEditing();

		const newUnqieNotEmptyData =
			this.value === newData || newData === undefined
				? JSON.parse(JSON.stringify(this.value))
				: newData;

		this.setOriginal(newUnqieNotEmptyData);
		this.updateDirty();

		this.saved.emit(newUnqieNotEmptyData);

		this.propagateChange(newUnqieNotEmptyData);
	}

	public patchSave(propName: keyof T, newDataParts: T[keyof T]): void {
		if (this._originalValue === undefined || this._originalValue === null) return;

		if (typeof this._originalValue !== "object") {
			console.warn("patch save called on not object type");
			return;
		}

		(this._originalValue as any)[propName] = newDataParts;

		this.setOriginal(this._originalValue);

		this.updateDirty();

		if (!this.isDirty) this.close();
	}

	// change model without opening or closing the editor

	public silentPatch(dict: Partial<T>): void {
		for (const key in dict) {
			// eslint-disable-next-line no-prototype-builtins
			if (!dict.hasOwnProperty(key)) continue;

			const propVal = dict[key];

			if (this._value !== undefined && this._value !== null) {
				this._value[key] = propVal as any;
			}

			if (this._originalValue !== undefined && this._originalValue !== null) {
				this._originalValue[key] = propVal as any;
			}
		}
		this._storedData = JSON.stringify(this._originalValue);
	}

	public cancel(): void {
		if (!this._inEditMode) return;

		this.finishEditing();

		// reset value
		this.setValue(this._originalValue);

		this.canceled.emit();
	}

	public close(): void {
		this.finishEditing();
	}

	private finishEditing(): void {
		if (!this._inEditMode) return;

		this._inEditMode = false;
		this.editModeChange.emit(false);
	}

	private setValue(newValue?: T): void {
		this._value =
			newValue === null || newValue === undefined
				? undefined
				: JSON.parse(JSON.stringify(newValue));

		this.valueChange.emit(this._value);
	}

	public set value(newValue: T | undefined) {
		this.setValue(newValue);
	}

	public get value(): T | undefined {
		return this._value;
	}

	public updateDirty(): void {
		this._isDirty = JSON.stringify(this._value) !== this._storedData;
	}

	public markAsDirty(): void {
		this._isDirty = true;
	}

	public get isDirty(): boolean {
		return this._isDirty;
	}

	private setOriginal(newOriginalValue: T) {
		this._originalValue = newOriginalValue;
		this._storedData = JSON.stringify(newOriginalValue);
	}

	// #region ngModel

	writeValue(newValue: T): void {
		this.finishEditing();

		this.setOriginal(newValue);
		this._isDirty = false;

		this.setValue(newValue);
	}

	setDisabledState?(): void {}

	registerOnChange(tellAngularThatSomethingIsChanged: (newValue: T) => void): void {
		this.propagateChange = (newValue: T): void => {
			tellAngularThatSomethingIsChanged(newValue);
		};
	}

	registerOnTouched(): void {}

	// #endregion
}
