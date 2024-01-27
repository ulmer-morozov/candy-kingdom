import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DataService } from '../data.service';

@Component({
  standalone: true,
  imports: [CommonModule],
  selector: 'app-admin-settings',
  templateUrl: './admin-settings.component.html',
})
export class AdminSettingsComponent implements OnInit {
  private readonly _dataService = inject(DataService);

  public ngOnInit(): void {

  }
}
