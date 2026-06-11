# SkyRoute Flight Booking Application

## Overview

SkyRoute is a flight search and booking application built using Angular and ASP.NET Core. The application allows users to search for available flights, compare fares from multiple airlines, and complete bookings by providing passenger details.

The system consists of two primary modules:

* **Search Flights**
* **Book Flights**

The application demonstrates modern web development practices including REST API integration, form validation, routing, asynchronous request handling, and clean architecture principles.

---

## Features

### Flight Search

* Search flights between supported airports
* Select departure date
* Choose cabin class (Economy, Business, First)
* Specify passenger count (1–9)
* Real-time fare calculation
* Display multiple airline options
* Validation for search criteria

### Flight Booking

* Select a flight from search results
* Enter passenger details
* Validate booking information
* Generate booking confirmation
* Calculate total fare automatically

### Fare Calculation

Supports multiple airline pricing models:

#### SkyRoute Airline

```text
Base Fare = Distance × 0.20
Discount = 10%
Final Fare = Base Fare - Discount
```

#### Global Air

```text
Base Fare = Distance × 0.15
Fuel Surcharge = 15%
Final Fare = Base Fare + Fuel Surcharge
```

---

## Application Flow

```text
User Search
     │
     ▼
Search Component
     │
     ▼
Search API
     │
     ▼
Flight Results
     │
     ▼
Select Flight
     │
     ▼
Booking Component
     │
     ▼
Booking API
     │
     ▼
Booking Confirmation
```

---

## Technology Stack

### Frontend

* Angular
* TypeScript
* HTML5
* CSS3
* Reactive Forms / Template Forms
* Angular Router
* HttpClient

### Backend

* ASP.NET Core Web API
* C#
* REST APIs
* Dependency Injection
* Validation Attributes

### Development Tools

* Visual Studio / VS Code
* Git
* Postman

---

## Supported Airports

| Code | Airport         |
| ---- | --------------- |
| JFK  | New York (JFK)  |
| LHR  | London Heathrow |
| DXB  | Dubai           |
| SIN  | Singapore       |
| HND  | Tokyo Haneda    |
| SYD  | Sydney          |

---

## API Endpoints

### Search Flights

```http
GET /api/SearchFlights
```

#### Query Parameters

| Parameter     | Description              |
| ------------- | ------------------------ |
| from          | Source airport code      |
| to            | Destination airport code |
| departureDate | Travel date              |
| passengers    | Number of passengers     |
| cabinClass    | Economy, Business, First |

---

### Book Flight

```http
POST /api/BookFlights
```

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
    }
  ],
  "contactEmail": "jane.doe@example.com",
  "contactPhone": "+1-202-555-0143"
}
```

---

## Validation Rules

### Search Validation

* Source and destination airports must be valid.
* Source and destination cannot be the same.
* Departure date cannot be in the past.
* Passenger count must be between 1 and 9.
* Cabin class must be:

  * Economy
  * Business
  * First

### Booking Validation

* Flight must exist.
* Sufficient seats must be available.
* Passenger details are mandatory.
* Email format must be valid.
* Cabin class must match the selected flight.

---

## Project Structure

```text
src/
│
├── app/
│   ├── search/
│   │   ├── search.component.ts
│   │   ├── search.component.html
│   │   └── search.component.css
│   │
│   ├── booking/
│   │   ├── booking.component.ts
│   │   ├── booking.component.html
│   │   └── booking.component.css
│   │
│   ├── services/
│   │   └── flight.service.ts
│   │
│   ├── models/
│   │   └── flight.models.ts
│   │
│   └── app.routes.ts
│
└── backend/
    ├── Controllers/
    │   ├── SearchFlightsController.cs
    │   └── BookFlightsController.cs
    │
    ├── Models/
    └── Services/
```

---

## Key Concepts Demonstrated

### Angular

* Standalone Components
* Routing
* Dependency Injection
* Form Handling
* HTTP Client
* State Management

### ASP.NET Core

* RESTful API Design
* Model Validation
* Dependency Injection
* Middleware
* Exception Handling
* Controller-Based APIs

### Software Design

* Separation of Concerns
* Clean Architecture Principles
* Reusable Services
* Strongly Typed Models

---

## Future Enhancements

* User Authentication (JWT)
* Role-Based Authorization (RBAC)
* Payment Gateway Integration
* Flight Cancellation
* Booking History
* Email Notifications
* Database Integration (SQL Server)
* Real-Time Seat Availability
* Caching and Performance Optimization
* Docker Containerization

---

## Getting Started

### Frontend

```bash
npm install
ng serve
```

Application URL:

```text
http://localhost:4200
```

### Backend

```bash
dotnet restore
dotnet run
```

API URL:

```text
https://localhost:5001
```

---

## Author

Developed as a full-stack flight booking application demonstrating Angular, ASP.NET Core Web API, REST architecture, validation, routing, and modern software development practices.
