import { Component, EffectRef, OnDestroy, effect, inject } from '@angular/core';
import { AuthService } from '../service';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  imports: [CommonModule],
  selector: 'app-admin-home',
  templateUrl: './admin-home.component.html',
})
export default class AdminHomeComponent implements OnDestroy {
  public isSignedIn: boolean = false;

  private readonly authService = inject(AuthService);
  private readonly _effectCleanup?: EffectRef;

  constructor() {
    effect(() => {
      this.isSignedIn = this.authService.authState();
    });
  }

  ngOnDestroy(): void {
    this._effectCleanup?.destroy();
  }
}
