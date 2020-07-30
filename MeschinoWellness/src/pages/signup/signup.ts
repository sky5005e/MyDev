import { Component } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import {
  IonicPage,
  NavController,
  PickerController,
  PickerOptions,
  Events,
  AlertController,
  LoadingController
} from "ionic-angular";
import { FormControl, FormGroup, FormBuilder } from "@angular/forms";

import { UserService } from "../../providers";
//import { LoginPage } from "../login/login";
//import { DashboardPage } from "../dashboard/dashboard";
import { IntroPage } from "../intro/intro";
import { MyHraPage } from "../my-hra/my-hra";
import { TemplatePage } from "../templates/template";
import { IntroVideoPage } from "../intro-video/intro-video";
import { WellnessConstants } from "../../providers/settings/wellnessconstant";
import { AlertMessagesService } from "../../providers/settings/alertmessage.service";

@IonicPage()
@Component({
  selector: "page-signup",
  templateUrl: "signup.html"
})
export class SignupPage {
  // The account fields for the login form.
  // If you're using the username field with or without email, make
  // sure to add it to the type
  account: {
    FirstName: string;
    LastName: string;
    UserName: string;
    PhoneNumber: string;
    Gender: string;
    BirthDate: string;
    Height: string;
    Password: string;
    apiKey: string;
    confirmpassword: string;
    deviceid: string;
    IsAgreeTermsCondiotion: boolean;
    ReferralCode: string;
  } = {
    FirstName: "",
    LastName: "",
    UserName: "",
    PhoneNumber:"",
    Gender: "",
    BirthDate: "",
    Height: "",
    Password: "",
    confirmpassword: "",
    apiKey: "",
    deviceid: "",
    IsAgreeTermsCondiotion: false,
    ReferralCode:""
  };

  loader: any;

  signupForm = new FormGroup({
    Firstname: new FormControl(),
    LastName: new FormControl(),
    PhoneNumber: new FormControl(),
    UserName: new FormControl(),
    Gender: new FormControl(),
    BirthDate: new FormControl(),
    Height: new FormControl(),
    Password: new FormControl(),
    confirmpassword: new FormControl(),
    IsAgreeTermsCondiotion: new FormControl(),
    ReferralCode:new FormControl()
  });
  validateScreen: any;

  createForm() {
    this.signupForm = this.formBuilder.group({
      FirstName: "",
      LastName: "",
      UserName: "",
      PhoneNumber:"",
      Gender: "",
      BirthDate: "",
      Height: "",
      Password: "",
      confirmpassword: "",
      IsAgreeTermsCondiotion: false,
      ReferralCode:""
    });
  }

  slides = [
    { active: true },
    { active: false },
    { active: false },
    { active: false },
    { active: false }
  ];
  currentSlide: number = 0;
  buttonText = "Next";

  dateData: any;
  //dateYear:any;
  dateValue: string;
  heightDate: any;
  heightValue: string;
  parentColumns: any;
  constructor(
    public navCtrl: NavController,
    public user: UserService,
    public formBuilder: FormBuilder,
    public translateService: TranslateService,
    public pickerCtl: PickerController,
    public alertCtl: AlertController,
    public loading: LoadingController,
    private events: Events,
    private alertMessageSrv: AlertMessagesService
  ) {
    // Using parentCol
    this.parentColumns = WellnessConstants.parentColumns;
    this.createForm();
  }
  /*
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
 */
  async showGenderPicker() {
    let opts: PickerOptions = {
      buttons: [
        {
          text: "Cancel",
          role: "cancel"
        },
        {
          text: "Done"
        }
      ],
      columns: [
        {
          name: "Gender",
          options: [
            { text: "Male", value: "Male" },
            { text: "Female", value: "Female" }
          ]
        }
      ]
    };
    let picker = await this.pickerCtl.create(opts);
    picker.present();
    picker.onDidDismiss(async data => {
      let col = await picker.getColumn("Gender");
      this.account.Gender = col.options[col.selectedIndex].value;
    });
  }

  IsFormValidated() {
    var msg: string;
    if (this.currentSlide == 0) {
      if (this.account.FirstName == "") {
        msg = "Please enter First Name.";
      } else if (this.account.LastName == "") {
        msg = "Please enter Last Name.";
      } 
      else if (this.account.PhoneNumber == "") {
        msg = "Please enter PhoneNumber.";
      }
      else if (this.account.UserName == "") {
        msg = "Please enter Username.";
      }
      else if(this.account.PhoneNumber !== "") {
        var info = this.isValidMobile(this.account.PhoneNumber)
        if(!info.isValid)
        { 
          msg = info.msg
        }
        else 
        { 
          msg = "";
        }
     }
      
    } else if (this.currentSlide == 1) {
      if (this.account.Gender == "") {
        msg = "Please select your Gender.";
      }
    } else if (this.currentSlide == 2) {
      if (this.account.BirthDate == "") {
        msg = "Please select your date of birth.";
      }
    } else if (this.currentSlide == 3) {
      if (this.account.Height == "") {
        msg = "Please select your Height.";
      }
    } else if (this.currentSlide == 4) {
      if (this.account.Password == "") {
        msg = "Please enter a Password.";
      } else if (this.account.confirmpassword == "") {
        msg = "Confirm Password & Password must be same.";
      } else if (this.account.Password != this.account.confirmpassword) {
        msg = "Confirm Password & Password must be same.";
      } 
      else if (this.account.ReferralCode.length > 0 && this.account.ReferralCode.length!== 6) {
        msg = "Referral code must have 9 characters.";
      }
      else if (
        this.account.IsAgreeTermsCondiotion === undefined ||
        this.account.IsAgreeTermsCondiotion == false
      ) {
        msg = "Please accept privacy policy and terms.";
      }
    }
    if (msg != "" && msg != undefined) {
      this.alertMessageSrv.presentAlert(msg);
      return true;
    } else {
      return false;
    }
  }

  private async SaveUserTokenInDB() {
    const accountinfo = {
      deviceid: localStorage.getItem("deviceid"),
      SecretToken: localStorage.getItem("SecretToken"),
      DeviceType: localStorage.getItem("DeviceType"),
      UserTokenId: localStorage.getItem("FCMToken")
    };
    await this.user.SaveUserTokenIdData(accountinfo).subscribe(
      resp => {
        console.log("save token success");
      },
      err => {
        //this.presentAlert(err.error.SystemMessage);
        console.log(err);
      }
    );
  }

  numberOnlyValidation(value) {
    if (isNaN(value) || value.includes(".")) {
      console.log(false);
      // invalid character, prevent input
      return false;
    } else {
      console.log(true);
      return true;
    }
  }
  isValidMobile(value) {
    
    let msgInfo =  { msg : "", isValid : true }
    
    let firstLetter = value.charAt(0);
    let remainingLetter =  "";
    if(firstLetter == "+")
    {
       remainingLetter =  value.replace(firstLetter, "");
    }
    else{
       remainingLetter =  value;
    }
    // remove space
    remainingLetter = remainingLetter.replace(/\s/g, "");
    console.log(firstLetter);
    console.log(remainingLetter);    
    let regExp = /^[0-9]{10,20}$/;

    if (msgInfo.isValid && !regExp.test(remainingLetter)) {
      msgInfo.isValid = false;
      msgInfo.msg = "Phone number start with + country code and should be 10-20 length";
      //console.log(msgInfo.isValid, 'if test isvalid');
    }
    else
    {
      msgInfo.isValid = true;
      //console.log(msgInfo.isValid, 'else isvalid');
    }
    return msgInfo;
}

  doSignup() {
    this.loader = this.alertMessageSrv.PleaseWait();
    this.account.Password != this.account.confirmpassword;
    this.account.apiKey = "71e73c14-2723-4d6e-a489-c9675738fdf4";
    this.account.deviceid = localStorage.getItem("remUUID");
    localStorage.setItem("deviceid", this.account.deviceid);
    //alert('deviceid : '+this.account.deviceid);
    if (localStorage.getItem("remUUID") == null)
      this.account.deviceid = "c9675738fdf4";

    this.loader.present().then(() => {
      this.SetHeight_inInches();

      this.user.signup(this.account).subscribe(
        resp => {
          // Set for user name and password
          localStorage.setItem("UserName", this.account.UserName);
          localStorage.setItem("Password", this.account.Password);
          localStorage.setItem("FirstName", this.account.FirstName);
          localStorage.setItem("LastName", this.account.LastName);
          localStorage.setItem("PhoneNumber", this.account.PhoneNumber);
          this.SetUserInfo(resp);
          if (
            localStorage.getItem("deviceid") !== "" &&
            localStorage.getItem("deviceid") !== undefined &&
            localStorage.getItem("FCMToken") !== "" &&
            localStorage.getItem("FCMToken") !== undefined &&
            localStorage.getItem("DeviceType") !== "" &&
            localStorage.getItem("DeviceType") !== undefined
          ) {
            this.SaveUserTokenInDB();
          }

          localStorage.setItem("remuser", this.account.UserName);
          localStorage.setItem("rempwd", this.account.Password);
          setTimeout(() => {
            this.loader.dismiss();
            this.navCtrl.push(IntroVideoPage);
          }, 3000);

          //this.navCtrl.setRoot(MyHraPage);
          //this.navCtrl.push(IntroPage);
        },
        err => {
          console.log(err);
          this.loader.dismiss();
          this.alertMessageSrv.ErrorMsg(err, "App User Register");
        }
      );
    });
  }

  SetUserInfo(data) {
    this.events.publish("user:created", data);
  }
  SetHeight_inInches() {
    let inputVal = this.account.Height;
    var arrVal = inputVal.split("-");
    if (arrVal[0].indexOf("cm") !== -1) {
      let inch = Math.round((parseInt(arrVal[1]) * 1) / 2.54);
      console.log(inch, "CM - INCH");
      this.account.Height = inch + "";
    } else if (arrVal[0].indexOf("feet") !== -1) {
      let inch = parseInt(arrVal[1]) * 12 + parseInt(arrVal[2]);
      console.log(inch, "FEET - INCH");
      this.account.Height = inch + "";
    }
  }

  changeTabButton(index) {
    if (!this.IsFormValidated()) {
      this.currentSlide = index;
      this.slides.forEach(function(val, index) {
        val.active = false;
      });
      this.slides[this.currentSlide].active = true;
      if (this.slides.length - 1 <= this.currentSlide) {
        this.buttonText = "Submit";
      }
    }
  }
  changeTab(index) {
    if (!this.IsFormValidated()) {
      if (index == this.slides.length - 1) {
        this.doSignup();
        return false;
      }

      this.slides.forEach(function(val, index) {
        val.active = false;
      });
      this.currentSlide = index + 1;
      this.slides[this.currentSlide].active = true;
      if (this.slides.length - 1 <= this.currentSlide) {
        this.buttonText = "Submit";
      }
    }
  }

  ViewTemplate(Name: any) {
    this.navCtrl.push(TemplatePage, {
      Content: Name
    });
  }
}
