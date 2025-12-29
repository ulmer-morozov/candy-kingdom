import { ChangeDetectorRef, Directive, OnInit, inject, effect, EffectRef, OnDestroy } from '@angular/core';
import { Bone } from '../generated';
import { LocalizeServiceBase } from '../localization/LocalizeServiceBase';

@Directive({
  selector: '[bonBoneDir]',
  standalone: true
})
export class BoneDirective<T extends Bone = Bone> implements OnInit, OnDestroy {
  private _bone?: T;

  private readonly cd = inject(ChangeDetectorRef);
  private readonly localizationService = inject(LocalizeServiceBase);
  private readonly _effectCleanup?: EffectRef

  constructor() {
    this.cd.detach();

    // todo: check if this is needed

    // Watch locale changes using signal
    this._effectCleanup = effect(() => {
      // Access the signal to track changes
      this.localizationService.locale();
      // need to detect changes in our detached components
      this.cd.detectChanges();
    });
  }

  public ngOnInit(): void {
  }

  public set bone(value: T) {
    this._bone = value;

    this.cd.detectChanges();
  }

  public get bone(): T {
    if (this._bone === undefined || this._bone === null)
      throw new Error('The property "bone" should be set at least once. For example in skeleton');

    return this._bone;
  }

  public ngOnDestroy(): void {
    this._effectCleanup?.destroy();
  }
}
