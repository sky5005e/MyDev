import { Component, ViewChild } from "@angular/core";
import {
  IonicPage,
  NavController,
  NavParams,
  LoadingController,
  AlertController,
  Navbar,
  MenuController,
  Events
} from "ionic-angular";
import { UserService, StepChallengeService } from "../../providers";
import { NotificationPage } from "../notification/notification";
import { LogOverviewPage } from "./log-overview/logoverview";
import { LogStepsPage } from "./log-steps/logsteps";
/**
 * Generated class for the IntroPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: "page-step-dashboard",
  templateUrl: "step-dashboard.html"
})
export class StepDashboardPage {
  subtitle: any;
  percent: any;
  @ViewChild("navbar") navBar: Navbar;
  loader: any;
  model: any = {};
  progressmodel: any = {};
  notificationCount: string = ""; //localStorage.getItem("NotificationCount");
  constructor(
    public navCtrl: NavController,
    public navParams: NavParams,
    public loadingCtrl: LoadingController,
    public alertCtl: AlertController,
    public userService: UserService,
    public stepChallengeService: StepChallengeService,
    public events: Events,
    public menu: MenuController
  ) {
    this.menu.enable(true);
    
    this.loadInitialData();
  }
  loadInitialData() {
    const userAcc = {
      DeviceId: localStorage.getItem("deviceid"),
      SecretToken: localStorage.getItem("SecretToken")
    };
    this.userService.GetPushNotificationCount(userAcc).subscribe((res: any) => {
      let msgcount = res.Count;
      this.notificationCount = msgcount > 0 ? msgcount : "";
      localStorage.setItem("notificationCount", this.notificationCount);
    });
    this.loader = this.loadingCtrl.create({
      content: "Please wait..."
    });
    this.loader.present().then(() => {
      this.stepChallengeService
        .GetUserOverallProgressOfStepChallenge(userAcc)
        .subscribe(
          (resp: any) => {
            this.loader.dismiss();
            if(resp.SystemStatus=="Success")
            {
            this.model = resp;
            this.percent = this.model.PercentOfStep;
            this.subtitle = this.model.PercentOfStep +"%";
            console.log(this.model);
            }
            else
            {
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

        this.stepChallengeService
        .GetUserStepChallengeProgressOverviewCount(userAcc)
        .subscribe(
          (resp: any) => {
            
            if(resp.SystemStatus=="Success")
            {
            this.progressmodel = resp;
            console.log(this.progressmodel, 'progressmodel');
            }
            else
            {
              this.presentAlert(resp.SystemMessage);
            }
            },
          err => {
            this.presentAlert(
              "Server Message - Get User Step Challenge Progress Overview Count : " +
                JSON.stringify(err)
            );
          }
        );
    });
  }
  EmitterNotificationCount() {
    this.events.subscribe("PushNotification", PushNotification => {
      let msgcount = PushNotification.Count;
      this.notificationCount = msgcount > 0 ? msgcount : "";
      localStorage.setItem("notificationCount", this.notificationCount);
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
    //  this.navBar.backButtonClick = () => {
    //    console.log('Back button click');
    //this.navCtrl.push(NotificationPage)
    //  };
  }
  GoOverView(name) {
    this.navCtrl.setPages([{page :name}])
    
  }
  goLogsteps() {
    this.navCtrl.push(LogStepsPage);
  }
  goNotification() {
    this.navCtrl.push(NotificationPage);
  }
}
