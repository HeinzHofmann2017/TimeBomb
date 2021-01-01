import { Component } from '@angular/core';

@Component({
  selector: 'app-manual-component',
  templateUrl: './manual.component.html'
})
export class CounterComponent {
  public currentCount = 0;


  public incrementCounter() {
    this.currentCount++;
  }
}
