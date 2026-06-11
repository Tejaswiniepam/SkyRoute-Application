# SkyRoute Flight Booking — API Specification

## 1. Overview

SkyRoute is a flight search and booking application. The frontend consists of two
components — `SearchComponent` and `BookingComponent` — which interact with two
backend APIs.

```
┌─────────────────────┐
│ SearchComponent     │
└──────────┬──────────┘
           │
           │ User enters:
           │ From
           │ To
           │ Date
           │ Passengers
           │ Cabin Class
           ▼
      Search API
           │
           ▼
     Flight Results
           │
           │ User selects flight
           ▼
┌─────────────────────┐
│ BookingComponent    │
└──────────┬──────────┘
           │
           │ Enter passenger details
           │
           ▼
       Booking API
           │
           ▼
  Booking Confirmation
```

---

## 2. Supported Airports (Hardcoded)

| Code | Airport |
|------|---------|
| JFK  | New York (JFK) |
| LHR  | London Heathrow |
| DXB  | Dubai |
| SIN  | Singapore |
| HND  | Tokyo Haneda |
| SYD  | Sydney |

---

## 3. Search API

### `GET /api/SearchFlights`

Returns available flights matching the search criteria, including computed fares.

#### Query Parameters

| Parameter      | Type     | Required | Constraints                                  |
|----------------|----------|----------|-----------------------------------------------|
| `from`         | string   | Yes      | One of the 6 hardcoded airport codes |
| `to`           | string   | Yes      | One of the 6 hardcoded airport codes, must differ from `from` |
| `departureDate`| date     | Yes      | Format `yyyy-MM-dd`, must be today or later |
| `passengers`   | int      | Yes      | Range **1–9** |
| `cabinClass`   | string   | Yes      | One of: `Economy`, `Business`, `First` |

#### Validation Rules

- `from` and `to` must each be one of the 6 supported airport codes.
- `from` cannot equal `to`.
- `departureDate` cannot be in the past.
- `passengers` must be an integer between 1 and 9 inclusive.
- `cabinClass` must be exactly one of `Economy`, `Business`, `First`.
- If any rule fails → return `400 Bad Request` with a descriptive message.
- If no flights match → return `404 Not Found`.

#### Fare Calculation Rules

Two fare models are applied depending on airline, returned per flight:

**SkyRoute fare:**
```
baseFare      = distanceMiles * 0.20        // $20 per 100 miles
discount      = baseFare * 0.10             // 10% promotional discount
finalFare     = max(29.99, round(baseFare - discount, 2))
```

**Global Air fare:**
```
baseFare      = round(distanceMiles * 0.15, 2)   // $15 per 100 miles
fuelSurcharge = round(baseFare * 0.15, 2)        // 15% fuel surcharge
finalFare     = round(baseFare + fuelSurcharge, 2)
```

Fares are recalculated at search time (not stored), so pricing always reflects
the current rules and applies only to flights with available seats.

#### Sample Request

```
GET /api/SearchFlights?from=JFK&to=LHR&departureDate=2026-07-15&passengers=2&cabinClass=Economy
```

#### Sample Response — `200 OK`

```json
[
  {
    "flightId": "SR-204",
    "airline": "SkyRoute",
    "from": "JFK",
    "to": "LHR",
    "departureTime": "2026-07-15T18:30:00Z",
    "arrivalTime": "2026-07-16T06:45:00Z",
    "distanceMiles": 3450,
    "cabinClass": "Economy",
    "seatsAvailable": 24,
    "fare": {
      "baseFare": 690.00,
      "discount": 69.00,
      "finalFare": 621.00
    }
  },
  {
    "flightId": "GA-118",
    "airline": "Global Air",
    "from": "JFK",
    "to": "LHR",
    "departureTime": "2026-07-15T21:00:00Z",
    "arrivalTime": "2026-07-16T09:10:00Z",
    "distanceMiles": 3450,
    "cabinClass": "Economy",
    "seatsAvailable": 12,
    "fare": {
      "baseFare": 517.50,
      "fuelSurcharge": 77.63,
      "finalFare": 595.13
    }
  }
]
```

#### Sample Response — `400 Bad Request`

```json
{
  "error": "Passengers must be between 1 and 9."
}
```

#### Sample Response — `404 Not Found`

```json
{
  "error": "No flights available for the selected route and date."
}
```

---

## 4. Booking API

### `POST /api/BookFlights`

Creates a booking for a selected flight and returns a confirmation.

#### Request Body

```json
{
  "flightId": "SR-204",
  "cabinClass": "Economy",
  "passengers": [
    {
      "firstName": "Jane",
      "lastName": "Doe",
      "dateOfBirth": "1990-04-12",
      "passportNumber": "X1234567"
    },
    {
      "firstName": "John",
      "lastName": "Doe",
      "dateOfBirth": "1988-11-02",
      "passportNumber": "X7654321"
    }
  ],
  "contactEmail": "jane.doe@example.com",
  "contactPhone": "+1-202-555-0143"
}
```

#### Validation Rules

- `flightId` must reference a flight returned by Search API and must still have
  enough `seatsAvailable` for the number of passengers.
- Number of `passengers` entries must match the `passengers` count from the
  original search (1–9).
- Each passenger requires `firstName`, `lastName`, `dateOfBirth`, `passportNumber`.
- `contactEmail` must be a valid email format.
- `cabinClass` must match one of `Economy`, `Business`, `First` and be available
  on the selected flight.

#### Sample Response — `201 Created`

```json
{
  "bookingId": "BKG-87421",
  "status": "Confirmed",
  "flightId": "SR-204",
  "airline": "SkyRoute",
  "from": "JFK",
  "to": "LHR",
  "departureTime": "2026-07-15T18:30:00Z",
  "cabinClass": "Economy",
  "passengers": 2,
  "totalFare": 1242.00,
  "createdAt": "2026-06-11T10:42:00Z"
}
```

#### Sample Response — `400 Bad Request`

```json
{
  "error": "Not enough seats available for the requested number of passengers."
}
```

#### Sample Response — `404 Not Found`

```json
{
  "error": "Flight not found."
}
```

---

## 5. End-to-End Flow

1. **SearchComponent** collects `From`, `To`, `Date`, `Passengers`, `Cabin Class`.
2. Frontend calls `GET /api/SearchFlights` with these as query parameters.
3. Backend validates inputs, finds matching flights, computes fares, returns results.
4. **SearchComponent** displays the **Flight Results** list to the user.
5. User selects a flight → frontend navigates to **BookingComponent**, passing the
   selected `flightId`, `cabinClass`, and `passengers` count.
6. **BookingComponent** collects passenger details and contact info.
7. Frontend calls `POST /api/BookFlights` with the booking payload.
8. Backend validates, creates the booking, and returns a **Booking Confirmation**.
9. **BookingComponent** displays the confirmation (`bookingId`, fare, flight details).

---

## 6. Controller File Mapping

| Endpoint | File |
|----------|------|
| `GET /api/SearchFlights` | `SearchFlights.cs` |
| `POST /api/BookFlights` | `BookFlights.cs` |

`SearchFlights.cs` contains: airport list constants, request model with validation
attributes (`Range(1,9)` for passengers, allowed values for cabin class), fare
calculation logic for both SkyRoute and Global Air, and the search endpoint.
