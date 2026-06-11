export enum CabinClass {
  Economy = 0,
  Bussiness = 1,
  FirstClass = 2,
}

// Shared flight result shape (normalized from both airlines)
export interface FlightResult {
  flightId: number;
  airline: string;
  origin: string;
  destination: string;
  departureTime: string;
  arrivalTime: string;
  distanceMiles: number;
  baseFare: number;
  finalFare: number;
  // BudgetWings specific
  discountAmount?: number;
  // GlobalAir specific
  fuelSurcharge?: number;
}

export interface PassengerDetails {
  fullName: string;
  emailAddress: string;
  documentNumber: string;
}

export interface BookingRequest {
  flightId: number;
  cabinClass: CabinClass;
  passengers: PassengerDetails[];
}

export interface BookingResponse {
  success: boolean;
  bookingReference: string;
  farePerPassenger: number;
  totalFare: number;
  passengerCount: number;
  passengerNames: string[];
}