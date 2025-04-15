import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'superscript' })
export class SuperscriptPipe implements PipeTransform {
  transform(value: string): string {
    const superscriptMap: { [key: string]: string } = {
      '0': '⁰', '1': '¹', '2': '²', '3': '³', '4': '⁴',
      '5': '⁵', '6': '⁶', '7': '⁷', '8': '⁸', '9': '⁹',
      '+': '⁺', '-': '⁻', '=': '⁼', '(': '⁽', ')': '⁾'
    };

    return value.replace(/\^([0-9+\-=()])/g, (_, char) => superscriptMap[char] || char);
  }
}

