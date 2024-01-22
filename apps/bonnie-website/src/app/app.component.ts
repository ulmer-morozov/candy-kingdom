import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NxWelcomeComponent } from './nx-welcome.component';
import { MarcyElementsModule } from '@candy-kingdom/bonnie';

@Component({
  standalone: true,
  imports: [NxWelcomeComponent, MarcyElementsModule, RouterModule],
  selector: 'candy-kingdom-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'bonnie-website';
}
