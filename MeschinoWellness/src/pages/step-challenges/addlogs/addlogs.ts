import { Component } from "@angular/core";
import {
  IonicPage,
  ViewController,
  NavParams,
  LoadingController,
  AlertController,
} from "ionic-angular";
import { StepChallengeService } from "../../../providers";
import { WellnessConstants } from "../../../providers/settings/wellnessconstant";
/**
 * Generated class for the IntroPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: "page-addlogs",
  templateUrl: "addlogs.html",
})
export class AddlogsPage {
  loader: any;
  data: any;
  listItems: any = [];
  model: any = { exduration: "", stepscount: "", exname: "", exerciseType: "" };
  curDate: string;
  constructor(
    public viewCtrl: ViewController,
    public navParams: NavParams,
    public loadingCtrl: LoadingController,
    public alertCtl: AlertController,
    public stepChallengeService: StepChallengeService
  ) {
    // obtain data
    console.log(this.navParams, "nav params");
    this.data = this.navParams.data.Activities;
    this.curDate = this.navParams.data.curDate;
    this.listItems = this.data[0].lstIntensity;
    (this.model.exname = this.data[0].exname),
      (this.model.exerciseType = this.data[0].exerciseType),
      console.log(this.listItems);
  }

  ionViewDidLoad() {
    console.log("ionViewDidLoad TemplatePage");
  }

  closedPopup() {
    let resp = { SystemMessage: "Dismissed" };
    this.viewCtrl.dismiss(resp);
  }

  onSave() {
    if (this.model.intensity === undefined || (this.model.intensity !== undefined && this.model.intensity === "")) {
      this.presentAlert("Please select any intensity");
      return;
    } 

    if ( this.model.exduration === undefined || this.model.exduration == "" ||
      (this.model.exduration !== undefined &&
      this.model.exduration !== "" &&
      !this.numberOnlyValidation(this.model.exduration))
    ) {
      this.presentAlert("Please enter any number between 1-999");
      return;
    }
   
      let data = this.model;
      const userinfo = {
        DeviceId: localStorage.getItem("deviceid"),
        SecretToken: localStorage.getItem("SecretToken"),
        exerciseType: this.model.exerciseType,
        exname: this.model.exname.trim(),
        exduration: this.model.exduration,
        exDate: WellnessConstants.GetFormatedDate(this.curDate, 'AddLogs'),
        intensity: this.model.intensity,
      };
      console.log(userinfo);
      this.loader = this.loadingCtrl.create({
        content: "Please wait...",
      });
      this.loader.present().then(() => {
        this.stepChallengeService.SaveStepsActivitiesData(userinfo).subscribe(
          (resp: any) => {
            this.loader.dismiss();
            if (resp.SystemStatus == "Success") {
              this.viewCtrl.dismiss(resp);
            } else {
              this.presentAlert(resp.SystemMessage);
            }
          },
          (err) => {
            this.loader.dismiss();
            this.presentAlert(
              "Server Message - Get User Overall Progress Of StepChallenge : " +
                JSON.stringify(err)
            );
          }
        );
      });
   
  }

  numberOnlyValidation(value) {
    let num = parseInt(value);
    if(num === undefined || num == 0 || num > 999 || num < 1)
    {
      return false;
    }
    else if (isNaN(value) || value.includes(".")) {
      return false;
    }
    else {
      return true;
    }
  }

  async presentAlert(msg) {
    const alert = await this.alertCtl.create({
      message: msg,
      cssClass: "action-sheets-basic-page",
      buttons: [
        {
          text: "OK",
          handler: () => {},
        },
      ],
    });
    await alert.present();
  }
}
