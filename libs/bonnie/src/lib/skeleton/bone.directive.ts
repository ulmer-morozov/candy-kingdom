import { ChangeDetectorRef, Directive, OnInit, inject } from '@angular/core';
import { Bone } from '../generated';
import { UnsubscriberService } from '../core/unsubscribe.service';
import { LocalizeServiceBase } from '../localization/LocalizeServiceBase';

@Directive({
  selector: '[bonBoneDir]',
  standalone: true
})
export class BoneDirective<T extends Bone = Bone> implements OnInit {
  private _bone?: T;

  private readonly cd = inject(ChangeDetectorRef);
  private readonly _u = inject(UnsubscriberService);
  private readonly localizationService = inject(LocalizeServiceBase);

  constructor() {
    this.cd.detach();
  }

  public ngOnInit(): void {
    this.localizationService.locale$
      .pipe(this._u.takeUntilDestroy)
      .subscribe(() => {
        // need to detect changes in our detached components
        this.cd.detectChanges();
      })
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
}
