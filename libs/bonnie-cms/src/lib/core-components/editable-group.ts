import { QueryList, ContentChildren, AfterContentInit, OnDestroy, Output, EventEmitter, Component } from '@angular/core';
import { Subscription, Subject, debounceTime } from 'rxjs';

import { EditableDirective } from './editable.directive';

@Component({
  selector: 'bonc-editable-group',
  standalone: true,
  template: '<ng-content></ng-content>'
})
export class EditableGroupComponent implements AfterContentInit, OnDestroy {
  @Output()
  public readonly editModeChange = new EventEmitter<boolean>();

  @Output()
  public readonly saved = new EventEmitter<void>();

  @Output()
  public readonly requestEditorClose = new EventEmitter<boolean>();

  @ContentChildren(EditableDirective, { descendants: true })
  public editables!: QueryList<EditableDirective>;

  private readonly subscriptions: Subscription[] = [];
  private _inEditMode = false;

  private readonly saveSubject = new Subject<void>();

  public ngAfterContentInit(): void {
    this.saveSubject
      .asObservable()
      .pipe(debounceTime(100))
      .subscribe(() => this.saved.emit());

    this.updateSubscriptions();

    this.editables.changes.subscribe
      (
        () => {
          this.clearSubscribtions();
          this.updateSubscriptions();
        }
      );
  }

  public get inEditMode(): boolean {
    return this._inEditMode;
  }

  public saveAll(): void {
    this.editables.forEach(x => x.requestSave());
  }

  public cancelAll(): void {
    this.editables.forEach(x => x.cancel());
  }

  public closeAll(): void {
    this.requestEditorClose.emit(true);
    this.editables.forEach(x => x.close());
  }

  private clearSubscribtions(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
    this.subscriptions.splice(0, this.subscriptions.length);
  }

  private updateSubscriptions(): void {

    this.editables.forEach
      (
        editable => {

          this.subscriptions.push(
            editable.saved.subscribe(
              () => this.onSave()
            )
          );

          this.subscriptions.push(
            ...editable.subscribe({
              onEditModeChange: this.updateEditMode.bind(this)
            })
          );
        }
      );
  }

  private onSave(): void {
    this.saveSubject.next();
  }

  private updateEditMode(): void {
    const newEditMode = this.editables
      .filter(x => x.inEditMode)
      .length > 0;

    if (newEditMode === this._inEditMode)
      return;

    this._inEditMode = newEditMode;

    this.editModeChange.next(newEditMode);
  }

  public ngOnDestroy(): void {
    this.clearSubscribtions();
  }
}
