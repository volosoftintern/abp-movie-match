import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'timeDiffer'
})
export class TimeDifferPipe implements PipeTransform {

  transform(value: number): string {
    return this.getTimeDiffer(value);
  }

  getTimeDiffer(creationTime: number): string {

    const date = new Date(creationTime);
    const currentTime = new Date();
    const diff = currentTime.getTime() - date.getTime();
    const minute = 60 * 1000
    const hour = 60 * minute;
    const day = 24 * hour;
    const month = 30 * day;

    if (diff < hour) {
      return `${Math.floor(diff / minute)}m`;
    } else if (diff < day) {
      return `${Math.floor(diff / hour)}h`;
    } else if (diff < month) {
      return `${Math.floor(diff / day)}d`;
    }
    return date.toLocaleDateString();

  }
}
