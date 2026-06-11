import { Routes } from '@angular/router';
import { SearchComponent } from './search/search.component';
import { BookingComponent } from './booking/booking.component';

export const routes: Routes = [
  { path: '', redirectTo: 'search', pathMatch: 'full' },
  { path: 'search', component: SearchComponent },
  { path: 'booking', component: BookingComponent },
];