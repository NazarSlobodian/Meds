import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'monthTransform'
})
export class MonthTransformPipe implements PipeTransform {

  transform(value: number | null): string {
    if (value == null)
      return "All";
    if (value == 1)
      return "January";
    if (value == 2)
      return "February";
    if (value == 3)
      return "March";
    if (value == 4)
      return "April";
    if (value == 5)
      return "May";
    if (value == 6)
      return "June";
    if (value == 7)
      return "July";
    if (value == 8)
      return "August";
    if (value == 9)
      return "September";
    if (value == 10)
      return "October";
    if (value == 11)
      return "November";
    if (value == 12)
      return "December";
    return "Unknown month";
  }

}
