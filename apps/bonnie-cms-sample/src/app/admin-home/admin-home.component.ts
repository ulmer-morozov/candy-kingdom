import { Component, OnInit, inject } from '@angular/core';
import { AuthService } from '../service';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  imports: [CommonModule],
  selector: 'app-admin-home',
  templateUrl: './admin-home.component.html',
})
export default class AdminHomeComponent implements OnInit {
  public isSignedIn: boolean = false;

  private readonly authService = inject(AuthService);

  ngOnInit(): void {
    this.authService.onStateChanged().forEach((state: boolean) => {
      this.isSignedIn = state;
    });
    this.authService.isSignedIn().forEach((signedIn: boolean) => {
      this.isSignedIn = signedIn;
    });
  }
}
