import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-not-found',
  standalone: true,
  imports: [RouterLink, MatButtonModule],
  template: `
    <div class="error-page">
      <h1>404</h1>
      <p>The page you're looking for doesn't exist.</p>
      <a mat-raised-button color="primary" routerLink="/devices">Go to Dashboard</a>
    </div>
  `,
  styles: [`
    .error-page {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      height: 100vh;
      gap: 16px;
      h1 { font-size: 96px; margin: 0; color: var(--mat-sys-primary); }
      p { font-size: 18px; color: var(--mat-sys-on-surface-variant); }
    }
  `]
})
export class NotFound { }
