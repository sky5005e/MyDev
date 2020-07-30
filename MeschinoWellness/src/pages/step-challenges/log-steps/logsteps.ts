import { Component, ViewChild } from "@angular/core";
import {
  IonicPage,
  NavController,
  NavParams,
  LoadingController,
  AlertController,
  Navbar,
  MenuController,
  ModalController,
  PickerOptions,
  PickerController
} from "ionic-angular";
import { AddlogsPage } from "../addlogs/addlogs";
import { StepChallengeService } from "../../../providers";
import { StepDashboardPage } from "../step-dashboard";
import { WellnessConstants } from "../../../providers/settings/wellnessconstant";
/**
 * Generated class for the IntroPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: "page-logsteps",
  templateUrl: "logsteps.html"
})
export class LogStepsPage {
  @ViewChild("navbar") navBar: Navbar;
  account: any = { Activities: "", LogStepsNumber: "" };
  loader: any;
  LogsStepActivities: any = [];
  TotalSteps:string = "";
  OtherActivities: any = [];
  OptionItems: any = [];
  CurrentDate: string = new Date().toDateString();
  AddActivities: any = [];
  constructor(
    public navCtrl: NavController,
    public navParams: NavParams,
    public loadingCtrl: LoadingController,
    public pickerCtl: PickerController,
    public alertCtl: AlertController,
    public modalCtrl: ModalController,
    public stepChallengeService: StepChallengeService
  ) {
    this.GetLogsActivities();
    this.LoadOtherActivities();
  }

  AddlogsModal() {
    let selDate = new Date(this.CurrentDate);
    if(selDate > new Date())
    {
      this.presentAlert("You can't add logs for future dates!");
      return;
    }

    if (this.account.Activities != null && this.account.Activities) {
      let profileModal = this.modalCtrl.create(AddlogsPage, {
        Activities: this.AddActivities,
        curDate: this.CurrentDate
      });
      profileModal.onDidDismiss(data => {
        console.log(data);
        if (data.SystemStatus == "Success") {
          console.log(data);
          this.presentAlert(data.SystemMessage);
          this.account.Activities = "";
          this.GetLogsActivities();
        } else {
          this.presentAlert(data.SystemMessage);
        }
      });
      profileModal.present();
    } else {
      this.presentAlert("Please select activities!");
    }
  }

  AddLogs() {
    let selDate = new Date(this.CurrentDate);
    if(selDate > new Date())
    {
      this.presentAlert("You can't add logs for future dates!");
      return;
    }
    if ( this.account.LogStepsNumber === undefined || this.account.LogStepsNumber == "" ||
      (this.account.LogStepsNumber !== undefined &&
        this.account.LogStepsNumber !== "" &&
      !this.numberOnlyValidation(this.account.LogStepsNumber))
    ) {
      this.presentAlert("Please enter any number between 1-999999");
      return;
    }


   
      const userAcc = {
        DeviceId: localStorage.getItem("deviceid"),
        SecretToken: localStorage.getItem("SecretToken"),
        LogStepsNumber: this.account.LogStepsNumber,
        Date: WellnessConstants.GetFormatedDate(this.CurrentDate, 'Logsteps'),
      };
      console.log(userAcc, 'request');
      this.loader = this.loadingCtrl.create({
        content: "Please wait..."
      });
      this.loader.present().then(() => {
        this.stepChallengeService.SaveLogStepsNumber(userAcc).subscribe(
          (resp: any) => {
            this.loader.dismiss();
            if (resp.SystemStatus == "Success") {
              console.log(resp);
              this.account.LogStepsNumber = "";
              this.GetLogsActivities();
            } else {
              this.presentAlert(resp.SystemMessage);
            }
          },
          err => {
            this.loader.dismiss();
            this.presentAlert(
              "Server Message - Save Log Steps Number : " + JSON.stringify(err)
            );
          }
        );
      });
  }

  GetLogsActivities() {
    const userAcc = {
      DeviceId: localStorage.getItem("deviceid"),
      SecretToken: localStorage.getItem("SecretToken"),
      Date: this.CurrentDate
    };
    this.loader = this.loadingCtrl.create({
      content: "Please wait..."
    });
    this.loader.present().then(() => {
      this.stepChallengeService
        .GetLoggedStepsActivitiesByDate(userAcc)
        .subscribe(
          (resp: any) => {
            this.loader.dismiss();
            if (resp.SystemStatus == "Success") {
              this.LogsStepActivities = resp.ListOfSteps;
              this.TotalSteps = resp.TotalSteps;
              console.log(resp);
            } else {
              this.presentAlert(resp.SystemMessage);
            }
          },
          err => {
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
    if(num === undefined || num == 0 || num > 999999 || num < 1)
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

  LoadOtherActivities() {
    const userAcc = {
      DeviceId: localStorage.getItem("deviceid"),
      SecretToken: localStorage.getItem("SecretToken"),
      SearchText: ""
    };

    this.stepChallengeService.SearchStepsActivities(userAcc).subscribe(
      (resp: any) => {
        if (resp.SystemStatus == "Success") {
          let listOfActivities = resp.ListOfActivities;
          this.OtherActivities = resp.ListOfActivities;
          listOfActivities.forEach(item => {
            let oactivities = { text: item.exname, value: item.id };
            this.OptionItems.push(oactivities);
          });
          console.log(this.OtherActivities, "OtherActivities");
        } else {
          this.presentAlert(resp.SystemMessage);
        }
      },
      err => {
        this.presentAlert(
          "Server Message - Get User Overall Progress Of StepChallenge : " +
            JSON.stringify(err)
        );
      }
    );
  }

  DeleteLoggedStepsActivities(id) {
    const userAcc = {
      DeviceId: localStorage.getItem("deviceid"),
      SecretToken: localStorage.getItem("SecretToken"),
      step_tracking_num: id
    };
    this.loader = this.loadingCtrl.create({
      content: "Please wait..."
    });
    this.loader.present().then(() => {
      this.stepChallengeService.DeleteLoggedStepsActivities(userAcc).subscribe(
        (resp: any) => {
          this.loader.dismiss();
          if (resp.SystemStatus == "Success") {
            this.presentAlert(resp.SystemMessage);
            this.GetLogsActivities();
          } else {
            this.presentAlert(resp.SystemMessage);
          }
        },
        err => {
          this.loader.dismiss();
          this.presentAlert(
            "Server Message - Delete Logged Steps Activities : " +
              JSON.stringify(err)
          );
        }
      );
    });
  }

  ionViewDidLoad() {
    console.log("ionViewDidLoad TemplatePage");
  }

  async presentAlert(msg) {
    const alert = await this.alertCtl.create({
      message: msg,
      cssClass: "action-sheets-basic-page",
      buttons: [
        {
          text: "OK",
          handler: () => {}
        }
      ]
    });
    await alert.present();
  }
  ionViewDidEnter() {
       this.navBar.backButtonClick = () => {
       console.log('Back button click - logsteps');
       this.navCtrl.setRoot(StepDashboardPage)
     };
  }
  NextScreen(name) {
    localStorage.setItem("backstepspage", 'LogStepsPage');
    this.navCtrl.push(name);
  }
  IsCancelled: boolean = false;
  async showActivitiesPicker() {
    let opts: PickerOptions = {
      buttons: [
        {
          text: "Cancel",
          role: "cancel",
          handler: () => {
            this.IsCancelled = true;
            this.account.Activities = "";
          }
        },
        {
          text: "Done",
          handler: () => {
            this.IsCancelled = false;
          }
        }
      ],
      columns: [
        {
          name: "Activities",
          options: this.OptionItems
          // options: [
          //   { text: "select", value: "" },
          //   { text: "Activities 1", value: "Activities 1" },
          //   { text: "Activities 2", value: "Activities 2" }
          // ]
        }
      ]
    };
    let picker = await this.pickerCtl.create(opts);
    picker.present();

    picker.onDidDismiss(async data => {
      console.log(data, "test - p");
      let col = await picker.getColumn("Activities");
      if (col.options[col.selectedIndex].value != "" && !this.IsCancelled) {
        this.account.Activities = col.options[col.selectedIndex].text;
        this.AddActivities = this.OtherActivities.filter(
          q => q.id == col.options[col.selectedIndex].value
        );
        console.log(this.AddActivities, "Add -act");
      }
      //console.log( col.options[col.selectedIndex].value);
    });
  }

  SetDate(title: string, action: string) {
    let curDate = new Date(this.CurrentDate);
    switch (action) {
      case "Previous":
        curDate.setDate(curDate.getDate() - 1);
        this.CurrentDate = curDate.toDateString();
        break;
      case "Next":
        curDate.setDate(curDate.getDate() + 1);
        this.CurrentDate = curDate.toDateString();
        break;
      default:
        this.CurrentDate = new Date().toDateString();
        break;
    }
    this.GetLogsActivities();
  }
}
