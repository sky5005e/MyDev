import { Component, ViewChild } from "@angular/core";
import {
  IonicPage,
  NavController,
  NavParams,
  LoadingController,
  AlertController,
  Navbar,
  MenuController,
} from "ionic-angular";
import { StepChallengeService } from "../../../providers";
import { arrayMax } from "highcharts";

/**
 * Generated class for the IntroPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: "page-stepchallengehistory",
  templateUrl: "stepchallengehistory.html",
})
export class StepChallengeHistoryPage {
  model: any = {};
  loader: any;
  dataActivities: any = [];
  @ViewChild("navbar") navBar: Navbar;
  constructor(
    public navCtrl: NavController,
    public navParams: NavParams,
    public loadingCtrl: LoadingController,
    public alertCtl: AlertController,
    public menu: MenuController,
    public stepChallengeService: StepChallengeService
  ) {
    this.loadInitialData();
  }
  loadInitialData() {
    const userAcc = {
      DeviceId: localStorage.getItem("deviceid"),
      SecretToken: localStorage.getItem("SecretToken"),
    };
    this.loader = this.loadingCtrl.create({
      content: "Please wait...",
    });
    this.loader.present().then(() => {
      this.stepChallengeService.GetUserStepChallengeHistory(userAcc).subscribe(
        (resp: any) => {
          this.loader.dismiss();
          if (resp.SystemStatus == "Success") {
            this.dataActivities = resp.result;
            console.log(resp);
            let max_user_step_challenge_num = resp.result.map(q=>q.user_step_challenge_num);
            console.log('max id user_step_challenge_num', max_user_step_challenge_num)
            let max_id = Math.max.apply(Math, max_user_step_challenge_num);
            console.log('max id user_step_challenge_num', max_id)
            this.dataActivities.forEach((item) => {
              if (item.status == "Failed") {
                item["classname"] = "bg-danger";
                item["buttonTitle"] = "Delete";
              } else if (
                item.status == "Achieved" &&
                item.user_step_challenge_num == max_id
              ) {
                item["classname"] = "bg-success";
                item["buttonTitle"] = "Start New Challenge";
              } else if (
                item.status == "Achieved" &&
                (new Date(item.end_date) <= new Date() || new Date(item.end_date) >= new Date())
              ) {
                item["classname"] = "bg-success";
                item["buttonTitle"] = "Delete";
              } else if (item.status == "In Progress") {
                item["classname"] = "bg-warning";
                item["buttonTitle"] = "Reset";
              } else {
                item["classname"] = "bg-danger";
                item["buttonTitle"] = "Delete";
              }

            });
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

  ionViewDidLoad() {
    console.log("ionViewDidLoad TemplatePage");
  }
  StepChallengeHistoryAction(id, buttonTitle) {
    console.log(id, buttonTitle);
    switch (buttonTitle) {
      case "Delete":
        this.DeletePreviousChallengeHistory(id);
        break;
      case "Start New Challenge":
        this.StartUserNewStepChallenge();
        break;
      case "Reset":
        this.ResetUserStepChallenge();
        break;
    }
  }
  DeletePreviousChallengeHistory(id) {
    const userAcc = {
      DeviceId: localStorage.getItem("deviceid"),
      SecretToken: localStorage.getItem("SecretToken"),
      user_step_challenge_num: id,
    };
    this.loader = this.loadingCtrl.create({
      content: "Please wait...",
    });
    this.loader.present().then(() => {
      this.stepChallengeService
        .DeletePreviousChallengeHistory(userAcc)
        .subscribe(
          (resp: any) => {
            this.loader.dismiss();
            if (resp.SystemStatus == "Success") {
              this.presentAlert(resp.SystemMessage);
              this.loadInitialData();
            } else {
              this.presentAlert(resp.SystemMessage);
            }
          },
          (err) => {
            this.loader.dismiss();
            this.presentAlert(
              "Server Message - Delete Previous Challenge History : " +
                JSON.stringify(err)
            );
          }
        );
    });
  }

  ResetUserStepChallenge() {
    const userAcc = {
      DeviceId: localStorage.getItem("deviceid"),
      SecretToken: localStorage.getItem("SecretToken"),
    };
    this.loader = this.loadingCtrl.create({
      content: "Please wait...",
    });
    this.loader.present().then(() => {
      this.stepChallengeService.ResetUserStepChallenge(userAcc).subscribe(
        (resp: any) => {
          this.loader.dismiss();
          if (resp.SystemStatus == "Success") {
            this.presentAlert(resp.SystemMessage);
            this.loadInitialData();
          } else {
            this.presentAlert(resp.SystemMessage);
          }
        },
        (err) => {
          this.loader.dismiss();
          this.presentAlert(
            "Server Message - Reset User Step Challenge : " +
              JSON.stringify(err)
          );
        }
      );
    });
  }

  StartUserNewStepChallenge() {
    const userAcc = {
      DeviceId: localStorage.getItem("deviceid"),
      SecretToken: localStorage.getItem("SecretToken"),
    };
    this.loader = this.loadingCtrl.create({
      content: "Please wait...",
    });
    this.loader.present().then(() => {
      this.stepChallengeService.StartUserNewStepChallenge(userAcc).subscribe(
        (resp: any) => {
          this.loader.dismiss();
          if (resp.SystemStatus == "Success") {
            this.presentConfirm("Congratulations! You have achieved your step challenge before the end date. You have decided to start a new challenge which will delete steps and activities logged for today. Do you want to continue?");//resp.SystemMessage);
          } else {
            this.presentAlert(resp.SystemMessage);
          }
        },
        (err) => {
          this.loader.dismiss();
          this.presentAlert(
            "Server Message - Start User New Step Challenge : " +
              JSON.stringify(err)
          );
        }
      );
    });
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
  async presentConfirm(msg) {

    let alertP = this.alertCtl.create({
      message: msg,      
      cssClass: "action-sheets-basic-page",
      buttons: [
        {
          text: "Ok",
          handler: () => {
           
            this.loadInitialData();
          }
        },
        {
          text: "Cancel",
          handler: () => {
            //alertP.dismiss();
          }
        }
      ]
    });
    alertP.present();
  }
  
  ionViewDidEnter() {
    this.navBar.backButtonClick = () => {
      let name = localStorage.getItem("backstepspage");
      if (name !== "LogOverviewPage") {
        console.log("Back button click stepchallenge");
        localStorage.removeItem("backstepspage");
        this.navCtrl.push(name);
      } else {
        this.navCtrl.setPages([{ page: name }]);
      }
    };
  }
  NextScreen(name) {
    this.navCtrl.push(name);
  }
}
