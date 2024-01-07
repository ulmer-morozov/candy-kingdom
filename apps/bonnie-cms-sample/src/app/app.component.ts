import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  standalone: true,
  imports: [RouterModule],
  selector: 'candy-kingdom-root',
  template:
    '<nav><a routerLink="/">Face</a><a routerLink="/admin">Admin</a></nav><router-outlet></router-outlet>',
})
export class AppComponent {
  title = 'bonnie-cms-sample';
}
