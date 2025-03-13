import { Component } from '@angular/core';

@Component({
  selector: 'app-theme-toggler',
  templateUrl: './theme-toggler.component.html',
  styleUrl: './theme-toggler.component.css'
})
export class ThemeTogglerComponent {
  private currentTheme: string = "light";
  constructor() {
    const savedTheme = localStorage.getItem("theme");
    if (savedTheme) {
      this.setTheme(savedTheme);
    } else {
      this.setTheme('light');
    }
  }
  toggleTheme() {
    this.currentTheme = this.currentTheme === "light" ? "dark" : "light";
    this.setTheme(this.currentTheme);
  }
  setTheme(theme: string) {
    this.currentTheme = theme;
    document.documentElement.setAttribute('data-theme', theme);
    localStorage.setItem("theme", theme);
  }
  isLightTheme() {
    return document.documentElement.getAttribute("data-theme") === "light";
  }
}
