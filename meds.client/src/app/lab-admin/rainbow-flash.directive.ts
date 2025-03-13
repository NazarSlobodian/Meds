import { Directive, ElementRef, Input, Renderer2 } from '@angular/core';

@Directive({
  selector: '[appRainbowFlash]'
})
export class RainbowFlashDirective {
  @Input() colors: string[] = ['red', 'orange', 'yellow', 'green', 'blue', 'indigo', 'violet'];
  @Input() interval: number = 500;

  private intervalId: any;
  private currentIndex = 0;
  constructor(private el: ElementRef, private renderer: Renderer2) { }

  ngOnInit() {
    this.intervalId = setInterval(() => {
      this.changeColor();
    }, this.interval);
  }
  ngOnDestroy() {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }
  changeColor() {
    this.renderer.setStyle(
      this.el.nativeElement,
      'color', this.colors[this.currentIndex]
    );
    this.currentIndex = (this.currentIndex + 1) % this.colors.length;
  }
}
