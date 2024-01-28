import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from './service';
import { CommonModule } from '@angular/common';
import { AuthGuard } from './guard';

@Component({
  standalone: true,
  imports: [CommonModule, RouterModule],
  providers: [AuthGuard, AuthService],
  selector: 'app-admin',
  templateUrl: './admin.component.html'
})
export default class AdminComponent implements OnInit {
  public isSignedIn: boolean = false;

  constructor(private auth: AuthService, private router: Router) { }

  ngOnInit() {
    this.auth.onStateChanged().forEach((state: any) => {
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
