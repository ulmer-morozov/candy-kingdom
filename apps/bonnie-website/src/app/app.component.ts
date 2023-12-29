import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NxWelcomeComponent } from './nx-welcome.component';
import { BonnieComponent } from '@candy-kingdom/bonnie';

@Component({
  standalone: true,
  imports: [NxWelcomeComponent,BonnieComponent, RouterModule],
  selector: 'candy-kingdom-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'bonnie-website';
}
