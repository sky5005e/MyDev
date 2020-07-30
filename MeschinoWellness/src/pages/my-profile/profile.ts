import { Component } from "@angular/core";
import { WheelSelector } from "@ionic-native/wheel-selector";
import {
  IonicPage,
  NavController,
  NavParams,
  LoadingController,
  ToastController,
  AlertController,
  PickerController,
  PickerOptions,
  Events
} from "ionic-angular";

import {
  FileTransfer,
  FileUploadOptions,
  FileTransferObject
} from "@ionic-native/file-transfer";
import { File } from "@ionic-native/file";
import { Camera, CameraOptions } from "@ionic-native/camera";
import { HttpClient } from "@angular/common/http";

import * as $ from "jquery";
import { FormGroup, FormControl, FormBuilder } from "@angular/forms";
import { UserService } from "../../providers";
import { WellnessConstants } from "../../providers/settings/wellnessconstant";

/**
 * Generated class for the DashboardPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: "page-profile",
  templateUrl: "profile.html"
})
export class ProfilePage {
  imageDATA: any;
  imageFileName: any;
  UIHeight: string;
  IsShowDone : boolean= false;
  IsCompanySMSEnable : boolean = false;
  ischecked = false;
  account: {
    FirstName: string;
    LastName: string;
    PhoneNumber:string;
    UserName: string;
    Height: string;
    BirthDate: string;
    
    Gender: string;
    IsUserSMSEnable : boolean;
  } = {
    FirstName: localStorage.getItem("FirstName"),
    LastName: localStorage.getItem("LastName"),
    UserName: localStorage.getItem("UserName"),
    PhoneNumber:localStorage.getItem("PhoneNumber"),
    Height: "cm-50-null",
    BirthDate: "2006-10-10",    
    Gender: localStorage.getItem("Gender"),
    IsUserSMSEnable : localStorage.getItem("IsUserSMSEnable") == "true" ? true : false    
  };

  dateData: any;
  //dateYear:any;
  dateValue: string;
  heightDate: any;
  heightValue: string;
  loader: any;
  rewardpoints: number = 0;
  bio_age: string = "0";
  mhrs_score: string = "0";
  base64: any;
  parentColumns: any;

  appForm = new FormGroup({
    Gender: new FormControl(),
    BirthDate: new FormControl(),
    Height: new FormControl()
  });

  createForm() {
    this.appForm = this.formBuilder.group({
      Gender: "",
      BirthDate: "",
      Height: ""
    });
  }

  constructor(
    public navCtrl: NavController,
    public formBuilder: FormBuilder,
    public navParams: NavParams,
    private camera: Camera,
    public loadingCtrl: LoadingController,
    public alertCtl: AlertController,
    public http: HttpClient,
    public pickerCtl: PickerController,
    private events: Events,
    private userService: UserService
  ) {
    this.imageFileName =
      localStorage.getItem("ProfileImage") !== null
        ? localStorage.getItem("ProfileImage")
        : "assets/img/loader.gif";

    this.UIHeight =
      "cm-" +
      Math.round(parseInt(localStorage.getItem("Height")) * 2.54) +
      "-null";
    this.account.BirthDate = localStorage.getItem("BirthDate");
    this.loader = this.loadingCtrl.create({
      content: "Please wait..."
    });
    this.loader.present().then(() => {
      this.LoadUserInfo();
    });
    
    this.IsCompanySMSEnable =
      localStorage.getItem("IsCompanySMSEnable") == "true" ? true : false;
    console.log(this.IsCompanySMSEnable, "IsCompanySMSEnable");

    this.ischecked = this.account.IsUserSMSEnable;
    // Using parentCol
    this.parentColumns = WellnessConstants.parentColumns;
  }

  ionViewDidLoad() {
    console.log("ionViewDidLoad DashboardPage");
  }

  LoadUserInfo() {
    this.rewardpoints = parseInt(localStorage.getItem("RewardPoint"));
    this.imageFileName = localStorage.getItem("ProfileImage");

    const userAcc = {
      DeviceId: localStorage.getItem("deviceid"),
      SecretToken: localStorage.getItem("SecretToken")
    };
    this.userService.getUserData(userAcc).subscribe((res: any) => {
      localStorage.setItem("RewardPoint", res.RewardPoint);
      localStorage.setItem("bio_age", res.bio_age);
      localStorage.setItem("mhrs_score", res.mhrs_score);
      localStorage.setItem("IsUserSMSEnable", res.IsUserSMSEnable)
      this.rewardpoints = parseInt(localStorage.getItem("RewardPoint"));
      this.bio_age = localStorage.getItem("bio_age");
      this.mhrs_score = localStorage.getItem("mhrs_score");
      this.ischecked = res.IsUserSMSEnable;

      const data = {};
      // Read all the data;
      data["FirstName"] = localStorage.getItem("FirstName");
      data["LastName"] = localStorage.getItem("LastName");
      data["PhoneNumber"] = localStorage.getItem("PhoneNumber");
      data["ProfileImage"] = localStorage.getItem("ProfileImage");
      data["Gender"] = localStorage.getItem("Gender");
      data["Height"] = localStorage.getItem("Height");
      data["BirthDate"] = localStorage.getItem("BirthDate");
      data["RewardPoint"] = localStorage.getItem("RewardPoint");
      data["bio_age"] = localStorage.getItem("bio_age");
      data["mhrs_score"] = localStorage.getItem("mhrs_score");
      data["IsUserSMSEnable"] = localStorage.getItem("IsUserSMSEnable");
      console.log("Event published : " + data);
      this.events.publish("user:created", data);
    });
    this.loader.dismiss();
  }
  ChangeDate()
  {
    this.IsShowDone= true;
  }
  onModelChange(event)
  {
    this.IsShowDone= true;
    console.log(event);
  }
  async showGenderPicker() {
    this.IsShowDone = true;
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

  editprofilepic() {
    console.log("show image upload");
    this.IsShowDone = true;
    let alert = this.alertCtl.create({
      title: "Please select option",
      cssClass: "action-sheets-basic-page",
      buttons: [
        {
          text: "Take photo",
          handler: () => {
            this.captureImage(false);
          }
        },
        {
          text: "Choose photo from Gallery",
          handler: () => {
            this.captureImage(true);
          }
        }
        // ,
        // {
        //   text: "Cancel",
        //   handler: () => {
        //     alert.dismiss();
        //   }
        // }
      ]
    });
    alert.present();
  }

  SetHeight_inInches() {
    let inputVal = this.UIHeight;
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

  captureImage(useAlbum: boolean) {

    const options: CameraOptions = {
      quality: 25, //100,
      targetWidth: 300,
      targetHeight: 300,
      destinationType: this.camera.DestinationType.DATA_URL,
      encodingType: this.camera.EncodingType.JPEG,
      ...(useAlbum
        ? { sourceType: this.camera.PictureSourceType.SAVEDPHOTOALBUM }
        : {
            saveToPhotoAlbum: true
          })
    };

    this.camera.getPicture(options).then(
      imageData => {
        this.imageDATA = imageData;
        this.imageFileName = this.imageDATA;
        this.imageFileName = "data:image/jpeg;base64," + imageData;
        //this.imageFileName = (<any>window).Ionic.WebView.convertFileSrc(
        // imageData
        //);
      },
      err => {
        //alert(err);
        console.log(err);
      }
    );
  }

  commandUrl: string =
    WellnessConstants.App_Url + "api/WellnessAPI/UpdateUserProfile";

  uploadFile() {
    //alert('API is not yet completed!');
    //debugger;
    if (this.IsFormValid()) {
    
    let loader = this.loadingCtrl.create({
      content: "Updating..."
    });
    loader.present();
    this.SetHeight_inInches();
    const formData = new FormData();
    const dataJson = {
      DeviceId: localStorage.getItem("deviceid"),
      SecretToken: localStorage.getItem("SecretToken"),
      FirstName: this.account.FirstName,
      LastName: this.account.LastName,
      PhoneNumber:this.account.PhoneNumber,
      BirthDate: this.account.BirthDate,
      Height: this.account.Height,
      Gender: this.account.Gender,
      IsUserSMSEnable : this.account.IsUserSMSEnable
    };
    //alert("old path "+ localStorage.getItem("ProfileImage"));
    const jsonString = JSON.stringify(dataJson);
    formData.append("model", jsonString);

    if (this.imageDATA !== undefined) {
      // call method that creates a blob from dataUri
      const imageBlob = this.dataURItoBlob(this.imageDATA);
      //alert('found imagedata');
      //const imageFile = new Blob([imageBlob],{ type: 'image/jpeg' })
      formData.append("ProfileImage", imageBlob, "userimg.jpeg");
      //alert(imageBlob);
    } else {
      //alert(' no image modify ');
    }

    this.DisplayHeight = false;
    this.DisplayBirthDate = false;
    this.DisplayGender = false;
    this.DisplayFirstName = false;
    this.DisplayLastName = false;
    this.DisplayUserName = false;
    this.DisplayPhoneNumber = false;
    this.IsShowDone = false;
    this.PostFile(
      this.commandUrl,
      formData,
      res => {
        if (res.SystemStatus == "Success") {
          this.SetUserInfo(res);
        }
        //this.presentAlert(res.SystemMessage);
        console.log(res.SystemMessage);
        this.presentAlert("Profile updated successfully.");
        loader.dismiss();
      },
      err => {
        loader.dismiss();
        //this.presentAlert("Server Message - Update User Profile: " + err.error.SystemMessage);
        this.presentAlert(
          "Server Message - Update User Profile: " + JSON.stringify(err)
        );
      }
    );
    }
  }
  
  EventEnableSMS(event)
  {
    this.IsShowDone = true;
    console.log(event.checked);
    this.ischecked = event.checked;
    this.account.IsUserSMSEnable = this.ischecked;
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

  IsFormValid() {
    debugger;
    console.log(this.account.PhoneNumber , "-phone number-")
    var msg: string;
    if (this.account.FirstName == "" && this.account.LastName == "") {
      msg = "Please enter First Name & Please enter Last Name.";
    } else if (this.account.FirstName == "") {
      msg = "Please enter First Name.";
    } else if (this.account.LastName == "") {
      msg = "Please enter Last Name.";
    }
    else if (this.account.PhoneNumber == "" || this.account.PhoneNumber === null) {
      msg = "Please enter PhoneNumber.";
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
    if (msg != "" && msg != undefined) {
      this.presentAlert(msg);
      return false;
    } else {
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
          handler: () => {}
        }
      ]
    });
    await alert.present();
  }

  SetUserInfo(data) {
    //alert("new Path " + data.ProfileImage);
    localStorage.setItem("FirstName", data.FirstName);
    localStorage.setItem("LastName", data.LastName);
    localStorage.setItem("ProfileImage", data.ProfileImage);
    localStorage.setItem("Gender", data.Gender);
    localStorage.setItem("Height", data.Height);
    localStorage.setItem("BirthDate", data.BirthDate);
    localStorage.setItem('PhoneNumber',data.PhoneNumber);
    localStorage.setItem('IsCompanySMSEnable',data.IsCompanySMSEnable);
    localStorage.setItem('IsUserSMSEnable',data.IsUserSMSEnable);
    data["RewardPoint"] = localStorage.getItem("RewardPoint");
    data["bio_age"] = localStorage.getItem("bio_age");
    data["mhrs_score"] = localStorage.getItem("mhrs_score");
    this.events.publish("user:created", data);
  }

  DisplayHeight: boolean = false;
  DisplayBirthDate: boolean = false;
  DisplayGender: boolean = false;
  DisplayFirstName: boolean = false;
  DisplayLastName: boolean = false;
  DisplayUserName: boolean = false;
  DisplayPhoneNumber:boolean = false;

  EditSection(item: string) {
    this.IsShowDone = true;
    switch (item) {
      case "FirstName":
        this.DisplayFirstName = true;
        break;
      case "LastName":
        this.DisplayLastName = true;
        break;
      case "PhoneNumber":
          this.DisplayPhoneNumber = true;
          break;  
      case "UserName":
        this.DisplayUserName = true;
        break;
      case "Height":
        this.DisplayHeight = true;
        break;
      case "BirthDate":
        this.DisplayBirthDate = true;
        break;
      case "Gender":
        this.DisplayGender = true;
        break;
    }
  }

  dataURItoBlob(dataURI) {
    const byteString = window.atob(dataURI);
    const arrayBuffer = new ArrayBuffer(byteString.length);
    const int8Array = new Uint8Array(arrayBuffer);
    for (let i = 0; i < byteString.length; i++) {
      int8Array[i] = byteString.charCodeAt(i);
    }
    const blob = new Blob([int8Array], { type: "image/jpeg" });
    return blob;
  }

  PostFile(postUrl, formData, fnSuccessCallBack, fnErrorCallBack) {
    $.ajax({
      type: "POST",
      url: postUrl,
      data: formData, //JSON.stringify(model),
      processData: false,
      contentType: false,
      success: function(msg) {
        if (fnSuccessCallBack != undefined) {
          fnSuccessCallBack(msg);
        }
      },
      error: function(xhr, errStatus, error) {
        if (fnErrorCallBack == undefined) {
          console.log(error + " " + errStatus);
        } else {
          fnErrorCallBack(error, errStatus);
        }
      }
    });
  }
}
