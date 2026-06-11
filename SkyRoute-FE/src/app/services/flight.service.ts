import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, forkJoin, map, catchError, of } from 'rxjs';
import { FlightResult, CabinClass, BookingRequest, BookingResponse } from '../models/flight.models';

@Injectable({ providedIn: 'root' })
export class FlightService {
  private readonly baseUrl = 'https://localhost:7290/api';

  constructor(private http: HttpClient) {}

  private buildParams(from: string, to: string, departureDate: string, passengers: number, cabinClass: CabinClass): HttpParams {
    return new HttpParams()
      .set('from', from)
      .set('to', to)
      .set('departureDate', departureDate)
      .set('passengers', passengers.toString())
      .set('cabinClass', cabinClass.toString());
  }

  searchFlights(
    from: string,
    to: string,
    departureDate: string,
    passengers: number,
    cabinClass: CabinClass
  ): Observable<FlightResult[]> {
    console.log('FlightService: Searching flights with params', { from, to, departureDate, passengers, cabinClass });
    const params = this.buildParams(from, to, departureDate, passengers, cabinClass);

    const budgetWings$ = this.http
      .get<FlightResult[]>(`${this.baseUrl}/BudgetWings`, { params })
      .pipe(catchError(() => of([])));  // if one airline fails, still show the other

    const globalAir$ = this.http
      .get<FlightResult[]>(`${this.baseUrl}/GlobalAirFlights`, { params })
      .pipe(catchError(() => of([])));

    return forkJoin({ budget: budgetWings$, global: globalAir$ }).pipe(
      map(({ budget, global }) => {
        // Attach origin/destination since API doesn't return them
        const tag = (flights: FlightResult[]) =>
          flights.map(f => ({ ...f, origin: from, destination: to }));
        return [...tag(budget), ...tag(global)];
      })
    );
  }

  bookFlight(request: BookingRequest): Observable<BookingResponse> {
    return this.http.post<BookingResponse>(`${this.baseUrl}/BookFlights`, request);
  }
}