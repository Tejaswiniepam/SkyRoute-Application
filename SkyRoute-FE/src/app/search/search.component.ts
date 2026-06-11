import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { FlightService } from '../services/flight.service';
import { FlightResult, CabinClass } from '../models/flight.models';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss'],
})
export class SearchComponent {
  CabinClass = CabinClass;

  form = {
    from: '',
    to: '',
    departureDate: '',
    passengers: 1,
    cabinClass: CabinClass.Economy,
  };

  flights: FlightResult[] = [];
  loading = false;
  searched = false;
  errorMessage = '';

  airports = ['NYC', 'LAX', 'MIA', 'ORD', 'DFW', 'ATL', 'SFO'];

  cabinLabels: Record<CabinClass, string> = {
    [CabinClass.Economy]: 'Economy',
    [CabinClass.Bussiness]: 'Bussiness',
    [CabinClass.FirstClass]: 'FirstClass',
  };

  constructor(private flightService: FlightService, private router: Router, private cdr: ChangeDetectorRef) {}

  get isFormValid(): boolean {
    return (
      this.form.from.trim() !== '' &&
      this.form.to.trim() !== '' &&
      this.form.from !== this.form.to &&
      this.form.departureDate !== '' &&
      this.form.passengers >= 1 &&
      this.form.passengers <= 9
    );
  }

  getFlightDuration(flight: FlightResult): string {
    const dep = new Date(flight.departureTime);
    const arr = new Date(flight.arrivalTime);
    const diffMs = arr.getTime() - dep.getTime();
    const hours = Math.floor(diffMs / 3600000);
    const mins = Math.floor((diffMs % 3600000) / 60000);
    return `${hours}h ${mins}m`;
  }

  onSearch(): void {
    console.log('Searching flights with criteria:', this.form);
    if (!this.isFormValid) return;
    this.loading = true;
    this.searched = false;
    this.errorMessage = '';
    this.flights = [];

    this.flightService
      .searchFlights(
        this.form.from,
        this.form.to,
        this.form.departureDate,
        this.form.passengers,
        this.form.cabinClass
      )
      .subscribe({
        next: (results) => {
          console.log('Received flight search results:', results);
          this.flights = results;
          this.searched = true;
          this.loading = false;
          this.cdr.detectChanges();
        },
        error: () => {
          this.errorMessage = 'Unable to reach the server. Make sure the .NET backend is running.';
          this.searched = true;
          this.loading = false;
        },
      });
  }

  // Navigate to booking page passing flight + search context
  onBook(flight: FlightResult): void {
    this.router.navigate(['/booking'], {
      state: {
        flight,
        cabinClass: this.form.cabinClass,
        passengers: this.form.passengers,
        from: this.form.from,
        to: this.form.to,
      },
    });
  }
}