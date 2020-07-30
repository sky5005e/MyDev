import { Component, ViewChild, NgZone } from "@angular/core";
import {
  IonicPage,
  NavController,
  NavParams,
  LoadingController,
  AlertController,
  Navbar,
  MenuController,
} from "ionic-angular";

import * as HighCharts from "highcharts";
import { StepChallengeService } from "../../../providers";
import { StepDashboardPage } from "../step-dashboard";
/**
 * Generated class for the IntroPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: "page-logoverview",
  templateUrl: "logoverview.html",
})
export class LogOverviewPage {
  model: any = {};
  loader: any;
  DialyClass: string = "current";
  MonthlyClass: string = "";
  WeeklyClass: string = "";
  XAxisCategories: any = [];
  SeriesData: any = [];
  xpositionnum: number = 1;
  xMinWidth: number = 700;
  subtitle: string = "";
  CurrentStartDate: string = "";
  CurrentEndDate: string = "";
  zone: NgZone;
  @ViewChild("navbar") navBar: Navbar;
  constructor(
    public navCtrl: NavController,
    public navParams: NavParams,
    public loadingCtrl: LoadingController,
    public alertCtl: AlertController,
    public menu: MenuController,
    public stepChallengeService: StepChallengeService
  ) {
    this.zone = new NgZone({ enableLongStackTrace: false });
  }

  LoadChartData() {
    console.log("LoadChartData");
    //let appChart = 
    HighCharts.chart("container", {
      chart: {
        type: "column",
        scrollablePlotArea: {
          minWidth: this.xMinWidth,
          scrollPositionX: this.xpositionnum,
        },
      },
      credits: {
        enabled: false,
      },
      title: {
        text: "Steps Info",
      },
      subtitle: {
        text: this.subtitle,
      },
      xAxis: {
        categories: this.XAxisCategories,
       // categories: [          "01",          "02",          "03",          "04",          "05",          "06",          "07",          "08",          "09",          "10",          "11",          "12" , "13", "14", "15"       ],
      },
      yAxis: {
        min: 0,
        title: {
          text: "(Steps)",
        },
      },
      tooltip: {
        valueSuffix: " step counts",
      },
      plotOptions: {
        column: {
          pointPadding: 0.2,
          borderWidth: 0,
        },
      },
      series: [
        {
          type: undefined,
          name: "",
          data: this.SeriesData,
          /* data: [
            49.9,
            71.5,
            106.4,
            129.2,
            144.0,
            176.0,
            135.6,
            148.5,
            216.4,
            194.1,
            95.6,
            54.4,
            194.1,
            93.6,
            64.4
          ] */
        },
        
      ],
    });

    //appChart.redraw();
  }
  
  ionViewDidLoad() {
    console.log("ionViewDidLoad TemplatePage - logooveriew");
    //this.zone.run(() => {
      this.loadInitialData("Daily", null, true);
    //});
  }

  ngAfterViewInit() {
   
    
  }
  loadInitialData(
    ProgressType: string,
    DateType: string,
    isFirstTime: boolean
  ) {
    const userAcc = {
      DeviceId: localStorage.getItem("deviceid"),
      SecretToken: localStorage.getItem("SecretToken"),
      ProgressType: ProgressType,
      CurrentStartDate: null, //"Jan 04 2020",
      CurrentEndDate: null, //"Jan 19 2020",
      DateType: null, //next
    };
    if (!isFirstTime) {
      userAcc.CurrentStartDate = this.CurrentStartDate;
      userAcc.CurrentEndDate = this.CurrentEndDate;
      userAcc.DateType = DateType;
    }
    
    this.loader = this.loadingCtrl.create({
      content: "Please wait...",
    });
    this.loader.present().then(() => {
      this.stepChallengeService
        .GetOverviewAndGraphicViewData(userAcc)
        .subscribe(
          (resp: any) => {
            this.loader.dismiss();
            this.XAxisCategories = [];
            this.SeriesData = [];
            if (resp.SystemStatus == "Success") {
              this.model = resp;
              console.log(this.model);
              this.model.resultGraphicView.forEach((item) => {
                this.XAxisCategories.push(item.date);
                this.SeriesData.push(item.steps);
                this.subtitle = this.model.StartDate + " - " + this.model.EndDate;
                this.CurrentStartDate = this.model.StartDate;
                this.CurrentEndDate = this.model.EndDate;
              });
              this.LoadChartData();
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
  LoadGraph(title: string, action: string) {
    this.DialyClass = "";
    this.WeeklyClass = "";
    this.MonthlyClass = "";
    switch (title) {
      case "Daily":
        this.DialyClass = "current";
        this.xpositionnum = 1;
        this.xMinWidth = 700;
        break;
      case "Weekly":
        this.WeeklyClass = "current";
        this.xpositionnum = 0;
        this.xMinWidth = 0;
        break;
      case "Monthly":
        this.MonthlyClass = "current";
        this.xpositionnum = 1;
        this.xMinWidth = 700;
        break;
    }
    //this.LoadChartData();
    console.log(action);
    if (action.length == 0) {
      this.loadInitialData(title, null, true);
    } else {
      this.loadInitialData(title, action, false);
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
  ionViewDidEnter() {

    this.navBar.backButtonClick = () => {
      console.log("Back button click StepDashboardPage");
      this.navCtrl.setRoot(StepDashboardPage);
    };

  }
  NextScreen(name) {
    localStorage.setItem("backstepspage", "LogOverviewPage");
    this.navCtrl.push(name);
  }
  
}
