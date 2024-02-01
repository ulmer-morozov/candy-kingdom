import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from './service';
import { CommonModule } from '@angular/common';
import { AuthGuard } from './guard';
import { RouterLocalizeService } from './router-localize.service';
import { LocalizeServiceBase } from '@candy-kingdom/bonnie';

@Component({
  standalone: true,
  imports: [CommonModule, RouterModule],
  providers: [AuthGuard, AuthService, { provide: LocalizeServiceBase, useClass: RouterLocalizeService }],
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styles: `
    :host{
      --bg-color: white;
      --text-color: black;
    }
  `
})
export default class AdminComponent implements OnInit {
  public isSignedIn: boolean = false;

  constructor(private auth: AuthService, private router: Router) { }

  ngOnInit() {
    this.auth.onStateChanged().forEach(() => {
      this.auth
        .isSignedIn()
        .forEach((signedIn: boolean) => (this.isSignedIn = signedIn));
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
}
