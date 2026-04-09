import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Navbar } from './shared/components/navbar/navbar';
import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, Navbar],
  template: `
    @if (authService.isLoggedIn()) {
      <app-navbar />
    }
    <router-outlet />
  `,
  styles: [`
    :host { display: block; min-height: 100vh; }
  `]
})
export class App {
  authService = inject(AuthService);
}
