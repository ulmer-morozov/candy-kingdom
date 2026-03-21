import { Directive, forwardRef, output, signal } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";

@Directive({
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

	// eslint-disable-next-line @typescript-eslint/no-empty-function
	private propagateChange: (newValue: T) => void = () => {};

	private readonly _inEditMode = signal(false);
	public readonly inEditMode = this._inEditMode.asReadonly();

	private readonly _isDirty = signal(false);
	public readonly isDirty = this._isDirty.asReadonly();

	private _value?: T;
	private _originalValue?: T;
	private _storedData?: string;

	public requestSave() {
		this.externalSaveCall.emit();
	}

	public startEditing = (): void => {
		if (this.inEditMode()) {
			this.updateDirty();
			return;
		}

		this._inEditMode.set(true);
		this.updateDirty();
		this.editModeChange.emit(true);
	};

	public save(newData?: T): void {
		if (!this.inEditMode()) {
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
		if (this._originalValue === undefined || this._originalValue === null) {
			return;
		}

		if (typeof this._originalValue !== "object") {
			console.warn("patch save called on not object type");
			return;
		}

		(this._originalValue as any)[propName] = newDataParts;

		this.setOriginal(this._originalValue);

		this.updateDirty();

		if (!this.isDirty()) {
			this.close();
		}
	}

	// change model without opening or closing the editor

	public silentPatch(dict: Partial<T>): void {
		for (const key in dict) {
			if (!Object.hasOwn(dict, key)) {
				continue;
			}

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
		if (!this.inEditMode()) {
			return;
		}

		this.finishEditing();

		// reset value
		this.setValue(this._originalValue);

		this.canceled.emit();
	}

	public close(): void {
		this.finishEditing();
	}

	private finishEditing(): void {
		if (!this.inEditMode()) {
			return;
		}

		this._inEditMode.set(false);
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
		this._isDirty.set(JSON.stringify(this._value) !== this._storedData);
	}

	public markAsDirty(): void {
		this._isDirty.set(true);
	}

	private setOriginal(newOriginalValue: T) {
		this._originalValue = newOriginalValue;
		this._storedData = JSON.stringify(newOriginalValue);
	}

	// #region ngModel

	writeValue(newValue: T): void {
		this.finishEditing();

		this.setOriginal(newValue);
		this._isDirty.set(false);

		this.setValue(newValue);
	}

	setDisabledState?(): void {
		// required by ControlValueAccessor
	}

	registerOnChange(tellAngularThatSomethingIsChanged: (newValue: T) => void): void {
		this.propagateChange = (newValue: T): void => {
			tellAngularThatSomethingIsChanged(newValue);
		};
	}

	registerOnTouched(): void {
		// required by ControlValueAccessor
	}

	// #endregion
}
