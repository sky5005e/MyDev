import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { AddlogsPage } from './addlogs';


@NgModule({
  declarations: [
    AddlogsPage,
  ],
  imports: [
    IonicPageModule.forChild(AddlogsPage),
  ],
})
export class AddlogsModule {}
