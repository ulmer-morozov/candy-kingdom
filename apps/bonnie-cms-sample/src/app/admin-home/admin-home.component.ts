import { Component, OnInit } from '@angular/core';
import { AuthService } from '../service';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  imports: [CommonModule],
  selector: 'candy-kingdom-home',
  templateUrl: './admin-home.component.html',
})
export class AdminHomeComponent implements OnInit {
  public isSignedIn: boolean = false;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.authService.onStateChanged().forEach((state: boolean) => {
      this.isSignedIn = state;
    });
    this.authService.isSignedIn().forEach((signedIn: boolean) => {
      this.isSignedIn = signedIn;
    });
  }
}
