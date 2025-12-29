import { Component, EffectRef, OnDestroy, ViewEncapsulation, effect, inject } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../service';
import { CommonModule } from '@angular/common';
import { AuthGuard } from '../guard';
import { RouterLocalizeService } from '../router-localize.service';
import { LocalizeServiceBase } from '@candy-kingdom/bonnie';

@Component({
  standalone: true,
  imports: [CommonModule, RouterModule],
  providers: [AuthGuard, AuthService, { provide: LocalizeServiceBase, useClass: RouterLocalizeService }],
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss',
  encapsulation: ViewEncapsulation.None,
  styles: `
    :host{
      --bg-color: white;
      --text-color: black;
    }
  `
})
export default class AdminComponent implements OnDestroy {
  public isSignedIn: boolean = false;

  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);
  private readonly _effectCleanup?: EffectRef;

  constructor() {
    effect(() => {
      this.isSignedIn = this.auth.authState();
    });
  }

  signOut() {
    if (this.isSignedIn) {
      this.auth.signOut().forEach((response) => {
        if (response) {
          this.router.navigateByUrl('');
        }
      });
    }
  }

  ngOnDestroy(): void {
    this._effectCleanup?.destroy();
  }
}
