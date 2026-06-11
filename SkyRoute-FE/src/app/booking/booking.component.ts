import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ChangeDetectorRef } from '@angular/core';
import { FlightService } from '../services/flight.service';
import {
  FlightResult,
  CabinClass,
  BookingRequest,
  BookingResponse,
  PassengerDetails,
} from '../models/flight.models';

@Component({
  selector: 'app-booking',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.scss'],
})
export class BookingComponent implements OnInit {
  CabinClass = CabinClass;

  flight!: FlightResult;
  selectedCabin!: CabinClass;
  passengers!: number;

  cabinLabels: Record<CabinClass, string> = {
    [CabinClass.Economy]: 'Economy',
    [CabinClass.Bussiness]: 'Business',
    [CabinClass.FirstClass]: 'First Class',
  };

  passengerForms: PassengerDetails[] = [];

  loading = false;
  confirmation: BookingResponse | null = null;
  errorMessage = '';

  constructor(private flightService: FlightService, private router: Router, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    const state = history.state;
    if (!state?.flight) {
      this.router.navigate(['/search']);
      return;
    }

    this.flight = state.flight;
    this.selectedCabin = state.cabinClass ?? CabinClass.Economy;
    this.passengers = state.passengers ?? 1;

    // Build one blank form per passenger (1–9)
    this.passengerForms = Array.from({ length: this.passengers }, () => ({
      fullName: '',
      emailAddress: '',
      documentNumber: '',
    }));
  }

  get totalFare(): number {
    return Math.round(this.flight.finalFare * this.passengers * 100) / 100;
  }

  get flightDuration(): string {
    const dep = new Date(this.flight.departureTime);
    const arr = new Date(this.flight.arrivalTime);
    const diffMs = arr.getTime() - dep.getTime();
    const hours = Math.floor(diffMs / 3600000);
    const mins = Math.floor((diffMs % 3600000) / 60000);
    return `${hours}h ${mins}m`;
  }

  isPassengerValid(p: PassengerDetails): boolean {
    return (
      p.fullName.trim().length >= 2 &&
      /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(p.emailAddress) &&
      p.documentNumber.trim().length >= 4
    );
  }

  get isFormValid(): boolean {
    return this.passengerForms.length > 0 &&
      this.passengerForms.every(p => this.isPassengerValid(p));
  }

  onBack(): void {
    this.router.navigate(['/search']);
  }

  onConfirm(): void {
    if (!this.isFormValid) return;
    this.loading = true;
    this.errorMessage = '';

    const request: BookingRequest = {
      flightId: this.flight.flightId,
      cabinClass: this.selectedCabin,
      passengers: this.passengerForms,  // all 1–9 passengers
    };

    this.flightService.bookFlight(request).subscribe({
      next: (res) => {
        this.confirmation = res;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.errorMessage = err?.error?.error ?? 'Booking failed. Please try again.';
        this.loading = false;
      },
    });
  }

  onNewSearch(): void {
    this.router.navigate(['/search']);
  }

  trackByIndex(index: number): number {
    return index;
  }
}