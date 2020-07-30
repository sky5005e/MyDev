import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { StepDashboardPage } from './step-dashboard';
// Import ng-circle-progress
import { NgCircleProgressModule } from 'ng-circle-progress';

@NgModule({
  declarations: [
    StepDashboardPage,
  ],
  imports: [
    NgCircleProgressModule.forRoot({
     // "radius": 60,
      "space": -15,
      "outerStrokeGradient": true,
       "outerStrokeWidth": 15,
      "outerStrokeColor": "#4882c2",
      "outerStrokeGradientStopColor": "#53a9ff",
      "innerStrokeColor": "#e7e8ea",
      "innerStrokeWidth": 15,
      "animateTitle": false,
      "animationDuration": 1000,
      "showUnits": false,
      "showBackground": false,
      "clockwise": false,
      "startFromZero": false,
      "backgroundPadding": -40,
      "subtitleColor":"#35b2ee",
      "backgroundStrokeWidth":0
    }),
    IonicPageModule.forChild(StepDashboardPage),
  ],
})
export class StepDashboardPageModule {}
