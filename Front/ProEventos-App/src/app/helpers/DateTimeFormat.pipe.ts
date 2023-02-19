import { DatePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';
import { Constants } from '../util/constants';
import { isValid, parse } from 'date-fns';

@Pipe({
  name: 'DateTimeFormatPipe'
})

export class DateTimeFormatPipe extends DatePipe implements PipeTransform {

  override transform(value: any, args?: any): any {
    const dateValue = parse(value, 'dd/MM/yyyy HH:mm:ss', new Date());
    if (isValid(dateValue)) {
      return super.transform(dateValue, Constants.DATE_TIME_FMT);
    }
  }
}
