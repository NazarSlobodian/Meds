import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'tExterminator'
})
export class TExterminatorPipe implements PipeTransform {

  transform(string: string): unknown {
    return string.replace("T", " ");
  }

}
