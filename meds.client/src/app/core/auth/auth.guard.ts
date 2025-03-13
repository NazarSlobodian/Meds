import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router)
  const requestedPath = route.pathFromRoot.map(r => r.routeConfig?.path).join('/');
  if (authService.getRole() === 'patient' && requestedPath.startsWith("/patient")) {
    return true;
  }
  if (authService.getRole() === 'technician' && requestedPath.startsWith('/technician')) {
    return true;
  }
  if (authService.getRole() === "admin" && requestedPath.startsWith("/lab-admin")) {
    return true;
  }
  router.navigate(['/login']);
  return false;
};
