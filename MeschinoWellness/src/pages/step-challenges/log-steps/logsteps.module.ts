import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { LogStepsPage } from './logsteps';


@NgModule({
  declarations: [
    LogStepsPage,
  ],
  imports: [
    IonicPageModule.forChild(LogStepsPage),
  ],
})
export class LogStepsPageModule {}
