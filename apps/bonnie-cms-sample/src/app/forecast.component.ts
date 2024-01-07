import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

@Component({
  standalone: true,
  imports: [CommonModule],
  selector: 'candy-kingdom-forecast',
  templateUrl: './forecast.component.html',
  styleUrls: ['./forecast.component.scss'],
})
export class ForecastComponent implements OnInit {
  public forecasts: WeatherForecast[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.getForecasts();
  }

  getForecasts() {
    this.http
      .get<WeatherForecast[]>('/api/weatherforecast', {
        withCredentials: true, // can use an interceptor for this as well
      })
      .forEach((result) => {
        this.forecasts = result;
      })
      .catch((error) => {
        console.error(error);
      });
  }
}
