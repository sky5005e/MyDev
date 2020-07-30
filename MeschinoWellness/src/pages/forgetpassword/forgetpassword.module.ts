import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { IonicPageModule } from 'ionic-angular';
import { ForgetPasswordPage } from './forgetpassword';



@NgModule({
  declarations: [
    ForgetPasswordPage,
  ],
  imports: [
    IonicPageModule.forChild(ForgetPasswordPage),
    TranslateModule.forChild()
  ],
  exports: [
    ForgetPasswordPage
  ]
})
export class ForgetPageModule {

 
 }
