webpackJsonp([1],{

/***/ 147:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return SignupPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__ngx_translate_core__ = __webpack_require__(34);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_forms__ = __webpack_require__(18);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__providers__ = __webpack_require__(15);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__templates_template__ = __webpack_require__(93);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__intro_video_intro_video__ = __webpack_require__(148);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__providers_settings_wellnessconstant__ = __webpack_require__(44);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__providers_settings_alertmessage_service__ = __webpack_require__(149);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};









var SignupPage = /** @class */ (function () {
    function SignupPage(navCtrl, user, formBuilder, translateService, pickerCtl, alertCtl, loading, events, alertMessageSrv) {
        this.navCtrl = navCtrl;
        this.user = user;
        this.formBuilder = formBuilder;
        this.translateService = translateService;
        this.pickerCtl = pickerCtl;
        this.alertCtl = alertCtl;
        this.loading = loading;
        this.events = events;
        this.alertMessageSrv = alertMessageSrv;
        // The account fields for the login form.
        // If you're using the username field with or without email, make
        // sure to add it to the type
        this.account = {
            FirstName: "",
            LastName: "",
            UserName: "",
            PhoneNumber: "",
            Gender: "",
            BirthDate: "",
            Height: "",
            Password: "",
            confirmpassword: "",
            apiKey: "",
            deviceid: "",
            IsAgreeTermsCondiotion: false,
            ReferralCode: ""
        };
        this.signupForm = new __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormGroup"]({
            Firstname: new __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormControl"](),
            LastName: new __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormControl"](),
            PhoneNumber: new __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormControl"](),
            UserName: new __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormControl"](),
            Gender: new __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormControl"](),
            BirthDate: new __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormControl"](),
            Height: new __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormControl"](),
            Password: new __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormControl"](),
            confirmpassword: new __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormControl"](),
            IsAgreeTermsCondiotion: new __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormControl"](),
            ReferralCode: new __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormControl"]()
        });
        this.slides = [
            { active: true },
            { active: false },
            { active: false },
            { active: false },
            { active: false }
        ];
        this.currentSlide = 0;
        this.buttonText = "Next";
        // Using parentCol
        this.parentColumns = __WEBPACK_IMPORTED_MODULE_7__providers_settings_wellnessconstant__["a" /* WellnessConstants */].parentColumns;
        this.createForm();
    }
    SignupPage.prototype.createForm = function () {
        this.signupForm = this.formBuilder.group({
            FirstName: "",
            LastName: "",
            UserName: "",
            PhoneNumber: "",
            Gender: "",
            BirthDate: "",
            Height: "",
            Password: "",
            confirmpassword: "",
            IsAgreeTermsCondiotion: false,
            ReferralCode: ""
        });
    };
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
    SignupPage.prototype.showGenderPicker = function () {
        return __awaiter(this, void 0, void 0, function () {
            var opts, picker;
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        opts = {
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
                        return [4 /*yield*/, this.pickerCtl.create(opts)];
                    case 1:
                        picker = _a.sent();
                        picker.present();
                        picker.onDidDismiss(function (data) { return __awaiter(_this, void 0, void 0, function () {
                            var col;
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0: return [4 /*yield*/, picker.getColumn("Gender")];
                                    case 1:
                                        col = _a.sent();
                                        this.account.Gender = col.options[col.selectedIndex].value;
                                        return [2 /*return*/];
                                }
                            });
                        }); });
                        return [2 /*return*/];
                }
            });
        });
    };
    SignupPage.prototype.IsFormValidated = function () {
        var msg;
        if (this.currentSlide == 0) {
            if (this.account.FirstName == "") {
                msg = "Please enter First Name.";
            }
            else if (this.account.LastName == "") {
                msg = "Please enter Last Name.";
            }
            else if (this.account.PhoneNumber == "") {
                msg = "Please enter PhoneNumber.";
            }
            else if (this.account.UserName == "") {
                msg = "Please enter Username.";
            }
            else if (this.account.PhoneNumber !== "") {
                var info = this.isValidMobile(this.account.PhoneNumber);
                if (!info.isValid) {
                    msg = info.msg;
                }
                else {
                    msg = "";
                }
            }
        }
        else if (this.currentSlide == 1) {
            if (this.account.Gender == "") {
                msg = "Please select your Gender.";
            }
        }
        else if (this.currentSlide == 2) {
            if (this.account.BirthDate == "") {
                msg = "Please select your date of birth.";
            }
        }
        else if (this.currentSlide == 3) {
            if (this.account.Height == "") {
                msg = "Please select your Height.";
            }
        }
        else if (this.currentSlide == 4) {
            if (this.account.Password == "") {
                msg = "Please enter a Password.";
            }
            else if (this.account.confirmpassword == "") {
                msg = "Confirm Password & Password must be same.";
            }
            else if (this.account.Password != this.account.confirmpassword) {
                msg = "Confirm Password & Password must be same.";
            }
            else if (this.account.ReferralCode.length > 0 && this.account.ReferralCode.length !== 6) {
                msg = "Referral code must have 9 characters.";
            }
            else if (this.account.IsAgreeTermsCondiotion === undefined ||
                this.account.IsAgreeTermsCondiotion == false) {
                msg = "Please accept privacy policy and terms.";
            }
        }
        if (msg != "" && msg != undefined) {
            this.alertMessageSrv.presentAlert(msg);
            return true;
        }
        else {
            return false;
        }
    };
    SignupPage.prototype.SaveUserTokenInDB = function () {
        return __awaiter(this, void 0, void 0, function () {
            var accountinfo;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        accountinfo = {
                            deviceid: localStorage.getItem("deviceid"),
                            SecretToken: localStorage.getItem("SecretToken"),
                            DeviceType: localStorage.getItem("DeviceType"),
                            UserTokenId: localStorage.getItem("FCMToken")
                        };
                        return [4 /*yield*/, this.user.SaveUserTokenIdData(accountinfo).subscribe(function (resp) {
                                console.log("save token success");
                            }, function (err) {
                                //this.presentAlert(err.error.SystemMessage);
                                console.log(err);
                            })];
                    case 1:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    SignupPage.prototype.numberOnlyValidation = function (value) {
        if (isNaN(value) || value.includes(".")) {
            console.log(false);
            // invalid character, prevent input
            return false;
        }
        else {
            console.log(true);
            return true;
        }
    };
    SignupPage.prototype.isValidMobile = function (value) {
        var msgInfo = { msg: "", isValid: true };
        var firstLetter = value.charAt(0);
        var remainingLetter = "";
        if (firstLetter == "+") {
            remainingLetter = value.replace(firstLetter, "");
        }
        else {
            remainingLetter = value;
        }
        // remove space
        remainingLetter = remainingLetter.replace(/\s/g, "");
        console.log(firstLetter);
        console.log(remainingLetter);
        var regExp = /^[0-9]{10,20}$/;
        if (msgInfo.isValid && !regExp.test(remainingLetter)) {
            msgInfo.isValid = false;
            msgInfo.msg = "Phone number start with + country code and should be 10-20 length";
            //console.log(msgInfo.isValid, 'if test isvalid');
        }
        else {
            msgInfo.isValid = true;
            //console.log(msgInfo.isValid, 'else isvalid');
        }
        return msgInfo;
    };
    SignupPage.prototype.doSignup = function () {
        var _this = this;
        this.loader = this.alertMessageSrv.PleaseWait();
        this.account.Password != this.account.confirmpassword;
        this.account.apiKey = "71e73c14-2723-4d6e-a489-c9675738fdf4";
        this.account.deviceid = localStorage.getItem("remUUID");
        localStorage.setItem("deviceid", this.account.deviceid);
        //alert('deviceid : '+this.account.deviceid);
        if (localStorage.getItem("remUUID") == null)
            this.account.deviceid = "c9675738fdf4";
        this.loader.present().then(function () {
            _this.SetHeight_inInches();
            _this.user.signup(_this.account).subscribe(function (resp) {
                // Set for user name and password
                localStorage.setItem("UserName", _this.account.UserName);
                localStorage.setItem("Password", _this.account.Password);
                localStorage.setItem("FirstName", _this.account.FirstName);
                localStorage.setItem("LastName", _this.account.LastName);
                localStorage.setItem("PhoneNumber", _this.account.PhoneNumber);
                _this.SetUserInfo(resp);
                if (localStorage.getItem("deviceid") !== "" &&
                    localStorage.getItem("deviceid") !== undefined &&
                    localStorage.getItem("FCMToken") !== "" &&
                    localStorage.getItem("FCMToken") !== undefined &&
                    localStorage.getItem("DeviceType") !== "" &&
                    localStorage.getItem("DeviceType") !== undefined) {
                    _this.SaveUserTokenInDB();
                }
                localStorage.setItem("remuser", _this.account.UserName);
                localStorage.setItem("rempwd", _this.account.Password);
                setTimeout(function () {
                    _this.loader.dismiss();
                    _this.navCtrl.push(__WEBPACK_IMPORTED_MODULE_6__intro_video_intro_video__["a" /* IntroVideoPage */]);
                }, 3000);
                //this.navCtrl.setRoot(MyHraPage);
                //this.navCtrl.push(IntroPage);
            }, function (err) {
                console.log(err);
                _this.loader.dismiss();
                _this.alertMessageSrv.ErrorMsg(err, "App User Register");
            });
        });
    };
    SignupPage.prototype.SetUserInfo = function (data) {
        this.events.publish("user:created", data);
    };
    SignupPage.prototype.SetHeight_inInches = function () {
        var inputVal = this.account.Height;
        var arrVal = inputVal.split("-");
        if (arrVal[0].indexOf("cm") !== -1) {
            var inch = Math.round((parseInt(arrVal[1]) * 1) / 2.54);
            console.log(inch, "CM - INCH");
            this.account.Height = inch + "";
        }
        else if (arrVal[0].indexOf("feet") !== -1) {
            var inch = parseInt(arrVal[1]) * 12 + parseInt(arrVal[2]);
            console.log(inch, "FEET - INCH");
            this.account.Height = inch + "";
        }
    };
    SignupPage.prototype.changeTabButton = function (index) {
        if (!this.IsFormValidated()) {
            this.currentSlide = index;
            this.slides.forEach(function (val, index) {
                val.active = false;
            });
            this.slides[this.currentSlide].active = true;
            if (this.slides.length - 1 <= this.currentSlide) {
                this.buttonText = "Submit";
            }
        }
    };
    SignupPage.prototype.changeTab = function (index) {
        if (!this.IsFormValidated()) {
            if (index == this.slides.length - 1) {
                this.doSignup();
                return false;
            }
            this.slides.forEach(function (val, index) {
                val.active = false;
            });
            this.currentSlide = index + 1;
            this.slides[this.currentSlide].active = true;
            if (this.slides.length - 1 <= this.currentSlide) {
                this.buttonText = "Submit";
            }
        }
    };
    SignupPage.prototype.ViewTemplate = function (Name) {
        this.navCtrl.push(__WEBPACK_IMPORTED_MODULE_5__templates_template__["a" /* TemplatePage */], {
            Content: Name
        });
    };
    SignupPage = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: "page-signup",template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\signup\signup.html"*/'<ion-header>\n  <ion-navbar color="primary">\n    <ion-title>{{ \'SIGNUP_TITLE\' | translate }}</ion-title>\n  </ion-navbar>\n</ion-header>\n<ion-content>   \n  <ul class="custom-tab">\n    <li  [ngClass]="{\'active\':slides[0].active}" (click)="changeTabButton(0)">\n         <span class="ico-detail ico"></span>\n        <span class="text">Enter Details</span>\n      </li>\n    <li [ngClass]="{\'active\':slides[1].active}" (click)="changeTabButton(1)">\n      <span class="ico-gender ico"></span>\n        <span class="text">Gender</span>\n      </li>\n    <li [ngClass]="{\'active\':slides[2].active}" (click)="changeTabButton(2)">\n        <span class="ico-age ico"></span>\n        <span class="text">Date of Birth</span>\n      </li>\n    <li [ngClass]="{\'active\':slides[3].active}" (click)="changeTabButton(3)">\n        <span class="ico-measure ico"></span>\n        <span class="text">Height</span>\n      </li>\n    <li [ngClass]="{\'active\':slides[4].active}" (click)="changeTabButton(4)">\n        <span class="ico-password ico"></span>\n        <span class="text">Enter Password</span>\n      </li>\n  </ul>\n  <form [formGroup]="signupForm">\n    <ion-card class="signup-card" *ngIf="slides[0].active">\n      <ion-card-content>\n        <ion-list>\n          <ion-item class="custom-border">\n            <ion-label stacked>First Name</ion-label>\n            <ion-input type="text" [(ngModel)]="account.FirstName" formControlName="FirstName"></ion-input>\n          </ion-item>\n          <ion-item class="custom-border">\n            <ion-label stacked>Last Name</ion-label>\n            <ion-input type="text" [(ngModel)]="account.LastName" formControlName="LastName"></ion-input>\n          </ion-item>\n          <ion-item class="custom-border">\n            <ion-label stacked>Phone Number</ion-label>\n            <ion-input type="text" [(ngModel)]="account.PhoneNumber" formControlName="PhoneNumber" placeholder="+1 604 555 8955"></ion-input>\n          </ion-item>\n\n          <ion-item class="custom-border">\n            <ion-label stacked>User Name</ion-label>\n            <ion-input type="email" [(ngModel)]="account.UserName" formControlName="UserName" ></ion-input>\n          </ion-item>\n        </ion-list>\n        <div class="note">\n          <p>*Use your email as Username.</p>\n        </div>\n      </ion-card-content>\n    </ion-card>\n    <ion-card class="signup-card" *ngIf="slides[1].active">\n      <ion-card-content class="cardcontent">\n\n          <!-- <ion-item> -->\n              <ion-label>I\'M A: </ion-label>\n              <ion-label color="primary">{{ account.Gender }} &nbsp;&nbsp;</ion-label>\n              <a (click)="showGenderPicker()">Select</a>\n            <!-- </ion-item> -->\n\n          \n      </ion-card-content>\n    </ion-card>\n    <ion-card class="signup-card" *ngIf="slides[2].active" >\n        <ion-card-content>\n            <ion-list radio-group>\n                <ion-list-header>\n                    WHEN WHERE YOU BORN?\n                </ion-list-header>\n                <ion-item class="custom-border">\n                  <ion-label stacked>Birthdate</ion-label>\n                  <ion-datetime [(ngModel)]="account.BirthDate" formControlName="BirthDate" ></ion-datetime>\n                </ion-item>\n              </ion-list>\n        </ion-card-content>\n      </ion-card>\n    <ion-card class="signup-card" *ngIf="slides[3].active">\n      <ion-card-content>\n          <ion-list radio-group>\n                  <ion-list-header>\n                      HOW TALL ARE YOU?\n                  </ion-list-header>\n                \n                  <ion-item class="custom-border">\n                    <ion-label stacked>Height</ion-label>\n                    <ion-multi-picker  [(ngModel)]="account.Height" \n                    formControlName="Height"  item-content [multiPickerColumns]="parentColumns" [separator]="\'-\'"></ion-multi-picker>\n                  </ion-item>\n                </ion-list>\n          </ion-card-content>\n        </ion-card>\n        <ion-card class="signup-card" *ngIf="slides[4].active">\n            <ion-card-content>\n                <ion-item class="custom-border">\n                    <ion-label stacked>{{ \'PASSWORD\' | translate }}</ion-label>\n                    <ion-input type="password" [(ngModel)]="account.Password" formControlName="Password"></ion-input>\n                </ion-item>\n                <ion-item class="custom-border">\n                    <ion-label stacked>Confirm password</ion-label>\n                    <ion-input type="password" [(ngModel)]="account.confirmpassword" formControlName="confirmpassword"></ion-input>\n                </ion-item>\n                <ion-item class="custom-border">\n                  <ion-label stacked>Referral Code</ion-label>\n                  <ion-input type="text" [(ngModel)]="account.ReferralCode" formControlName="ReferralCode" ></ion-input>\n               </ion-item>\n                <ion-item>\n                  <div class="msg-content">Password must have minimum 6 characters and includes one uppercase letter (‘A - Z’), one lowercase letter (‘a - z’) and a number (‘0 - 9’).</div>\n                </ion-item>                \n                <div class="term-note">\n                  <ion-checkbox [(ngModel)]="account.IsAgreeTermsCondiotion" formControlName="IsAgreeTermsCondiotion"></ion-checkbox>\n                  <div class="signup-text">By singing up for meschino wellness you are agreeing to our \n                    <a href="#" (click)="ViewTemplate(\'TermsAndConditions\')">privacy policy and terms</a></div>\n                </div>\n            </ion-card-content>\n          </ion-card>\n          </form>\n</ion-content>\n<ion-footer>\n  <ion-toolbar>\n    <button ion-button color="primary" class="big-btn" (click)="changeTab(currentSlide)" >{{buttonText}}</button>\n  </ion-toolbar>\n</ion-footer>\n'/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\signup\signup.html"*/
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_2_ionic_angular__["NavController"],
            __WEBPACK_IMPORTED_MODULE_4__providers__["f" /* UserService */],
            __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormBuilder"],
            __WEBPACK_IMPORTED_MODULE_1__ngx_translate_core__["c" /* TranslateService */],
            __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["PickerController"],
            __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["AlertController"],
            __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["LoadingController"],
            __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["Events"],
            __WEBPACK_IMPORTED_MODULE_8__providers_settings_alertmessage_service__["a" /* AlertMessagesService */]])
    ], SignupPage);
    return SignupPage;
}());

//# sourceMappingURL=signup.js.map

/***/ }),

/***/ 148:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return IntroVideoPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__my_hra_my_hra__ = __webpack_require__(261);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



/**
 * Generated class for the IntroVideoPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
var IntroVideoPage = /** @class */ (function () {
    function IntroVideoPage(navCtrl, navParams, menu) {
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.menu = menu;
    }
    IntroVideoPage.prototype.ionViewDidLoad = function () {
        this.video = this.mVideoPlayer.nativeElement;
        this.video.play();
        console.log('ionViewDidLoad IntroVideoPage');
    };
    IntroVideoPage.prototype.goToMyHra = function () {
        this.video.pause();
        this.menu.enable(true);
        this.navCtrl.push(__WEBPACK_IMPORTED_MODULE_2__my_hra_my_hra__["a" /* MyHraPage */]);
    };
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('videoPlayer'),
        __metadata("design:type", Object)
    ], IntroVideoPage.prototype, "mVideoPlayer", void 0);
    IntroVideoPage = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'page-intro-video',template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\intro-video\intro-video.html"*/'<!--\n  Generated template for the IntroVideoPage page.\n\n  See http://ionicframework.com/docs/components/#navigation for more info on\n  Ionic pages and navigation.\n-->\n<ion-header class="custom-intro-header content-bg">\n  <ion-navbar color="primary">\n    <ion-title>HRA Introduction</ion-title>\n  </ion-navbar>\n</ion-header>\n<ion-content class="content-bg">\n  <ion-list class="list-video">\n    <ion-item class="content-bg">\n      <video width="100%" controls autoplay="autoplay" #videoPlayer>\n        <source src="https://mediasvc611wkrxkhq32k.blob.core.windows.net/asset-cc27435d-1500-80c6-2ea5-f1e515046a95/Health%20Risk%20Assessment%20-%20No%20Title.mp4?sv=2012-02-12&sr=c&si=f57a5f8d-3c43-454b-9c2a-d1f1805e9111&sig=BozyWHb1N1TOlIdDsC9GQOc2tV4vKewhxqFmrbMtw18%3D&st=2015-06-18T11%3A15%3A28Z&se=2115-05-25T11%3A15%3A28Z" type="video/mp4">\n        <source src="mov_bbb.ogg" type="video/ogg">\n        Your browser does not support HTML5 video.\n      </video>\n    </ion-item>\n    </ion-list>\n</ion-content>\n<ion-footer>\n  <ion-toolbar>\n    <button ion-button color="primary" class="big-btn" (click)="goToMyHra()">Start</button>\n  </ion-toolbar>\n</ion-footer>\n  \n'/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\intro-video\intro-video.html"*/,
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavController"], __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavParams"], __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["MenuController"]])
    ], IntroVideoPage);
    return IntroVideoPage;
}());

//# sourceMappingURL=intro-video.js.map

/***/ }),

/***/ 149:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AlertMessagesService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};


var AlertMessagesService = /** @class */ (function () {
    function AlertMessagesService(toastCtl, loadingCtl, alertCtl) {
        this.toastCtl = toastCtl;
        this.loadingCtl = loadingCtl;
        this.alertCtl = alertCtl;
    }
    AlertMessagesService.prototype.presentToast = function (message) {
        return __awaiter(this, void 0, void 0, function () {
            var toast;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.toastCtl.create({
                            message: message,
                            duration: 4000,
                            position: "top"
                        })];
                    case 1:
                        toast = _a.sent();
                        toast.present();
                        return [2 /*return*/];
                }
            });
        });
    };
    AlertMessagesService.prototype.presentErrorToast = function (message) {
        return __awaiter(this, void 0, void 0, function () {
            var toast;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.toastCtl.create({
                            message: message,
                            position: "top",
                            duration: 4000,
                            cssClass: "toast-error"
                        })];
                    case 1:
                        toast = _a.sent();
                        toast.present();
                        return [2 /*return*/];
                }
            });
        });
    };
    AlertMessagesService.prototype.PleaseWait = function () {
        return this.loadingCtl.create({
            content: "Please wait..."
        });
    };
    AlertMessagesService.prototype.LoadingMsg = function (msg) {
        return this.loadingCtl.create({
            content: msg
        });
    };
    AlertMessagesService.prototype.presentAlert = function (msg) {
        return __awaiter(this, void 0, void 0, function () {
            var alert;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.alertCtl.create({
                            message: msg,
                            cssClass: "action-sheets-basic-page",
                            buttons: [
                                {
                                    text: "OK",
                                    handler: function () { }
                                }
                            ]
                        })];
                    case 1:
                        alert = _a.sent();
                        return [4 /*yield*/, alert.present()];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    AlertMessagesService.prototype.ErrorMsg = function (err, apiName) {
        if (err.error !== undefined && err.error.SystemMessage !== undefined) {
            this.presentAlert(err.error.SystemMessage);
        }
        else {
            this.presentAlert("Server Message -" + apiName + " : " + JSON.stringify(err));
        }
    };
    AlertMessagesService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["ToastController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["LoadingController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["AlertController"]])
    ], AlertMessagesService);
    return AlertMessagesService;
}());

//# sourceMappingURL=alertmessage.service.js.map

/***/ }),

/***/ 15:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__api_api__ = __webpack_require__(71);
/* harmony reexport (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return __WEBPACK_IMPORTED_MODULE_0__api_api__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__mocks_providers_items__ = __webpack_require__(259);
/* unused harmony reexport Items */
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__settings_settings__ = __webpack_require__(534);
/* harmony reexport (binding) */ __webpack_require__.d(__webpack_exports__, "d", function() { return __WEBPACK_IMPORTED_MODULE_2__settings_settings__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__hra_hra__ = __webpack_require__(535);
/* harmony reexport (binding) */ __webpack_require__.d(__webpack_exports__, "b", function() { return __WEBPACK_IMPORTED_MODULE_3__hra_hra__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__user_notification__ = __webpack_require__(536);
/* harmony reexport (binding) */ __webpack_require__.d(__webpack_exports__, "c", function() { return __WEBPACK_IMPORTED_MODULE_4__user_notification__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__steps_stepschallenge__ = __webpack_require__(260);
/* harmony reexport (binding) */ __webpack_require__.d(__webpack_exports__, "e", function() { return __WEBPACK_IMPORTED_MODULE_5__steps_stepschallenge__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__user_user__ = __webpack_require__(538);
/* harmony reexport (binding) */ __webpack_require__.d(__webpack_exports__, "f", function() { return __WEBPACK_IMPORTED_MODULE_6__user_user__["a"]; });







//# sourceMappingURL=index.js.map

/***/ }),

/***/ 150:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return StepDashboardPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__providers__ = __webpack_require__(15);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__notification_notification__ = __webpack_require__(72);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__log_steps_logsteps__ = __webpack_require__(289);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};





/**
 * Generated class for the IntroPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
var StepDashboardPage = /** @class */ (function () {
    function StepDashboardPage(navCtrl, navParams, loadingCtrl, alertCtl, userService, stepChallengeService, events, menu) {
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.loadingCtrl = loadingCtrl;
        this.alertCtl = alertCtl;
        this.userService = userService;
        this.stepChallengeService = stepChallengeService;
        this.events = events;
        this.menu = menu;
        this.model = {};
        this.progressmodel = {};
        this.notificationCount = ""; //localStorage.getItem("NotificationCount");
        this.menu.enable(true);
        this.loadInitialData();
    }
    StepDashboardPage.prototype.loadInitialData = function () {
        var _this = this;
        var userAcc = {
            DeviceId: localStorage.getItem("deviceid"),
            SecretToken: localStorage.getItem("SecretToken")
        };
        this.userService.GetPushNotificationCount(userAcc).subscribe(function (res) {
            var msgcount = res.Count;
            _this.notificationCount = msgcount > 0 ? msgcount : "";
            localStorage.setItem("notificationCount", _this.notificationCount);
        });
        this.loader = this.loadingCtrl.create({
            content: "Please wait..."
        });
        this.loader.present().then(function () {
            _this.stepChallengeService
                .GetUserOverallProgressOfStepChallenge(userAcc)
                .subscribe(function (resp) {
                _this.loader.dismiss();
                if (resp.SystemStatus == "Success") {
                    _this.model = resp;
                    _this.percent = _this.model.PercentOfStep;
                    _this.subtitle = _this.model.PercentOfStep + "%";
                    console.log(_this.model);
                }
                else {
                    _this.presentAlert(resp.SystemMessage);
                }
            }, function (err) {
                _this.loader.dismiss();
                _this.presentAlert("Server Message - Get User Overall Progress Of StepChallenge : " +
                    JSON.stringify(err));
            });
            _this.stepChallengeService
                .GetUserStepChallengeProgressOverviewCount(userAcc)
                .subscribe(function (resp) {
                if (resp.SystemStatus == "Success") {
                    _this.progressmodel = resp;
                    console.log(_this.progressmodel, 'progressmodel');
                }
                else {
                    _this.presentAlert(resp.SystemMessage);
                }
            }, function (err) {
                _this.presentAlert("Server Message - Get User Step Challenge Progress Overview Count : " +
                    JSON.stringify(err));
            });
        });
    };
    StepDashboardPage.prototype.EmitterNotificationCount = function () {
        var _this = this;
        this.events.subscribe("PushNotification", function (PushNotification) {
            var msgcount = PushNotification.Count;
            _this.notificationCount = msgcount > 0 ? msgcount : "";
            localStorage.setItem("notificationCount", _this.notificationCount);
        });
    };
    StepDashboardPage.prototype.ionViewDidLoad = function () {
        console.log("ionViewDidLoad TemplatePage");
    };
    StepDashboardPage.prototype.presentAlert = function (msg) {
        return __awaiter(this, void 0, void 0, function () {
            var alert;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.alertCtl.create({
                            message: msg,
                            cssClass: "action-sheets-basic-page",
                            buttons: [
                                {
                                    text: "OK",
                                    handler: function () { }
                                }
                            ]
                        })];
                    case 1:
                        alert = _a.sent();
                        return [4 /*yield*/, alert.present()];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    StepDashboardPage.prototype.ionViewDidEnter = function () {
        //  this.navBar.backButtonClick = () => {
        //    console.log('Back button click');
        //this.navCtrl.push(NotificationPage)
        //  };
    };
    StepDashboardPage.prototype.GoOverView = function (name) {
        this.navCtrl.setPages([{ page: name }]);
    };
    StepDashboardPage.prototype.goLogsteps = function () {
        this.navCtrl.push(__WEBPACK_IMPORTED_MODULE_4__log_steps_logsteps__["a" /* LogStepsPage */]);
    };
    StepDashboardPage.prototype.goNotification = function () {
        this.navCtrl.push(__WEBPACK_IMPORTED_MODULE_3__notification_notification__["a" /* NotificationPage */]);
    };
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])("navbar"),
        __metadata("design:type", __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["Navbar"])
    ], StepDashboardPage.prototype, "navBar", void 0);
    StepDashboardPage = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: "page-step-dashboard",template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\step-challenges\step-dashboard.html"*/'<ion-header>\n\n    <ion-navbar color="primary">\n\n        <button ion-button menuToggle icon-only>\n\n            <ion-icon name=\'menu\'></ion-icon>\n\n        </button>\n\n        <ion-title>Step Challenge</ion-title>\n\n        <ion-buttons end class="left-nav-buttons">\n\n            <button id="notification-button" ion-button icon-only (click)="goNotification()">\n\n                <ion-icon name="notifications">\n\n                    <ion-badge id="notifications-badge" color="danger">{{notificationCount}}</ion-badge>\n\n                </ion-icon>\n\n            </button>\n\n        </ion-buttons>\n\n    </ion-navbar>\n\n</ion-header>\n\n<ion-content>\n\n    <div class="pt-3 pb-3">\n\n        <div class="app-cart ml-3 mr-3 mb-4 position-relative overflow-hidden">\n\n            <img class="sports-girl img-fluid" src="assets/img/sports-girl-bg.png" alt="sports-girl">\n\n            <!-- <div class="pie_progress" role="progressbar" data-goal="58" data-barsize="15" aria-valuemin="0"\n\n                aria-valuemax="100">\n\n                <span class="pie_progress__number">0%</span>\n\n            </div> -->\n\n            <div class="pie_progress">\n\n                <circle-progress [percent]="percent" [radius]="60" [clockwise]="true" [showSubtitle]="true"\n\n                    [showTitle]="false" [responsive]="true" [subtitle]="subtitle" [subtitleFontSize]="24">\n\n                </circle-progress>\n\n            </div>\n\n            <div class="text-center">\n\n                <button type="button"\n\n                    class="custom-btn custom-btn-light mt-3 w-100 font-family-Conv_Gotham-Medium font-weight-bold py-2 font-size-17">{{model.TotalNumOfSteps}}\n\n                    Steps\n\n                    in {{model.TotalDays}} Days</button>\n\n            </div>\n\n        </div>\n\n        <div class="app-cart ml-3 mr-3 mb-4">\n\n            <div class="custom-row">\n\n                <div class="custom-col">\n\n                    <p>Challenge Started</p>\n\n                    <span>{{model.StartDate}}</span>\n\n                </div>\n\n                <div class="custom-col-auto">\n\n                    <img class="img-fluid" src="assets/img/goal-icon.png" alt="goal-icon">\n\n                </div>\n\n            </div>\n\n            <div class="custom-row">\n\n                <div class="custom-col">\n\n                    <p>Steps Remaining</p>\n\n                    <span>{{model.StepsRemaining}}</span>\n\n                </div>\n\n                <div class="custom-col-auto">\n\n                    <img class="img-fluid" src="assets/img/steps-icon.png" alt="steps-icon">\n\n                </div>\n\n            </div>\n\n            <div class="custom-row">\n\n                <div class="custom-col">\n\n                    <p>Days Remaining</p>\n\n                    <span>{{model.DaysRemaining}}</span>\n\n                </div>\n\n                <div class="custom-col-auto">\n\n                    <img class="img-fluid" src="assets/img/calender-icon.png" alt="calender-icon">\n\n                </div>\n\n            </div>\n\n            <div class="custom-row">\n\n                <div class="custom-col">\n\n                    <p>Personal Best</p>\n\n                    <span>{{progressmodel.PersonalBest}}</span>\n\n                </div>\n\n                <div class="custom-col-auto">\n\n                    <img class="img-fluid" src="assets/img/steps-icon.png" alt="steps-icon">\n\n                </div>\n\n            </div>\n\n            <div class="custom-row">\n\n                <div class="custom-col">\n\n                    <p>Steps Average</p>\n\n                    <span>{{progressmodel.StepAverage}}</span>\n\n                </div>\n\n                <div class="custom-col-auto">\n\n                    <img class="img-fluid" src="assets/img/steps-icon.png" alt="steps-icon">\n\n                </div>\n\n            </div>\n\n\n\n        </div>\n\n\n\n        <div class="ml-3 mr-3 mb-4 custom-row text-center">\n\n            <div class="custom-col pr-2">\n\n                <button type="button" class="custom-btn custom-btn-primary w-100 font-weight-bold font-size-17"\n\n                    (click)="goLogsteps()"> <img class="align-middle img-fluid pr-2" src="assets/img/log-steps.png"\n\n                        style="width: 33px;" alt="log-steps">Log\n\n                    Steps</button>\n\n            </div>\n\n            <div class="custom-col pl-2">\n\n                <button type="button" class="custom-btn custom-btn-primary w-100 font-weight-bold font-size-17"\n\n                    (click)="GoOverView(\'LogOverviewPage\')"><img class="align-middle img-fluid pr-2"\n\n                        src="assets/img/overview.png" style="width: 25px;" alt="overview">Overview</button>\n\n            </div>\n\n        </div>\n\n    </div>\n\n\n\n</ion-content>'/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\step-challenges\step-dashboard.html"*/
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavParams"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["LoadingController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["AlertController"],
            __WEBPACK_IMPORTED_MODULE_2__providers__["f" /* UserService */],
            __WEBPACK_IMPORTED_MODULE_2__providers__["e" /* StepChallengeService */],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["Events"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["MenuController"]])
    ], StepDashboardPage);
    return StepDashboardPage;
}());

//# sourceMappingURL=step-dashboard.js.map

/***/ }),

/***/ 204:
/***/ (function(module, exports) {

function webpackEmptyAsyncContext(req) {
	// Here Promise.resolve().then() is used instead of new Promise() to prevent
	// uncatched exception popping up in devtools
	return Promise.resolve().then(function() {
		throw new Error("Cannot find module '" + req + "'.");
	});
}
webpackEmptyAsyncContext.keys = function() { return []; };
webpackEmptyAsyncContext.resolve = webpackEmptyAsyncContext;
module.exports = webpackEmptyAsyncContext;
webpackEmptyAsyncContext.id = 204;

/***/ }),

/***/ 255:
/***/ (function(module, exports, __webpack_require__) {

var map = {
	"../pages/dashboard/dashboard.module": [
		256
	],
	"../pages/forgetpassword/forgetpassword.module": [
		257
	],
	"../pages/hra-body/hra-body.module": [
		267
	],
	"../pages/intro-video/intro-video.module": [
		268
	],
	"../pages/intro/intro.module": [
		269
	],
	"../pages/login/login.module": [
		270
	],
	"../pages/menu/menu.module": [
		578,
		0
	],
	"../pages/my-hra/my-hra.module": [
		271
	],
	"../pages/my-profile/profile.module": [
		273
	],
	"../pages/my-trackers/mytracker.module": [
		272
	],
	"../pages/my-wellness-wallet/hra-qa/hra-qa.module": [
		278
	],
	"../pages/my-wellness-wallet/hra-result/hra-result.module": [
		279
	],
	"../pages/my-wellness-wallet/my-wellness-wallet.module": [
		280
	],
	"../pages/my-wellness-wallet/mywellnessplan/mywellnessplan.module": [
		281
	],
	"../pages/my-wellness-wallet/riskdetail/riskdetail.module": [
		282
	],
	"../pages/notification/notification.module": [
		284
	],
	"../pages/notificationmsg/notimsg.module": [
		283
	],
	"../pages/signup/signup.module": [
		287
	],
	"../pages/step-challenges/addlogs/addlogs.module": [
		285
	],
	"../pages/step-challenges/log-overview/logoverview.module": [
		288
	],
	"../pages/step-challenges/log-steps/logsteps.module": [
		290
	],
	"../pages/step-challenges/step-dashboard.module": [
		291
	],
	"../pages/step-challenges/stepchallengehistory/stepchallengehistory.module": [
		292
	],
	"../pages/templates/template.module": [
		293
	],
	"../pages/welcome/welcome.module": [
		294
	]
};
function webpackAsyncContext(req) {
	var ids = map[req];
	if(!ids)
		return Promise.reject(new Error("Cannot find module '" + req + "'."));
	return Promise.all(ids.slice(1).map(__webpack_require__.e)).then(function() {
		return __webpack_require__(ids[0]);
	});
};
webpackAsyncContext.keys = function webpackAsyncContextKeys() {
	return Object.keys(map);
};
webpackAsyncContext.id = 255;
module.exports = webpackAsyncContext;

/***/ }),

/***/ 256:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DashboardPageModule", function() { return DashboardPageModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__dashboard__ = __webpack_require__(531);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var DashboardPageModule = /** @class */ (function () {
    function DashboardPageModule() {
    }
    DashboardPageModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__dashboard__["a" /* DashboardPage */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_2__dashboard__["a" /* DashboardPage */]),
            ],
        })
    ], DashboardPageModule);
    return DashboardPageModule;
}());

//# sourceMappingURL=dashboard.module.js.map

/***/ }),

/***/ 257:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ForgetPageModule", function() { return ForgetPageModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__ngx_translate_core__ = __webpack_require__(34);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__forgetpassword__ = __webpack_require__(258);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};




var ForgetPageModule = /** @class */ (function () {
    function ForgetPageModule() {
    }
    ForgetPageModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_3__forgetpassword__["a" /* ForgetPasswordPage */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_3__forgetpassword__["a" /* ForgetPasswordPage */]),
                __WEBPACK_IMPORTED_MODULE_1__ngx_translate_core__["b" /* TranslateModule */].forChild()
            ],
            exports: [
                __WEBPACK_IMPORTED_MODULE_3__forgetpassword__["a" /* ForgetPasswordPage */]
            ]
        })
    ], ForgetPageModule);
    return ForgetPageModule;
}());

//# sourceMappingURL=forgetpassword.module.js.map

/***/ }),

/***/ 258:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ForgetPasswordPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__ngx_translate_core__ = __webpack_require__(34);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_forms__ = __webpack_require__(18);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__providers__ = __webpack_require__(15);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__login_login__ = __webpack_require__(92);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__providers_settings_wellnessconstant__ = __webpack_require__(44);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};







var ForgetPasswordPage = /** @class */ (function () {
    function ForgetPasswordPage(navCtrl, user, formBuilder, translateService, loading, alertCtl) {
        this.navCtrl = navCtrl;
        this.user = user;
        this.formBuilder = formBuilder;
        this.translateService = translateService;
        this.loading = loading;
        this.alertCtl = alertCtl;
        // The account fields for the login form.
        // If you're using the username field with or without email, make
        // sure to add it to the type
        this.account = {
            Email: "",
            apiKey: "",
            deviceid: ""
        };
        this.commandUrl = __WEBPACK_IMPORTED_MODULE_6__providers_settings_wellnessconstant__["a" /* WellnessConstants */].App_Url + 'api/WellnessAPI/UserForgotPassword';
    }
    ForgetPasswordPage.prototype.validateFuncation = function () {
        var msg;
        if (this.account.Email == "") {
            msg = "Please enter email.";
        }
        if (msg != "" && msg != undefined) {
            this.presentAlert(msg);
            return true;
        }
        else {
            return false;
        }
    };
    ForgetPasswordPage.prototype.doSubmit = function () {
        var _this = this;
        if (!this.validateFuncation()) {
            this.loader = this.loading.create({
                content: "Please wait..."
            });
            this.account.apiKey = "71e73c14-2723-4d6e-a489-c9675738fdf4";
            this.account.deviceid = "c9675738fdf4";
            this.loader.present().then(function () {
                _this.user.forgetpassword(_this.account).subscribe(function (res) {
                    _this.loader.dismiss();
                    console.log(res);
                    _this.presentAlert(res.SystemMessage);
                }, function (err) {
                    _this.loader.dismiss();
                    //this.presentAlert("Server Message - User Forgot Password : "+ err.error.SystemMessage);
                    _this.presentAlert("Server Message - User Forgot Password : " + JSON.stringify(err));
                });
            });
        }
    };
    ForgetPasswordPage.prototype.presentAlert = function (msg) {
        return __awaiter(this, void 0, void 0, function () {
            var alert;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.alertCtl.create({
                            message: msg,
                            cssClass: "action-sheets-basic-page",
                            buttons: [
                                {
                                    text: "OK",
                                    handler: function () { }
                                }
                            ]
                        })];
                    case 1:
                        alert = _a.sent();
                        return [4 /*yield*/, alert.present()];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    ForgetPasswordPage.prototype.loginup = function () {
        this.navCtrl.push(__WEBPACK_IMPORTED_MODULE_5__login_login__["a" /* LoginPage */]);
    };
    ForgetPasswordPage = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: "page-forgetpassword",template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\forgetpassword\forgetpassword.html"*/'<ion-content scroll="false">\n  <div class="splash-bg">\n    <h1>Forget password</h1>\n  </div>\n <ion-card class="login-card">\n  <ion-card-content>\n      <ion-list>\n          <ion-item class="custom-border">\n              <ion-label stacked>Email</ion-label>\n            <ion-input type="text" [(ngModel)]="account.Email" name="Email"></ion-input>\n          </ion-item>\n        </ion-list>\n  </ion-card-content>\n</ion-card>\n  <div padding class="button-position">\n      <button  ion-button color="primary"  class="big-btn"  (click)="doSubmit()">Submit </button>\n      <button ion-button clear full class="forgot-btn" (click)="loginup()">Back to login </button>\n</div>\n</ion-content>\n '/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\forgetpassword\forgetpassword.html"*/
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_2_ionic_angular__["NavController"],
            __WEBPACK_IMPORTED_MODULE_4__providers__["f" /* UserService */],
            __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormBuilder"],
            __WEBPACK_IMPORTED_MODULE_1__ngx_translate_core__["c" /* TranslateService */],
            __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["LoadingController"],
            __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["AlertController"]])
    ], ForgetPasswordPage);
    return ForgetPasswordPage;
}());

//# sourceMappingURL=forgetpassword.js.map

/***/ }),

/***/ 259:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return Items; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__models_item__ = __webpack_require__(533);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var Items = /** @class */ (function () {
    function Items() {
        this.items = [];
        this.defaultItem = {
            "name": "Burt Bear",
            "profilePic": "assets/img/speakers/bear.jpg",
            "about": "Burt is a Bear.",
        };
        var items = [
            {
                "name": "Burt Bear",
                "profilePic": "assets/img/speakers/bear.jpg",
                "about": "Burt is a Bear."
            },
            {
                "name": "Charlie Cheetah",
                "profilePic": "assets/img/speakers/cheetah.jpg",
                "about": "Charlie is a Cheetah."
            },
            {
                "name": "Donald Duck",
                "profilePic": "assets/img/speakers/duck.jpg",
                "about": "Donald is a Duck."
            },
            {
                "name": "Eva Eagle",
                "profilePic": "assets/img/speakers/eagle.jpg",
                "about": "Eva is an Eagle."
            },
            {
                "name": "Ellie Elephant",
                "profilePic": "assets/img/speakers/elephant.jpg",
                "about": "Ellie is an Elephant."
            },
            {
                "name": "Molly Mouse",
                "profilePic": "assets/img/speakers/mouse.jpg",
                "about": "Molly is a Mouse."
            },
            {
                "name": "Paul Puppy",
                "profilePic": "assets/img/speakers/puppy.jpg",
                "about": "Paul is a Puppy."
            }
        ];
        for (var _i = 0, items_1 = items; _i < items_1.length; _i++) {
            var item = items_1[_i];
            this.items.push(new __WEBPACK_IMPORTED_MODULE_1__models_item__["a" /* Item */](item));
        }
    }
    Items.prototype.query = function (params) {
        if (!params) {
            return this.items;
        }
        return this.items.filter(function (item) {
            for (var key in params) {
                var field = item[key];
                if (typeof field == 'string' && field.toLowerCase().indexOf(params[key].toLowerCase()) >= 0) {
                    return item;
                }
                else if (field == params[key]) {
                    return item;
                }
            }
            return null;
        });
    };
    Items.prototype.add = function (item) {
        this.items.push(item);
    };
    Items.prototype.delete = function (item) {
        this.items.splice(this.items.indexOf(item), 1);
    };
    Items = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [])
    ], Items);
    return Items;
}());

//# sourceMappingURL=items.js.map

/***/ }),

/***/ 260:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return StepChallengeService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_rxjs_add_operator_toPromise__ = __webpack_require__(146);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_rxjs_add_operator_toPromise___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_0_rxjs_add_operator_toPromise__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__api_api__ = __webpack_require__(71);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var StepChallengeService = /** @class */ (function () {
    function StepChallengeService(api) {
        this.api = api;
    }
    StepChallengeService.prototype.GetUserOverallProgressOfStepChallenge = function (accountInfo) {
        var seq = this.api
            .post("api/WellnessAPI/GetUserOverallProgressOfStepChallenge", accountInfo)
            .share();
        seq.subscribe(function (res) { }, function (err) {
            console.error("ERROR", err);
        });
        return seq;
    };
    StepChallengeService.prototype.GetUserStepChallengeProgressOverviewCount = function (accountInfo) {
        var seq = this.api
            .post("api/WellnessAPI/GetUserStepChallengeProgressOverviewCount", accountInfo)
            .share();
        seq.subscribe(function (res) { }, function (err) {
            console.error("ERROR", err);
        });
        return seq;
    };
    StepChallengeService.prototype.SaveLogStepsNumber = function (accountInfo) {
        var seq = this.api
            .post("api/WellnessAPI/SaveLogStepsNumber", accountInfo)
            .share();
        seq.subscribe(function (res) { }, function (err) {
            console.error("ERROR", err);
        });
        return seq;
    };
    StepChallengeService.prototype.GetLoggedStepsActivitiesByDate = function (accountInfo) {
        var seq = this.api
            .post("api/WellnessAPI/GetLoggedStepsActivitiesByDate", accountInfo)
            .share();
        seq.subscribe(function (res) { }, function (err) {
            console.error("ERROR", err);
        });
        return seq;
    };
    StepChallengeService.prototype.DeleteLoggedStepsActivities = function (accountInfo) {
        var seq = this.api
            .post("api/WellnessAPI/DeleteLoggedStepsActivities", accountInfo)
            .share();
        seq.subscribe(function (res) { }, function (err) {
            console.error("ERROR", err);
        });
        return seq;
    };
    StepChallengeService.prototype.SearchStepsActivities = function (accountInfo) {
        var seq = this.api
            .post("api/WellnessAPI/SearchStepsActivities", accountInfo)
            .share();
        seq.subscribe(function (res) { }, function (err) {
            console.error("ERROR", err);
        });
        return seq;
    };
    StepChallengeService.prototype.SaveStepsActivitiesData = function (accountInfo) {
        var seq = this.api
            .post("api/WellnessAPI/SaveStepsActivitiesData", accountInfo)
            .share();
        seq.subscribe(function (res) { }, function (err) {
            console.error("ERROR", err);
        });
        return seq;
    };
    StepChallengeService.prototype.GetOverviewAndGraphicViewData = function (accountInfo) {
        var seq = this.api
            .post("api/WellnessAPI/GetOverviewAndGraphicViewData", accountInfo)
            .share();
        seq.subscribe(function (res) { }, function (err) {
            console.error("ERROR", err);
        });
        return seq;
    };
    StepChallengeService.prototype.GetUserStepChallengeHistory = function (accountInfo) {
        var seq = this.api
            .post("api/WellnessAPI/GetUserStepChallengeHistory", accountInfo)
            .share();
        seq.subscribe(function (res) { }, function (err) {
            console.error("ERROR", err);
        });
        return seq;
    };
    StepChallengeService.prototype.DeletePreviousChallengeHistory = function (accountInfo) {
        var seq = this.api
            .post("api/WellnessAPI/DeletePreviousChallengeHistory", accountInfo)
            .share();
        seq.subscribe(function (res) { }, function (err) {
            console.error("ERROR", err);
        });
        return seq;
    };
    StepChallengeService.prototype.ResetUserStepChallenge = function (accountInfo) {
        var seq = this.api
            .post("api/WellnessAPI/ResetUserStepChallenge", accountInfo)
            .share();
        seq.subscribe(function (res) { }, function (err) {
            console.error("ERROR", err);
        });
        return seq;
    };
    StepChallengeService.prototype.StartUserNewStepChallenge = function (accountInfo) {
        var seq = this.api
            .post("api/WellnessAPI/StartUserNewStepChallenge", accountInfo)
            .share();
        seq.subscribe(function (res) { }, function (err) {
            console.error("ERROR", err);
        });
        return seq;
    };
    StepChallengeService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_1__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_2__api_api__["a" /* Api */]])
    ], StepChallengeService);
    return StepChallengeService;
}());

//# sourceMappingURL=stepschallenge.js.map

/***/ }),

/***/ 261:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return MyHraPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__my_wellness_wallet_hra_qa_hra_qa__ = __webpack_require__(262);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__providers__ = __webpack_require__(15);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};




/**
 * Generated class for the MyHraPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
var MyHraPage = /** @class */ (function () {
    function MyHraPage(navCtrl, navParams, hraApi, alertCtl, loading) {
        var _this = this;
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.hraApi = hraApi;
        this.alertCtl = alertCtl;
        this.loading = loading;
        this.account = {
            deviceid: "",
            SecretToken: "",
            FirstName: "",
            LastName: ""
        };
        this.rewardpoints = 0;
        this.profilepic = "assets/img/avtar.jpg";
        this.bio_age = "0";
        this.mhrs_score = "0";
        this.HraDetailSections = { lst_hra_section: [] };
        this.resetFlg = true;
        this.loader = this.loading.create({
            content: "Please wait..."
        });
        this.loader.present().then(function () {
            //this.loadUserInfo();
            //});
            _this.GetMyHraDetail();
        });
        this.RiskReportDetails = JSON.parse(localStorage.getItem("RiskReportDetails"));
        if (this.RiskReportDetails && this.RiskReportDetails.length > 0) {
            this.presentConfirm();
        }
        else {
            console.log("Reset Form");
        }
    }
    // loadUserInfo() {
    //   this.account.FirstName = localStorage.getItem("FirstName");
    //   this.account.LastName = localStorage.getItem("LastName");
    //   this.rewardpoints = parseInt(localStorage.getItem("RewardPoint"));
    //   this.profilepic = localStorage.getItem("ProfileImage");
    //   //this.bio_age = localStorage.getItem("bio_age");
    //   //this.mhrs_score = localStorage.getItem("mhrs_score");
    //   // this.loader.dismiss();
    // }
    MyHraPage.prototype.presentConfirm = function () {
        var _this = this;
        var alert = this.alertCtl.create({
            message: "You did not complete the HRA during your last session, Would you like to continue from where left off?",
            cssClass: "action-sheets-basic-page",
            buttons: [
                {
                    text: "Resume",
                    role: "cancel",
                    handler: function () {
                        _this.resetFlg = false;
                    }
                },
                {
                    text: "Beginning",
                    handler: function () {
                        _this.resetFlg = true;
                    }
                }
            ]
        });
        alert.present();
    };
    MyHraPage.prototype.ionViewCanEnter = function () {
        console.log('page view enter');
    };
    MyHraPage.prototype.ionViewDidLoad = function () {
        console.log("ionViewDidLoad MyHraPage");
    };
    MyHraPage.prototype.presentAlert = function (msg) {
        return __awaiter(this, void 0, void 0, function () {
            var alert;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.alertCtl.create({
                            message: msg,
                            cssClass: "action-sheets-basic-page",
                            buttons: [
                                {
                                    text: "OK",
                                    handler: function () { }
                                }
                            ]
                        })];
                    case 1:
                        alert = _a.sent();
                        return [4 /*yield*/, alert.present()];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    MyHraPage.prototype.GetMyHraDetail = function () {
        var _this = this;
        this.account.deviceid = localStorage.getItem("deviceid");
        this.account.SecretToken = localStorage.getItem("SecretToken");
        this.loader.present().then(function () {
            _this.hraApi.GetHraSections(_this.account).subscribe(function (resp) {
                _this.loader.dismiss();
                _this.HraDetailSections = resp;
                console.log("HRA-response : ", _this.HraDetailSections);
                localStorage.setItem("HraDetailSections", JSON.stringify(_this.HraDetailSections.lst_hra_section));
                localStorage.setItem("RiskReportDetails", JSON.stringify(_this.HraDetailSections.RiskReportDetails));
            }, function (err) {
                _this.loader.dismiss();
                // Unable to log in
                //this.presentAlert("Server Message- Get HRA Sections : "+err.error.SystemMessage);
                _this.presentAlert("Server Message - Get HRA Sections " + JSON.stringify(err));
                //this.navCtrl.push(LoginPage);
            });
        });
    };
    MyHraPage.prototype.goToHraDetail = function (cardData, nextCardIndex) {
        if (this.IsCheckAllow(cardData, true)) {
            this.navCtrl.push(__WEBPACK_IMPORTED_MODULE_2__my_wellness_wallet_hra_qa_hra_qa__["a" /* HraQaPage */], {
                cardData: cardData,
                nextCardIndex: nextCardIndex,
                resetFlg: this.resetFlg
            });
        }
        else {
            this.presentAlert("Invalid steps!");
        }
    };
    MyHraPage.prototype.IsCheckAllow = function (cardData, allowNext) {
        if (this.HraDetailSections.lstCompletedSection.indexOf(cardData.report_section_num) !== -1 ||
            (this.HraDetailSections.lstCompletedSection.indexOf(cardData.report_section_num - 1) !== -1 && allowNext)
            || cardData.report_section_num == 16) {
            return true;
        }
        else {
            return false;
        }
    };
    MyHraPage = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: "page-my-hra",template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\my-hra\my-hra.html"*/'<ion-header>\n  <ion-navbar color="primary">\n    <ion-title>My HRA</ion-title>\n       <button ion-button menuToggle>\n        <ion-icon name="menu"></ion-icon>\n      </button> \n  </ion-navbar>\n</ion-header>\n<ion-content>\n  <div class="title">\n  <h3>My Wellness Wallet</h3>    \n  <p>Please complete the HRA process</p>\n</div> \n  <ion-card (click)="goToHraDetail(cardData,(i+1))" *ngFor="let cardData of HraDetailSections.lst_hra_section; let i = index;" [ngClass] = "(HraDetailSections.lstCompletedSection.indexOf(cardData.report_section_num) !== -1) ? \'active\' : \'\' ">\n    <ion-icon *ngIf="HraDetailSections.lstCompletedSection.indexOf(cardData.report_section_num) !== -1" class="list-icon" color="primary" name="checkmark-circle"></ion-icon>\n    <ion-icon *ngIf="HraDetailSections.lstCompletedSection.indexOf(cardData.report_section_num) === -1" class="list-icon" name="radio-button-off" ></ion-icon>\n    <button menuClose ion-item [disabled]="IsCheckAllow(cardData, false) ? false : true">\n     {{cardData.description}}\n    </button>\n    \n  </ion-card>\n</ion-content>'/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\my-hra\my-hra.html"*/
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavParams"],
            __WEBPACK_IMPORTED_MODULE_3__providers__["b" /* HraService */],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["AlertController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["LoadingController"]])
    ], MyHraPage);
    return MyHraPage;
}());

//# sourceMappingURL=my-hra.js.map

/***/ }),

/***/ 262:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return HraQaPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__providers__ = __webpack_require__(15);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_forms__ = __webpack_require__(18);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__hra_result_hra_result__ = __webpack_require__(263);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};





var HraQaPage = /** @class */ (function () {
    function HraQaPage(navCtrl, navParams, hraApi, alertCtl, loadingCtrl, form) {
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.hraApi = hraApi;
        this.alertCtl = alertCtl;
        this.loadingCtrl = loadingCtrl;
        this.form = form;
        this.account = {
            deviceid: "",
            SecretToken: "",
            report_section_num: 0,
            lst_hra_response: []
        };
        this.HraQuestion = [{ hraAnswer: [] }];
        this.HraDetailQuestionList = { lst_hra_question: [] };
        this.FormSectionData = [{ answer: 0 }];
        this.resetFlg = false;
        this.hideQuestion = [{ check: true }];
        this.IsScrolled = true;
        this.IsFirstScrolled = false;
        this.cardData = this.navParams.get("cardData");
        this.nextCardIndex = this.navParams.get("nextCardIndex");
        this.resetFlg = this.navParams.get("resetFlg");
        this.RiskReportDetails = JSON.parse(localStorage.getItem("RiskReportDetails"));
        this.HraDetailSections = JSON.parse(localStorage.getItem("HraDetailSections"));
        // if (this.HraDetailSections.length > 0) {
        // }
        if (this.resetFlg) {
            this.FormSectionData = [{ answer: 0 }];
        }
        else {
            for (var Data in this.RiskReportDetails) {
                var temp = {
                    answer: this.RiskReportDetails[Data].answer_num || 0
                };
                var temBool = { check: false };
                this.FormSectionData[this.RiskReportDetails[Data].question_num] = temp;
                this.hideQuestion[this.RiskReportDetails[Data].question_num] = temBool;
            }
        }
        this.zone = new __WEBPACK_IMPORTED_MODULE_0__angular_core__["NgZone"]({ enableLongStackTrace: false });
        console.log(this.nextCardIndex, "nextCardIndex");
        if (this.nextCardIndex == 1) {
            this.IsScrolled = false;
        }
        this.loader = this.loadingCtrl.create({
            content: "Please wait..."
        });
        this.RiskReportDetails = JSON.parse(localStorage.getItem("RiskReportDetails"));
    }
    HraQaPage_1 = HraQaPage;
    HraQaPage.prototype.ionViewDidLoad = function () {
        console.log("ionViewDidLoad HraBodyPage");
        this.GetSectionDataBySection();
    };
    HraQaPage.prototype.ngAfterViewInit = function () {
        var _this = this;
        this.ContentTemplate.ionScrollEnd.subscribe(function (data) {
            var div_scrollElement = document.getElementById('div_scrollElement');
            //console.log(data, 'scrolling');
            var scrollTop = _this.ContentTemplate.scrollTop;
            var dimensions = _this.ContentTemplate.getContentDimensions();
            var contentHeight = dimensions.contentHeight;
            _this.WindowHeight = contentHeight;
            var scrollHeight = dimensions.scrollHeight;
            // console.log(dimensions, 'scrolling');
            if (div_scrollElement !== null && div_scrollElement !== undefined) {
                div_scrollElement.hidden = true;
            }
            //console.log( (scrollTop + contentHeight + 20)); 
            if ((scrollTop + contentHeight + 30) > scrollHeight) {
                _this.zone.run(function () {
                    _this.IsScrolled = false;
                });
            }
        });
    };
    HraQaPage.prototype.presentAlert = function (msg) {
        return __awaiter(this, void 0, void 0, function () {
            var alert;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.alertCtl.create({
                            message: msg,
                            cssClass: "action-sheets-basic-page",
                            buttons: [
                                {
                                    text: "OK",
                                    handler: function () { }
                                }
                            ]
                        })];
                    case 1:
                        alert = _a.sent();
                        return [4 /*yield*/, alert.present()];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    HraQaPage.prototype.presentAlertRedirect = function (msg) {
        return __awaiter(this, void 0, void 0, function () {
            var alert;
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.alertCtl.create({
                            message: msg,
                            cssClass: "action-sheets-basic-page"
                        })];
                    case 1:
                        alert = _a.sent();
                        // code for hide
                        return [4 /*yield*/, alert.present().then(function () {
                                // hide the alert after 2 sec
                                setTimeout(function () {
                                    alert.dismiss();
                                    _this.goToHraDetail(_this.HraDetailSections[_this.nextCardIndex], _this.nextCardIndex + 1);
                                }, 2000);
                            })];
                    case 2:
                        // code for hide
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    HraQaPage.prototype.presentAlertLastRedirect = function (msg) {
        return __awaiter(this, void 0, void 0, function () {
            var alert;
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.alertCtl.create({
                            message: msg,
                            cssClass: "action-sheets-basic-page"
                        })];
                    case 1:
                        alert = _a.sent();
                        return [4 /*yield*/, alert.present().then(function () {
                                // hide the alert after 2 sec
                                setTimeout(function () {
                                    alert.dismiss();
                                    _this.navCtrl.setRoot(__WEBPACK_IMPORTED_MODULE_4__hra_result_hra_result__["a" /* MyHraResultPage */]);
                                }, 2000);
                            })];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    HraQaPage.prototype.submitLastSection = function () {
        var _this = this;
        var ansData = [];
        for (var Data in this.HraDetailQuestionList.lst_hra_question) {
            var questionNumber = this.HraDetailQuestionList.lst_hra_question[Data]
                .question_num;
            if (this.FormSectionData[questionNumber]) {
                ansData.push({
                    QuestionNum: questionNumber,
                    AnswerNum: this.FormSectionData[questionNumber].answer
                });
            }
        }
        this.account.deviceid = localStorage.getItem("deviceid");
        this.account.SecretToken = localStorage.getItem("SecretToken");
        this.account.lst_hra_response = ansData;
        this.loader = this.loadingCtrl.create({
            content: "Please wait..."
        });
        this.loader.present().then(function () {
            _this.hraApi.SaveHRAResponse(_this.account).subscribe(function (res) {
                console.log(res);
                _this.loader.dismiss();
                _this.presentAlertLastRedirect(_this.cardData.description + " section data saved successfully.");
            }, function (err) {
                _this.loader.dismiss();
                _this.presentAlert("Save HRA Response - " +
                    JSON.stringify(err)
                //err.error.SystemMessage
                );
            });
        });
    };
    HraQaPage.prototype.submitSection = function () {
        var _this = this;
        var ansData = [];
        for (var Data in this.HraDetailQuestionList.lst_hra_question) {
            var questionNumber = this.HraDetailQuestionList.lst_hra_question[Data]
                .question_num;
            if ((questionNumber == 277 || questionNumber == 278) &&
                this.FormSectionData[questionNumber]) {
                console.log("current question ", questionNumber);
                ansData.push({
                    QuestionNum: questionNumber,
                    AnswerNum: this.GetBodyMetricsAnswerValue(questionNumber, this.FormSectionData[questionNumber].answer)
                });
            }
            else if (this.FormSectionData[questionNumber]) {
                ansData.push({
                    QuestionNum: questionNumber,
                    AnswerNum: this.FormSectionData[questionNumber].answer
                });
            }
        }
        if (ansData.length > 0) {
            this.account.deviceid = localStorage.getItem("deviceid");
            this.account.SecretToken = localStorage.getItem("SecretToken");
            this.account.lst_hra_response = ansData;
            console.log(ansData, "ansData");
            this.loader = this.loadingCtrl.create({
                content: "Please wait..."
            });
            this.loader.present().then(function () {
                _this.hraApi.SaveHRAResponse(_this.account).subscribe(function (res) {
                    _this.loader.dismiss();
                    _this.presentAlertRedirect(_this.cardData.description + " section data saved successfully.");
                }, function (err) {
                    _this.loader.dismiss();
                    console.error("ERROR", err);
                    _this.presentAlert("Save HRA Response - " +
                        JSON.stringify(err)
                    //err.error.SystemMessage
                    );
                });
            });
        }
        else {
            this.presentAlert(this.cardData.description + " Answer not selected ");
        }
    };
    HraQaPage.prototype.goToHraDetail = function (cardData, nextCardIndex) {
        this.navCtrl.push(HraQaPage_1, {
            cardData: cardData,
            nextCardIndex: nextCardIndex
        });
    };
    HraQaPage.prototype.GetSectionDataBySection = function () {
        var _this = this;
        this.account.deviceid = localStorage.getItem("deviceid");
        this.account.SecretToken = localStorage.getItem("SecretToken");
        this.account.report_section_num = this.cardData.report_section_num;
        this.loader.present().then(function () {
            _this.hraApi.GetHRAQuestionDetails(_this.account).subscribe(function (resp) {
                _this.loader.dismiss();
                _this.HraDetailQuestionList = resp;
                // console.log("resp", resp);
                for (var Data in _this.HraDetailQuestionList.lst_hra_question) {
                    var questionNumber = _this.HraDetailQuestionList.lst_hra_question[Data].question_num;
                    if (!_this.FormSectionData[questionNumber]) {
                        var temp = {
                            answer: _this.HraDetailQuestionList.lst_hra_question[Data]
                                .hra_answer[0].answer_num
                        };
                        _this.FormSectionData[questionNumber] = temp;
                        var temBool = { check: false };
                        _this.hideQuestion[questionNumber] = temBool;
                    }
                    _this.GetCorrectAnswer(Data, questionNumber);
                }
                _this.InitialSupressQuestion();
            }, function (err) {
                _this.loader.dismiss();
                // Unable to log in
                _this.presentAlert("Get HRA Question Details " +
                    JSON.stringify(err)
                //err.error.SystemMessage
                );
            });
        });
    };
    HraQaPage.prototype.GetCorrectAnswer = function (Data, queNo) {
        var questiondetails = this.HraDetailQuestionList.lst_hra_question.filter(function (q) { return q.question_num == queNo; });
        if (questiondetails !== null && questiondetails.length > 0) {
            var ansdefault = questiondetails[0].hra_answer.filter(function (a) { return a.default_answer == true; });
            if (ansdefault !== null) {
                this.FormSectionData[queNo].answer = ansdefault[0].answer_num;
            }
            else {
                this.FormSectionData[queNo].answer = 1;
            }
        }
        else {
        }
    };
    HraQaPage.prototype.InitialSupressQuestion = function () {
        console.log(this.HraDetailQuestionList, "this.HraDetailQuestionList ");
        var supressQuestionData = this.HraDetailQuestionList
            .lst_hra_question_suppress;
        var uniqueSupressQues = supressQuestionData
            .map(function (item) { return item.question_num; })
            .filter(function (value, index, self) { return self.indexOf(value) === index; });
        console.log(uniqueSupressQues, "uniqueSupressQues");
        var _loop_1 = function (que) {
            ansdefault = this_1.GetAnswerValue(uniqueSupressQues[que]);
            var quesSupressInit = supressQuestionData.filter(function (q) {
                return q.question_num == uniqueSupressQues[que] && q.answer_num == ansdefault;
            });
            if (quesSupressInit !== null) {
                for (var supque in quesSupressInit) {
                    this_1.hideQuestion[quesSupressInit[supque].question_num_suppress] = {
                        check: true
                    };
                }
            }
        };
        var this_1 = this, ansdefault;
        for (var que in uniqueSupressQues) {
            _loop_1(que);
        }
    };
    HraQaPage.prototype.supressQuestion = function (qdata) {
        console.log(qdata, " qdata ");
        console.log(this.HraDetailQuestionList, "this.HraDetailQuestionList ");
        var supressQuestionData = this.HraDetailQuestionList
            .lst_hra_question_suppress;
        // console.log(supressQuestionData, ' supressQuestionData ')
        var uniqueSupressQues = supressQuestionData.filter(function (q) { return q.question_num == qdata.question_num; });
        // console.log(uniqueSupressQues, 'uniqueSupressQues');
        var ansdefault = this.FormSectionData[qdata.question_num].answer;
        //console.log(this.FormSectionData, ' FormSectionData ')
        //console.log(ansdefault, ' ansdefault  supressQuestion ')
        // show all
        for (var que in uniqueSupressQues) {
            this.hideQuestion[uniqueSupressQues[que].question_num_suppress] = {
                check: false
            };
        }
        // now hide selected
        for (var que in uniqueSupressQues) {
            if (uniqueSupressQues[que].answer_num == ansdefault) {
                this.hideQuestion[uniqueSupressQues[que].question_num_suppress] = {
                    check: true
                };
            }
        }
    };
    HraQaPage.prototype.GetAnswerValue = function (queNo) {
        console.log(queNo, "queNo");
        var ansValue = 1;
        var questiondetails = this.HraDetailQuestionList.lst_hra_question.filter(function (q) { return q.question_num == queNo; });
        if (questiondetails !== null && questiondetails.length > 0) {
            var ansdefault = questiondetails[0].hra_answer.filter(function (a) { return a.default_answer == true; });
            ansValue = ansdefault !== null ? ansdefault[0].answer_num : 1;
            return ansValue;
        }
    };
    HraQaPage.prototype.GetBodyMetricsAnswerValue = function (queNo, answer_num) {
        console.log(queNo, "GetBodyMetricsAnswerValue");
        var ansValue = answer_num;
        var questiondetails = this.HraDetailQuestionList.lst_hra_question.filter(function (q) { return q.question_num == queNo; });
        if (questiondetails !== null && questiondetails.length > 0) {
            var ansoptions = questiondetails[0].hra_answer.filter(function (a) { return a.answer_num == answer_num; });
            ansValue = ansoptions[0].answer;
            return ansValue;
        }
        else {
            return answer_num;
        }
    };
    HraQaPage.prototype.scrollEnd = function () {
        this.ContentTemplate.scrollTo(this.WindowHeight, 300); //scrollToBottom();
        var div_scrollElement = document.getElementById('div_scrollElement');
        div_scrollElement.hidden = true;
    };
    var HraQaPage_1;
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])(__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["Content"]),
        __metadata("design:type", __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["Content"])
    ], HraQaPage.prototype, "ContentTemplate", void 0);
    HraQaPage = HraQaPage_1 = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: "page-hra-qa",template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\my-wellness-wallet\hra-qa\hra-qa.html"*/'<ion-content #ContentTemplate>\n  <div class="splashbg">\n    <ion-header>\n      <ion-navbar>\n        <ion-title>Health Risk Assessment\n          (HRA) </ion-title>\n      </ion-navbar>\n    </ion-header>\n    <div class="subtitle">\n      <p>You can pause and return to complete the HRA, however, you must complete the entire HRA before it can be\n        processed </p>\n    </div>\n  </div>\n  <ion-card>\n    <ion-card-header>\n      {{cardData.description}}\n    </ion-card-header>\n    <ion-card-content>\n      <div *ngFor="let wdata of HraDetailQuestionList.lst_hra_question ; index as i">\n        <div [hidden]="hideQuestion[wdata.question_num].check">\n          <div *ngIf="wdata.answer_type == \'Dropdown\'">\n            <ion-list-header> {{i + 1}} . {{wdata.question}} </ion-list-header>\n            <ion-select [(ngModel)]="FormSectionData[wdata.question_num].answer" placeholder="Select Value"\n              (ionChange)="supressQuestion(wdata)">\n              <ion-option *ngFor="let qanswer of wdata.hra_answer; index as l" value="{{qanswer.answer_num}}">\n                {{qanswer.answer}}</ion-option>\n            </ion-select>\n          </div>\n        </div>\n        <div *ngIf="wdata.answer_type == \'Objective\'">\n          <ion-list radio-group [(ngModel)]="FormSectionData[wdata.question_num].answer"\n            [hidden]="hideQuestion[wdata.question_num].check">\n            <ion-list-header>\n              {{ i + 1 }}. {{wdata.question}}\n            </ion-list-header>\n            <ion-item *ngFor="let qanswer of wdata.hra_answer; index as l">\n              <ion-radio value="{{qanswer.answer_num}}" (ionSelect)="supressQuestion(wdata)"></ion-radio>\n              <ion-label>{{qanswer.answer}}</ion-label>\n            </ion-item>\n          </ion-list>\n        </div>\n      </div>\n    </ion-card-content>\n  </ion-card>\n  \n</ion-content>\n<ion-footer>\n  <div class="bot-scroll" *ngIf="IsScrolled" id="div_scrollElement" (click)="scrollEnd();">\n    <img src="assets/img/mouse.png" class="scrollicon">\n  </div>\n  <ion-toolbar>\n\n    <button *ngIf="this.HraDetailSections.length != nextCardIndex" ion-button color="primary" class="big-btn"\n      (click)="submitSection()" [disabled]="IsScrolled">Next</button>\n    <button *ngIf="this.HraDetailSections.length == nextCardIndex" ion-button color="primary" class="big-btn "\n      (click)="submitLastSection()" [disabled]="IsScrolled">Finish</button>\n  </ion-toolbar>\n</ion-footer>'/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\my-wellness-wallet\hra-qa\hra-qa.html"*/
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavParams"],
            __WEBPACK_IMPORTED_MODULE_2__providers__["b" /* HraService */],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["AlertController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["LoadingController"],
            __WEBPACK_IMPORTED_MODULE_3__angular_forms__["NgForm"]])
    ], HraQaPage);
    return HraQaPage;
}());

//# sourceMappingURL=hra-qa.js.map

/***/ }),

/***/ 263:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return MyHraResultPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__mywellnessplan_mywellnessplan__ = __webpack_require__(264);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__providers__ = __webpack_require__(15);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};




/**
 * Generated class for the MyHra result Page page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
var MyHraResultPage = /** @class */ (function () {
    function MyHraResultPage(navCtrl, navParams, alertCtl, loading, hraApi, events, userService) {
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.alertCtl = alertCtl;
        this.loading = loading;
        this.hraApi = hraApi;
        this.events = events;
        this.userService = userService;
        this.account = {
            deviceid: "",
            SecretToken: "",
            RiskReportNum: 0,
            Accept: false
        };
        this.resetFlg = true;
        this.userName =
            localStorage.getItem("FirstName") +
                " " +
                localStorage.getItem("LastName");
        this.loader = this.loading.create({
            content: "Please wait..."
        });
        localStorage.setItem("IsHRACompleted", "true");
        this.GetMyHraResults();
    }
    MyHraResultPage.prototype.ionViewDidLoad = function () {
        console.log("ionViewDidLoad My HRA confirm Page");
    };
    MyHraResultPage.prototype.GetMyHraResults = function () {
        var _this = this;
        this.account.deviceid = localStorage.getItem("deviceid");
        this.account.SecretToken = localStorage.getItem("SecretToken");
        this.loader.present().then(function () {
            //this.loader.dismiss();
            _this.hraApi.GetRiskReportNum(_this.account).subscribe(function (resp1) {
                //debugger;
                //console.log(resp1);
                console.log(_this.hraApi.hraRiskReport.RiskReportNum);
                _this.account.RiskReportNum = parseInt(_this.hraApi.hraRiskReport.RiskReportNum); //localStorage.getItem('riskreportnum'));
                _this.hraApi.GetHraReport(_this.account).subscribe(function (resp) {
                    _this.loader.dismiss();
                    console.log(_this.hraApi.hraResults);
                    _this.RiskReportDetails = _this.hraApi.hraResults.UserConditions;
                    //debugger;
                    console.log({ LstReport: _this.RiskReportDetails });
                }, function (err) {
                    _this.loader.dismiss();
                    // Unable to log in
                    _this.presentAlert("Server Message - Get Identified Conditions API: " + JSON.stringify(err));
                    //this.navCtrl.push(LoginPage);
                });
            }, function (err) {
                _this.loader.dismiss();
                // Unable to log in
                //this.presentAlert(err.error.SystemMessage);
                //this.navCtrl.push(LoginPage);
            });
        });
    };
    MyHraResultPage.prototype.submitConfirm = function () {
        console.log(this.account.Accept);
        if (this.account.Accept) {
            this.presentAlertRedirect("HRA confirm section data saved successfully.");
        }
        else {
            this.presentAlert("Please tick check box to accept");
        }
    };
    MyHraResultPage.prototype.presentAlertRedirect = function (msg) {
        return __awaiter(this, void 0, void 0, function () {
            var alert;
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.alertCtl.create({
                            message: msg,
                            cssClass: "action-sheets-basic-page",
                        })];
                    case 1:
                        alert = _a.sent();
                        //await alert.present();
                        return [4 /*yield*/, alert.present().then(function () {
                                // hide the alert after 2 sec
                                _this.LoadHRAUserData();
                                setTimeout(function () {
                                    alert.dismiss();
                                    _this.navCtrl.setRoot(__WEBPACK_IMPORTED_MODULE_2__mywellnessplan_mywellnessplan__["a" /* MyWellnessPlanPage */]);
                                }, 1000);
                            })];
                    case 2:
                        //await alert.present();
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    MyHraResultPage.prototype.presentAlert = function (msg) {
        return __awaiter(this, void 0, void 0, function () {
            var alert;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.alertCtl.create({
                            message: msg,
                            cssClass: "action-sheets-basic-page",
                            buttons: [
                                {
                                    text: "OK",
                                    handler: function () { }
                                }
                            ]
                        })];
                    case 1:
                        alert = _a.sent();
                        return [4 /*yield*/, alert.present()];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    MyHraResultPage.prototype.LoadHRAUserData = function () {
        var _this = this;
        var userAcc = {
            DeviceId: localStorage.getItem("deviceid"),
            SecretToken: localStorage.getItem("SecretToken")
        };
        this.userService.getUserData(userAcc).subscribe(function (res) {
            console.log(res, "getUserData");
            var data = {};
            localStorage.setItem("RewardPoint", res.RewardPoint);
            localStorage.setItem("bio_age", res.bio_age);
            localStorage.setItem("mhrs_score", res.mhrs_score);
            // Read all the data;
            data["FirstName"] = localStorage.getItem("FirstName");
            data["LastName"] = localStorage.getItem("LastName");
            data["ProfileImage"] = localStorage.getItem("ProfileImage");
            data["Gender"] = localStorage.getItem("Gender");
            data["Height"] = localStorage.getItem("Height");
            data["BirthDate"] = localStorage.getItem("BirthDate");
            data["RewardPoint"] = localStorage.getItem("RewardPoint");
            data["bio_age"] = localStorage.getItem("bio_age");
            data["mhrs_score"] = localStorage.getItem("mhrs_score");
            console.log("Event published : " + data);
            _this.events.publish("user:created", data);
        });
    };
    MyHraResultPage = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: "page-my-hra-result",template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\my-wellness-wallet\hra-result\hra-result.html"*/'<ion-header>\n\n\n\n  <ion-navbar color="primary">\n\n    <ion-title>Consent</ion-title>\n\n    <!--\n\n    <ion-item>\n\n      <ion-avatar item-start>\n\n        <img src="assets/img/avtar.jpg">\n\n      </ion-avatar>\n\n      <h2>{{account.FirstName}} {{account.LastName}}</h2>\n\n      <p>\n\n        <ion-icon name="medal"></ion-icon>296 Points\n\n      </p>\n\n    </ion-item>\n\n    <button ion-button menuToggle>\n\n      <ion-icon name="menu"></ion-icon>\n\n    </button> -->\n\n  </ion-navbar>\n\n</ion-header>\n\n<ion-content>\n\n  <!-- <div class="title">\n\n    <h3>My HRA Wallet</h3>\n\n    <p>HRA process - Confirmation\n\n\n\n    </p>\n\n  </div> -->\n\n  <div class="div-consent">\n\n    <p>\n\n      Congratulations {{userName}} on completing your Meschino Health Risk Assessment.\n\n    </p>\n\n    <p>\n\n      This report identifies important health risks as well as lifestyle strategies to help manage health issues of\n\n      importance to you.\n\n    </p>\n\n    <p>\n\n      This report has identified the following health issue(s) based on your responses.\n\n    </p>\n\n    <div>\n\n\n\n      <ul>\n\n        <li *ngFor="let rpt of RiskReportDetails ; index as i">\n\n          <img src="assets/img/arrow.png"> <label class="col-lg-12 mrg-top5"> {{rpt.description}} </label>\n\n        </li>\n\n      </ul>\n\n      <div>\n\n\n\n        <div class="col-lg-12" id="divHRAMessage1" style="padding: 20px 0 5px 0;">\n\n          <p>Receive the latest research updates on the prevention and management of key health issues, along with Dr.\n\n            Meschino\'s short “Will Power Moment” videos, explaining proven strategies to keep you on track and\n\n            motivated.\n\n          </p>\n\n        </div>\n\n\n\n        <ion-item>\n\n          <ion-label>I accept that Meschino Health & Wellness will send me valuable information described in the above\n\n            paragraph.</ion-label>\n\n          <ion-checkbox [(ngModel)]="account.Accept" name="Accept"></ion-checkbox>\n\n        </ion-item>\n\n\n\n      </div>\n\n    </div>\n\n  </div>\n\n</ion-content>\n\n<ion-footer>\n\n  <ion-toolbar>\n\n\n\n    <button ion-button color="primary" class="big-btn" (click)="submitConfirm()">CONFIRM CONSENT</button>\n\n  </ion-toolbar>\n\n\n\n  <!-- <p class="imp-note">\n\n        <b>Important Note:</b>&nbsp;By opting in to our communications you will begin receiving emails with valuable\n\n        information on health and nutrition from Dr. Meschino. We respect your privacy - we do not sell or\n\n        distribute information gathered in this form. Information gathered in this form is used solely for the\n\n        purpose of providing valuable information on health and nutrition from Dr. Meschino. Review our&nbsp;privacy\n\n        policy.\n\n      </p>\n\n      <div class="address">\n\n        <b>Address:</b> Meschino Health &amp; Wellness,\n\n        5500 Explorer Drive, 3rd Floor,\n\n        Mississauga, Ontario,\n\n        <br>\n\n        Canada L4W 5C7.\n\n        <b>Contact Number:</b>1-844-770-1075 x390 <b>Email:</b> <a\n\n          href="mailto:support@meschinowellness.com">support@meschinowellness.com</a>\n\n      </div>\n\n    -->\n\n</ion-footer>'/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\my-wellness-wallet\hra-result\hra-result.html"*/
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavParams"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["AlertController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["LoadingController"],
            __WEBPACK_IMPORTED_MODULE_3__providers__["b" /* HraService */],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["Events"],
            __WEBPACK_IMPORTED_MODULE_3__providers__["f" /* UserService */]])
    ], MyHraResultPage);
    return MyHraResultPage;
}());

//# sourceMappingURL=hra-result.js.map

/***/ }),

/***/ 264:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return MyWellnessPlanPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__providers__ = __webpack_require__(15);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__riskdetail_riskdetail__ = __webpack_require__(265);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__notification_notification__ = __webpack_require__(72);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};





/**
 * Generated class for the MyHraPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
var MyWellnessPlanPage = /** @class */ (function () {
    function MyWellnessPlanPage(navCtrl, navParams, alertCtl, loading, hraApi, userService, notificationService, events, menu) {
        var _this = this;
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.alertCtl = alertCtl;
        this.loading = loading;
        this.hraApi = hraApi;
        this.userService = userService;
        this.notificationService = notificationService;
        this.events = events;
        this.menu = menu;
        this.account = {
            deviceid: "",
            SecretToken: "",
            UserId: ""
        };
        this.notificationCount = ""; //localStorage.getItem("NotificationCount");
        this.menu.enable(true);
        this.loader = this.loading.create({
            content: "Please wait..."
        });
        localStorage.removeItem("RiskNumber");
        this.loader.present().then(function () {
            _this.loadInitialNotificationCount();
            _this.EmitterNotificationCount();
            _this.GetHealthRisks();
        });
    }
    MyWellnessPlanPage.prototype.ionViewDidLoad = function () {
        console.log("ionViewDidLoad MyWellnessPlanPage");
    };
    MyWellnessPlanPage.prototype.ionViewDidLeave = function () {
        console.log('ionViewDidLeave');
        //debugger;
    };
    MyWellnessPlanPage.prototype.ionViewWillLeave = function () {
        console.log('ionViewWillLeave');
        //debugger;
    };
    MyWellnessPlanPage.prototype.loadInitialNotificationCount = function () {
        var _this = this;
        var userAcc = {
            DeviceId: localStorage.getItem("deviceid"),
            SecretToken: localStorage.getItem("SecretToken"),
        };
        this.userService.GetPushNotificationCount(userAcc).subscribe(function (res) {
            var msgcount = res.Count;
            _this.notificationCount = msgcount > 0 ? msgcount : "";
            localStorage.setItem("notificationCount", _this.notificationCount);
        });
    };
    MyWellnessPlanPage.prototype.EmitterNotificationCount = function () {
        var _this = this;
        this.events.subscribe("PushNotification", function (PushNotification) {
            var msgcount = PushNotification.Count;
            _this.notificationCount = msgcount > 0 ? msgcount : "";
            localStorage.setItem("notificationCount", _this.notificationCount);
        });
    };
    MyWellnessPlanPage.prototype.GetHealthRisks = function () {
        var _this = this;
        this.account.deviceid = localStorage.getItem("deviceid");
        this.account.SecretToken = localStorage.getItem("SecretToken");
        this.account.UserId = localStorage.getItem("UserId");
        this.hraApi.GetMajorHealthRisks(this.account).subscribe(function (resp) {
            _this.loader.dismiss();
            _this.HealthRisks = _this.hraApi.MajorHealthRisks.RiskList;
        }, function (err) {
            _this.loader.dismiss();
            // Unable to log in
            _this.presentAlert("Server Message - Get Major Health Risks: " + JSON.stringify(err));
            //this.navCtrl.push(LoginPage);
        });
    };
    MyWellnessPlanPage.prototype.presentAlert = function (msg) {
        return __awaiter(this, void 0, void 0, function () {
            var alert;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.alertCtl.create({
                            message: msg,
                            cssClass: "action-sheets-basic-page",
                            buttons: [
                                {
                                    text: "OK",
                                    handler: function () { }
                                }
                            ]
                        })];
                    case 1:
                        alert = _a.sent();
                        return [4 /*yield*/, alert.present()];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    MyWellnessPlanPage.prototype.goToReportDetail = function (id) {
        localStorage.setItem("RiskNumber", id);
        this.navCtrl.push(__WEBPACK_IMPORTED_MODULE_3__riskdetail_riskdetail__["a" /* RiskDetailPage */]);
    };
    MyWellnessPlanPage.prototype.goNotification = function () {
        this.navCtrl.push(__WEBPACK_IMPORTED_MODULE_4__notification_notification__["a" /* NotificationPage */]);
    };
    MyWellnessPlanPage = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: "page-my-wellness-plan",template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\my-wellness-wallet\mywellnessplan\mywellnessplan.html"*/'<!--\n\n  Generated template for the DashboardPage page.\n\n\n\n  See http://ionicframework.com/docs/components/#navigation for more info on\n\n  Ionic pages and navigation.\n\n-->\n\n<ion-header>\n\n  <ion-navbar color="primary">\n\n    <button ion-button menuToggle icon-only>\n\n      <ion-icon name=\'menu\'></ion-icon>\n\n    </button>\n\n    <ion-title>Create My Wellness Plan</ion-title>\n\n    <ion-buttons end class="left-nav-buttons">\n\n      <button id="notification-button" ion-button icon-only (click)="goNotification()">\n\n        <ion-icon name="notifications">\n\n          <ion-badge id="notifications-badge" color="danger">{{notificationCount}}</ion-badge>\n\n        </ion-icon>\n\n      </button>\n\n    </ion-buttons>\n\n  </ion-navbar>\n\n</ion-header>\n\n<ion-content>\n\n  <div class="title">\n\n    <h3> Major Health Risk </h3>\n\n  </div>\n\n  <ion-card>\n\n    <ion-card-content>\n\n      <table class="tbl-hra">\n\n        <tr (click)="goToReportDetail(d.risk_page_num)" *ngFor="let d of HealthRisks">\n\n          <td style="padding: 10px">\n\n            {{d.title}}\n\n          </td>\n\n          <td>\n\n            <img src="assets/img/down_arrow.png" style="padding: 10px">\n\n          </td>\n\n        </tr>\n\n      </table>\n\n      <div style="height: 10px;">\n\n      </div>\n\n    </ion-card-content>\n\n  </ion-card>\n\n</ion-content>'/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\my-wellness-wallet\mywellnessplan\mywellnessplan.html"*/
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavParams"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["AlertController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["LoadingController"],
            __WEBPACK_IMPORTED_MODULE_2__providers__["b" /* HraService */],
            __WEBPACK_IMPORTED_MODULE_2__providers__["f" /* UserService */],
            __WEBPACK_IMPORTED_MODULE_2__providers__["c" /* NotificationService */],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["Events"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["MenuController"]])
    ], MyWellnessPlanPage);
    return MyWellnessPlanPage;
}());

//# sourceMappingURL=mywellnessplan.js.map

/***/ }),

/***/ 265:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return RiskDetailPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__providers__ = __webpack_require__(15);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__providers_settings_wellnessconstant__ = __webpack_require__(44);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};

//import { InAppBrowser } from 'ionic-native';



/**
 * Generated class for the MyHraPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
var RiskDetailPage = /** @class */ (function () {
    function RiskDetailPage(navCtrl, navParams, alertCtl, loading, hraApi) {
        var _this = this;
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.alertCtl = alertCtl;
        this.loading = loading;
        this.hraApi = hraApi;
        this.account = {
            deviceid: "",
            SecretToken: "",
            RiskNumber: 0,
            password: ""
        };
        this.GoalList = [];
        this.MeterList = [];
        this.siteUrl = __WEBPACK_IMPORTED_MODULE_3__providers_settings_wellnessconstant__["a" /* WellnessConstants */].App_Url;
        this.redirectURL = "";
        this.ShowRecommendedGoals = true;
        this.loader = this.loading.create({
            content: "Please wait..."
        });
        this.title = "Major Health Risk Detail";
        this.redirectURL =
            this.siteUrl +
                "account/SetGoalLogin?SecretToken=" +
                localStorage.getItem("SecretToken") +
                "&DeviceId=" +
                localStorage.getItem("deviceid") +
                "&tokenP=" +
                localStorage.getItem("Password") +
                "&GoalNum=";
        this.ShowRecommendedGoals = false;
        this.loader.present().then(function () {
            _this.GetHealthRiskDetails();
        });
    }
    RiskDetailPage.prototype.presentAlert = function (msg) {
        return __awaiter(this, void 0, void 0, function () {
            var alert;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.alertCtl.create({
                            message: msg,
                            cssClass: "action-sheets-basic-page",
                            buttons: [
                                {
                                    text: "OK",
                                    handler: function () { }
                                }
                            ]
                        })];
                    case 1:
                        alert = _a.sent();
                        return [4 /*yield*/, alert.present()];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    RiskDetailPage.prototype.ionViewDidLoad = function () {
        console.log("ionViewDidLoad RiskDetailPage");
    };
    RiskDetailPage.prototype.GetHealthRiskDetails = function () {
        var _this = this;
        this.account.deviceid = localStorage.getItem("deviceid");
        this.account.SecretToken = localStorage.getItem("SecretToken");
        this.account.RiskNumber = parseInt(localStorage.getItem("RiskNumber"));
        this.loader.present().then(function () {
            _this.hraApi.GetHealthRiskDetail(_this.account).subscribe(function (resp) {
                _this.loader.dismiss();
                _this.ShowRecommendedGoals = true;
                _this.HealthRiskDetail = _this.hraApi.HealthRiskDetail;
                _this.MeterList = _this.hraApi.HealthRiskDetail.MeterList;
                _this.GoalList = _this.hraApi.HealthRiskDetail.GoalList;
            }, function (err) {
                _this.loader.dismiss();
                // Unable to log in
                _this.presentAlert("Server Message - Get Health Risk Detail : " + JSON.stringify(err));
                //this.navCtrl.push(LoginPage);
            });
        });
    };
    RiskDetailPage.prototype.goToReportDetail = function (cardData, nextCardIndex) {
        //this.navCtrl.push();
    };
    RiskDetailPage.prototype.SetGoals = function (num) {
        if (localStorage.getItem("UserAccessLevel") !== null && localStorage.getItem("UserAccessLevel") !== "Full") {
            var alertP = this.alertCtl.create({
                message: "We are sorry, you do not have the permission to set goals. This features is available only on premium version. Please contact the administrator to upgrade.",
                cssClass: "action-sheets-basic-page",
                buttons: [
                    {
                        text: "Ok"
                    }
                ]
            });
            alertP.present();
        }
        else {
            var url_1 = this.redirectURL + num;
            var alertP = this.alertCtl.create({
                message: "You will be redirected to a browser. Do you want to continue?",
                cssClass: "action-sheets-basic-page",
                buttons: [
                    {
                        text: "Yes",
                        handler: function () {
                            console.log(url_1, "url");
                            //alert(url);
                            setTimeout(function () {
                                window.open(url_1, "_system", "location=yes");
                            }, 1000);
                        }
                    },
                    {
                        text: "No",
                        handler: function () {
                            //alertP.dismiss();
                        }
                    }
                ]
            });
            alertP.present();
        }
    };
    RiskDetailPage = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: "page-my-riskdetail",template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\my-wellness-wallet\riskdetail\riskdetail.html"*/'<!--\n\n  Generated template for the DashboardPage page.\n\n\n\n  See http://ionicframework.com/docs/components/#navigation for more info on\n\n  Ionic pages and navigation.\n\n-->\n\n\n\n<ion-content>\n\n    <ion-header>\n\n        <ion-navbar color="primary">\n\n            <ion-title>Create My Wellness Plan</ion-title>\n\n        </ion-navbar>\n\n\n\n        <div class="div-center">\n\n\n\n            <div class="div-content-risk">\n\n                <ion-title>{{title}}</ion-title>\n\n                <div>\n\n                    <div *ngFor="let m of MeterList">\n\n                        <!-- <ion-title>{{m.risk_description}}</ion-title>\n\n                        <img src="assets/img/title_line.png">\n\n                         -->\n\n                         <ion-card-header>\n\n                                {{m.risk_description}}\n\n                        </ion-card-header>\n\n                        <div class="div-cirle-center">\n\n                            <ng-container *ngIf="m.risk_score_num == 1">\n\n                                <div class="canvas__container"\n\n                                    style="background-image: url(\'../../../assets/img/red.png\') ;">\n\n                                    <img src="{{siteUrl}}images/new-layout/Risks Icons/{{m.image_location}}">\n\n                                </div>\n\n                            </ng-container>\n\n                            <ng-container *ngIf="m.risk_score_num == 2">\n\n                                <div class="canvas__container"\n\n                                    style="background-image: url(\'../../../assets/img/yellow.png\') ;">\n\n                                    <img src="{{siteUrl}}images/new-layout/Risks Icons/{{m.image_location}}">\n\n                                </div>\n\n                            </ng-container>\n\n                            <ng-container *ngIf="m.risk_score_num == 3">\n\n                                <div class="canvas__container"\n\n                                    style="background-image: url(\'../../../assets/img/green.png\') ;">\n\n                                    <img src="{{siteUrl}}images/new-layout/Risks Icons/{{m.image_location}}">\n\n                                </div>\n\n                            </ng-container>\n\n\n\n\n\n                        </div>\n\n                        <!-- <img style="height: 100px; width:100px; "\n\n                        src="http://stage-meschinowellness.azurewebsites.net/images/new-layout/Risks Icons/{{m.image_location}}"> -->\n\n                        <p>\n\n                            {{m.feedback}}\n\n                        </p>\n\n                        <ion-title> {{m.tool_phoneApp_description}} </ion-title>\n\n                        <span>\n\n                            {{m.last_updated | date: \'short\'}}\n\n                        </span>\n\n                        <hr>\n\n                    </div>\n\n                    <div class="div-goals" *ngIf="ShowRecommendedGoals">\n\n                        <!-- <ion-title>Recommended Goals</ion-title>\n\n                        <img src="assets/img/title_line.png"> -->\n\n                        \n\n                        <ion-card-header>\n\n                                Recommended Goals\n\n                        </ion-card-header>\n\n                        <table class="tbl-hra">\n\n                            <tbody *ngIf="GoalList?.length != 0">\n\n                                <tr *ngFor="let g of GoalList" class="div-goals-items">\n\n                                    <td>\n\n                                        <img src="{{siteUrl}}/{{g.goal_image_location.replace(\'~/\', \'\')}}">\n\n                                    </td>\n\n                                    <td>\n\n                                        <ion-title> {{g.goal_description}} </ion-title>\n\n                                    </td>\n\n                                    <td>\n\n                                        <ng-container *ngIf="g.goal_status_num == null || g.goal_status_num == 1">\n\n                                            <button ion-button color="primary" class="big-btn" (click)="SetGoals(g.goal_num);">\n\n                                                SET GOAL\n\n                                            </button>\n\n                                        </ng-container>\n\n                                        <ng-container *ngIf="g.goal_status_num !== null && g.goal_status_num != 1 ">\n\n                                            <button ion-button color="primary" class="big-btn" (click)="SetGoals(g.goal_num);">\n\n                                                VIEW GOAL\n\n                                            </button>\n\n                                        </ng-container>\n\n\n\n                                    </td>\n\n                                </tr>\n\n                            </tbody>\n\n                            <tbody *ngIf="GoalList?.length == 0">\n\n                                <tr>\n\n                                    <td style="padding-left: 50px;text-align: center;" colspan="3"> No Recommended Goals\n\n                                    </td>\n\n                                </tr>\n\n                            <tbody>\n\n\n\n                        </table>\n\n\n\n                    </div>\n\n\n\n                    <div style="height: 60px;">\n\n\n\n                    </div>\n\n                </div>\n\n            </div>\n\n        </div>\n\n\n\n    </ion-header>\n\n\n\n</ion-content>'/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\my-wellness-wallet\riskdetail\riskdetail.html"*/
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavParams"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["AlertController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["LoadingController"],
            __WEBPACK_IMPORTED_MODULE_2__providers__["b" /* HraService */]])
    ], RiskDetailPage);
    return RiskDetailPage;
}());

//# sourceMappingURL=riskdetail.js.map

/***/ }),

/***/ 266:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return NotificationMsgPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__providers__ = __webpack_require__(15);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__notification_notification__ = __webpack_require__(72);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};




/**
 * Generated class for the IntroPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
var NotificationMsgPage = /** @class */ (function () {
    function NotificationMsgPage(navCtrl, navParams, loadingCtrl, alertCtl, notificationService) {
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.loadingCtrl = loadingCtrl;
        this.alertCtl = alertCtl;
        this.notificationService = notificationService;
        this.item = {};
        this.item = JSON.parse(localStorage.getItem("noti-item"));
        this.Title = "Message Info";
        this.MarkMessageRead(this.item);
    }
    NotificationMsgPage.prototype.MarkMessageRead = function (item) {
        var _this = this;
        if (!item.IsRead) {
            var userAcc = {
                DeviceId: localStorage.getItem("deviceid"),
                SecretToken: localStorage.getItem("SecretToken"),
                Id: item.Id
            };
            this.notificationService
                .UpdateIsReadPushNotificationDetail(userAcc)
                .subscribe(function (resp) {
                console.log("Marked user read data");
                item.IsRead = true;
            }, function (err) {
                _this.loader.dismiss();
                _this.presentAlert("Server Message - Update Is Read Push Notification Detail : " +
                    JSON.stringify(err));
            });
        }
    };
    NotificationMsgPage.prototype.ionViewDidLoad = function () {
        console.log("ionViewDidLoad TemplatePage");
    };
    NotificationMsgPage.prototype.presentAlert = function (msg) {
        return __awaiter(this, void 0, void 0, function () {
            var alert;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.alertCtl.create({
                            message: msg,
                            cssClass: "action-sheets-basic-page",
                            buttons: [
                                {
                                    text: "OK",
                                    handler: function () { }
                                }
                            ]
                        })];
                    case 1:
                        alert = _a.sent();
                        return [4 /*yield*/, alert.present()];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    NotificationMsgPage.prototype.ionViewDidEnter = function () {
        var _this = this;
        this.navBar.backButtonClick = function () {
            ///here you can do wathever you want to replace the backbutton event
            console.log('Back button click');
            localStorage.removeItem("noti-item");
            _this.navCtrl.push(__WEBPACK_IMPORTED_MODULE_3__notification_notification__["a" /* NotificationPage */]);
        };
    };
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('navbar'),
        __metadata("design:type", __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["Navbar"])
    ], NotificationMsgPage.prototype, "navBar", void 0);
    NotificationMsgPage = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: "page-notimsg",template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\notificationmsg\notimsg.html"*/'<!--\n\n  Generated template for the IntroPage page.\n\n\n\n  See http://ionicframework.com/docs/components/#navigation for more info on\n\n  Ionic pages and navigation.\n\n-->\n\n<ion-header>\n\n    <ion-navbar #navbar color="primary">\n\n        <ion-title>{{Title}}</ion-title>\n\n    </ion-navbar>\n\n</ion-header>\n\n<ion-content>\n\n    <ion-card class="signup-card custom-border intro-desc">\n\n        <ion-card-content>\n\n            {{item.Message}}\n\n\n\n            <br />\n\n            <span class="msg-col2"> {{ item.CreateDate | date: \'longDate\'}} </span>\n\n    \n\n        </ion-card-content>\n\n    </ion-card>\n\n</ion-content>\n\n<ion-footer>\n\n</ion-footer>'/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\notificationmsg\notimsg.html"*/
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavParams"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["LoadingController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["AlertController"],
            __WEBPACK_IMPORTED_MODULE_2__providers__["c" /* NotificationService */]])
    ], NotificationMsgPage);
    return NotificationMsgPage;
}());

//# sourceMappingURL=notimsg.js.map

/***/ }),

/***/ 267:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "HraBodyPageModule", function() { return HraBodyPageModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__hra_body__ = __webpack_require__(539);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var HraBodyPageModule = /** @class */ (function () {
    function HraBodyPageModule() {
    }
    HraBodyPageModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__hra_body__["a" /* HraBodyPage */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_2__hra_body__["a" /* HraBodyPage */]),
            ],
        })
    ], HraBodyPageModule);
    return HraBodyPageModule;
}());

//# sourceMappingURL=hra-body.module.js.map

/***/ }),

/***/ 268:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "IntroVideoPageModule", function() { return IntroVideoPageModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__intro_video__ = __webpack_require__(148);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var IntroVideoPageModule = /** @class */ (function () {
    function IntroVideoPageModule() {
    }
    IntroVideoPageModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__intro_video__["a" /* IntroVideoPage */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_2__intro_video__["a" /* IntroVideoPage */]),
            ],
        })
    ], IntroVideoPageModule);
    return IntroVideoPageModule;
}());

//# sourceMappingURL=intro-video.module.js.map

/***/ }),

/***/ 269:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "IntroPageModule", function() { return IntroPageModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__intro__ = __webpack_require__(540);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var IntroPageModule = /** @class */ (function () {
    function IntroPageModule() {
    }
    IntroPageModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__intro__["a" /* IntroPage */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_2__intro__["a" /* IntroPage */]),
            ],
        })
    ], IntroPageModule);
    return IntroPageModule;
}());

//# sourceMappingURL=intro.module.js.map

/***/ }),

/***/ 270:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "LoginPageModule", function() { return LoginPageModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__ngx_translate_core__ = __webpack_require__(34);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__login__ = __webpack_require__(92);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};




var LoginPageModule = /** @class */ (function () {
    function LoginPageModule() {
    }
    LoginPageModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_3__login__["a" /* LoginPage */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_3__login__["a" /* LoginPage */]),
                __WEBPACK_IMPORTED_MODULE_1__ngx_translate_core__["b" /* TranslateModule */].forChild()
            ],
            exports: [
                __WEBPACK_IMPORTED_MODULE_3__login__["a" /* LoginPage */]
            ]
        })
    ], LoginPageModule);
    return LoginPageModule;
}());

//# sourceMappingURL=login.module.js.map

/***/ }),

/***/ 271:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MyHraPageModule", function() { return MyHraPageModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__my_hra__ = __webpack_require__(261);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var MyHraPageModule = /** @class */ (function () {
    function MyHraPageModule() {
    }
    MyHraPageModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__my_hra__["a" /* MyHraPage */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_2__my_hra__["a" /* MyHraPage */]),
            ],
        })
    ], MyHraPageModule);
    return MyHraPageModule;
}());

//# sourceMappingURL=my-hra.module.js.map

/***/ }),

/***/ 272:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MyTrackerPageModule", function() { return MyTrackerPageModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__mytracker__ = __webpack_require__(541);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var MyTrackerPageModule = /** @class */ (function () {
    function MyTrackerPageModule() {
    }
    MyTrackerPageModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__mytracker__["a" /* MyTrackerPage */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_2__mytracker__["a" /* MyTrackerPage */]),
            ],
        })
    ], MyTrackerPageModule);
    return MyTrackerPageModule;
}());

//# sourceMappingURL=mytracker.module.js.map

/***/ }),

/***/ 273:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ProfilePageModule", function() { return ProfilePageModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__profile__ = __webpack_require__(542);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_ion_multi_picker__ = __webpack_require__(275);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_ion_multi_picker___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_3_ion_multi_picker__);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};


// import { File } from '@ionic-native/file';
// import { Camera } from '@ionic-native/camera';


var ProfilePageModule = /** @class */ (function () {
    function ProfilePageModule() {
    }
    ProfilePageModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__profile__["a" /* ProfilePage */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_2__profile__["a" /* ProfilePage */]),
                __WEBPACK_IMPORTED_MODULE_3_ion_multi_picker__["MultiPickerModule"]
            ],
            providers: [
            //Camera
            ]
        })
    ], ProfilePageModule);
    return ProfilePageModule;
}());

//# sourceMappingURL=profile.module.js.map

/***/ }),

/***/ 278:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "HraQaPageModule", function() { return HraQaPageModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__hra_qa__ = __webpack_require__(262);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var HraQaPageModule = /** @class */ (function () {
    function HraQaPageModule() {
    }
    HraQaPageModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__hra_qa__["a" /* HraQaPage */]
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_2__hra_qa__["a" /* HraQaPage */])
            ],
        })
    ], HraQaPageModule);
    return HraQaPageModule;
}());

//# sourceMappingURL=hra-qa.module.js.map

/***/ }),

/***/ 279:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "HraResultPageModule", function() { return HraResultPageModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__hra_result__ = __webpack_require__(263);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var HraResultPageModule = /** @class */ (function () {
    function HraResultPageModule() {
    }
    HraResultPageModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__hra_result__["a" /* MyHraResultPage */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_2__hra_result__["a" /* MyHraResultPage */]),
            ],
        })
    ], HraResultPageModule);
    return HraResultPageModule;
}());

//# sourceMappingURL=hra-result.module.js.map

/***/ }),

/***/ 280:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MyWellnessWalletPageModule", function() { return MyWellnessWalletPageModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__my_wellness_wallet__ = __webpack_require__(94);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var MyWellnessWalletPageModule = /** @class */ (function () {
    function MyWellnessWalletPageModule() {
    }
    MyWellnessWalletPageModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__my_wellness_wallet__["a" /* MyWellnessWalletPage */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_2__my_wellness_wallet__["a" /* MyWellnessWalletPage */]),
            ],
        })
    ], MyWellnessWalletPageModule);
    return MyWellnessWalletPageModule;
}());

//# sourceMappingURL=my-wellness-wallet.module.js.map

/***/ }),

/***/ 281:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MyWellnessPlanPageModule", function() { return MyWellnessPlanPageModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__mywellnessplan__ = __webpack_require__(264);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var MyWellnessPlanPageModule = /** @class */ (function () {
    function MyWellnessPlanPageModule() {
    }
    MyWellnessPlanPageModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__mywellnessplan__["a" /* MyWellnessPlanPage */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_2__mywellnessplan__["a" /* MyWellnessPlanPage */]),
            ],
        })
    ], MyWellnessPlanPageModule);
    return MyWellnessPlanPageModule;
}());

//# sourceMappingURL=mywellnessplan.module.js.map

/***/ }),

/***/ 282:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "RiskDetailPageModule", function() { return RiskDetailPageModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__riskdetail__ = __webpack_require__(265);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var RiskDetailPageModule = /** @class */ (function () {
    function RiskDetailPageModule() {
    }
    RiskDetailPageModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__riskdetail__["a" /* RiskDetailPage */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_2__riskdetail__["a" /* RiskDetailPage */]),
            ],
        })
    ], RiskDetailPageModule);
    return RiskDetailPageModule;
}());

//# sourceMappingURL=riskdetail.module.js.map

/***/ }),

/***/ 283:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "NotificationMsgPageModule", function() { return NotificationMsgPageModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__notimsg__ = __webpack_require__(266);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var NotificationMsgPageModule = /** @class */ (function () {
    function NotificationMsgPageModule() {
    }
    NotificationMsgPageModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__notimsg__["a" /* NotificationMsgPage */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_2__notimsg__["a" /* NotificationMsgPage */])
            ],
            providers: []
        })
    ], NotificationMsgPageModule);
    return NotificationMsgPageModule;
}());

//# sourceMappingURL=notimsg.module.js.map

/***/ }),

/***/ 284:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "NotificationPageModule", function() { return NotificationPageModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__notification__ = __webpack_require__(72);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var NotificationPageModule = /** @class */ (function () {
    function NotificationPageModule() {
    }
    NotificationPageModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__notification__["a" /* NotificationPage */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_2__notification__["a" /* NotificationPage */])
            ],
            providers: [
            //Camera
            ]
        })
    ], NotificationPageModule);
    return NotificationPageModule;
}());

//# sourceMappingURL=notification.module.js.map

/***/ }),

/***/ 285:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AddlogsModule", function() { return AddlogsModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__addlogs__ = __webpack_require__(286);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var AddlogsModule = /** @class */ (function () {
    function AddlogsModule() {
    }
    AddlogsModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__addlogs__["a" /* AddlogsPage */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_2__addlogs__["a" /* AddlogsPage */]),
            ],
        })
    ], AddlogsModule);
    return AddlogsModule;
}());

//# sourceMappingURL=addlogs.module.js.map

/***/ }),

/***/ 286:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AddlogsPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__providers__ = __webpack_require__(15);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__providers_settings_wellnessconstant__ = __webpack_require__(44);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};




/**
 * Generated class for the IntroPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
var AddlogsPage = /** @class */ (function () {
    function AddlogsPage(viewCtrl, navParams, loadingCtrl, alertCtl, stepChallengeService) {
        this.viewCtrl = viewCtrl;
        this.navParams = navParams;
        this.loadingCtrl = loadingCtrl;
        this.alertCtl = alertCtl;
        this.stepChallengeService = stepChallengeService;
        this.listItems = [];
        this.model = { exduration: "", stepscount: "", exname: "", exerciseType: "" };
        // obtain data
        console.log(this.navParams, "nav params");
        this.data = this.navParams.data.Activities;
        this.curDate = this.navParams.data.curDate;
        this.listItems = this.data[0].lstIntensity;
        (this.model.exname = this.data[0].exname),
            (this.model.exerciseType = this.data[0].exerciseType),
            console.log(this.listItems);
    }
    AddlogsPage.prototype.ionViewDidLoad = function () {
        console.log("ionViewDidLoad TemplatePage");
    };
    AddlogsPage.prototype.closedPopup = function () {
        var resp = { SystemMessage: "Dismissed" };
        this.viewCtrl.dismiss(resp);
    };
    AddlogsPage.prototype.onSave = function () {
        var _this = this;
        if (this.model.intensity === undefined || (this.model.intensity !== undefined && this.model.intensity === "")) {
            this.presentAlert("Please select any intensity");
            return;
        }
        if (this.model.exduration === undefined || this.model.exduration == "" ||
            (this.model.exduration !== undefined &&
                this.model.exduration !== "" &&
                !this.numberOnlyValidation(this.model.exduration))) {
            this.presentAlert("Please enter any number between 1-999");
            return;
        }
        var data = this.model;
        var userinfo = {
            DeviceId: localStorage.getItem("deviceid"),
            SecretToken: localStorage.getItem("SecretToken"),
            exerciseType: this.model.exerciseType,
            exname: this.model.exname.trim(),
            exduration: this.model.exduration,
            exDate: __WEBPACK_IMPORTED_MODULE_3__providers_settings_wellnessconstant__["a" /* WellnessConstants */].GetFormatedDate(this.curDate, 'AddLogs'),
            intensity: this.model.intensity,
        };
        console.log(userinfo);
        this.loader = this.loadingCtrl.create({
            content: "Please wait...",
        });
        this.loader.present().then(function () {
            _this.stepChallengeService.SaveStepsActivitiesData(userinfo).subscribe(function (resp) {
                _this.loader.dismiss();
                if (resp.SystemStatus == "Success") {
                    _this.viewCtrl.dismiss(resp);
                }
                else {
                    _this.presentAlert(resp.SystemMessage);
                }
            }, function (err) {
                _this.loader.dismiss();
                _this.presentAlert("Server Message - Get User Overall Progress Of StepChallenge : " +
                    JSON.stringify(err));
            });
        });
    };
    AddlogsPage.prototype.numberOnlyValidation = function (value) {
        var num = parseInt(value);
        if (num === undefined || num == 0 || num > 999 || num < 1) {
            return false;
        }
        else if (isNaN(value) || value.includes(".")) {
            return false;
        }
        else {
            return true;
        }
    };
    AddlogsPage.prototype.presentAlert = function (msg) {
        return __awaiter(this, void 0, void 0, function () {
            var alert;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.alertCtl.create({
                            message: msg,
                            cssClass: "action-sheets-basic-page",
                            buttons: [
                                {
                                    text: "OK",
                                    handler: function () { },
                                },
                            ],
                        })];
                    case 1:
                        alert = _a.sent();
                        return [4 /*yield*/, alert.present()];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    AddlogsPage = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: "page-addlogs",template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\step-challenges\addlogs\addlogs.html"*/'<ion-content no-bounce>\n\n    <ion-item>\n\n        <ion-label>Intensity</ion-label>\n\n        <ion-select class="combo-large" [(ngModel)]="model.intensity" okText="Okay" cancelText="Dismiss">\n\n            <ion-option *ngFor="let item of listItems" value="{{item.Text}}">{{item.Text}}</ion-option>\n\n        </ion-select>\n\n    </ion-item>\n\n    <!-- <ion-item>\n\n        <ion-label>Minutes</ion-label>\n\n        <input placeholder="Enter duration" type="text" [(ngModel)]="model.exduration">\n\n    </ion-item> -->\n\n    <ion-item>\n\n        <ion-label color="light-grey">\n\n            <ion-icon item-left name="time"></ion-icon>Minutes\n\n        </ion-label>\n\n        <ion-input type="text" [(ngModel)]="model.exduration" class="input-brdr"></ion-input>\n\n    </ion-item>\n\n    <ion-grid>\n\n        <ion-row>\n\n            <ion-col width-50>\n\n                <button ion-button block outline (click)="onSave()">SAVE</button>\n\n            </ion-col>\n\n            <ion-col width-50>\n\n                <button ion-button block outline (click)="closedPopup();">CANCEL</button>\n\n            </ion-col>\n\n        </ion-row>\n\n    </ion-grid>\n\n</ion-content>'/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\step-challenges\addlogs\addlogs.html"*/,
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["ViewController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavParams"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["LoadingController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["AlertController"],
            __WEBPACK_IMPORTED_MODULE_2__providers__["e" /* StepChallengeService */]])
    ], AddlogsPage);
    return AddlogsPage;
}());

//# sourceMappingURL=addlogs.js.map

/***/ }),

/***/ 287:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SignupPageModule", function() { return SignupPageModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__ngx_translate_core__ = __webpack_require__(34);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_ion_multi_picker__ = __webpack_require__(275);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_ion_multi_picker___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_3_ion_multi_picker__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__signup__ = __webpack_require__(147);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};





var SignupPageModule = /** @class */ (function () {
    function SignupPageModule() {
    }
    SignupPageModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_4__signup__["a" /* SignupPage */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_4__signup__["a" /* SignupPage */]),
                __WEBPACK_IMPORTED_MODULE_1__ngx_translate_core__["b" /* TranslateModule */].forChild(),
                __WEBPACK_IMPORTED_MODULE_3_ion_multi_picker__["MultiPickerModule"]
            ],
            exports: [
                __WEBPACK_IMPORTED_MODULE_4__signup__["a" /* SignupPage */]
            ]
        })
    ], SignupPageModule);
    return SignupPageModule;
}());

//# sourceMappingURL=signup.module.js.map

/***/ }),

/***/ 288:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "LogOtherActivitiesPageModule", function() { return LogOtherActivitiesPageModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__logoverview__ = __webpack_require__(546);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var LogOtherActivitiesPageModule = /** @class */ (function () {
    function LogOtherActivitiesPageModule() {
    }
    LogOtherActivitiesPageModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__logoverview__["a" /* LogOverviewPage */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_2__logoverview__["a" /* LogOverviewPage */]),
            ],
        })
    ], LogOtherActivitiesPageModule);
    return LogOtherActivitiesPageModule;
}());

//# sourceMappingURL=logoverview.module.js.map

/***/ }),

/***/ 289:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return LogStepsPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__addlogs_addlogs__ = __webpack_require__(286);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__providers__ = __webpack_require__(15);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__step_dashboard__ = __webpack_require__(150);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__providers_settings_wellnessconstant__ = __webpack_require__(44);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};






/**
 * Generated class for the IntroPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
var LogStepsPage = /** @class */ (function () {
    function LogStepsPage(navCtrl, navParams, loadingCtrl, pickerCtl, alertCtl, modalCtrl, stepChallengeService) {
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.loadingCtrl = loadingCtrl;
        this.pickerCtl = pickerCtl;
        this.alertCtl = alertCtl;
        this.modalCtrl = modalCtrl;
        this.stepChallengeService = stepChallengeService;
        this.account = { Activities: "", LogStepsNumber: "" };
        this.LogsStepActivities = [];
        this.TotalSteps = "";
        this.OtherActivities = [];
        this.OptionItems = [];
        this.CurrentDate = new Date().toDateString();
        this.AddActivities = [];
        this.IsCancelled = false;
        this.GetLogsActivities();
        this.LoadOtherActivities();
    }
    LogStepsPage.prototype.AddlogsModal = function () {
        var _this = this;
        var selDate = new Date(this.CurrentDate);
        if (selDate > new Date()) {
            this.presentAlert("You can't add logs for future dates!");
            return;
        }
        if (this.account.Activities != null && this.account.Activities) {
            var profileModal = this.modalCtrl.create(__WEBPACK_IMPORTED_MODULE_2__addlogs_addlogs__["a" /* AddlogsPage */], {
                Activities: this.AddActivities,
                curDate: this.CurrentDate
            });
            profileModal.onDidDismiss(function (data) {
                console.log(data);
                if (data.SystemStatus == "Success") {
                    console.log(data);
                    _this.presentAlert(data.SystemMessage);
                    _this.account.Activities = "";
                    _this.GetLogsActivities();
                }
                else {
                    _this.presentAlert(data.SystemMessage);
                }
            });
            profileModal.present();
        }
        else {
            this.presentAlert("Please select activities!");
        }
    };
    LogStepsPage.prototype.AddLogs = function () {
        var _this = this;
        var selDate = new Date(this.CurrentDate);
        if (selDate > new Date()) {
            this.presentAlert("You can't add logs for future dates!");
            return;
        }
        if (this.account.LogStepsNumber === undefined || this.account.LogStepsNumber == "" ||
            (this.account.LogStepsNumber !== undefined &&
                this.account.LogStepsNumber !== "" &&
                !this.numberOnlyValidation(this.account.LogStepsNumber))) {
            this.presentAlert("Please enter any number between 1-999999");
            return;
        }
        var userAcc = {
            DeviceId: localStorage.getItem("deviceid"),
            SecretToken: localStorage.getItem("SecretToken"),
            LogStepsNumber: this.account.LogStepsNumber,
            Date: __WEBPACK_IMPORTED_MODULE_5__providers_settings_wellnessconstant__["a" /* WellnessConstants */].GetFormatedDate(this.CurrentDate, 'Logsteps'),
        };
        console.log(userAcc, 'request');
        this.loader = this.loadingCtrl.create({
            content: "Please wait..."
        });
        this.loader.present().then(function () {
            _this.stepChallengeService.SaveLogStepsNumber(userAcc).subscribe(function (resp) {
                _this.loader.dismiss();
                if (resp.SystemStatus == "Success") {
                    console.log(resp);
                    _this.account.LogStepsNumber = "";
                    _this.GetLogsActivities();
                }
                else {
                    _this.presentAlert(resp.SystemMessage);
                }
            }, function (err) {
                _this.loader.dismiss();
                _this.presentAlert("Server Message - Save Log Steps Number : " + JSON.stringify(err));
            });
        });
    };
    LogStepsPage.prototype.GetLogsActivities = function () {
        var _this = this;
        var userAcc = {
            DeviceId: localStorage.getItem("deviceid"),
            SecretToken: localStorage.getItem("SecretToken"),
            Date: this.CurrentDate
        };
        this.loader = this.loadingCtrl.create({
            content: "Please wait..."
        });
        this.loader.present().then(function () {
            _this.stepChallengeService
                .GetLoggedStepsActivitiesByDate(userAcc)
                .subscribe(function (resp) {
                _this.loader.dismiss();
                if (resp.SystemStatus == "Success") {
                    _this.LogsStepActivities = resp.ListOfSteps;
                    _this.TotalSteps = resp.TotalSteps;
                    console.log(resp);
                }
                else {
                    _this.presentAlert(resp.SystemMessage);
                }
            }, function (err) {
                _this.loader.dismiss();
                _this.presentAlert("Server Message - Get User Overall Progress Of StepChallenge : " +
                    JSON.stringify(err));
            });
        });
    };
    LogStepsPage.prototype.numberOnlyValidation = function (value) {
        var num = parseInt(value);
        if (num === undefined || num == 0 || num > 999999 || num < 1) {
            return false;
        }
        else if (isNaN(value) || value.includes(".")) {
            return false;
        }
        else {
            return true;
        }
    };
    LogStepsPage.prototype.LoadOtherActivities = function () {
        var _this = this;
        var userAcc = {
            DeviceId: localStorage.getItem("deviceid"),
            SecretToken: localStorage.getItem("SecretToken"),
            SearchText: ""
        };
        this.stepChallengeService.SearchStepsActivities(userAcc).subscribe(function (resp) {
            if (resp.SystemStatus == "Success") {
                var listOfActivities = resp.ListOfActivities;
                _this.OtherActivities = resp.ListOfActivities;
                listOfActivities.forEach(function (item) {
                    var oactivities = { text: item.exname, value: item.id };
                    _this.OptionItems.push(oactivities);
                });
                console.log(_this.OtherActivities, "OtherActivities");
            }
            else {
                _this.presentAlert(resp.SystemMessage);
            }
        }, function (err) {
            _this.presentAlert("Server Message - Get User Overall Progress Of StepChallenge : " +
                JSON.stringify(err));
        });
    };
    LogStepsPage.prototype.DeleteLoggedStepsActivities = function (id) {
        var _this = this;
        var userAcc = {
            DeviceId: localStorage.getItem("deviceid"),
            SecretToken: localStorage.getItem("SecretToken"),
            step_tracking_num: id
        };
        this.loader = this.loadingCtrl.create({
            content: "Please wait..."
        });
        this.loader.present().then(function () {
            _this.stepChallengeService.DeleteLoggedStepsActivities(userAcc).subscribe(function (resp) {
                _this.loader.dismiss();
                if (resp.SystemStatus == "Success") {
                    _this.presentAlert(resp.SystemMessage);
                    _this.GetLogsActivities();
                }
                else {
                    _this.presentAlert(resp.SystemMessage);
                }
            }, function (err) {
                _this.loader.dismiss();
                _this.presentAlert("Server Message - Delete Logged Steps Activities : " +
                    JSON.stringify(err));
            });
        });
    };
    LogStepsPage.prototype.ionViewDidLoad = function () {
        console.log("ionViewDidLoad TemplatePage");
    };
    LogStepsPage.prototype.presentAlert = function (msg) {
        return __awaiter(this, void 0, void 0, function () {
            var alert;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.alertCtl.create({
                            message: msg,
                            cssClass: "action-sheets-basic-page",
                            buttons: [
                                {
                                    text: "OK",
                                    handler: function () { }
                                }
                            ]
                        })];
                    case 1:
                        alert = _a.sent();
                        return [4 /*yield*/, alert.present()];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    LogStepsPage.prototype.ionViewDidEnter = function () {
        var _this = this;
        this.navBar.backButtonClick = function () {
            console.log('Back button click - logsteps');
            _this.navCtrl.setRoot(__WEBPACK_IMPORTED_MODULE_4__step_dashboard__["a" /* StepDashboardPage */]);
        };
    };
    LogStepsPage.prototype.NextScreen = function (name) {
        localStorage.setItem("backstepspage", 'LogStepsPage');
        this.navCtrl.push(name);
    };
    LogStepsPage.prototype.showActivitiesPicker = function () {
        return __awaiter(this, void 0, void 0, function () {
            var opts, picker;
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        opts = {
                            buttons: [
                                {
                                    text: "Cancel",
                                    role: "cancel",
                                    handler: function () {
                                        _this.IsCancelled = true;
                                        _this.account.Activities = "";
                                    }
                                },
                                {
                                    text: "Done",
                                    handler: function () {
                                        _this.IsCancelled = false;
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
                        return [4 /*yield*/, this.pickerCtl.create(opts)];
                    case 1:
                        picker = _a.sent();
                        picker.present();
                        picker.onDidDismiss(function (data) { return __awaiter(_this, void 0, void 0, function () {
                            var col;
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0:
                                        console.log(data, "test - p");
                                        return [4 /*yield*/, picker.getColumn("Activities")];
                                    case 1:
                                        col = _a.sent();
                                        if (col.options[col.selectedIndex].value != "" && !this.IsCancelled) {
                                            this.account.Activities = col.options[col.selectedIndex].text;
                                            this.AddActivities = this.OtherActivities.filter(function (q) { return q.id == col.options[col.selectedIndex].value; });
                                            console.log(this.AddActivities, "Add -act");
                                        }
                                        return [2 /*return*/];
                                }
                            });
                        }); });
                        return [2 /*return*/];
                }
            });
        });
    };
    LogStepsPage.prototype.SetDate = function (title, action) {
        var curDate = new Date(this.CurrentDate);
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
    };
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])("navbar"),
        __metadata("design:type", __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["Navbar"])
    ], LogStepsPage.prototype, "navBar", void 0);
    LogStepsPage = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: "page-logsteps",template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\step-challenges\log-steps\logsteps.html"*/'<ion-header>\n\n    <ion-navbar #navbar color="primary">\n\n        <ion-title>Log Steps / Activities</ion-title>\n\n    </ion-navbar>\n\n</ion-header>\n\n<ion-content>\n\n    <div class="custom-row align-items-center mt-3 mb-3">\n\n        <div class="custom-col pl-3">\n\n            <div class="font-family-Conv_gotham-book-webfont font-size-20 font-weight-800">\n\n                <button (click)="SetDate(\'Today\', \'Previous\')" class="btn-pre">&#8249;</button>\n\n                <span (click)="SetDate(\'Today\', \'\')"> TODAY </span>\n\n                <button (click)="SetDate(\'Today\', \'Next\')" class="btn-next">&#8250;</button>\n\n            </div>\n\n            <small class="text-light">{{CurrentDate}}</small>\n\n        </div>\n\n        <div class="custom-col-auto mr-3">\n\n            <!-- <div class="custom-btn-primary rounded-circle d-flex align-items-center justify-content-center"\n\n                style="width: 30px; height: 30px;">\n\n                <img class="pb-2 pt-2 pl-2 pr-2" src="assets/img/calendar.png" alt="calendar" style="width:34px;">\n\n            </div> -->\n\n        </div>\n\n    </div>\n\n    <div class="app-cart ml-3 mr-3 mb-3">\n\n        <div class="titli-1">Log Steps</div>\n\n        <div class="custom-row">\n\n            <div class="custom-col">\n\n                <input placeholder="Enter number of steps to log" type="text" class="form-control"\n\n                    [(ngModel)]="account.LogStepsNumber">\n\n            </div>\n\n        </div>\n\n        <button type="button" class="custom-btn custom-btn-primary w-100 font-weight-bold font-size-17 py-2 px-2"\n\n            (click)="AddLogs()">\n\n            <img class="align-middle img-fluid pr-2" src="assets/img/log-steps.png" style="width:37px;"\n\n                alt="log-steps">ADD</button>\n\n    </div>\n\n    <div class="text-center primary-color mb-3 font-family-Conv_Gotham-Medium font-size-20">OR</div>\n\n    <div class="app-cart ml-3 mr-3 mb-3">\n\n        <div class="titli-1">Log Other Activities</div>\n\n        <div class="custom-row">\n\n            <div class="custom-col">\n\n                <!-- <ion-searchbar #searchbar placeholder="Search activity by name"\n\n               class="form-control"></ion-searchbar> -->\n\n\n\n                <input placeholder="Search activity by name" readonly type="text" class="form-control"\n\n                    (focus)="showActivitiesPicker()" [(ngModel)]="account.Activities"> \n\n            </div>\n\n        </div>\n\n        <button type="button" class="custom-btn custom-btn-primary w-100 font-weight-bold font-size-17 py-2 px-2"\n\n            (click)="AddlogsModal();">\n\n            ADD</button>\n\n    </div>\n\n    <div class="app-cart ml-3 mr-3 mb-4">\n\n        <div class="titli-1">Logged Steps / Activities</div>\n\n        <div class="custom-row" *ngFor="let item of LogsStepActivities">\n\n            <div class="custom-col">\n\n                <span>{{item.exercise_name}}</span>\n\n                <div class="small font-family-Conv_Gotham-Medium">{{item.steps_count}}</div>\n\n            </div>\n\n            <div class="custom-col-auto" (click)="DeleteLoggedStepsActivities(item.step_tracking_num)">\n\n                <img style="width: 20px;" class="img-fluid" src="assets/img/remove.png" alt="remove">\n\n            </div>\n\n        </div>\n\n\n\n        <div class="mt-4 pb-2 text-center">\n\n            <div class="primary-color font-family-Conv_Gotham-Medium font-size-25">{{TotalSteps}}</div>\n\n            <div class="font-family-Conv_gotham-book-webfont font-size-17">Total Steps</div>\n\n        </div>\n\n    </div>\n\n    <div class="ml-3 mr-3 mb-3">\n\n        <button (click)="NextScreen(\'StepChallengeHistoryPage\')" type="button"\n\n            class="custom-btn text-uppercase custom-btn-primary w-100 font-weight-bold font-size-17 py-2 px-2">\n\n            My Challenges</button>\n\n    </div>\n\n</ion-content>'/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\step-challenges\log-steps\logsteps.html"*/
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavParams"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["LoadingController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["PickerController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["AlertController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["ModalController"],
            __WEBPACK_IMPORTED_MODULE_3__providers__["e" /* StepChallengeService */]])
    ], LogStepsPage);
    return LogStepsPage;
}());

//# sourceMappingURL=logsteps.js.map

/***/ }),

/***/ 290:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "LogStepsPageModule", function() { return LogStepsPageModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__logsteps__ = __webpack_require__(289);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var LogStepsPageModule = /** @class */ (function () {
    function LogStepsPageModule() {
    }
    LogStepsPageModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__logsteps__["a" /* LogStepsPage */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_2__logsteps__["a" /* LogStepsPage */]),
            ],
        })
    ], LogStepsPageModule);
    return LogStepsPageModule;
}());

//# sourceMappingURL=logsteps.module.js.map

/***/ }),

/***/ 291:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "StepDashboardPageModule", function() { return StepDashboardPageModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__step_dashboard__ = __webpack_require__(150);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_ng_circle_progress__ = __webpack_require__(548);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



// Import ng-circle-progress

var StepDashboardPageModule = /** @class */ (function () {
    function StepDashboardPageModule() {
    }
    StepDashboardPageModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__step_dashboard__["a" /* StepDashboardPage */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_3_ng_circle_progress__["a" /* NgCircleProgressModule */].forRoot({
                    // "radius": 60,
                    "space": -15,
                    "outerStrokeGradient": true,
                    "outerStrokeWidth": 15,
                    "outerStrokeColor": "#4882c2",
                    "outerStrokeGradientStopColor": "#53a9ff",
                    "innerStrokeColor": "#e7e8ea",
                    "innerStrokeWidth": 15,
                    "animateTitle": false,
                    "animationDuration": 1000,
                    "showUnits": false,
                    "showBackground": false,
                    "clockwise": false,
                    "startFromZero": false,
                    "backgroundPadding": -40,
                    "subtitleColor": "#35b2ee",
                    "backgroundStrokeWidth": 0
                }),
                __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_2__step_dashboard__["a" /* StepDashboardPage */]),
            ],
        })
    ], StepDashboardPageModule);
    return StepDashboardPageModule;
}());

//# sourceMappingURL=step-dashboard.module.js.map

/***/ }),

/***/ 292:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "StepChallengeHistoryPageModule", function() { return StepChallengeHistoryPageModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__stepchallengehistory__ = __webpack_require__(549);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var StepChallengeHistoryPageModule = /** @class */ (function () {
    function StepChallengeHistoryPageModule() {
    }
    StepChallengeHistoryPageModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__stepchallengehistory__["a" /* StepChallengeHistoryPage */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_2__stepchallengehistory__["a" /* StepChallengeHistoryPage */]),
            ],
        })
    ], StepChallengeHistoryPageModule);
    return StepChallengeHistoryPageModule;
}());

//# sourceMappingURL=stepchallengehistory.module.js.map

/***/ }),

/***/ 293:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "TemplatePageModule", function() { return TemplatePageModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__template__ = __webpack_require__(93);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var TemplatePageModule = /** @class */ (function () {
    function TemplatePageModule() {
    }
    TemplatePageModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__template__["a" /* TemplatePage */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_2__template__["a" /* TemplatePage */])
            ],
            providers: []
        })
    ], TemplatePageModule);
    return TemplatePageModule;
}());

//# sourceMappingURL=template.module.js.map

/***/ }),

/***/ 294:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "WelcomePageModule", function() { return WelcomePageModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__ngx_translate_core__ = __webpack_require__(34);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__welcome__ = __webpack_require__(295);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};




var WelcomePageModule = /** @class */ (function () {
    function WelcomePageModule() {
    }
    WelcomePageModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_3__welcome__["a" /* WelcomePage */],
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["IonicPageModule"].forChild(__WEBPACK_IMPORTED_MODULE_3__welcome__["a" /* WelcomePage */]),
                __WEBPACK_IMPORTED_MODULE_1__ngx_translate_core__["b" /* TranslateModule */].forChild()
            ],
            exports: [
                __WEBPACK_IMPORTED_MODULE_3__welcome__["a" /* WelcomePage */]
            ]
        })
    ], WelcomePageModule);
    return WelcomePageModule;
}());

//# sourceMappingURL=welcome.module.js.map

/***/ }),

/***/ 295:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WelcomePage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__login_login__ = __webpack_require__(92);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__signup_signup__ = __webpack_require__(147);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__my_wellness_wallet_my_wellness_wallet__ = __webpack_require__(94);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__providers_settings_alertmessage_service__ = __webpack_require__(149);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};






/**
 * The Welcome Page is a splash page that quickly describes the app,
 * and then directs the user to create an account or log in.
 * If you'd like to immediately put the user onto a login/signup page,
 * we recommend not using the Welcome page.
 */
var WelcomePage = /** @class */ (function () {
    function WelcomePage(navCtrl, toastCtrl, events, menu, alertSrv) {
        var _this = this;
        this.navCtrl = navCtrl;
        this.toastCtrl = toastCtrl;
        this.events = events;
        this.menu = menu;
        this.alertSrv = alertSrv;
        this.loader = this.alertSrv.LoadingMsg("Loading...");
        this.loader.present().then(function () {
            if (localStorage.getItem("UserInfo") !== undefined &&
                localStorage.getItem("UserInfo") !== null) {
                var userInfo = JSON.parse(localStorage.getItem("UserInfo"));
                _this.menu.enable(true);
                console.log(userInfo);
                _this.events.publish("user:created", userInfo);
                setTimeout(function () {
                    _this.loader.dismiss();
                    _this.navCtrl.setRoot(__WEBPACK_IMPORTED_MODULE_4__my_wellness_wallet_my_wellness_wallet__["a" /* MyWellnessWalletPage */]);
                }, 1000);
            }
            else {
                _this.menu.enable(false);
                _this.loader.dismiss();
            }
        });
    }
    WelcomePage.prototype.ionViewDidLoad = function () { };
    WelcomePage.prototype.login = function () {
        this.navCtrl.push(__WEBPACK_IMPORTED_MODULE_2__login_login__["a" /* LoginPage */]);
    };
    WelcomePage.prototype.signup = function () {
        this.navCtrl.push(__WEBPACK_IMPORTED_MODULE_3__signup_signup__["a" /* SignupPage */]);
    };
    WelcomePage = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: "page-welcome",template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\welcome\welcome.html"*/'<ion-content scroll="false">\n  <div class="splash-bg"></div>\n    <div padding class="button-info">\n    <button ion-button block (click)="signup()" class="signup">SIGNUP</button>\n    <button ion-button block (click)="login()" class="login">LOGIN</button>\n  </div>\n</ion-content>\n'/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\welcome\welcome.html"*/
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["ToastController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["Events"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["MenuController"],
            __WEBPACK_IMPORTED_MODULE_5__providers_settings_alertmessage_service__["a" /* AlertMessagesService */]])
    ], WelcomePage);
    return WelcomePage;
}());

//# sourceMappingURL=welcome.js.map

/***/ }),

/***/ 336:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return FcmService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__ionic_native_firebase__ = __webpack_require__(337);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_angularfire2_firestore__ = __webpack_require__(568);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_angularfire2_firestore___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_3_angularfire2_firestore__);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};




var FcmService = /** @class */ (function () {
    function FcmService(firebase, afs, platform) {
        this.firebase = firebase;
        this.afs = afs;
        this.platform = platform;
    }
    FcmService.prototype.deleteToken = function () {
        this.firebase.unregister();
    };
    FcmService.prototype.getToken = function (userId) {
        return __awaiter(this, void 0, void 0, function () {
            var token, deviceType;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        if (!this.platform.is("android")) return [3 /*break*/, 2];
                        deviceType = "android";
                        return [4 /*yield*/, this.firebase.getToken()];
                    case 1:
                        token = _a.sent();
                        _a.label = 2;
                    case 2:
                        if (!this.platform.is("ios")) return [3 /*break*/, 5];
                        deviceType = "ios";
                        return [4 /*yield*/, this.firebase.getToken()];
                    case 3:
                        token = _a.sent();
                        //alert(token);
                        return [4 /*yield*/, this.firebase.grantPermission()];
                    case 4:
                        //alert(token);
                        _a.sent();
                        _a.label = 5;
                    case 5:
                        localStorage.setItem("FCMToken", token);
                        localStorage.setItem("DeviceType", deviceType);
                        this.saveToken(token, userId);
                        return [2 /*return*/];
                }
            });
        });
    };
    FcmService.prototype.saveToken = function (token, userId) {
        if (!token)
            return;
        var devicesRef = this.afs.collection("devices");
        var data = {
            token: token,
            userId: userId
        };
        return devicesRef.doc(token).set(data);
    };
    FcmService.prototype.onNotifications = function () {
        return this.firebase.onNotificationOpen();
    };
    FcmService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__ionic_native_firebase__["a" /* Firebase */],
            __WEBPACK_IMPORTED_MODULE_3_angularfire2_firestore__["AngularFirestore"],
            __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["Platform"]])
    ], FcmService);
    return FcmService;
}());

//# sourceMappingURL=fcm.service.js.map

/***/ }),

/***/ 344:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser_dynamic__ = __webpack_require__(345);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__app_module__ = __webpack_require__(469);


Object(__WEBPACK_IMPORTED_MODULE_0__angular_platform_browser_dynamic__["a" /* platformBrowserDynamic */])().bootstrapModule(__WEBPACK_IMPORTED_MODULE_1__app_module__["a" /* AppModule */]);
//# sourceMappingURL=main.js.map

/***/ }),

/***/ 44:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WellnessConstants; });
var WellnessConstants = /** @class */ (function () {
    function WellnessConstants() {
    }
    Object.defineProperty(WellnessConstants, "Prod_url", {
        get: function () {
            return "https://www.meschinowellness.com/";
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WellnessConstants, "Demo_url", {
        get: function () {
            return "http://demo-meschinowellness.azurewebsites.net/";
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WellnessConstants, "Staging_url", {
        get: function () {
            return "http://stage-meschinowellness.azurewebsites.net/";
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WellnessConstants, "App_Url", {
        get: function () {
            return "https://www.meschinowellness.com/";
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WellnessConstants, "weightData", {
        get: function () {
            return [
                { description: "140" },
                { description: "141" },
                { description: "142" },
                { description: "143" },
                { description: "144" },
                { description: "145" },
                { description: "146" },
                { description: "147" },
                { description: "148" },
                { description: "149" },
                { description: "150" },
                { description: "151" },
                { description: "152" },
                { description: "153" },
                { description: "154" },
                { description: "155" },
                { description: "156" },
                { description: "157" },
                { description: "158" },
                { description: "159" }
            ];
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WellnessConstants, "waistData", {
        get: function () {
            return [
                { description: "28" },
                { description: "30" },
                { description: "32" },
                { description: "34" },
                { description: "36" },
                { description: "38" },
                { description: "40" },
                { description: "42" },
                { description: "44" },
                { description: "46" },
                { description: "48" },
                { description: "50" },
                { description: "52" },
                { description: "54" },
                { description: "56" },
                { description: "58" },
                { description: "60" },
                { description: "62" },
                { description: "64" },
                { description: "66" },
                { description: "68" },
                { description: "70" }
            ];
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WellnessConstants, "parentColumns", {
        get: function () {
            return [
                {
                    name: "unit",
                    options: [
                        { text: "Feet/ Inches", value: "feet" },
                        { text: "CMS", value: "cm" }
                    ]
                },
                {
                    name: "Value",
                    parentCol: "unit",
                    options: [
                        { text: "  1", value: "1", parentVal: "feet" },
                        { text: "  2", value: "2", parentVal: "feet" },
                        { text: "  3", value: "3", parentVal: "feet" },
                        { text: "  4", value: "4", parentVal: "feet" },
                        { text: "  5", value: "5", parentVal: "feet" },
                        { text: "  6", value: "6", parentVal: "feet" },
                        { text: "  7", value: "7", parentVal: "feet" },
                        //{ text: "  8", value: "8", parentVal: "feet" },
                        { text: " 30", value: "30", parentVal: "cm" },
                        { text: " 31", value: "31", parentVal: "cm" },
                        { text: " 32", value: "32", parentVal: "cm" },
                        { text: " 33", value: "33", parentVal: "cm" },
                        { text: " 34", value: "34", parentVal: "cm" },
                        { text: " 35", value: "35", parentVal: "cm" },
                        { text: " 36", value: "36", parentVal: "cm" },
                        { text: " 37", value: "37", parentVal: "cm" },
                        { text: " 38", value: "38", parentVal: "cm" },
                        { text: " 39", value: "39", parentVal: "cm" },
                        { text: " 40", value: "40", parentVal: "cm" },
                        { text: " 41", value: "41", parentVal: "cm" },
                        { text: " 42", value: "42", parentVal: "cm" },
                        { text: " 43", value: "43", parentVal: "cm" },
                        { text: " 44", value: "44", parentVal: "cm" },
                        { text: " 45", value: "45", parentVal: "cm" },
                        { text: " 46", value: "46", parentVal: "cm" },
                        { text: " 47", value: "47", parentVal: "cm" },
                        { text: " 48", value: "48", parentVal: "cm" },
                        { text: " 49", value: "49", parentVal: "cm" },
                        { text: " 50", value: "50", parentVal: "cm" },
                        { text: " 51", value: "51", parentVal: "cm" },
                        { text: " 52", value: "52", parentVal: "cm" },
                        { text: " 53", value: "53", parentVal: "cm" },
                        { text: " 54", value: "54", parentVal: "cm" },
                        { text: " 55", value: "55", parentVal: "cm" },
                        { text: " 56", value: "56", parentVal: "cm" },
                        { text: " 57", value: "57", parentVal: "cm" },
                        { text: " 58", value: "58", parentVal: "cm" },
                        { text: " 59", value: "59", parentVal: "cm" },
                        { text: " 60", value: "60", parentVal: "cm" },
                        { text: " 61", value: "61", parentVal: "cm" },
                        { text: " 62", value: "62", parentVal: "cm" },
                        { text: " 63", value: "63", parentVal: "cm" },
                        { text: " 64", value: "64", parentVal: "cm" },
                        { text: " 65", value: "65", parentVal: "cm" },
                        { text: " 66", value: "66", parentVal: "cm" },
                        { text: " 67", value: "67", parentVal: "cm" },
                        { text: " 68", value: "68", parentVal: "cm" },
                        { text: " 69", value: "69", parentVal: "cm" },
                        { text: " 70", value: "70", parentVal: "cm" },
                        { text: " 71", value: "71", parentVal: "cm" },
                        { text: " 72", value: "72", parentVal: "cm" },
                        { text: " 73", value: "73", parentVal: "cm" },
                        { text: " 74", value: "74", parentVal: "cm" },
                        { text: " 75", value: "75", parentVal: "cm" },
                        { text: " 76", value: "76", parentVal: "cm" },
                        { text: " 77", value: "77", parentVal: "cm" },
                        { text: " 78", value: "78", parentVal: "cm" },
                        { text: " 79", value: "79", parentVal: "cm" },
                        { text: " 80", value: "80", parentVal: "cm" },
                        { text: " 81", value: "81", parentVal: "cm" },
                        { text: " 82", value: "82", parentVal: "cm" },
                        { text: " 83", value: "83", parentVal: "cm" },
                        { text: " 84", value: "84", parentVal: "cm" },
                        { text: " 85", value: "85", parentVal: "cm" },
                        { text: " 86", value: "86", parentVal: "cm" },
                        { text: " 87", value: "87", parentVal: "cm" },
                        { text: " 88", value: "88", parentVal: "cm" },
                        { text: " 89", value: "89", parentVal: "cm" },
                        { text: " 90", value: "90", parentVal: "cm" },
                        { text: " 91", value: "91", parentVal: "cm" },
                        { text: " 92", value: "92", parentVal: "cm" },
                        { text: " 93", value: "93", parentVal: "cm" },
                        { text: " 94", value: "94", parentVal: "cm" },
                        { text: " 95", value: "95", parentVal: "cm" },
                        { text: " 96", value: "96", parentVal: "cm" },
                        { text: " 97", value: "97", parentVal: "cm" },
                        { text: " 98", value: "98", parentVal: "cm" },
                        { text: " 99", value: "99", parentVal: "cm" },
                        { text: " 100", value: "100", parentVal: "cm" },
                        { text: " 101", value: "101", parentVal: "cm" },
                        { text: " 102", value: "102", parentVal: "cm" },
                        { text: " 103", value: "103", parentVal: "cm" },
                        { text: " 104", value: "104", parentVal: "cm" },
                        { text: " 105", value: "105", parentVal: "cm" },
                        { text: " 106", value: "106", parentVal: "cm" },
                        { text: " 107", value: "107", parentVal: "cm" },
                        { text: " 108", value: "108", parentVal: "cm" },
                        { text: " 109", value: "109", parentVal: "cm" },
                        { text: " 110", value: "110", parentVal: "cm" },
                        { text: " 111", value: "111", parentVal: "cm" },
                        { text: " 112", value: "112", parentVal: "cm" },
                        { text: " 113", value: "113", parentVal: "cm" },
                        { text: " 114", value: "114", parentVal: "cm" },
                        { text: " 115", value: "115", parentVal: "cm" },
                        { text: " 116", value: "116", parentVal: "cm" },
                        { text: " 117", value: "117", parentVal: "cm" },
                        { text: " 118", value: "118", parentVal: "cm" },
                        { text: " 119", value: "119", parentVal: "cm" },
                        { text: " 120", value: "120", parentVal: "cm" },
                        { text: " 121", value: "121", parentVal: "cm" },
                        { text: " 122", value: "122", parentVal: "cm" },
                        { text: " 123", value: "123", parentVal: "cm" },
                        { text: " 124", value: "124", parentVal: "cm" },
                        { text: " 125", value: "125", parentVal: "cm" },
                        { text: " 126", value: "126", parentVal: "cm" },
                        { text: " 127", value: "127", parentVal: "cm" },
                        { text: " 128", value: "128", parentVal: "cm" },
                        { text: " 129", value: "129", parentVal: "cm" },
                        { text: " 130", value: "130", parentVal: "cm" },
                        { text: " 131", value: "131", parentVal: "cm" },
                        { text: " 132", value: "132", parentVal: "cm" },
                        { text: " 133", value: "133", parentVal: "cm" },
                        { text: " 134", value: "134", parentVal: "cm" },
                        { text: " 135", value: "135", parentVal: "cm" },
                        { text: " 136", value: "136", parentVal: "cm" },
                        { text: " 137", value: "137", parentVal: "cm" },
                        { text: " 138", value: "138", parentVal: "cm" },
                        { text: " 139", value: "139", parentVal: "cm" },
                        { text: " 140", value: "140", parentVal: "cm" },
                        { text: " 141", value: "141", parentVal: "cm" },
                        { text: " 142", value: "142", parentVal: "cm" },
                        { text: " 143", value: "143", parentVal: "cm" },
                        { text: " 144", value: "144", parentVal: "cm" },
                        { text: " 145", value: "145", parentVal: "cm" },
                        { text: " 146", value: "146", parentVal: "cm" },
                        { text: " 147", value: "147", parentVal: "cm" },
                        { text: " 148", value: "148", parentVal: "cm" },
                        { text: " 149", value: "149", parentVal: "cm" },
                        { text: " 150", value: "150", parentVal: "cm" },
                        { text: " 151", value: "151", parentVal: "cm" },
                        { text: " 152", value: "152", parentVal: "cm" },
                        { text: " 153", value: "153", parentVal: "cm" },
                        { text: " 154", value: "154", parentVal: "cm" },
                        { text: " 155", value: "155", parentVal: "cm" },
                        { text: " 156", value: "156", parentVal: "cm" },
                        { text: " 157", value: "157", parentVal: "cm" },
                        { text: " 158", value: "158", parentVal: "cm" },
                        { text: " 159", value: "159", parentVal: "cm" },
                        { text: " 160", value: "160", parentVal: "cm" },
                        { text: " 161", value: "161", parentVal: "cm" },
                        { text: " 162", value: "162", parentVal: "cm" },
                        { text: " 163", value: "163", parentVal: "cm" },
                        { text: " 164", value: "164", parentVal: "cm" },
                        { text: " 165", value: "165", parentVal: "cm" },
                        { text: " 166", value: "166", parentVal: "cm" },
                        { text: " 167", value: "167", parentVal: "cm" },
                        { text: " 168", value: "168", parentVal: "cm" },
                        { text: " 169", value: "169", parentVal: "cm" },
                        { text: " 170", value: "170", parentVal: "cm" },
                        { text: " 171", value: "171", parentVal: "cm" },
                        { text: " 172", value: "172", parentVal: "cm" },
                        { text: " 173", value: "173", parentVal: "cm" },
                        { text: " 174", value: "174", parentVal: "cm" },
                        { text: " 175", value: "175", parentVal: "cm" },
                        { text: " 176", value: "176", parentVal: "cm" },
                        { text: " 177", value: "177", parentVal: "cm" },
                        { text: " 178", value: "178", parentVal: "cm" },
                        { text: " 179", value: "179", parentVal: "cm" },
                        { text: " 180", value: "180", parentVal: "cm" },
                        { text: " 181", value: "181", parentVal: "cm" },
                        { text: " 182", value: "182", parentVal: "cm" },
                        { text: " 183", value: "183", parentVal: "cm" },
                        { text: " 184", value: "184", parentVal: "cm" },
                        { text: " 185", value: "185", parentVal: "cm" },
                        { text: " 186", value: "186", parentVal: "cm" },
                        { text: " 187", value: "187", parentVal: "cm" },
                        { text: " 188", value: "188", parentVal: "cm" },
                        { text: " 189", value: "189", parentVal: "cm" },
                        { text: " 190", value: "190", parentVal: "cm" },
                        { text: " 191", value: "191", parentVal: "cm" },
                        { text: " 192", value: "192", parentVal: "cm" },
                        { text: " 193", value: "193", parentVal: "cm" },
                        { text: " 194", value: "194", parentVal: "cm" },
                        { text: " 195", value: "195", parentVal: "cm" },
                        { text: " 196", value: "196", parentVal: "cm" },
                        { text: " 197", value: "197", parentVal: "cm" },
                        { text: " 198", value: "198", parentVal: "cm" },
                        { text: " 199", value: "199", parentVal: "cm" },
                        { text: " 200", value: "200", parentVal: "cm" },
                        { text: " 201", value: "201", parentVal: "cm" },
                        { text: " 202", value: "202", parentVal: "cm" },
                        { text: " 203", value: "203", parentVal: "cm" },
                        { text: " 204", value: "204", parentVal: "cm" },
                        { text: " 205", value: "205", parentVal: "cm" },
                        { text: " 206", value: "206", parentVal: "cm" },
                        { text: " 207", value: "207", parentVal: "cm" },
                        { text: " 208", value: "208", parentVal: "cm" },
                        { text: " 209", value: "209", parentVal: "cm" },
                        { text: " 210", value: "210", parentVal: "cm" },
                        { text: " 211", value: "211", parentVal: "cm" },
                        { text: " 212", value: "212", parentVal: "cm" },
                        { text: " 213", value: "213", parentVal: "cm" },
                        { text: " 214", value: "214", parentVal: "cm" },
                        { text: " 215", value: "215", parentVal: "cm" },
                        { text: " 216", value: "216", parentVal: "cm" },
                        { text: " 217", value: "217", parentVal: "cm" },
                        { text: " 218", value: "218", parentVal: "cm" },
                        { text: " 219", value: "219", parentVal: "cm" },
                        { text: " 220", value: "220", parentVal: "cm" },
                        { text: " 221", value: "221", parentVal: "cm" },
                        { text: " 222", value: "222", parentVal: "cm" },
                        { text: " 223", value: "223", parentVal: "cm" },
                        { text: " 224", value: "224", parentVal: "cm" },
                        { text: " 225", value: "225", parentVal: "cm" },
                        { text: " 226", value: "226", parentVal: "cm" },
                        { text: " 227", value: "227", parentVal: "cm" },
                        { text: " 228", value: "228", parentVal: "cm" },
                        { text: " 229", value: "229", parentVal: "cm" },
                        { text: " 230", value: "230", parentVal: "cm" },
                        { text: " 231", value: "231", parentVal: "cm" },
                        { text: " 232", value: "232", parentVal: "cm" },
                        { text: " 233", value: "233", parentVal: "cm" },
                        { text: " 234", value: "234", parentVal: "cm" },
                        { text: " 235", value: "235", parentVal: "cm" },
                        { text: " 236", value: "236", parentVal: "cm" },
                        { text: " 237", value: "237", parentVal: "cm" },
                        { text: " 238", value: "238", parentVal: "cm" },
                        { text: " 239", value: "239", parentVal: "cm" },
                        { text: " 240", value: "240", parentVal: "cm" },
                        { text: " 241", value: "241", parentVal: "cm" },
                        { text: " 242", value: "242", parentVal: "cm" },
                        { text: " 243", value: "243", parentVal: "cm" }
                    ]
                },
                {
                    name: "inches",
                    parentCol: "Value",
                    options: [
                        { text: "0", value: "0", parentVal: "1" },
                        { text: "1", value: "1", parentVal: "1" },
                        { text: "2", value: "2", parentVal: "1" },
                        { text: "3", value: "3", parentVal: "1" },
                        { text: "4", value: "4", parentVal: "1" },
                        { text: "5", value: "5", parentVal: "1" },
                        { text: "6", value: "6", parentVal: "1" },
                        { text: "7", value: "7", parentVal: "1" },
                        { text: "8", value: "8", parentVal: "1" },
                        { text: "9", value: "9", parentVal: "1" },
                        { text: "10", value: "10", parentVal: "1" },
                        { text: "11", value: "11", parentVal: "1" },
                        { text: "12", value: "12", parentVal: "1" },
                        { text: "0", value: "0", parentVal: "2" },
                        { text: "1", value: "1", parentVal: "2" },
                        { text: "2", value: "2", parentVal: "2" },
                        { text: "3", value: "3", parentVal: "2" },
                        { text: "4", value: "4", parentVal: "2" },
                        { text: "5", value: "5", parentVal: "2" },
                        { text: "6", value: "6", parentVal: "2" },
                        { text: "7", value: "7", parentVal: "2" },
                        { text: "8", value: "8", parentVal: "2" },
                        { text: "9", value: "9", parentVal: "2" },
                        { text: "10", value: "10", parentVal: "2" },
                        { text: "11", value: "11", parentVal: "2" },
                        { text: "12", value: "12", parentVal: "2" },
                        { text: "0", value: "0", parentVal: "3" },
                        { text: "1", value: "1", parentVal: "3" },
                        { text: "2", value: "2", parentVal: "3" },
                        { text: "3", value: "3", parentVal: "3" },
                        { text: "4", value: "4", parentVal: "3" },
                        { text: "5", value: "5", parentVal: "3" },
                        { text: "6", value: "6", parentVal: "3" },
                        { text: "7", value: "7", parentVal: "3" },
                        { text: "8", value: "8", parentVal: "3" },
                        { text: "9", value: "9", parentVal: "3" },
                        { text: "10", value: "10", parentVal: "3" },
                        { text: "11", value: "11", parentVal: "3" },
                        { text: "12", value: "12", parentVal: "3" },
                        { text: "0", value: "0", parentVal: "4" },
                        { text: "1", value: "1", parentVal: "4" },
                        { text: "2", value: "2", parentVal: "4" },
                        { text: "3", value: "3", parentVal: "4" },
                        { text: "4", value: "4", parentVal: "4" },
                        { text: "5", value: "5", parentVal: "4" },
                        { text: "6", value: "6", parentVal: "4" },
                        { text: "7", value: "7", parentVal: "4" },
                        { text: "8", value: "8", parentVal: "4" },
                        { text: "9", value: "9", parentVal: "4" },
                        { text: "10", value: "10", parentVal: "4" },
                        { text: "11", value: "11", parentVal: "4" },
                        { text: "12", value: "12", parentVal: "4" },
                        { text: "0", value: "0", parentVal: "5" },
                        { text: "1", value: "1", parentVal: "5" },
                        { text: "2", value: "2", parentVal: "5" },
                        { text: "3", value: "3", parentVal: "5" },
                        { text: "4", value: "4", parentVal: "5" },
                        { text: "5", value: "5", parentVal: "5" },
                        { text: "6", value: "6", parentVal: "5" },
                        { text: "7", value: "7", parentVal: "5" },
                        { text: "8", value: "8", parentVal: "5" },
                        { text: "9", value: "9", parentVal: "5" },
                        { text: "10", value: "10", parentVal: "5" },
                        { text: "11", value: "11", parentVal: "5" },
                        { text: "12", value: "12", parentVal: "5" },
                        { text: "0", value: "0", parentVal: "6" },
                        { text: "1", value: "1", parentVal: "6" },
                        { text: "2", value: "2", parentVal: "6" },
                        { text: "3", value: "3", parentVal: "6" },
                        { text: "4", value: "4", parentVal: "6" },
                        { text: "5", value: "5", parentVal: "6" },
                        { text: "6", value: "6", parentVal: "6" },
                        { text: "7", value: "7", parentVal: "6" },
                        { text: "8", value: "8", parentVal: "6" },
                        { text: "9", value: "9", parentVal: "6" },
                        { text: "10", value: "10", parentVal: "6" },
                        { text: "11", value: "11", parentVal: "6" },
                        { text: "12", value: "12", parentVal: "6" },
                        { text: "0", value: "0", parentVal: "7" },
                        { text: "1", value: "1", parentVal: "7" },
                        { text: "2", value: "2", parentVal: "7" },
                        { text: "3", value: "3", parentVal: "7" },
                        { text: "4", value: "4", parentVal: "7" },
                        { text: "5", value: "5", parentVal: "7" },
                        { text: "6", value: "6", parentVal: "7" },
                        { text: "7", value: "7", parentVal: "7" },
                        { text: "8", value: "8", parentVal: "7" },
                        { text: "9", value: "9", parentVal: "7" },
                        { text: "10", value: "10", parentVal: "7" },
                        { text: "11", value: "11", parentVal: "7" },
                        { text: "12", value: "12", parentVal: "7" }
                        /*
                          // { text: "0", value: "0", parentVal: "8" },
                          // { text: "1", value: "1", parentVal: "8" },
                          // { text: "2", value: "2", parentVal: "8" },
                          // { text: "3", value: "3", parentVal: "8" },
                          // { text: "4", value: "4", parentVal: "8" },
                          // { text: "5", value: "5", parentVal: "8" },
                          // { text: "6", value: "6", parentVal: "8" },
                          // { text: "7", value: "7", parentVal: "8" },
                          // { text: "8", value: "8", parentVal: "8" },
                          // { text: "9", value: "9", parentVal: "8" },
                          // { text: "10", value: "10", parentVal: "8" },
                          // { text: "11", value: "11", parentVal: "8" }
                          // // { text: " 12", value: "12", parentVal: "8" },
                
                          { text: "0", value: "0", parentVal: "130" },
                          { text: "0", value: "0", parentVal: "141" },
                          { text: "0", value: "0", parentVal: "142" },
                          { text: "0", value: "0", parentVal: "143" },
                          { text: "0", value: "0", parentVal: "144" },
                          { text: "0", value: "0", parentVal: "145" },
                          { text: "0", value: "0", parentVal: "146" },
                          { text: "0", value: "0", parentVal: "147" },
                          { text: "0", value: "0", parentVal: "148" },
                          { text: "0", value: "0", parentVal: "149" },
                          { text: "0", value: "0", parentVal: "150" },
                          { text: "0", value: "0", parentVal: "151" },
                          { text: "0", value: "0", parentVal: "152" },
                          { text: "0", value: "0", parentVal: "153" },
                          { text: "0", value: "0", parentVal: "154" },
                          { text: "0", value: "0", parentVal: "155" },
                          { text: "0", value: "0", parentVal: "156" },
                          { text: "0", value: "0", parentVal: "157" },
                          { text: "0", value: "0", parentVal: "158" },
                          { text: "0", value: "0", parentVal: "159" },
                          { text: "0", value: "0", parentVal: "160" },
                          { text: "0", value: "0", parentVal: "161" },
                          { text: "0", value: "0", parentVal: "162" },
                          { text: "0", value: "0", parentVal: "163" },
                          { text: "0", value: "0", parentVal: "164" },
                          { text: "0", value: "0", parentVal: "165" },
                          { text: "0", value: "0", parentVal: "166" },
                          { text: "0", value: "0", parentVal: "167" },
                          { text: "0", value: "0", parentVal: "168" },
                          { text: "0", value: "0", parentVal: "169" },
                          { text: "0", value: "0", parentVal: "170" },
                          { text: "0", value: "0", parentVal: "171" },
                          { text: "0", value: "0", parentVal: "172" },
                          { text: "0", value: "0", parentVal: "173" },
                          { text: "0", value: "0", parentVal: "174" },
                          { text: "0", value: "0", parentVal: "175" },
                          { text: "0", value: "0", parentVal: "176" },
                          { text: "0", value: "0", parentVal: "177" },
                          { text: "0", value: "0", parentVal: "178" },
                          { text: "0", value: "0", parentVal: "179" },
                          { text: "0", value: "0", parentVal: "180" },
                          { text: "0", value: "0", parentVal: "181" },
                          { text: "0", value: "0", parentVal: "182" },
                          { text: "0", value: "0", parentVal: "183" },
                          { text: "0", value: "0", parentVal: "184" },
                          { text: "0", value: "0", parentVal: "185" },
                          { text: "0", value: "0", parentVal: "186" },
                          { text: "0", value: "0", parentVal: "187" },
                          { text: "0", value: "0", parentVal: "188" },
                          { text: "0", value: "0", parentVal: "189" },
                          { text: "0", value: "0", parentVal: "190" },
                          { text: "0", value: "0", parentVal: "191" },
                          { text: "0", value: "0", parentVal: "192" },
                          { text: "0", value: "0", parentVal: "193" },
                          { text: "0", value: "0", parentVal: "194" },
                          { text: "0", value: "0", parentVal: "195" },
                          { text: "0", value: "0", parentVal: "196" },
                          { text: "0", value: "0", parentVal: "197" },
                          { text: "0", value: "0", parentVal: "198" },
                          { text: "0", value: "0", parentVal: "199" },
                          { text: "0", value: "0", parentVal: "200" },
                          { text: "0", value: "0", parentVal: "201" },
                          { text: "0", value: "0", parentVal: "202" },
                          { text: "0", value: "0", parentVal: "203" },
                          { text: "0", value: "0", parentVal: "204" },
                          { text: "0", value: "0", parentVal: "205" },
                          { text: "0", value: "0", parentVal: "206" },
                          { text: "0", value: "0", parentVal: "207" },
                          { text: "0", value: "0", parentVal: "208" },
                          { text: "0", value: "0", parentVal: "209" },
                          { text: "0", value: "0", parentVal: "210" },
                          { text: "0", value: "0", parentVal: "211" },
                          { text: "0", value: "0", parentVal: "212" },
                          { text: "0", value: "0", parentVal: "213" },
                          { text: "0", value: "0", parentVal: "214" },
                          { text: "0", value: "0", parentVal: "215" },
                          { text: "0", value: "0", parentVal: "216" },
                          { text: "0", value: "0", parentVal: "217" },
                          { text: "0", value: "0", parentVal: "218" },
                          { text: "0", value: "0", parentVal: "219" },
                          { text: "0", value: "0", parentVal: "220" },
                          { text: "0", value: "0", parentVal: "221" },
                          { text: "0", value: "0", parentVal: "222" },
                          { text: "0", value: "0", parentVal: "223" },
                          { text: "0", value: "0", parentVal: "224" },
                          { text: "0", value: "0", parentVal: "225" },
                          { text: "0", value: "0", parentVal: "226" },
                          { text: "0", value: "0", parentVal: "227" },
                          { text: "0", value: "0", parentVal: "228" },
                          { text: "0", value: "0", parentVal: "229" },
                          { text: "0", value: "0", parentVal: "230" },
                          { text: "0", value: "0", parentVal: "231" },
                          { text: "0", value: "0", parentVal: "232" },
                          { text: "0", value: "0", parentVal: "233" },
                          { text: "0", value: "0", parentVal: "234" },
                          { text: "0", value: "0", parentVal: "235" },
                          { text: "0", value: "0", parentVal: "236" },
                          { text: "0", value: "0", parentVal: "237" },
                          { text: "0", value: "0", parentVal: "238" },
                          { text: "0", value: "0", parentVal: "239" },
                          { text: "0", value: "0", parentVal: "240" },
                          { text: "0", value: "0", parentVal: "241" },
                          { text: "0", value: "0", parentVal: "242" },
                          { text: "0", value: "0", parentVal: "243" }*/
                    ]
                }
            ];
        },
        enumerable: true,
        configurable: true
    });
    WellnessConstants.GetFormatedDate = function (value, fromCall) {
        var curStartDate = new Date(value);
        console.log(curStartDate, fromCall);
        var months = [
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December",
        ];
        var WeekDays = [
            "Sunday",
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday"
        ];
        var retDate = WeekDays[curStartDate.getDay()] +
            ", " +
            months[curStartDate.getMonth()] +
            " " +
            curStartDate.getDate() +
            ", " +
            curStartDate.getFullYear(); //curDate.toDateString();
        console.log(retDate, 'Return dates');
        return retDate;
    };
    return WellnessConstants;
}());

//# sourceMappingURL=wellnessconstant.js.map

/***/ }),

/***/ 469:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* unused harmony export createTranslateLoader */
/* unused harmony export provideSettings */
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_common_http__ = __webpack_require__(64);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_platform_browser__ = __webpack_require__(45);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__ionic_native_splash_screen__ = __webpack_require__(207);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__ionic_native_status_bar__ = __webpack_require__(209);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__ionic_native_wheel_selector__ = __webpack_require__(483);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__ionic_storage__ = __webpack_require__(210);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__ngx_translate_core__ = __webpack_require__(34);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__ngx_translate_http_loader__ = __webpack_require__(506);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__angular_forms__ = __webpack_require__(18);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_11__mocks_providers_items__ = __webpack_require__(259);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_12__providers__ = __webpack_require__(15);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_13__app_component__ = __webpack_require__(567);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_14__ionic_native_unique_device_id__ = __webpack_require__(335);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_15__pages_welcome_welcome_module__ = __webpack_require__(294);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_16__pages_hra_body_hra_body_module__ = __webpack_require__(267);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_17__pages_my_wellness_wallet_hra_qa_hra_qa_module__ = __webpack_require__(278);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_18__pages_my_hra_my_hra_module__ = __webpack_require__(271);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_19__pages_intro_intro_module__ = __webpack_require__(269);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_20__pages_intro_video_intro_video_module__ = __webpack_require__(268);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_21__pages_signup_signup_module__ = __webpack_require__(287);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_22__pages_login_login_module__ = __webpack_require__(270);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_23__pages_dashboard_dashboard_module__ = __webpack_require__(256);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_24__pages_my_wellness_wallet_mywellnessplan_mywellnessplan_module__ = __webpack_require__(281);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_25__pages_my_wellness_wallet_hra_result_hra_result_module__ = __webpack_require__(279);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_26__pages_my_wellness_wallet_my_wellness_wallet_module__ = __webpack_require__(280);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_27__pages_my_profile_profile_module__ = __webpack_require__(273);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_28__pages_my_trackers_mytracker_module__ = __webpack_require__(272);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_29__pages_my_wellness_wallet_riskdetail_riskdetail_module__ = __webpack_require__(282);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_30__ionic_native_camera__ = __webpack_require__(274);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_31__pages_forgetpassword_forgetpassword_module__ = __webpack_require__(257);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_32__pages_notification_notification_module__ = __webpack_require__(284);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_33__pages_templates_template_module__ = __webpack_require__(293);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_34__pages_notificationmsg_notimsg_module__ = __webpack_require__(283);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_35__angular_fire__ = __webpack_require__(74);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_36__angular_fire_firestore__ = __webpack_require__(338);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_37__providers_settings_fcm_service__ = __webpack_require__(336);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_38__ionic_native_firebase__ = __webpack_require__(337);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_39__providers_settings_alertmessage_service__ = __webpack_require__(149);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_40__pages_step_challenges_step_dashboard_module__ = __webpack_require__(291);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_41__pages_step_challenges_log_overview_logoverview_module__ = __webpack_require__(288);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_42__pages_step_challenges_log_steps_logsteps_module__ = __webpack_require__(290);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_43__providers_steps_stepschallenge__ = __webpack_require__(260);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_44__pages_step_challenges_addlogs_addlogs_module__ = __webpack_require__(285);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_45__pages_step_challenges_stepchallengehistory_stepchallengehistory_module__ = __webpack_require__(292);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};














































// The translate loader needs to know where to load i18n files
// in Ionic's static asset pipeline.
function createTranslateLoader(http) {
    return new __WEBPACK_IMPORTED_MODULE_8__ngx_translate_http_loader__["a" /* TranslateHttpLoader */](http, './assets/i18n/', '.json');
}
// const config = {
//   apiKey: "AIzaSyCm7w_4jwHl7PlmgUTYZrvw4FOFoZRVHhE",
//   authDomain: "meschinowellness-ff60f.firebaseapp.com",
//   databaseURL: "https://meschinowellness-ff60f.firebaseio.com",
//   projectId: "meschinowellness-ff60f",
//   storageBucket: "meschinowellness-ff60f.appspot.com",
//   messagingSenderId: "511777519229",
// };
var config = {
    apiKey: "AIzaSyCxxXSgvooUrArq-9boQWd1bOabjuLQTZQ",
    authDomain: "meschinowellness-61e33.firebaseapp.com",
    databaseURL: "https://meschinowellness-61e33.firebaseio.com",
    projectId: "meschinowellness-61e33",
    storageBucket: "meschinowellness-61e33.appspot.com",
    messagingSenderId: "414018168893"
};
//  appId: "1:414018168893:web:11a33e2116c614a104a254",
//   measurementId: "G-0HRV1R9WPD"
function provideSettings(storage) {
    /**
     * The Settings provider takes a set of default settings for your app.
     *
     * You can add new settings options at any time. Once the settings are saved,
     * these values will not overwrite the saved values (this can be done manually if desired).
     */
    return;
    // return new Settings(storage, {
    //   option1: true,
    //   option2: 'Ionitron J. Framework',
    //   option3: '3',
    //   option4: 'Hello'
    // });
}
var AppModule = /** @class */ (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_1__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_13__app_component__["a" /* MyApp */]
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_2__angular_platform_browser__["a" /* BrowserModule */],
                __WEBPACK_IMPORTED_MODULE_0__angular_common_http__["b" /* HttpClientModule */],
                __WEBPACK_IMPORTED_MODULE_10__angular_forms__["ReactiveFormsModule"],
                __WEBPACK_IMPORTED_MODULE_19__pages_intro_intro_module__["IntroPageModule"],
                __WEBPACK_IMPORTED_MODULE_20__pages_intro_video_intro_video_module__["IntroVideoPageModule"],
                __WEBPACK_IMPORTED_MODULE_21__pages_signup_signup_module__["SignupPageModule"],
                __WEBPACK_IMPORTED_MODULE_22__pages_login_login_module__["LoginPageModule"],
                __WEBPACK_IMPORTED_MODULE_15__pages_welcome_welcome_module__["WelcomePageModule"],
                __WEBPACK_IMPORTED_MODULE_16__pages_hra_body_hra_body_module__["HraBodyPageModule"],
                __WEBPACK_IMPORTED_MODULE_17__pages_my_wellness_wallet_hra_qa_hra_qa_module__["HraQaPageModule"],
                __WEBPACK_IMPORTED_MODULE_18__pages_my_hra_my_hra_module__["MyHraPageModule"],
                __WEBPACK_IMPORTED_MODULE_25__pages_my_wellness_wallet_hra_result_hra_result_module__["HraResultPageModule"],
                __WEBPACK_IMPORTED_MODULE_29__pages_my_wellness_wallet_riskdetail_riskdetail_module__["RiskDetailPageModule"],
                __WEBPACK_IMPORTED_MODULE_23__pages_dashboard_dashboard_module__["DashboardPageModule"],
                __WEBPACK_IMPORTED_MODULE_24__pages_my_wellness_wallet_mywellnessplan_mywellnessplan_module__["MyWellnessPlanPageModule"],
                __WEBPACK_IMPORTED_MODULE_26__pages_my_wellness_wallet_my_wellness_wallet_module__["MyWellnessWalletPageModule"],
                __WEBPACK_IMPORTED_MODULE_44__pages_step_challenges_addlogs_addlogs_module__["AddlogsModule"],
                __WEBPACK_IMPORTED_MODULE_41__pages_step_challenges_log_overview_logoverview_module__["LogOtherActivitiesPageModule"],
                __WEBPACK_IMPORTED_MODULE_42__pages_step_challenges_log_steps_logsteps_module__["LogStepsPageModule"],
                __WEBPACK_IMPORTED_MODULE_45__pages_step_challenges_stepchallengehistory_stepchallengehistory_module__["StepChallengeHistoryPageModule"],
                __WEBPACK_IMPORTED_MODULE_40__pages_step_challenges_step_dashboard_module__["StepDashboardPageModule"],
                __WEBPACK_IMPORTED_MODULE_27__pages_my_profile_profile_module__["ProfilePageModule"],
                __WEBPACK_IMPORTED_MODULE_28__pages_my_trackers_mytracker_module__["MyTrackerPageModule"],
                __WEBPACK_IMPORTED_MODULE_31__pages_forgetpassword_forgetpassword_module__["ForgetPageModule"],
                __WEBPACK_IMPORTED_MODULE_32__pages_notification_notification_module__["NotificationPageModule"],
                __WEBPACK_IMPORTED_MODULE_34__pages_notificationmsg_notimsg_module__["NotificationMsgPageModule"],
                __WEBPACK_IMPORTED_MODULE_33__pages_templates_template_module__["TemplatePageModule"],
                __WEBPACK_IMPORTED_MODULE_7__ngx_translate_core__["b" /* TranslateModule */].forRoot({
                    loader: {
                        provide: __WEBPACK_IMPORTED_MODULE_7__ngx_translate_core__["a" /* TranslateLoader */],
                        useFactory: (createTranslateLoader),
                        deps: [__WEBPACK_IMPORTED_MODULE_0__angular_common_http__["a" /* HttpClient */]]
                    }
                }),
                __WEBPACK_IMPORTED_MODULE_9_ionic_angular__["IonicModule"].forRoot(__WEBPACK_IMPORTED_MODULE_13__app_component__["a" /* MyApp */], {
                    mode: "md"
                }, {
                    links: [
                        { loadChildren: '../pages/dashboard/dashboard.module#DashboardPageModule', name: 'DashboardPage', segment: 'dashboard', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/forgetpassword/forgetpassword.module#ForgetPageModule', name: 'ForgetPasswordPage', segment: 'forgetpassword', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/hra-body/hra-body.module#HraBodyPageModule', name: 'HraBodyPage', segment: 'hra-body', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/intro-video/intro-video.module#IntroVideoPageModule', name: 'IntroVideoPage', segment: 'intro-video', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/intro/intro.module#IntroPageModule', name: 'IntroPage', segment: 'intro', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/login/login.module#LoginPageModule', name: 'LoginPage', segment: 'login', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/menu/menu.module#MenuPageModule', name: 'MenuPage', segment: 'menu', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/my-hra/my-hra.module#MyHraPageModule', name: 'MyHraPage', segment: 'my-hra', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/my-trackers/mytracker.module#MyTrackerPageModule', name: 'MyTrackerPage', segment: 'mytracker', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/my-profile/profile.module#ProfilePageModule', name: 'ProfilePage', segment: 'profile', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/my-wellness-wallet/hra-qa/hra-qa.module#HraQaPageModule', name: 'HraQaPage', segment: 'hra-qa', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/my-wellness-wallet/hra-result/hra-result.module#HraResultPageModule', name: 'MyHraResultPage', segment: 'hra-result', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/my-wellness-wallet/my-wellness-wallet.module#MyWellnessWalletPageModule', name: 'MyWellnessWalletPage', segment: 'my-wellness-wallet', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/my-wellness-wallet/mywellnessplan/mywellnessplan.module#MyWellnessPlanPageModule', name: 'MyWellnessPlanPage', segment: 'mywellnessplan', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/my-wellness-wallet/riskdetail/riskdetail.module#RiskDetailPageModule', name: 'RiskDetailPage', segment: 'riskdetail', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/notificationmsg/notimsg.module#NotificationMsgPageModule', name: 'NotificationMsgPage', segment: 'notimsg', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/notification/notification.module#NotificationPageModule', name: 'NotificationPage', segment: 'notification', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/step-challenges/addlogs/addlogs.module#AddlogsModule', name: 'AddlogsPage', segment: 'addlogs', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/signup/signup.module#SignupPageModule', name: 'SignupPage', segment: 'signup', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/step-challenges/log-overview/logoverview.module#LogOtherActivitiesPageModule', name: 'LogOverviewPage', segment: 'logoverview', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/step-challenges/log-steps/logsteps.module#LogStepsPageModule', name: 'LogStepsPage', segment: 'logsteps', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/step-challenges/step-dashboard.module#StepDashboardPageModule', name: 'StepDashboardPage', segment: 'step-dashboard', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/step-challenges/stepchallengehistory/stepchallengehistory.module#StepChallengeHistoryPageModule', name: 'StepChallengeHistoryPage', segment: 'stepchallengehistory', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/templates/template.module#TemplatePageModule', name: 'TemplatePage', segment: 'template', priority: 'low', defaultHistory: [] },
                        { loadChildren: '../pages/welcome/welcome.module#WelcomePageModule', name: 'WelcomePage', segment: 'welcome', priority: 'low', defaultHistory: [] }
                    ]
                }),
                __WEBPACK_IMPORTED_MODULE_6__ionic_storage__["a" /* IonicStorageModule */].forRoot(),
                __WEBPACK_IMPORTED_MODULE_35__angular_fire__["a" /* AngularFireModule */].initializeApp(config),
                __WEBPACK_IMPORTED_MODULE_36__angular_fire_firestore__["AngularFirestoreModule"]
            ],
            bootstrap: [__WEBPACK_IMPORTED_MODULE_9_ionic_angular__["IonicApp"]],
            entryComponents: [
                __WEBPACK_IMPORTED_MODULE_13__app_component__["a" /* MyApp */]
            ],
            providers: [
                __WEBPACK_IMPORTED_MODULE_12__providers__["a" /* Api */],
                __WEBPACK_IMPORTED_MODULE_11__mocks_providers_items__["a" /* Items */],
                __WEBPACK_IMPORTED_MODULE_12__providers__["b" /* HraService */],
                __WEBPACK_IMPORTED_MODULE_12__providers__["c" /* NotificationService */],
                __WEBPACK_IMPORTED_MODULE_12__providers__["f" /* UserService */],
                __WEBPACK_IMPORTED_MODULE_43__providers_steps_stepschallenge__["a" /* StepChallengeService */],
                __WEBPACK_IMPORTED_MODULE_30__ionic_native_camera__["a" /* Camera */],
                __WEBPACK_IMPORTED_MODULE_3__ionic_native_splash_screen__["a" /* SplashScreen */],
                __WEBPACK_IMPORTED_MODULE_4__ionic_native_status_bar__["a" /* StatusBar */],
                __WEBPACK_IMPORTED_MODULE_0__angular_common_http__["a" /* HttpClient */],
                __WEBPACK_IMPORTED_MODULE_0__angular_common_http__["b" /* HttpClientModule */],
                __WEBPACK_IMPORTED_MODULE_5__ionic_native_wheel_selector__["a" /* WheelSelector */],
                __WEBPACK_IMPORTED_MODULE_38__ionic_native_firebase__["a" /* Firebase */],
                __WEBPACK_IMPORTED_MODULE_37__providers_settings_fcm_service__["a" /* FcmService */],
                __WEBPACK_IMPORTED_MODULE_39__providers_settings_alertmessage_service__["a" /* AlertMessagesService */],
                { provide: __WEBPACK_IMPORTED_MODULE_12__providers__["d" /* Settings */], useFactory: provideSettings, deps: [__WEBPACK_IMPORTED_MODULE_6__ionic_storage__["b" /* Storage */]] },
                // Keep this to enable Ionic's runtime error handling during development
                { provide: __WEBPACK_IMPORTED_MODULE_1__angular_core__["ErrorHandler"], useClass: __WEBPACK_IMPORTED_MODULE_9_ionic_angular__["IonicErrorHandler"] },
                __WEBPACK_IMPORTED_MODULE_14__ionic_native_unique_device_id__["a" /* UniqueDeviceID */],
                __WEBPACK_IMPORTED_MODULE_10__angular_forms__["NgForm"]
            ]
        })
    ], AppModule);
    return AppModule;
}());

//# sourceMappingURL=app.module.js.map

/***/ }),

/***/ 531:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return DashboardPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


/**
 * Generated class for the DashboardPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
var DashboardPage = /** @class */ (function () {
    function DashboardPage(navCtrl, navParams) {
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.account = {
            FirstName: "",
            LastName: ""
        };
        this.account.FirstName = localStorage.getItem("FirstName");
        this.account.LastName = localStorage.getItem("LastName");
    }
    DashboardPage.prototype.ionViewDidLoad = function () {
        console.log("ionViewDidLoad DashboardPage");
    };
    DashboardPage.prototype.gotoUrl = function (navurl) {
        this.navCtrl.setRoot(navurl);
    };
    DashboardPage = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: "page-dashboard",template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\dashboard\dashboard.html"*/'<ion-header>\n\n  <ion-navbar color="primary">\n    <button ion-button menuToggle icon-only>\n      <ion-icon name=\'menu\'></ion-icon>\n    </button>\n    <ion-title>Dashboard</ion-title>\n    <ion-buttons end>\n      <button id="notification-button" ion-button icon-only>\n         <ion-icon name="notifications">\n          <ion-badge id="notifications-badge" color="danger">7</ion-badge> \n        </ion-icon>\n      </button>\n    </ion-buttons>\n  </ion-navbar>\n</ion-header>\n<ion-content>\n\n  <ion-card>\n    <ion-card-content>\n      <ion-item>\n       <a (click)="gotoUrl(\'MyWellnessPlanPage\')"> My Health Risk</a>\n      </ion-item>\n    </ion-card-content>\n  </ion-card>\n</ion-content>\n<ion-footer>\n\n</ion-footer>'/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\dashboard\dashboard.html"*/
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavController"], __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavParams"]])
    ], DashboardPage);
    return DashboardPage;
}());

//# sourceMappingURL=dashboard.js.map

/***/ }),

/***/ 533:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return Item; });
/**
 * A generic model that our Master-Detail pages list, create, and delete.
 *
 * Change "Item" to the noun your app will use. For example, a "Contact," or a
 * "Customer," or an "Animal," or something like that.
 *
 * The Items service manages creating instances of Item, so go ahead and rename
 * that something that fits your app as well.
 */
var Item = /** @class */ (function () {
    function Item(fields) {
        // Quick and dirty extend/assign fields to this model
        for (var f in fields) {
            // @ts-ignore
            this[f] = fields[f];
        }
    }
    return Item;
}());

//# sourceMappingURL=item.js.map

/***/ }),

/***/ 534:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return Settings; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__ionic_storage__ = __webpack_require__(210);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


/**
 * A simple settings/config class for storing key/value pairs with persistence.
 */
var Settings = /** @class */ (function () {
    function Settings(storage, defaults) {
        this.storage = storage;
        this.SETTINGS_KEY = '_settings';
        this._defaults = defaults;
    }
    Settings.prototype.load = function () {
        var _this = this;
        return this.storage.get(this.SETTINGS_KEY).then(function (value) {
            if (value) {
                _this.settings = value;
                return _this._mergeDefaults(_this._defaults);
            }
            else {
                return _this.setAll(_this._defaults).then(function (val) {
                    _this.settings = val;
                });
            }
        });
    };
    Settings.prototype._mergeDefaults = function (defaults) {
        for (var k in defaults) {
            if (!(k in this.settings)) {
                this.settings[k] = defaults[k];
            }
        }
        return this.setAll(this.settings);
    };
    Settings.prototype.merge = function (settings) {
        for (var k in settings) {
            this.settings[k] = settings[k];
        }
        return this.save();
    };
    Settings.prototype.setValue = function (key, value) {
        this.settings[key] = value;
        return this.storage.set(this.SETTINGS_KEY, this.settings);
    };
    Settings.prototype.setAll = function (value) {
        return this.storage.set(this.SETTINGS_KEY, value);
    };
    Settings.prototype.getValue = function (key) {
        return this.storage.get(this.SETTINGS_KEY)
            .then(function (settings) {
            return settings[key];
        });
    };
    Settings.prototype.save = function () {
        return this.setAll(this.settings);
    };
    Object.defineProperty(Settings.prototype, "allSettings", {
        get: function () {
            return this.settings;
        },
        enumerable: true,
        configurable: true
    });
    Settings = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__ionic_storage__["b" /* Storage */], Object])
    ], Settings);
    return Settings;
}());

//# sourceMappingURL=settings.js.map

/***/ }),

/***/ 535:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return HraService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_common_http__ = __webpack_require__(64);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__api_api__ = __webpack_require__(71);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



/*
  Generated class for the HraProvider provider.

  See https://angular.io/guide/dependency-injection for more info on providers
  and Angular DI.
*/
var HraService = /** @class */ (function () {
    function HraService(http, api) {
        this.http = http;
        this.api = api;
        console.log("HraProvider Provider");
    }
    HraService.prototype.GetHraSections = function (accountInfo) {
        var _this = this;
        //this.api.url = "../../assets/data";
        var seq = this.api
            .post("api/WellnessAPI/GetHRASections", accountInfo)
            .share();
        seq.subscribe(function (res) {
            // If the API returned a successful response, mark the user as logged in
            _this.hraSections = res;
        }, function (err) {
            console.error("ERROR", err);
        });
        return seq;
    };
    HraService.prototype.GetHRAQuestionDetails = function (accountInfo) {
        var _this = this;
        var seq = this.api
            .post("api/WellnessAPI/GetHRAQuestionDetailsBySection", accountInfo)
            .share();
        seq.subscribe(function (res) {
            // If the API returned a successful response, mark the user as logged in
            _this.hraQuestionDetail = res;
        }, function (err) {
            console.error("ERROR", err);
        });
        return seq;
    };
    HraService.prototype.SaveHRAResponse = function (accountInfo) {
        var _this = this;
        var seq = this.api
            .post("api/WellnessAPI/SaveHRAResponse", accountInfo)
            .share();
        seq.subscribe(function (res) {
            // If the API returned a successful response, mark the user as logged in
            _this.hraQuestionDetail = res;
        }, function (err) {
            console.error("ERROR", err);
        });
        return seq;
    };
    HraService.prototype.GetRiskReportNum = function (accountInfo) {
        var _this = this;
        var seq = this.api
            .post("api/WellnessAPI/EvaluateRiskReportAPI", accountInfo)
            .share();
        seq.subscribe(function (res) {
            // If the API returned a successful response, mark the user as logged in
            _this.hraRiskReport = res;
        }, function (err) {
            console.error("ERROR", err);
        });
        return seq;
    };
    HraService.prototype.GetHraReport = function (accountInfo) {
        var _this = this;
        var seq = this.api
            .post("api/WellnessAPI/GetIdentifiedConditionsAPI", accountInfo)
            .share();
        seq.subscribe(function (res) {
            // If the API returned a successful response, mark the user as logged in
            _this.hraResults = res;
        }, function (err) {
            console.error("ERROR", err);
        });
        return seq;
    };
    HraService.prototype.GetMajorHealthRisks = function (accountInfo) {
        var _this = this;
        var seq = this.api
            .post("api/WellnessAPI/GetMajorHealthRisks", accountInfo)
            .share();
        seq.subscribe(function (res) {
            // If the API returned a successful response, mark the user as logged in
            _this.MajorHealthRisks = res;
        }, function (err) {
            console.error("ERROR", err);
        });
        return seq;
    };
    HraService.prototype.GetHealthRiskDetail = function (accountInfo) {
        var _this = this;
        var seq = this.api
            .post("api/WellnessAPI/GetHealthRiskDetail", accountInfo)
            .share();
        seq.subscribe(function (res) {
            // If the API returned a successful response, mark the user as logged in
            _this.HealthRiskDetail = res;
        }, function (err) {
            console.error("ERROR", err);
        });
        return seq;
    };
    HraService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_1__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_0__angular_common_http__["a" /* HttpClient */], __WEBPACK_IMPORTED_MODULE_2__api_api__["a" /* Api */]])
    ], HraService);
    return HraService;
}());

//# sourceMappingURL=hra.js.map

/***/ }),

/***/ 536:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return NotificationService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_rxjs_add_operator_toPromise__ = __webpack_require__(146);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_rxjs_add_operator_toPromise___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_0_rxjs_add_operator_toPromise__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__api_api__ = __webpack_require__(71);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var NotificationService = /** @class */ (function () {
    function NotificationService(api) {
        this.api = api;
    }
    NotificationService.prototype.UpdateIsReadPushNotificationDetail = function (accountInfo) {
        var seq = this.api
            .post("api/WellnessAPI/UpdateIsReadPushNotificationDetail", accountInfo)
            .share();
        seq.subscribe(function (res) { }, function (err) {
            console.error("ERROR", err);
        });
        return seq;
    };
    NotificationService.prototype.DeleteUserPushNotificationDetail = function (accountInfo) {
        var seq = this.api
            .post("api/WellnessAPI/DeleteUserPushNotificationDetail", accountInfo)
            .share();
        seq.subscribe(function (res) { }, function (err) {
            console.error("ERROR", err);
        });
        return seq;
    };
    NotificationService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_1__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_2__api_api__["a" /* Api */]])
    ], NotificationService);
    return NotificationService;
}());

//# sourceMappingURL=notification.js.map

/***/ }),

/***/ 538:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return UserService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_rxjs_add_operator_toPromise__ = __webpack_require__(146);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_rxjs_add_operator_toPromise___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_0_rxjs_add_operator_toPromise__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__api_api__ = __webpack_require__(71);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var UserService = /** @class */ (function () {
    function UserService(api) {
        this.api = api;
    }
    /**
     * Send a POST request to our login endpoint with the data
     * the user entered on the form.
     */
    UserService.prototype.login = function (accountInfo) {
        var _this = this;
        localStorage.setItem('Password', accountInfo.Password);
        localStorage.setItem('UserName', accountInfo.UserName);
        var seq = this.api.post('api/WellnessAPI/UserLogin', accountInfo).share();
        seq.subscribe(function (res) {
            //console.log("*********");
            console.log(res);
            // If the API returned a successful response, mark the user as logged in
            if (res.SystemStatus == "Success") {
                _this._loggedIn(res);
            }
        }, function (err) {
            console.log('ERROR', err);
            //   alert("call provider error " + err );
            //   for(var key in err) {
            //     alert('call provider error  : key: ' + key + '\n' + 'value: ' + err[key]);
            // }
        });
        return seq;
    };
    // Get Bio age/ reward points and mhrscore
    UserService.prototype.getUserData = function (accountInfo) {
        var seq = this.api.post('api/WellnessAPI/GetUserData', accountInfo).share();
        seq.subscribe(function (res) {
        }, function (err) {
            console.error('ERROR', err);
        });
        return seq;
    };
    /**
     * Send a POST request to our signup endpoint with the data
     * the user entered on the form.
     */
    UserService.prototype.signup = function (accountInfo) {
        var _this = this;
        var seq = this.api.post('api/WellnessAPI/AppUserRegister', accountInfo).share();
        seq.subscribe(function (res) {
            // If the API returned a successful response, mark the user as logged in
            _this._loggedIn(res);
        }, function (err) {
            console.error('ERROR', err);
        });
        return seq;
    };
    UserService.prototype.forgetpassword = function (accountInfo) {
        var seq = this.api.post('api/WellnessAPI/UserForgotPassword', accountInfo).share();
        seq.subscribe(function (res) {
        }, function (err) {
            console.error('ERROR', err);
        });
        return seq;
    };
    UserService.prototype.SaveUserAgreeTermsCondition = function (accountInfo) {
        var seq = this.api.post('api/WellnessAPI/SaveUserAgreeTermsCondition', accountInfo).share();
        seq.subscribe(function (res) { }, function (err) {
            console.error('ERROR', err);
        });
        return seq;
    };
    UserService.prototype.SaveUserTokenIdData = function (accountInfo) {
        var seq = this.api.post('api/WellnessAPI/SaveUserTokenIdData', accountInfo).share();
        seq.subscribe(function (res) {
            //alert('success in service')   
        }, function (err) {
            console.error('ERROR', err);
            //alert('error is service');
        });
        return seq;
    };
    UserService.prototype.SaveUserPushNotificationOnOff = function (accountInfo) {
        var seq = this.api.post('api/WellnessAPI/SaveUserPushNotificationOnOff', accountInfo).share();
        seq.subscribe(function (res) { }, function (err) {
            console.error('ERROR', err);
        });
        return seq;
    };
    UserService.prototype.GetUserPushNotificationDetail = function (accountInfo) {
        var seq = this.api.post('api/WellnessAPI/GetUserPushNotificationDetail', accountInfo).share();
        seq.subscribe(function (res) {
        }, function (err) {
            console.error('ERROR', err);
        });
        return seq;
    };
    UserService.prototype.GetPushNotificationCount = function (accountInfo) {
        var seq = this.api.post('api/WellnessAPI/GetPushNotificationCount', accountInfo).share();
        seq.subscribe(function (res) {
        }, function (err) {
            console.error('ERROR', err);
        });
        return seq;
    };
    /**
     * Send a POST request to our signup endpoint with the data
     * the user entered on the form.
     */
    UserService.prototype.checkEmailExist = function (accountInfo) {
        var _this = this;
        var seq = this.api.post('api/WellnessAPI/IsOnboardUserEmailExists', accountInfo).share();
        seq.subscribe(function (res) {
            // If the API returned a successful response, mark the user as logged in
            _this._loggedIn(res);
        }, function (err) {
            console.error('ERROR', err);
        });
        return seq;
    };
    /**
     * Log the user out, which forgets the session
     */
    // logout() {
    //   this._user = null;
    //   localStorage.clear();
    // }
    /**
     * Process a login/signup response to store user data
     */
    UserService.prototype._loggedIn = function (resp) {
        localStorage.setItem('UserInfo', JSON.stringify(resp));
        localStorage.setItem('FirstName', resp.FirstName);
        localStorage.setItem('LastName', resp.LastName);
        localStorage.setItem('SecretToken', resp.SecretToken);
        localStorage.setItem('ProfileImage', resp.ProfileImage);
        localStorage.setItem('RewardPoint', resp.RewardPoint);
        localStorage.setItem('bio_age', resp.bio_age);
        localStorage.setItem('mhrs_score', resp.mhrs_score);
        //localStorage.setItem('SecretToken','77d344e9-dbcb-4975-a76a-ab8a8256c624');
        localStorage.setItem('Gender', resp.Gender);
        localStorage.setItem('Height', resp.Height);
        localStorage.setItem('BirthDate', resp.BirthDate);
        localStorage.setItem('IsAgreeTermsCondition', resp.IsAgreeTermsCondition);
        localStorage.setItem('IsHRACompleted', resp.IsHRACompleted);
        localStorage.setItem('UserAccessLevel', resp.UserAccessLevel);
        localStorage.setItem('PushNotificationYesNo', resp.PushNotificationYesNo);
        localStorage.setItem('PhoneNumber', resp.PhoneNumber);
        localStorage.setItem('IsCompanySMSEnable', resp.IsCompanySMSEnable);
        localStorage.setItem('IsUserSMSEnable', resp.IsUserSMSEnable);
        //   this.uniqueDeviceID.get()
        // .then((uuid: any) => localStorage.setItem('deviceid',uuid))
        // .catch((error: any) => console.log(error));
        this._user = resp;
    };
    UserService.prototype._LogoutUser = function () {
        var remuser = localStorage.getItem("remuser");
        var rempwd = localStorage.getItem("rempwd");
        var remUUID = localStorage.getItem("remUUID");
        var token = localStorage.getItem("FCMToken");
        var deviceType = localStorage.getItem("DeviceType");
        localStorage.clear();
        localStorage.setItem("FCMToken", token);
        localStorage.setItem("DeviceType", deviceType);
        localStorage.setItem("remUUID", remUUID);
        localStorage.setItem("remuser", remuser);
        localStorage.setItem("rempwd", rempwd);
        localStorage.setItem("deviceid", remUUID);
    };
    UserService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_1__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_2__api_api__["a" /* Api */]])
    ], UserService);
    return UserService;
}());

//# sourceMappingURL=user.js.map

/***/ }),

/***/ 539:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return HraBodyPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__providers_settings_wellnessconstant__ = __webpack_require__(44);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



/**
 * Generated class for the HraBodyPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
var HraBodyPage = /** @class */ (function () {
    function HraBodyPage(navCtrl, navParams) {
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.account = {
            pounds: 150,
            inches: 38,
            deviceid: "",
            SecretToken: ""
        };
        this.waistData = __WEBPACK_IMPORTED_MODULE_2__providers_settings_wellnessconstant__["a" /* WellnessConstants */].waistData;
        this.weightData = __WEBPACK_IMPORTED_MODULE_2__providers_settings_wellnessconstant__["a" /* WellnessConstants */].weightData;
    }
    HraBodyPage.prototype.ionViewDidLoad = function () {
        console.log('ionViewDidLoad HraBodyPage');
    };
    HraBodyPage.prototype.submit = function () {
    };
    HraBodyPage = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'page-hra-body',template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\hra-body\hra-body.html"*/'\n\n\n<ion-content >\n    <div class="splashbg">\n        <ion-header >\n            <ion-navbar>\n              <ion-title>Health Risk Assessment <br>\n                  (HRA) </ion-title>\n            </ion-navbar>\n          </ion-header>\n          <div class="subtitle">\n          <p>You can pause and return to complete the HRA, however, you must complete the entire HRA before it can be processed </p>\n        </div>\n      </div>\n      <ion-card >\n\n          <ion-card-header>\n            Body Metrics \n          </ion-card-header>\n        \n          <ion-card-content>\n            <p>1. Please select your weight in pounds from the drop-down menu.</p>\n\n            <ion-select [(ngModel)]="account.pounds">\n              <ion-option *ngFor="let wdata of weightData" value="{{wdata.description}}">{{wdata.description}}</ion-option>\n            </ion-select>\n           \n            <p>2. Please select your waist circumference in inches from the drop-down menu. </p>\n            <ion-select [(ngModel)]="account.inches">\n              <ion-option *ngFor="let data of waistData" value="{{data.description}}">{{data.description}}</ion-option>\n            </ion-select>\n          </ion-card-content>\n        \n        </ion-card>\n\n</ion-content>\n\n<ion-footer>\n  <ion-toolbar>\n    <button ion-button color="primary" class="big-btn" (click)="submit()" >Next</button>\n  </ion-toolbar>\n</ion-footer>\n'/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\hra-body\hra-body.html"*/,
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavController"], __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavParams"]])
    ], HraBodyPage);
    return HraBodyPage;
}());

//# sourceMappingURL=hra-body.js.map

/***/ }),

/***/ 540:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return IntroPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__intro_video_intro_video__ = __webpack_require__(148);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__providers__ = __webpack_require__(15);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};




/**
 * Generated class for the IntroPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
var IntroPage = /** @class */ (function () {
    function IntroPage(user, navCtrl, navParams, alertCtl, loading) {
        this.user = user;
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.alertCtl = alertCtl;
        this.loading = loading;
        this.account = {
            deviceid: "",
            SecretToken: "",
            FirstName: "",
            LastName: ""
        };
        this.account.deviceid = localStorage.getItem("deviceid");
        this.account.SecretToken = localStorage.getItem("SecretToken");
        this.account.FirstName = localStorage.getItem("FirstName");
        this.account.LastName = localStorage.getItem("LastName");
    }
    IntroPage.prototype.ionViewDidLoad = function () {
        console.log("ionViewDidLoad IntroPage");
    };
    IntroPage.prototype.presentAlert = function (msg) {
        return __awaiter(this, void 0, void 0, function () {
            var alert;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.alertCtl.create({
                            message: msg,
                            cssClass: "action-sheets-basic-page",
                            buttons: [
                                {
                                    text: "OK",
                                    handler: function () { }
                                }
                            ]
                        })];
                    case 1:
                        alert = _a.sent();
                        return [4 /*yield*/, alert.present()];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    IntroPage.prototype.goToVideo = function () {
        var _this = this;
        this.loader = this.loading.create({
            content: "Please wait..."
        });
        this.loader.present().then(function () {
            _this.user.SaveUserAgreeTermsCondition(_this.account).subscribe(function (resp) {
                _this.loader.dismiss();
                _this.navCtrl.push(__WEBPACK_IMPORTED_MODULE_2__intro_video_intro_video__["a" /* IntroVideoPage */]);
            }, function (err) {
                _this.loader.dismiss();
                //this.presentAlert("Server Message - Save User Agree Terms Condition: "+ err.error.SystemMessage);
                _this.presentAlert("Server Message - Save User Agree Terms Condition" + JSON.stringify(err));
            });
        });
    };
    IntroPage = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: "page-intro",template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\intro\intro.html"*/'<!--\n  Generated template for the IntroPage page.\n\n  See http://ionicframework.com/docs/components/#navigation for more info on\n  Ionic pages and navigation.\n-->\n<ion-header class="custom-intro-header content-bg">\n  <ion-navbar color="primary">\n    <ion-title>Welcome {{account.FirstName}} {{account.LastName}}</ion-title>\n    <div class="tag-line"> A few things before you stared</div>\n  </ion-navbar>\n</ion-header>\n<ion-content class="content-bg">\n\n  <ion-card class="signup-card custom-border intro-desc">\n    <ion-card-content>\n      <div>\n        <h2>\n          <b>Terms and Conditions</b>\n        </h2>\n        <br />\n        <p>Meschino Wellness Account Privacy Statement</p>\n        \n        <p>Meschino Health & Wellness (thereafter referred to as “Meschino Wellness”) is committed to protecting your\n          privacy. This privacy statement applies to the personal information (which includes personal health\n          information) collected by the Meschino Wellness Platform.\n        </p>\n        <br />\n        <h2>\n          <b>Introduction</b></h2>\n        <p>\n          <b>The Meschino Wellness Platform is intended for educational purposes only. It is a tool that can help users\n            develop their own personal wellness plan, but does not replace the requirement to seek medical evaluation,\n            lifestyle advice and treatment from a medical professional.</b>\n        </p>\n        <br />\n        <p>\n          The Meschino Wellness Platform provides targeted nutrition and lifestyle content by collecting and analyzing\n          personal health data, which you may wish to share with your medical doctor and other healthcare professionals\n          who help manage your health. It can collect, analyze and store many different types of information such as\n          medication use, immunization records, data originating from health and fitness devices (including pedometers,\n          blood glucose monitors, blood pressure monitors) and from other applications (such as chronic management\n          applications, fitness training applications, weight loss applications, blood pressure applications and more).\n        </p>\n        <br />\n        <p>\n          Importantly, The Meschino Wellness Platform cannot detect or help manage food or drug allergies, food\n          sensitivities or intolerances. In these cases, and other cases involving health conditions of the intestinal\n          tract (i.e. active ulcer, inflammatory bowel disease, gastric by-pass surgery etc.) you must seek nutritional\n          guidance from your medical practitioner or designated registered dietician.\n        </p>\n        <br />\n        <h2>\n          <b>Integration</b></h2>\n        <p>\n          You can utilize components of the Meschino Wellness Platform directly to view and manage your health\n          information, or you can use selected websites and devices that have been created by application providers and\n          device manufacturers to work with Meschino Wellness. Several mechanisms allow you to manage how your health\n          information can be accessed, used and shared.\n          Meschino Wellness provides you with the technology and services to assist you in collecting, storing and\n          analyzing your health related information online. It is a technology platform that allows access by multiple\n          applications and devices, in order to work with your health data to improve personal health literacy and\n          overall wellness.\n\n        </p>\n\n        <br />\n        <h2>\n          <b>Collection of Personal Information</b></h2>\n        <p>\n          Meschino Wellness asks you to enter an identifier and password to sign in. The first time you sign in to\n          Meschino Wellness, Meschino Wellness asks you to create an account. To create an account, you must provide\n          personal information such as name, date of birth, e-mail address, postal code and country/region. Meschino\n          Wellness may request other optional information, but Meschino Wellness will clearly indicate that such\n          information is optional.\n        </p>\n\n        <br />\n        <h2>\n          <b>How Meschino Wellness uses your personal information</b></h2>\n        <p>\n          Meschino Wellness commits to use or disclose personal information collected through Meschino Wellness,\n          including personal health information, exclusively to provide Meschino Wellness services (which includes the\n          billing, support, maintenance and incident resolution services) and as described in this privacy statement,\n          unless expressly otherwise agreed to by you. Usage of the personal information (including personal health\n          information) for the provision of Meschino Wellness solutions includes that Meschino Wellness may use your\n          personal information:\n        </p>\n        <ul>\n          <li>to provide you with information about Meschino Wellness, including updates, and notifications\n          </li>\n          <li>\n            to send you Meschino Wellness e-mail communication, if any.\n          </li>\n        </ul>\n        <br />\n        <p>\n          Meschino Wellness occasionally hires other companies to provide services on its behalf, such as answering\n          customer questions about products and services. Meschino Wellness gives those companies only the personal\n          information they need to deliver the service. Meschino Wellness requires the companies to maintain the\n          confidentiality of the personal information and prohibits them from using such information for any other\n          purpose.\n          <br />\n          In addition, Meschino Wellness may use and/or disclose your personal information if Meschino Wellness believes\n          such action is necessary to comply with applicable legislation or legal process served on Meschino Wellness.\n          <br />\n          Personal information collected on Meschino Wellness is stored and processed in on servers in Canada or the\n          United State.\n          <br />Meschino Wellness has processes and employees (i.e. Head of Privacy and other resources) whose\n          responsibility is to ensure the protection of your privacy and to notify you in the event that Meschino\n          Wellness becomes aware of a breach affecting your personal information.\n\n        </p>\n\n        <br />\n        <h2>\n          <b>How Meschino Wellness uses aggregate information and statistics</b></h2>\n        <p>\n          Meschino Wellness may use aggregated information from Meschino Wellness to identify and compare the status of\n          specific common health issues as compared to other bench marks established in the specified industry or\n          organization. This data is represented as a percent of the entire organization only. This aggregated\n          information is not associated with any individual account and would not identify you. Meschino Wellness will\n          not use or disclose your individual account and record information from Meschino Wellness.\n        </p>\n\n        <br />\n        <h2>\n          <b>Account access and controls</b></h2>\n        <p>\n          The decision to create an account with Meschino Wellness is yours. The required account information consists\n          of a small amount of information such as your name, e-mail address, region and Meschino Wellness credentials.\n          Meschino Wellness may request other optional information, but clearly indicates that such information is\n          optional. You can review and update your account information. You can modify, add or delete any optional\n          account information by signing into your Meschino Wellness account and editing your account profile.\n          <br />When you close your account (by signing into your Meschino Wellness account and editing your account\n          profile), Meschino Wellness deletes all Records for which you are the sole Custodian. Meschino Wellness waits\n          90 days before permanently deleting your account information in order to help avoid accidental or malicious\n          removal of your health information.\n          <br />When a user with "View and modify" or Custodian access deletes a piece of health information, Meschino\n          Wellness archives the information so that it is visible only to Record of Custodians. Solutions and other\n          users with whom you have shared your information, but who are not Custodians of the Record, are not able to\n          see archived health information.\n\n        </p>\n        <br />\n        <h2>\n          <b>E-mail controls</b></h2>\n        <p>\n          Meschino Wellness may send you e-mail communications. You will only receive these communications because you\n          have authorized Meschino Wellness do so. If you do not want to receive this information, you can unsubscribe\n          through a link at the bottom of the newsletter.\n        </p>\n\n        <br />\n        <h2>\n          <b>Security of your personal information</b></h2>\n        <p>\n          Meschino Wellness is committed to protecting the security of your personal information. Meschino Wellness is\n          hosted on a robust industry leading hosting partner. The hosting platform meets a broad set of international\n          and industry-specific compliance standards, such as ISO 27001, HIPAA, FedRAMP, SOC 1 and SOC 2, as well as\n          country-specific standards including Australia IRAP, UK G-Cloud, and Singapore MTCS. The hosting partner was\n          also the first to adopt the uniform international code of practice for cloud privacy, ISO/IEC 27018, which\n          governs the processing of personal information by cloud service providers.\n          <br />\n          Rigorous third-party audits, such as by the British Standards Institute, verify the hosting platforms\n          adherence to the strict security controls these standards mandate. As part of our commitment to transparency,\n          you can verify our implementation of many security controls by requesting audit results from the certifying\n          third parties or through our Host Platform account representative.\n        </p>\n\n        <br />\n        <h2>\n          <b>Use of cookies</b></h2>\n        <p>\n          Meschino Wellness use cookies with Meschino Wellness to enable you to sign in and to help personalize Meschino\n          Wellness. A cookie is a small text file that a web page server places on your hard disk. It is not possible to\n          use cookies to run programs or deliver viruses to your computer. A Web server assigns cookies uniquely to you\n          and only a Web server in the domain that issued the cookie to you can read the cookies.\n          One of the primary purposes of cookies is to provide a convenience feature to save you time. For example, if\n          you personalize a Web page, or navigate within a site, a cookie helps the site to recall your specific\n          information on subsequent visits. Using cookies simplifies the process of delivering relevant content, eases\n          site navigation, and so on. When you return to the Web site, you can retrieve the information you previously\n          provided, so you can easily use the site\'s features that you customized.\n          You have the ability to accept or decline cookies. Most Web browsers automatically accept cookies, but you can\n          usually modify your browser setting to decline some or all cookies if you prefer. If you choose to decline all\n          cookies, you may not be able to use interactive features of this or other Web sites that depend on cookies.\n        </p>\n        <br />\n        <h2>\n          <b>Use of Web beacons</b></h2>\n\n        <p>\n          Meschinowellness.com Web pages may contain electronic images known as Web beacons sometimes called\n          single-pixel gifs that may be used:\n        </p>\n        <ul>\n          <li> to assist in delivering cookies on Meschino Wellness sites</li>\n          <li> to enable Meschino Wellness to count users who have visited those pages</li>\n          <li> to deliver co-branded services</li>\n        </ul>\n        <p>\n          Meschino Wellness may include Web beacons in e-mail messages or in its newsletters in order to determine\n          whether you opened or acted upon those messages.\n          <br />\n          Meschino Wellness may also employ Web beacons from third parties to help it compile aggregated statistics and\n          determine the effectiveness of its promotional campaigns. Meschino Wellness prohibits third parties from using\n          Web beacons on Meschino Wellness sites to collect or access your personal information. Meschino Wellness may\n          collect information about your visit to meschiowellness.com, including the pages you view, the links you\n          click, and other actions taken in connection with the Service. Meschino Wellness also collects certain\n          standard, non-personally identifiable information that your browser sends to every Web site you visit, such as\n          your IP address, browser type and language, access times, and referring Web site addresses.\n\n        </p>\n        <br />\n        <h2>\n          <b>Changes to this privacy statement</b></h2>\n        <p>\n          Meschino Wellness may occasionally update this privacy statement. In such event, Meschino Wellness will notify\n          you either by placing a prominent notice on the home page of the Meschino Wellness Web site or by sending you\n          a notification directly. Meschino Wellness encourages you to review this privacy statement periodically to\n          stay informed about how Meschino Wellness helps you to protect the personal information collected. Your\n          continued use of Meschino Wellness constitutes your agreement to this privacy statement and any updates.\n          Please be aware that this privacy statement does not apply to personal information you may have provided to\n          Meschino Wellness in the context of other, separately operated, Meschino Wellness products or services.\n        </p>\n        <br />\n        <h2> <b>Contact information for privacy related questions and/or complaint</b></h2>\n\n        <p>\n          Meschino Wellness welcomes your comments regarding this privacy statement. If you have a complaint concerning\n          Meschino Wellness’ privacy standards, please submit it to the applicable Privacy Office, however, as\n          recommended by the Office of the Privacy Commissioner of Canada, you are strongly encouraged to try first to\n          settle the matter directly with us, please contact us at <span style="color: blue;">\n            privacy@meschinowellness.com.</span> All questions and/or\n          complaints will be treated as confidential.\n        </p>\n\n        <br />\n        <h2>\n          <b>DISCLOSURE</b></h2>\n        <p>\n          The assessment you’re about to complete will evaluate important aspects of your diet, lifestyle and other\n          health practices, as well as health conditions and risk factors that play a significant role in your health\n          and longevity profile. Based upon scientific evidence available from experimental and clinical studies, we\n          will assemble a number of nutrition, exercise and other lifestyle considerations specific to your individual\n          circumstances.\n          <br />\n          Your personalized wellness report is to be used for educational purposes only and does not take into account\n          any food, drug or supplement allergies or sensitivities. You must consult your health practitioner before\n          making any changes to your dietary, exercise or supplementation practices.\n\n        </p>\n\n\n      </div>\n      <br/>\n      <p><b>By clicking \'I Agree\' you have:</b></p>\n      <ul>\n        <li>Read and undersatnd the disclosure. </li>\n        <li>At least 15 years old.</li>\n        <li>Trust Meschino Health and Wellness with the personal and private health information you provide.</li>\n      </ul>\n    </ion-card-content>\n  </ion-card>\n</ion-content>\n<ion-footer>\n  <ion-toolbar>\n    <button ion-button color="primary" class="big-btn" (click)="goToVideo()">I AGREE</button>\n  </ion-toolbar>\n</ion-footer>'/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\intro\intro.html"*/
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_3__providers__["f" /* UserService */],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavParams"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["AlertController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["LoadingController"]])
    ], IntroPage);
    return IntroPage;
}());

//# sourceMappingURL=intro.js.map

/***/ }),

/***/ 541:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return MyTrackerPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


/**
 * Generated class for the DashboardPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
var MyTrackerPage = /** @class */ (function () {
    function MyTrackerPage(navCtrl, navParams) {
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.account = {
            FirstName: '',
            LastName: '',
        };
        this.account.FirstName = localStorage.getItem('FirstName');
        this.account.LastName = localStorage.getItem('LastName');
    }
    MyTrackerPage.prototype.ionViewDidLoad = function () {
        console.log('ionViewDidLoad MyTrackerPage');
    };
    MyTrackerPage = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'page-mytracker',template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\my-trackers\mytracker.html"*/'<ion-header>\n\n  <ion-navbar color="primary">\n    <ion-title>Trackers</ion-title>\n    <ion-item>\n      <ion-avatar item-start>\n        <img src="assets/img/avtar.jpg">\n      </ion-avatar>\n      <h2>{{account.FirstName}} {{account.LastName}}</h2>\n      <p>\n        <ion-icon name="medal"></ion-icon>296 Points\n      </p>\n    </ion-item>\n    <button ion-button menuToggle>\n      <ion-icon name="menu"></ion-icon>\n    </button>\n  </ion-navbar>\n</ion-header>\n<ion-content>\n\n  <ion-card>\n    My health risk\n  </ion-card>\n</ion-content>\n<ion-footer>\n\n</ion-footer>'/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\my-trackers\mytracker.html"*/,
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavController"], __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavParams"]])
    ], MyTrackerPage);
    return MyTrackerPage;
}());

//# sourceMappingURL=mytracker.js.map

/***/ }),

/***/ 542:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ProfilePage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__ionic_native_camera__ = __webpack_require__(274);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_common_http__ = __webpack_require__(64);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_jquery__ = __webpack_require__(543);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_jquery___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_4_jquery__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__angular_forms__ = __webpack_require__(18);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__providers__ = __webpack_require__(15);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__providers_settings_wellnessconstant__ = __webpack_require__(44);
var __assign = (this && this.__assign) || Object.assign || function(t) {
    for (var s, i = 1, n = arguments.length; i < n; i++) {
        s = arguments[i];
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
            t[p] = s[p];
    }
    return t;
};
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};








/**
 * Generated class for the DashboardPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
var ProfilePage = /** @class */ (function () {
    function ProfilePage(navCtrl, formBuilder, navParams, camera, loadingCtrl, alertCtl, http, pickerCtl, events, userService) {
        var _this = this;
        this.navCtrl = navCtrl;
        this.formBuilder = formBuilder;
        this.navParams = navParams;
        this.camera = camera;
        this.loadingCtrl = loadingCtrl;
        this.alertCtl = alertCtl;
        this.http = http;
        this.pickerCtl = pickerCtl;
        this.events = events;
        this.userService = userService;
        this.IsShowDone = false;
        this.IsCompanySMSEnable = false;
        this.ischecked = false;
        this.account = {
            FirstName: localStorage.getItem("FirstName"),
            LastName: localStorage.getItem("LastName"),
            UserName: localStorage.getItem("UserName"),
            PhoneNumber: localStorage.getItem("PhoneNumber"),
            Height: "cm-50-null",
            BirthDate: "2006-10-10",
            Gender: localStorage.getItem("Gender"),
            IsUserSMSEnable: localStorage.getItem("IsUserSMSEnable") == "true" ? true : false
        };
        this.rewardpoints = 0;
        this.bio_age = "0";
        this.mhrs_score = "0";
        this.appForm = new __WEBPACK_IMPORTED_MODULE_5__angular_forms__["FormGroup"]({
            Gender: new __WEBPACK_IMPORTED_MODULE_5__angular_forms__["FormControl"](),
            BirthDate: new __WEBPACK_IMPORTED_MODULE_5__angular_forms__["FormControl"](),
            Height: new __WEBPACK_IMPORTED_MODULE_5__angular_forms__["FormControl"]()
        });
        this.commandUrl = __WEBPACK_IMPORTED_MODULE_7__providers_settings_wellnessconstant__["a" /* WellnessConstants */].App_Url + "api/WellnessAPI/UpdateUserProfile";
        this.DisplayHeight = false;
        this.DisplayBirthDate = false;
        this.DisplayGender = false;
        this.DisplayFirstName = false;
        this.DisplayLastName = false;
        this.DisplayUserName = false;
        this.DisplayPhoneNumber = false;
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
        this.loader.present().then(function () {
            _this.LoadUserInfo();
        });
        this.IsCompanySMSEnable =
            localStorage.getItem("IsCompanySMSEnable") == "true" ? true : false;
        console.log(this.IsCompanySMSEnable, "IsCompanySMSEnable");
        this.ischecked = this.account.IsUserSMSEnable;
        // Using parentCol
        this.parentColumns = __WEBPACK_IMPORTED_MODULE_7__providers_settings_wellnessconstant__["a" /* WellnessConstants */].parentColumns;
    }
    ProfilePage.prototype.createForm = function () {
        this.appForm = this.formBuilder.group({
            Gender: "",
            BirthDate: "",
            Height: ""
        });
    };
    ProfilePage.prototype.ionViewDidLoad = function () {
        console.log("ionViewDidLoad DashboardPage");
    };
    ProfilePage.prototype.LoadUserInfo = function () {
        var _this = this;
        this.rewardpoints = parseInt(localStorage.getItem("RewardPoint"));
        this.imageFileName = localStorage.getItem("ProfileImage");
        var userAcc = {
            DeviceId: localStorage.getItem("deviceid"),
            SecretToken: localStorage.getItem("SecretToken")
        };
        this.userService.getUserData(userAcc).subscribe(function (res) {
            localStorage.setItem("RewardPoint", res.RewardPoint);
            localStorage.setItem("bio_age", res.bio_age);
            localStorage.setItem("mhrs_score", res.mhrs_score);
            localStorage.setItem("IsUserSMSEnable", res.IsUserSMSEnable);
            _this.rewardpoints = parseInt(localStorage.getItem("RewardPoint"));
            _this.bio_age = localStorage.getItem("bio_age");
            _this.mhrs_score = localStorage.getItem("mhrs_score");
            _this.ischecked = res.IsUserSMSEnable;
            var data = {};
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
            _this.events.publish("user:created", data);
        });
        this.loader.dismiss();
    };
    ProfilePage.prototype.ChangeDate = function () {
        this.IsShowDone = true;
    };
    ProfilePage.prototype.onModelChange = function (event) {
        this.IsShowDone = true;
        console.log(event);
    };
    ProfilePage.prototype.showGenderPicker = function () {
        return __awaiter(this, void 0, void 0, function () {
            var opts, picker;
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        this.IsShowDone = true;
                        opts = {
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
                        return [4 /*yield*/, this.pickerCtl.create(opts)];
                    case 1:
                        picker = _a.sent();
                        picker.present();
                        picker.onDidDismiss(function (data) { return __awaiter(_this, void 0, void 0, function () {
                            var col;
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0: return [4 /*yield*/, picker.getColumn("Gender")];
                                    case 1:
                                        col = _a.sent();
                                        this.account.Gender = col.options[col.selectedIndex].value;
                                        return [2 /*return*/];
                                }
                            });
                        }); });
                        return [2 /*return*/];
                }
            });
        });
    };
    ProfilePage.prototype.editprofilepic = function () {
        var _this = this;
        console.log("show image upload");
        this.IsShowDone = true;
        var alert = this.alertCtl.create({
            title: "Please select option",
            cssClass: "action-sheets-basic-page",
            buttons: [
                {
                    text: "Take photo",
                    handler: function () {
                        _this.captureImage(false);
                    }
                },
                {
                    text: "Choose photo from Gallery",
                    handler: function () {
                        _this.captureImage(true);
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
    };
    ProfilePage.prototype.SetHeight_inInches = function () {
        var inputVal = this.UIHeight;
        var arrVal = inputVal.split("-");
        if (arrVal[0].indexOf("cm") !== -1) {
            var inch = Math.round((parseInt(arrVal[1]) * 1) / 2.54);
            console.log(inch, "CM - INCH");
            this.account.Height = inch + "";
        }
        else if (arrVal[0].indexOf("feet") !== -1) {
            var inch = parseInt(arrVal[1]) * 12 + parseInt(arrVal[2]);
            console.log(inch, "FEET - INCH");
            this.account.Height = inch + "";
        }
    };
    ProfilePage.prototype.captureImage = function (useAlbum) {
        var _this = this;
        var options = __assign({ quality: 25, targetWidth: 300, targetHeight: 300, destinationType: this.camera.DestinationType.DATA_URL, encodingType: this.camera.EncodingType.JPEG }, (useAlbum
            ? { sourceType: this.camera.PictureSourceType.SAVEDPHOTOALBUM }
            : {
                saveToPhotoAlbum: true
            }));
        this.camera.getPicture(options).then(function (imageData) {
            _this.imageDATA = imageData;
            _this.imageFileName = _this.imageDATA;
            _this.imageFileName = "data:image/jpeg;base64," + imageData;
            //this.imageFileName = (<any>window).Ionic.WebView.convertFileSrc(
            // imageData
            //);
        }, function (err) {
            //alert(err);
            console.log(err);
        });
    };
    ProfilePage.prototype.uploadFile = function () {
        var _this = this;
        //alert('API is not yet completed!');
        //debugger;
        if (this.IsFormValid()) {
            var loader_1 = this.loadingCtrl.create({
                content: "Updating..."
            });
            loader_1.present();
            this.SetHeight_inInches();
            var formData = new FormData();
            var dataJson = {
                DeviceId: localStorage.getItem("deviceid"),
                SecretToken: localStorage.getItem("SecretToken"),
                FirstName: this.account.FirstName,
                LastName: this.account.LastName,
                PhoneNumber: this.account.PhoneNumber,
                BirthDate: this.account.BirthDate,
                Height: this.account.Height,
                Gender: this.account.Gender,
                IsUserSMSEnable: this.account.IsUserSMSEnable
            };
            //alert("old path "+ localStorage.getItem("ProfileImage"));
            var jsonString = JSON.stringify(dataJson);
            formData.append("model", jsonString);
            if (this.imageDATA !== undefined) {
                // call method that creates a blob from dataUri
                var imageBlob = this.dataURItoBlob(this.imageDATA);
                //alert('found imagedata');
                //const imageFile = new Blob([imageBlob],{ type: 'image/jpeg' })
                formData.append("ProfileImage", imageBlob, "userimg.jpeg");
                //alert(imageBlob);
            }
            else {
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
            this.PostFile(this.commandUrl, formData, function (res) {
                if (res.SystemStatus == "Success") {
                    _this.SetUserInfo(res);
                }
                //this.presentAlert(res.SystemMessage);
                console.log(res.SystemMessage);
                _this.presentAlert("Profile updated successfully.");
                loader_1.dismiss();
            }, function (err) {
                loader_1.dismiss();
                //this.presentAlert("Server Message - Update User Profile: " + err.error.SystemMessage);
                _this.presentAlert("Server Message - Update User Profile: " + JSON.stringify(err));
            });
        }
    };
    ProfilePage.prototype.EventEnableSMS = function (event) {
        this.IsShowDone = true;
        console.log(event.checked);
        this.ischecked = event.checked;
        this.account.IsUserSMSEnable = this.ischecked;
    };
    ProfilePage.prototype.numberOnlyValidation = function (value) {
        if (isNaN(value) || value.includes(".")) {
            console.log(false);
            // invalid character, prevent input
            return false;
        }
        else {
            console.log(true);
            return true;
        }
    };
    ProfilePage.prototype.isValidMobile = function (value) {
        var msgInfo = { msg: "", isValid: true };
        var firstLetter = value.charAt(0);
        var remainingLetter = "";
        if (firstLetter == "+") {
            remainingLetter = value.replace(firstLetter, "");
        }
        else {
            remainingLetter = value;
        }
        remainingLetter = remainingLetter.replace(/\s/g, "");
        console.log(firstLetter);
        console.log(remainingLetter);
        var regExp = /^[0-9]{10,20}$/;
        if (msgInfo.isValid && !regExp.test(remainingLetter)) {
            msgInfo.isValid = false;
            msgInfo.msg = "Phone number start with + country code and should be 10-20 length";
            //console.log(msgInfo.isValid, 'if test isvalid');
        }
        else {
            msgInfo.isValid = true;
            //console.log(msgInfo.isValid, 'else isvalid');
        }
        return msgInfo;
    };
    ProfilePage.prototype.IsFormValid = function () {
        debugger;
        console.log(this.account.PhoneNumber, "-phone number-");
        var msg;
        if (this.account.FirstName == "" && this.account.LastName == "") {
            msg = "Please enter First Name & Please enter Last Name.";
        }
        else if (this.account.FirstName == "") {
            msg = "Please enter First Name.";
        }
        else if (this.account.LastName == "") {
            msg = "Please enter Last Name.";
        }
        else if (this.account.PhoneNumber == "" || this.account.PhoneNumber === null) {
            msg = "Please enter PhoneNumber.";
        }
        else if (this.account.PhoneNumber !== "") {
            var info = this.isValidMobile(this.account.PhoneNumber);
            if (!info.isValid) {
                msg = info.msg;
            }
            else {
                msg = "";
            }
        }
        if (msg != "" && msg != undefined) {
            this.presentAlert(msg);
            return false;
        }
        else {
            return true;
        }
    };
    ProfilePage.prototype.presentAlert = function (msg) {
        return __awaiter(this, void 0, void 0, function () {
            var alert;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.alertCtl.create({
                            message: msg,
                            cssClass: "action-sheets-basic-page",
                            buttons: [
                                {
                                    text: "OK",
                                    handler: function () { }
                                }
                            ]
                        })];
                    case 1:
                        alert = _a.sent();
                        return [4 /*yield*/, alert.present()];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    ProfilePage.prototype.SetUserInfo = function (data) {
        //alert("new Path " + data.ProfileImage);
        localStorage.setItem("FirstName", data.FirstName);
        localStorage.setItem("LastName", data.LastName);
        localStorage.setItem("ProfileImage", data.ProfileImage);
        localStorage.setItem("Gender", data.Gender);
        localStorage.setItem("Height", data.Height);
        localStorage.setItem("BirthDate", data.BirthDate);
        localStorage.setItem('PhoneNumber', data.PhoneNumber);
        localStorage.setItem('IsCompanySMSEnable', data.IsCompanySMSEnable);
        localStorage.setItem('IsUserSMSEnable', data.IsUserSMSEnable);
        data["RewardPoint"] = localStorage.getItem("RewardPoint");
        data["bio_age"] = localStorage.getItem("bio_age");
        data["mhrs_score"] = localStorage.getItem("mhrs_score");
        this.events.publish("user:created", data);
    };
    ProfilePage.prototype.EditSection = function (item) {
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
    };
    ProfilePage.prototype.dataURItoBlob = function (dataURI) {
        var byteString = window.atob(dataURI);
        var arrayBuffer = new ArrayBuffer(byteString.length);
        var int8Array = new Uint8Array(arrayBuffer);
        for (var i = 0; i < byteString.length; i++) {
            int8Array[i] = byteString.charCodeAt(i);
        }
        var blob = new Blob([int8Array], { type: "image/jpeg" });
        return blob;
    };
    ProfilePage.prototype.PostFile = function (postUrl, formData, fnSuccessCallBack, fnErrorCallBack) {
        __WEBPACK_IMPORTED_MODULE_4_jquery__["ajax"]({
            type: "POST",
            url: postUrl,
            data: formData,
            processData: false,
            contentType: false,
            success: function (msg) {
                if (fnSuccessCallBack != undefined) {
                    fnSuccessCallBack(msg);
                }
            },
            error: function (xhr, errStatus, error) {
                if (fnErrorCallBack == undefined) {
                    console.log(error + " " + errStatus);
                }
                else {
                    fnErrorCallBack(error, errStatus);
                }
            }
        });
    };
    ProfilePage = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: "page-profile",template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\my-profile\profile.html"*/'<ion-header>\n\n  <ion-navbar color="primary">\n    <button ion-button menuToggle>\n      <ion-icon name="menu"></ion-icon>\n    </button>\n    <ion-title>Profile</ion-title>\n    <!-- <ion-item>\n       <ion-avatar item-start>\n        <img src="{{imageFileName}}" id="imgprofile">\n        <ion-icon id="edit" (click)="editprofilepic();">\n          <img src="assets/img/edit_icn.png">\n        </ion-icon>\n      </ion-avatar> \n      <h2>{{account.FirstName}} {{account.LastName}} &nbsp;</h2>\n      <p>\n        <ion-icon name="medal"></ion-icon>{{rewardpoints}} Points\n      </p>\n    </ion-item> -->\n\n    <ion-buttons end>\n      <button *ngIf="IsShowDone" id="done-button" ion-button (click)="uploadFile();">\n        Done\n      </button>\n      <button *ngIf="!IsShowDone" id="done-button" ion-button menuToggle>\n        Back\n      </button>\n    </ion-buttons>\n\n  </ion-navbar>\n</ion-header>\n<ion-content>\n  <ion-item>\n    <div style="width: 100%;">\n      <ion-avatar item-start style="float: right;">\n        <img src="{{imageFileName}}" id="imgprofile">\n        <ion-icon id="edit" (click)="editprofilepic();">\n          <img src="assets/img/edit_icn.png">\n        </ion-icon>\n      </ion-avatar>\n    </div>\n  </ion-item>\n  <div class="div-title">BASIC INFO</div>\n  <ion-item>\n\n    <div class="div-lbl-edit div-border-n"> <span>Fast Name</span> <a (click)="EditSection(\'FirstName\');"\n        *ngIf="!DisplayFirstName"> {{account.FirstName}} <img src="assets/img/arrow.png">\n      </a>\n      <input type="text" [(ngModel)]="account.FirstName" *ngIf="DisplayFirstName" />\n    </div>\n\n\n    <div class="div-lbl-edit div-border-n"> <span>Last Name</span> <a (click)="EditSection(\'LastName\');"\n        *ngIf="!DisplayLastName"> {{account.LastName}} <img src="assets/img/arrow.png">\n      </a>\n      <input type="text" [(ngModel)]="account.LastName" *ngIf="DisplayLastName" />\n    </div>\n    \n    <div class="div-lbl-edit div-border-n"> <span>User Name</span> <a (click)="EditSection(\'UserNameNA\');"\n        *ngIf="!DisplayUserName"> {{account.UserName}} <img src="assets/img/arrow.png">\n      </a>\n      <input type="email" [(ngModel)]="account.UserName" *ngIf="DisplayUserName" />\n    </div>\n    <div class="div-lbl-edit div-border-n"> <span>Phone Number</span> <a (click)="EditSection(\'PhoneNumber\');"\n      *ngIf="!DisplayPhoneNumber"> {{account.PhoneNumber}} <img src="assets/img/arrow.png">\n    </a>\n    <input type="text" [(ngModel)]="account.PhoneNumber" *ngIf="DisplayPhoneNumber" placeholder="+1 604 555 8955" />\n  </div>\n\n  </ion-item>\n\n  <ion-item *ngIf="IsCompanySMSEnable">\n    <ion-label class="lbl-sms">SMS Notification</ion-label>\n    <ion-toggle slot="start" color="secondary" [checked]="ischecked" (ionChange)="EventEnableSMS($event)">\n      <div class="toggle-text">{{ischecked ? "YES" : "NO"}}</div>\n    </ion-toggle>\n  \n</ion-item>\n  <div class="div-title">GENDER</div>\n  <ion-item>\n\n    <div class="div-lbl-edit"> <span>I am</span>\n\n      <!-- <a (click)="EditSection(\'Gender\');"\n        *ngIf="!DisplayGender">{{ account.Gender}} <img src="assets/img/arrow.png"></a> -->\n\n\n\n      <a (click)="showGenderPicker()">{{ account.Gender}} <img src="assets/img/arrow.png"></a>\n\n      <!-- <ion-select [(ngModel)]="account.Gender" *ngIf="DisplayGender">\n        <ion-option value="male">Male</ion-option>\n        <ion-option value="female">Female</ion-option>\n      </ion-select> -->\n\n    </div>\n  </ion-item>\n  <div class="div-title">DATE OF BIRTH</div>\n\n  <ion-card class="ioncard" style="padding-top: 15px">\n    <div class="div-lbl-edit2"> <span>I\'m born on</span>\n      <a>\n        <ion-datetime [(ngModel)]="account.BirthDate" name="BirthDate" (ionChange)="ChangeDate()">\n\n        </ion-datetime>\n        &nbsp; <img src="assets/img/arrow.png">\n      </a>\n    </div>\n  </ion-card>\n  <div class="div-title">HEIGHT</div>\n\n\n\n  <ion-card class="ioncard" style="padding-top: 15px">\n    <div class="div-lbl-edit2"> <span>My height is</span>\n      <a>\n        <ion-multi-picker [(ngModel)]="UIHeight" item-content [multiPickerColumns]="parentColumns" [separator]="\'-\'"\n          name="Height" (ngModelChange)="onModelChange($event)">\n        </ion-multi-picker>\n        &nbsp; <img src="assets/img/arrow.png">\n      </a>\n    </div>\n  </ion-card>\n\n</ion-content>\n<ion-footer>\n\n</ion-footer>'/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\my-profile\profile.html"*/
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavController"],
            __WEBPACK_IMPORTED_MODULE_5__angular_forms__["FormBuilder"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavParams"],
            __WEBPACK_IMPORTED_MODULE_2__ionic_native_camera__["a" /* Camera */],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["LoadingController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["AlertController"],
            __WEBPACK_IMPORTED_MODULE_3__angular_common_http__["a" /* HttpClient */],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["PickerController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["Events"],
            __WEBPACK_IMPORTED_MODULE_6__providers__["f" /* UserService */]])
    ], ProfilePage);
    return ProfilePage;
}());

//# sourceMappingURL=profile.js.map

/***/ }),

/***/ 546:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return LogOverviewPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_highcharts__ = __webpack_require__(547);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_highcharts___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_2_highcharts__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__providers__ = __webpack_require__(15);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__step_dashboard__ = __webpack_require__(150);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};





/**
 * Generated class for the IntroPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
var LogOverviewPage = /** @class */ (function () {
    function LogOverviewPage(navCtrl, navParams, loadingCtrl, alertCtl, menu, stepChallengeService) {
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.loadingCtrl = loadingCtrl;
        this.alertCtl = alertCtl;
        this.menu = menu;
        this.stepChallengeService = stepChallengeService;
        this.model = {};
        this.DialyClass = "current";
        this.MonthlyClass = "";
        this.WeeklyClass = "";
        this.XAxisCategories = [];
        this.SeriesData = [];
        this.xpositionnum = 1;
        this.xMinWidth = 700;
        this.subtitle = "";
        this.CurrentStartDate = "";
        this.CurrentEndDate = "";
        this.zone = new __WEBPACK_IMPORTED_MODULE_0__angular_core__["NgZone"]({ enableLongStackTrace: false });
    }
    LogOverviewPage.prototype.LoadChartData = function () {
        console.log("LoadChartData");
        //let appChart = 
        __WEBPACK_IMPORTED_MODULE_2_highcharts__["chart"]("container", {
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
                },
            ],
        });
        //appChart.redraw();
    };
    LogOverviewPage.prototype.ionViewDidLoad = function () {
        console.log("ionViewDidLoad TemplatePage - logooveriew");
        //this.zone.run(() => {
        this.loadInitialData("Daily", null, true);
        //});
    };
    LogOverviewPage.prototype.ngAfterViewInit = function () {
    };
    LogOverviewPage.prototype.loadInitialData = function (ProgressType, DateType, isFirstTime) {
        var _this = this;
        var userAcc = {
            DeviceId: localStorage.getItem("deviceid"),
            SecretToken: localStorage.getItem("SecretToken"),
            ProgressType: ProgressType,
            CurrentStartDate: null,
            CurrentEndDate: null,
            DateType: null,
        };
        if (!isFirstTime) {
            userAcc.CurrentStartDate = this.CurrentStartDate;
            userAcc.CurrentEndDate = this.CurrentEndDate;
            userAcc.DateType = DateType;
        }
        this.loader = this.loadingCtrl.create({
            content: "Please wait...",
        });
        this.loader.present().then(function () {
            _this.stepChallengeService
                .GetOverviewAndGraphicViewData(userAcc)
                .subscribe(function (resp) {
                _this.loader.dismiss();
                _this.XAxisCategories = [];
                _this.SeriesData = [];
                if (resp.SystemStatus == "Success") {
                    _this.model = resp;
                    console.log(_this.model);
                    _this.model.resultGraphicView.forEach(function (item) {
                        _this.XAxisCategories.push(item.date);
                        _this.SeriesData.push(item.steps);
                        _this.subtitle = _this.model.StartDate + " - " + _this.model.EndDate;
                        _this.CurrentStartDate = _this.model.StartDate;
                        _this.CurrentEndDate = _this.model.EndDate;
                    });
                    _this.LoadChartData();
                }
                else {
                    _this.presentAlert(resp.SystemMessage);
                }
            }, function (err) {
                _this.loader.dismiss();
                _this.presentAlert("Server Message - Get User Overall Progress Of StepChallenge : " +
                    JSON.stringify(err));
            });
        });
    };
    LogOverviewPage.prototype.LoadGraph = function (title, action) {
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
        }
        else {
            this.loadInitialData(title, action, false);
        }
    };
    LogOverviewPage.prototype.presentAlert = function (msg) {
        return __awaiter(this, void 0, void 0, function () {
            var alert;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.alertCtl.create({
                            message: msg,
                            cssClass: "action-sheets-basic-page",
                            buttons: [
                                {
                                    text: "OK",
                                    handler: function () { },
                                },
                            ],
                        })];
                    case 1:
                        alert = _a.sent();
                        return [4 /*yield*/, alert.present()];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    LogOverviewPage.prototype.ionViewDidEnter = function () {
        var _this = this;
        this.navBar.backButtonClick = function () {
            console.log("Back button click StepDashboardPage");
            _this.navCtrl.setRoot(__WEBPACK_IMPORTED_MODULE_4__step_dashboard__["a" /* StepDashboardPage */]);
        };
    };
    LogOverviewPage.prototype.NextScreen = function (name) {
        localStorage.setItem("backstepspage", "LogOverviewPage");
        this.navCtrl.push(name);
    };
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])("navbar"),
        __metadata("design:type", __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["Navbar"])
    ], LogOverviewPage.prototype, "navBar", void 0);
    LogOverviewPage = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: "page-logoverview",template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\step-challenges\log-overview\logoverview.html"*/'<ion-header>\n\n    <ion-navbar #navbar color="primary">\n\n        <ion-title>Overview</ion-title>\n\n    </ion-navbar>\n\n</ion-header>\n\n<ion-content>\n\n\n\n    <ul class="tabs-wrap-main">\n\n        <li class="tab-link {{DialyClass}} main-li" data-tab="custom-tab-1">\n\n            <button (click)="LoadGraph(\'Daily\', \'Previous\')" class="btn-pre">&#8249;</button>\n\n            <a (click)="LoadGraph(\'Daily\', \'\')" class="btn-link"> Daily </a>\n\n            <button (click)="LoadGraph(\'Daily\', \'Next\')" class="btn-next">&#8250;</button>\n\n        </li>\n\n        <li class="tab-link {{WeeklyClass}} main-li" data-tab="custom-tab-2">\n\n            <button (click)="LoadGraph(\'Weekly\', \'Previous\')" class="btn-pre">&#8249;</button>\n\n            <a (click)="LoadGraph(\'Weekly\', \'\')" class="btn-link"> Weekly </a>\n\n            <button (click)="LoadGraph(\'Weekly\', \'Next\')" class="btn-next">&#8250;</button>\n\n        </li>\n\n        <li class="tab-link {{MonthlyClass}} main-li" data-tab="custom-tab-3">\n\n            <button (click)="LoadGraph(\'Monthly\', \'Previous\')" class="btn-pre">&#8249;</button>\n\n            <a (click)="LoadGraph(\'Monthly\', \'\')" class="btn-link"> Monthly </a>\n\n            <button (click)="LoadGraph(\'Monthly\', \'Next\')" class="btn-next">&#8250;</button>\n\n        </li>\n\n    </ul>\n\n    <div id="custom-tab-1" class="tab-content-main current">\n\n        <div class="custom-div custom-row px-2 text-center ">\n\n            <div class="custom-col">\n\n                <div class="div-pad-10">{{model.TotalSteps}}\n\n                    <br />\n\n                    Total Steps</div>\n\n            </div>\n\n            <div class="custom-col">\n\n                <div class="div-pad-10 border-lr"> {{model.Highest}}\n\n                    <br />\n\n                    Highest</div>\n\n            </div>\n\n            <div class="custom-col">\n\n                <div class="div-pad-10 ">{{model.Lowest}}\n\n                    <br />\n\n                    Lowest</div>\n\n            </div>\n\n            \n\n        </div>\n\n\n\n        <!-- <div class="custom-row align-items-center mt-3 mb-3">\n\n            <div class="custom-col pl-3">\n\n                <small class="text-blue"> <b>210</b> Today steps</small>\n\n            </div>\n\n            <div class="custom-col-auto mr-3">\n\n                <div class="custom-btn-primary rounded-circle d-flex align-items-center justify-content-center"\n\n                    style="width: 30px; height: 30px;">\n\n                    <img class="pb-2 pt-2 pl-2 pr-2" src="assets/img/calendar.png" alt="calendar" style="width:34px;">\n\n                </div>\n\n            </div>\n\n        </div> -->\n\n        <div id="container" name="container" style="display: block;"></div>\n\n        <div class="mb-3">\n\n            <button type="button" (click)="NextScreen(\'StepChallengeHistoryPage\')"\n\n                class="custom-btn text-uppercase custom-btn-primary w-100 font-weight-bold font-size-17 py-2 px-2">\n\n                My Challenges</button>\n\n        </div>\n\n    </div>\n\n    <div id="custom-tab-2" class="tab-content-main">\n\n        2\n\n    </div>\n\n    <div id="custom-tab-3" class="tab-content-main">\n\n        3\n\n    </div>\n\n\n\n\n\n</ion-content>'/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\step-challenges\log-overview\logoverview.html"*/,
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavParams"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["LoadingController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["AlertController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["MenuController"],
            __WEBPACK_IMPORTED_MODULE_3__providers__["e" /* StepChallengeService */]])
    ], LogOverviewPage);
    return LogOverviewPage;
}());

//# sourceMappingURL=logoverview.js.map

/***/ }),

/***/ 549:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return StepChallengeHistoryPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__providers__ = __webpack_require__(15);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};



/**
 * Generated class for the IntroPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
var StepChallengeHistoryPage = /** @class */ (function () {
    function StepChallengeHistoryPage(navCtrl, navParams, loadingCtrl, alertCtl, menu, stepChallengeService) {
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.loadingCtrl = loadingCtrl;
        this.alertCtl = alertCtl;
        this.menu = menu;
        this.stepChallengeService = stepChallengeService;
        this.model = {};
        this.dataActivities = [];
        this.loadInitialData();
    }
    StepChallengeHistoryPage.prototype.loadInitialData = function () {
        var _this = this;
        var userAcc = {
            DeviceId: localStorage.getItem("deviceid"),
            SecretToken: localStorage.getItem("SecretToken"),
        };
        this.loader = this.loadingCtrl.create({
            content: "Please wait...",
        });
        this.loader.present().then(function () {
            _this.stepChallengeService.GetUserStepChallengeHistory(userAcc).subscribe(function (resp) {
                _this.loader.dismiss();
                if (resp.SystemStatus == "Success") {
                    _this.dataActivities = resp.result;
                    console.log(resp);
                    var max_user_step_challenge_num = resp.result.map(function (q) { return q.user_step_challenge_num; });
                    console.log('max id user_step_challenge_num', max_user_step_challenge_num);
                    var max_id_1 = Math.max.apply(Math, max_user_step_challenge_num);
                    console.log('max id user_step_challenge_num', max_id_1);
                    _this.dataActivities.forEach(function (item) {
                        if (item.status == "Failed") {
                            item["classname"] = "bg-danger";
                            item["buttonTitle"] = "Delete";
                        }
                        else if (item.status == "Achieved" &&
                            item.user_step_challenge_num == max_id_1) {
                            item["classname"] = "bg-success";
                            item["buttonTitle"] = "Start New Challenge";
                        }
                        else if (item.status == "Achieved" &&
                            (new Date(item.end_date) <= new Date() || new Date(item.end_date) >= new Date())) {
                            item["classname"] = "bg-success";
                            item["buttonTitle"] = "Delete";
                        }
                        else if (item.status == "In Progress") {
                            item["classname"] = "bg-warning";
                            item["buttonTitle"] = "Reset";
                        }
                        else {
                            item["classname"] = "bg-danger";
                            item["buttonTitle"] = "Delete";
                        }
                    });
                }
                else {
                    _this.presentAlert(resp.SystemMessage);
                }
            }, function (err) {
                _this.loader.dismiss();
                _this.presentAlert("Server Message - Get User Overall Progress Of StepChallenge : " +
                    JSON.stringify(err));
            });
        });
    };
    StepChallengeHistoryPage.prototype.ionViewDidLoad = function () {
        console.log("ionViewDidLoad TemplatePage");
    };
    StepChallengeHistoryPage.prototype.StepChallengeHistoryAction = function (id, buttonTitle) {
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
    };
    StepChallengeHistoryPage.prototype.DeletePreviousChallengeHistory = function (id) {
        var _this = this;
        var userAcc = {
            DeviceId: localStorage.getItem("deviceid"),
            SecretToken: localStorage.getItem("SecretToken"),
            user_step_challenge_num: id,
        };
        this.loader = this.loadingCtrl.create({
            content: "Please wait...",
        });
        this.loader.present().then(function () {
            _this.stepChallengeService
                .DeletePreviousChallengeHistory(userAcc)
                .subscribe(function (resp) {
                _this.loader.dismiss();
                if (resp.SystemStatus == "Success") {
                    _this.presentAlert(resp.SystemMessage);
                    _this.loadInitialData();
                }
                else {
                    _this.presentAlert(resp.SystemMessage);
                }
            }, function (err) {
                _this.loader.dismiss();
                _this.presentAlert("Server Message - Delete Previous Challenge History : " +
                    JSON.stringify(err));
            });
        });
    };
    StepChallengeHistoryPage.prototype.ResetUserStepChallenge = function () {
        var _this = this;
        var userAcc = {
            DeviceId: localStorage.getItem("deviceid"),
            SecretToken: localStorage.getItem("SecretToken"),
        };
        this.loader = this.loadingCtrl.create({
            content: "Please wait...",
        });
        this.loader.present().then(function () {
            _this.stepChallengeService.ResetUserStepChallenge(userAcc).subscribe(function (resp) {
                _this.loader.dismiss();
                if (resp.SystemStatus == "Success") {
                    _this.presentAlert(resp.SystemMessage);
                    _this.loadInitialData();
                }
                else {
                    _this.presentAlert(resp.SystemMessage);
                }
            }, function (err) {
                _this.loader.dismiss();
                _this.presentAlert("Server Message - Reset User Step Challenge : " +
                    JSON.stringify(err));
            });
        });
    };
    StepChallengeHistoryPage.prototype.StartUserNewStepChallenge = function () {
        var _this = this;
        var userAcc = {
            DeviceId: localStorage.getItem("deviceid"),
            SecretToken: localStorage.getItem("SecretToken"),
        };
        this.loader = this.loadingCtrl.create({
            content: "Please wait...",
        });
        this.loader.present().then(function () {
            _this.stepChallengeService.StartUserNewStepChallenge(userAcc).subscribe(function (resp) {
                _this.loader.dismiss();
                if (resp.SystemStatus == "Success") {
                    _this.presentConfirm("Congratulations! You have achieved your step challenge before the end date. You have decided to start a new challenge which will delete steps and activities logged for today. Do you want to continue?"); //resp.SystemMessage);
                }
                else {
                    _this.presentAlert(resp.SystemMessage);
                }
            }, function (err) {
                _this.loader.dismiss();
                _this.presentAlert("Server Message - Start User New Step Challenge : " +
                    JSON.stringify(err));
            });
        });
    };
    StepChallengeHistoryPage.prototype.presentAlert = function (msg) {
        return __awaiter(this, void 0, void 0, function () {
            var alert;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.alertCtl.create({
                            message: msg,
                            cssClass: "action-sheets-basic-page",
                            buttons: [
                                {
                                    text: "OK",
                                    handler: function () { },
                                },
                            ],
                        })];
                    case 1:
                        alert = _a.sent();
                        return [4 /*yield*/, alert.present()];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    StepChallengeHistoryPage.prototype.presentConfirm = function (msg) {
        return __awaiter(this, void 0, void 0, function () {
            var alertP;
            var _this = this;
            return __generator(this, function (_a) {
                alertP = this.alertCtl.create({
                    message: msg,
                    cssClass: "action-sheets-basic-page",
                    buttons: [
                        {
                            text: "Ok",
                            handler: function () {
                                _this.loadInitialData();
                            }
                        },
                        {
                            text: "Cancel",
                            handler: function () {
                                //alertP.dismiss();
                            }
                        }
                    ]
                });
                alertP.present();
                return [2 /*return*/];
            });
        });
    };
    StepChallengeHistoryPage.prototype.ionViewDidEnter = function () {
        var _this = this;
        this.navBar.backButtonClick = function () {
            var name = localStorage.getItem("backstepspage");
            if (name !== "LogOverviewPage") {
                console.log("Back button click stepchallenge");
                localStorage.removeItem("backstepspage");
                _this.navCtrl.push(name);
            }
            else {
                _this.navCtrl.setPages([{ page: name }]);
            }
        };
    };
    StepChallengeHistoryPage.prototype.NextScreen = function (name) {
        this.navCtrl.push(name);
    };
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])("navbar"),
        __metadata("design:type", __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["Navbar"])
    ], StepChallengeHistoryPage.prototype, "navBar", void 0);
    StepChallengeHistoryPage = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: "page-stepchallengehistory",template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\step-challenges\stepchallengehistory\stepchallengehistory.html"*/'<ion-header>\n\n    <ion-navbar #navbar color="primary">\n\n        <ion-title>Step Challenge History</ion-title>\n\n    </ion-navbar>\n\n</ion-header>\n\n<ion-content lass="app-back">\n\n    <div class="app-cart ml-3 mt-3 mr-3 px-0" *ngFor="let item of dataActivities">\n\n        <div class="custom-row py-2 px-2 text-center text-light">\n\n            <div class="custom-col">\n\n                <div class="font-family-Conv_gotham-book-webfont font-size-13 main-head">\n\n                    <div class="fl-left">Duration {{item.duration}}</div>\n\n                    <div class="fl-right text-blue"\n\n                        (click)="StepChallengeHistoryAction(item.user_step_challenge_num,item.buttonTitle)"><img\n\n                            style="width: 17px;padding-right: 2px; vertical-align: middle;" class="img-fluid"\n\n                            src="assets/img/new_{{item.buttonTitle}}.png" alt="{{item.buttonTitle}}">{{item.buttonTitle}}</div>\n\n                </div>\n\n                <br />\n\n                <div class="text-left text-light clr"><small> {{item.start_date}} - {{item.end_date}}</small></div>\n\n            </div>\n\n        </div>\n\n        <div style="border-bottom: 1px !important;" class="border-bottom"></div>\n\n        <div class="custom-row px-2 text-center text-light">\n\n            <div class="custom-col">\n\n                <div class="font-family-Conv_gotham-book-webfont font-size-13">Goal Steps</div>\n\n            </div>\n\n            <div class="custom-col">\n\n                <div class="font-family-Conv_gotham-book-webfont font-size-13">Total Steps</div>\n\n            </div>\n\n            <div class="custom-col">\n\n                <div class="font-family-Conv_gotham-book-webfont font-size-13">Status</div>\n\n            </div>\n\n        </div>\n\n\n\n        <div class="custom-row px-2 text-center">\n\n            <div class="custom-col custom-mr-10">\n\n                <div class="font-family-Conv_gotham-book-webfont font-size-13">{{item.number_of_steps}}</div>\n\n            </div>\n\n            <div class="custom-col custom-mr-10">\n\n                <div class="font-family-Conv_gotham-book-webfont font-size-13">{{item.total_steps}}</div>\n\n            </div>\n\n            <div class="custom-col custom-mr-10">\n\n                <div class="{{item.classname}} border-radius-4 font-family-Conv_gotham-book-webfont  font-size-13 px-3 text-white">{{item.status}}</div>\n\n            </div>\n\n        </div>\n\n    </div>\n\n</ion-content>'/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\step-challenges\stepchallengehistory\stepchallengehistory.html"*/,
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavParams"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["LoadingController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["AlertController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["MenuController"],
            __WEBPACK_IMPORTED_MODULE_2__providers__["e" /* StepChallengeService */]])
    ], StepChallengeHistoryPage);
    return StepChallengeHistoryPage;
}());

//# sourceMappingURL=stepchallengehistory.js.map

/***/ }),

/***/ 567:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return MyApp; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__ionic_native_splash_screen__ = __webpack_require__(207);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__ionic_native_status_bar__ = __webpack_require__(209);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__ngx_translate_core__ = __webpack_require__(34);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__ionic_native_unique_device_id__ = __webpack_require__(335);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__providers__ = __webpack_require__(15);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__pages_welcome_welcome__ = __webpack_require__(295);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__pages_login_login__ = __webpack_require__(92);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__providers_settings_fcm_service__ = __webpack_require__(336);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__providers_settings_wellnessconstant__ = __webpack_require__(44);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_11__pages_templates_template__ = __webpack_require__(93);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};












//import { NotificationPage } from "../pages/notification/notification";
var MyApp = /** @class */ (function () {
    function MyApp(translate, platform, config, statusBar, splashScreen, events, fcmSrv, uniqueDeviceID, userSrv, toastController, menu, alertCtl) {
        var _this = this;
        this.translate = translate;
        this.platform = platform;
        this.config = config;
        this.statusBar = statusBar;
        this.splashScreen = splashScreen;
        this.events = events;
        this.fcmSrv = fcmSrv;
        this.uniqueDeviceID = uniqueDeviceID;
        this.userSrv = userSrv;
        this.toastController = toastController;
        this.menu = menu;
        this.alertCtl = alertCtl;
        this.rootPage = __WEBPACK_IMPORTED_MODULE_7__pages_welcome_welcome__["a" /* WelcomePage */];
        //rootPage = MyHraPage;
        this.IshiddenLink = false;
        this.pages = [
            {
                title: "My Profile",
                component: "ProfilePage",
                manuIcon: "user"
            },
            {
                title: "My Wellness Wallet",
                component: "MyWellnessWalletPage",
                manuIcon: "wallet"
            },
            {
                title: "Step Challenge",
                component: "StepDashboardPage",
                manuIcon: "log-steps"
            },
        ];
        //
        this.user_name = "";
        this.rewardpoints = 0;
        this.profilepic = "assets/img/loader.gif";
        this.MainSiteUrl = __WEBPACK_IMPORTED_MODULE_10__providers_settings_wellnessconstant__["a" /* WellnessConstants */].App_Url;
        this.PortalURL = "";
        platform.ready().then(function () {
            // Okay, so the platform is ready and our plugins are available.
            // Here you can do any higher level native things you might need.
            platform.registerBackButtonAction(function () {
                console.log('Exit');
                //this.platform.exitApp();
                _this.ShowExitAppAlert();
            });
            //this.statusBar.styleDefault();
            //this.splashScreen.hide();
            _this.statusBar.styleLightContent();
            _this.uniqueDeviceID
                .get()
                .then(function (uuid) {
                //alert('deviceid : '+uuid);
                localStorage.setItem("remUUID", uuid);
                _this.notificationSetup(uuid);
            })
                .catch(function (error) { return alert(error); });
            _this.menu.enable(false);
        });
        this.initTranslate();
        events.subscribe("user:created", function (user) {
            if (localStorage.getItem("UserInfo") !== undefined &&
                localStorage.getItem("UserInfo") !== null &&
                localStorage.getItem("UserName") !== undefined &&
                localStorage.getItem("UserName") !== null) {
                _this.user_name = user.FirstName + " " + user.LastName;
                _this.rewardpoints = user.RewardPoint;
                _this.profilepic = user.ProfileImage;
                //this.events.publish('user:created', user);
                //let userId = localStorage.getItem("UserName");
                if (localStorage.getItem("UserAccessLevel") !== null &&
                    localStorage.getItem("UserAccessLevel") !== "Full") {
                    _this.IshiddenLink = true;
                }
                else {
                    _this.IshiddenLink = false;
                }
            }
        });
        // code for load push message
        // this.LoadNotifications();
        // this.presentToast("Hello I am alert");
    }
    MyApp.prototype.ShowExitAppAlert = function () {
        var _this = this;
        this.alertCtl.create({
            title: 'Meschino Wellness',
            message: 'Are you sure you want to exit app?',
            enableBackdropDismiss: false,
            cssClass: "action-sheets-basic-page",
            buttons: [
                {
                    text: 'Yes',
                    handler: function () {
                        //this.isExitAlertOpen = false;
                        _this.userSrv._LogoutUser();
                        _this.platform.exitApp();
                    }
                }, {
                    text: 'Cancel',
                    role: 'cancel',
                    handler: function () {
                        //this.isExitAlertOpen = false;
                    }
                }
            ]
        }).present();
    };
    MyApp.prototype.initTranslate = function () {
        var _this = this;
        // Set the default language for translation strings, and the current language.
        this.translate.setDefaultLang("en");
        var browserLang = this.translate.getBrowserLang();
        if (browserLang) {
            if (browserLang === "zh") {
                var browserCultureLang = this.translate.getBrowserCultureLang();
                if (browserCultureLang.match(/-CN|CHS|Hans/i)) {
                    this.translate.use("zh-cmn-Hans");
                }
                else if (browserCultureLang.match(/-TW|CHT|Hant/i)) {
                    this.translate.use("zh-cmn-Hant");
                }
            }
            else {
                this.translate.use(this.translate.getBrowserLang());
            }
        }
        else {
            this.translate.use("en"); // Set your language here
        }
        this.translate.get(["BACK_BUTTON_TEXT"]).subscribe(function (values) {
            _this.config.set("ios", "backButtonText", "");
        });
    };
    MyApp.prototype.openPage = function (page) {
        // Reset the content nav to have just this page
        // we wouldn't want the back button to show in this scenario
        this.nav.setRoot(page.component);
    };
    MyApp.prototype.logOut = function () {
        this.userSrv._LogoutUser();
        // when user logout
        this.menu.enable(false);
        this.nav.push(__WEBPACK_IMPORTED_MODULE_8__pages_login_login__["a" /* LoginPage */]);
    };
    MyApp.prototype.downloadPDF = function () {
        var url = "https://meschinowellness.blob.core.windows.net/downloads/Optimal_Living_Program.pdf";
        window.open(url, "_system", "location=yes");
    };
    MyApp.prototype.openPortal = function () {
        this.PortalURL =
            this.MainSiteUrl +
                "account/MobileAppLogin?SecretToken=" +
                localStorage.getItem("SecretToken") +
                "&DeviceId=" +
                localStorage.getItem("deviceid") +
                "&tokenP=" +
                localStorage.getItem("Password");
        var url = this.PortalURL;
        console.log(url);
        window.open(url, "_system", "location=yes");
    };
    MyApp.prototype.LoadNotifications = function () {
        var _this = this;
        if (localStorage.getItem("UserInfo") !== undefined &&
            localStorage.getItem("UserInfo") !== null &&
            localStorage.getItem("UserName") !== undefined &&
            localStorage.getItem("UserName") !== null) {
            /*
            let res = {Count : 0};
            let oldCount = localStorage.getItem("notificationCount");
            if(oldCount !=="" && oldCount !== undefined )
            {
              res.Count = parseInt(oldCount) + 1;
              this.events.publish("PushNotification", res);
            }
      */
            var userAcc = {
                DeviceId: localStorage.getItem("deviceid"),
                SecretToken: localStorage.getItem("SecretToken")
            };
            this.userSrv.GetPushNotificationCount(userAcc).subscribe(function (res) {
                if (res.SystemStatus == "Success") {
                    _this.events.publish("PushNotification", res);
                }
            }, function (err) {
                //alert(JSON.stringify(err));
                console.log(JSON.stringify(err));
            });
        }
    };
    MyApp.prototype.notificationSetup = function (uuid) {
        var _this = this;
        this.fcmSrv.getToken(uuid);
        this.fcmSrv.onNotifications().subscribe(function (msg) {
            //     alert(msg);
            // if (msg.wasTapped) {
            //   alert('Received in background');
            // } else {
            //   alert('Received in foreground');
            // }
            if (_this.platform.is("ios")) {
                _this.presentToast(msg.body, msg);
            }
            else {
                _this.presentToast(msg.body, msg);
            }
        });
    };
    MyApp.prototype.presentToast = function (message, msg) {
        return __awaiter(this, void 0, void 0, function () {
            var toast;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.LoadNotifications()];
                    case 1:
                        _a.sent();
                        return [4 /*yield*/, this.toastController.create({
                                message: message,
                                duration: 4000,
                                position: "top"
                            })];
                    case 2:
                        toast = _a.sent();
                        toast.present();
                        return [2 /*return*/];
                }
            });
        });
    };
    MyApp.prototype.ViewTemplate = function (Name) {
        this.nav.push(__WEBPACK_IMPORTED_MODULE_11__pages_templates_template__["a" /* TemplatePage */], {
            Content: Name
        });
    };
    MyApp.prototype.gotostep = function (name) {
        this.nav.setRoot(name);
    };
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])(__WEBPACK_IMPORTED_MODULE_5_ionic_angular__["Nav"]),
        __metadata("design:type", __WEBPACK_IMPORTED_MODULE_5_ionic_angular__["Nav"])
    ], MyApp.prototype, "nav", void 0);
    MyApp = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: "page-app-main",template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\app\app.component.html"*/'<ion-menu class="main-menu" [content]="content">\n\n    <ion-header color="primary">\n\n        <ion-item>\n\n            <ion-avatar item-start>\n\n                <img src="{{ profilepic }}" />\n\n            </ion-avatar>\n\n            <h2>{{ user_name }}</h2>\n\n            <p>\n\n                <ion-icon name="medal"></ion-icon>{{ rewardpoints }} Points\n\n            </p>\n\n        </ion-item>\n\n    </ion-header>\n\n    <ion-content>\n\n        <ion-list>\n\n            <ion-item *ngFor="let p of pages">\n\n                <ion-icon><img src="assets/icon/{{ p.manuIcon }}.png" height="16px" width="16px"  class="img-menu"/></ion-icon>\n\n                <button menuClose class="menu-item" (click)="openPage(p)">\n\n                    {{ p.title }}\n\n                </button>\n\n            </ion-item>\n\n            <ion-item>\n\n                <ion-icon><img src="assets/img/policy.png" height="16px" width="16px" class="img-menu"/></ion-icon>\n\n                <button menuClose class="menu-item" (click)="ViewTemplate(\'TermsAndConditions\')">\n\n                    Privacy Policy and Terms\n\n                </button>\n\n            </ion-item>\n\n            <ion-item>\n\n                <ion-icon><img src="assets/img/precautions-disclaimers.png" height="16px" width="16px" class="img-menu"/></ion-icon>\n\n                <button menuClose class="menu-item" (click)="ViewTemplate(\'PrecautionsandDisclaimer\')">\n\n                    Precautions and Disclaimer\n\n                </button>\n\n            </ion-item>\n\n            <!-- <ion-item>\n\n                <ion-icon><img src="assets/img/log-steps.png" height="16px" width="16px" class="img-menu"/></ion-icon>\n\n                <button menuClose class="menu-item" (click)="gotostep(\'StepDashboardPage\')">\n\n                    Step Challenge\n\n                </button>\n\n            </ion-item> -->\n\n            <ion-item *ngIf="!IshiddenLink">\n\n                <ion-icon><img src="assets/img/website.png" height="16px" width="16px" class="img-menu"/></ion-icon>\n\n                <button class="menu-item" (click)="openPortal()">\n\n                    Visit Website\n\n                </button>\n\n            </ion-item>\n\n            <ion-item>\n\n                <img src="assets/img/banner.png" (click)="downloadPDF()" />\n\n            </ion-item>\n\n        </ion-list>\n\n    </ion-content>\n\n    <ion-footer padding color="primary">\n\n        <button menuClose (click)="logOut()" ion-button color="primary" class="big-btn">\n\n            <ion-icon padding name="shuffle"></ion-icon>\n\n            Logout\n\n        </button>\n\n    </ion-footer>\n\n</ion-menu>\n\n<ion-nav #content [root]="rootPage"></ion-nav>'/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\app\app.component.html"*/
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_3__ngx_translate_core__["c" /* TranslateService */],
            __WEBPACK_IMPORTED_MODULE_5_ionic_angular__["Platform"],
            __WEBPACK_IMPORTED_MODULE_5_ionic_angular__["Config"],
            __WEBPACK_IMPORTED_MODULE_2__ionic_native_status_bar__["a" /* StatusBar */],
            __WEBPACK_IMPORTED_MODULE_1__ionic_native_splash_screen__["a" /* SplashScreen */],
            __WEBPACK_IMPORTED_MODULE_5_ionic_angular__["Events"],
            __WEBPACK_IMPORTED_MODULE_9__providers_settings_fcm_service__["a" /* FcmService */],
            __WEBPACK_IMPORTED_MODULE_4__ionic_native_unique_device_id__["a" /* UniqueDeviceID */],
            __WEBPACK_IMPORTED_MODULE_6__providers__["f" /* UserService */],
            __WEBPACK_IMPORTED_MODULE_5_ionic_angular__["ToastController"],
            __WEBPACK_IMPORTED_MODULE_5_ionic_angular__["MenuController"],
            __WEBPACK_IMPORTED_MODULE_5_ionic_angular__["AlertController"]])
    ], MyApp);
    return MyApp;
}());

//# sourceMappingURL=app.component.js.map

/***/ }),

/***/ 71:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return Api; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_http__ = __webpack_require__(532);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common_http__ = __webpack_require__(64);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_core__ = __webpack_require__(0);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



/**
 * Api is a generic REST Api handler. Set your API url first.
 */
var Api = /** @class */ (function () {
    function Api(http) {
        this.http = http;
        this.url = "https://www.meschinowellness.com/"; //WellnessConstants.App_Url;
    }
    Api.prototype.get = function (endpoint, params, reqOpts) {
        if (!reqOpts) {
            reqOpts = {
                params: new __WEBPACK_IMPORTED_MODULE_1__angular_common_http__["c" /* HttpParams */]()
            };
        }
        // Support easy query params for GET requests
        if (params) {
            reqOpts.params = new __WEBPACK_IMPORTED_MODULE_1__angular_common_http__["c" /* HttpParams */]();
            for (var k in params) {
                reqOpts.params = reqOpts.params.set(k, params[k]);
            }
        }
        return this.http.get(this.url + endpoint, reqOpts);
    };
    Api.prototype.post = function (endpoint, body, reqOpts) {
        //const headers = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded' });
        var headers = new __WEBPACK_IMPORTED_MODULE_0__angular_http__["a" /* Headers */]({
            'Content-Type': 'application/x-www-form-urlencoded',
            "Access-Control-Allow-Origin": this.url,
            "Access-Control-Allow-Methods": "POST",
            "Access-Control-Allow-Headers": "Content-Type"
        });
        reqOpts = new __WEBPACK_IMPORTED_MODULE_0__angular_http__["b" /* RequestOptions */]({ headers: headers });
        if (this.url !== null && this.url !== undefined && endpoint != null && endpoint !== undefined) {
            var _endUrl = this.url + endpoint;
            console.log(_endUrl, "end point url");
            return this.http.post(_endUrl, body, reqOpts);
        }
        else {
            alert("URL is undefined");
            return;
        }
    };
    Api.prototype.put = function (endpoint, body, reqOpts) {
        return this.http.put(this.url + endpoint, body, reqOpts);
    };
    Api.prototype.delete = function (endpoint, reqOpts) {
        return this.http.delete(this.url + endpoint, reqOpts);
    };
    Api.prototype.patch = function (endpoint, body, reqOpts) {
        return this.http.patch(this.url + endpoint, body, reqOpts);
    };
    Api = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_2__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__angular_common_http__["a" /* HttpClient */]])
    ], Api);
    return Api;
}());

//# sourceMappingURL=api.js.map

/***/ }),

/***/ 72:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return NotificationPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_common_http__ = __webpack_require__(64);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_forms__ = __webpack_require__(18);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__providers__ = __webpack_require__(15);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__my_wellness_wallet_my_wellness_wallet__ = __webpack_require__(94);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__notificationmsg_notimsg__ = __webpack_require__(266);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};







/**
 * Generated class for the DashboardPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
var NotificationPage = /** @class */ (function () {
    function NotificationPage(navCtrl, formBuilder, navParams, loadingCtrl, alertCtl, http, userService, notificationService, events, menu) {
        this.navCtrl = navCtrl;
        this.formBuilder = formBuilder;
        this.navParams = navParams;
        this.loadingCtrl = loadingCtrl;
        this.alertCtl = alertCtl;
        this.http = http;
        this.userService = userService;
        this.notificationService = notificationService;
        this.events = events;
        this.menu = menu;
        this.customText = true;
        this.Notifications = [];
        this.account = {
            deviceid: "",
            SecretToken: "",
            PushNotificationYesNo: "",
            DeviceToken: localStorage.getItem("FCMToken"),
            PageSize: 10,
            PageIndex: 1
        };
        this.currentIndex = 1;
        this.ischecked = true;
        //this.menu.enable(true);
        this.zone = new __WEBPACK_IMPORTED_MODULE_0__angular_core__["NgZone"]({ enableLongStackTrace: false });
        this.account.deviceid = localStorage.getItem("deviceid");
        this.account.SecretToken = localStorage.getItem("SecretToken");
        if (localStorage.getItem("PushNotificationYesNo") !== null &&
            localStorage.getItem("PushNotificationYesNo") == "Yes") {
            this.ischecked = true;
        }
        else {
            this.ischecked = false;
        }
    }
    NotificationPage.prototype.ionViewDidLoad = function () {
        if (localStorage.getItem("NotificationsList") !== undefined && localStorage.getItem("NotificationsList") !== null) {
            this.Notifications = JSON.parse(localStorage.getItem("NotificationsList"));
        }
        else {
            this.currentIndex = 1;
            this.LoadMessage();
        }
        //localStorage.removeItem("currentIndex");
        //this.AddFromEvents();
        console.log("ionViewDidLoad DashboardPage");
    };
    NotificationPage.prototype.notify = function (event) {
        var _this = this;
        console.log(event.checked);
        this.ischecked = event.checked;
        if (event.checked) {
            this.account.PushNotificationYesNo = "Yes";
        }
        else {
            this.account.PushNotificationYesNo = "No";
        }
        this.loader = this.loadingCtrl.create({
            content: "Please wait..."
        });
        this.loader.present().then(function () {
            _this.userService.SaveUserPushNotificationOnOff(_this.account).subscribe(function (resp) {
                _this.loader.dismiss();
                _this.presentAlert("Successfully saved your request!");
                localStorage.setItem("PushNotificationYesNo", _this.account.PushNotificationYesNo);
            }, function (err) {
                _this.loader.dismiss();
                _this.presentAlert("Server Message - Save User Push Notification On Off : " +
                    JSON.stringify(err)
                //err.error.SystemMessage
                );
            });
        });
    };
    NotificationPage.prototype.doRefresh = function (refresher) {
        refresher.complete();
        this.LoadMessage();
    };
    NotificationPage.prototype.LoadMessage = function () {
        var _this = this;
        //let _CurrentNotifications = this.Notifications.length > 0 ? this.Notifications : [];
        this.zone.run(function () {
            _this.loader = _this.loadingCtrl.create({
                content: "Please wait..."
            });
            _this.loader.present().then(function () {
                _this.account.PageIndex = _this.currentIndex;
                _this.userService.GetUserPushNotificationDetail(_this.account).subscribe(function (resp) {
                    if (resp.lstPushNotificationDetail != null) {
                        resp.lstPushNotificationDetail.sort(function (a, b) {
                            return new Date(b.CreateDate).getTime() -
                                new Date(a.CreateDate).getTime();
                        });
                        var _curNotifications = resp.lstPushNotificationDetail;
                        for (var i = 0; i < _curNotifications.length; i++) {
                            _curNotifications[i].showfull = false;
                            if (_curNotifications[i].Message.length <= 55) {
                                _curNotifications[i].HideButton = true;
                            }
                            if (_this.currentIndex > 1 && _curNotifications.length > 0) {
                                // add to exiting loaded items
                                _this.Notifications.push(_curNotifications[i]);
                            }
                        }
                        if (_this.currentIndex == 1 && _curNotifications.length >= 0) {
                            _this.Notifications = [];
                            _this.Notifications = _curNotifications;
                        }
                    }
                    // if (refresher !== undefined && refresher !== null) {
                    //   refresher.complete();
                    // }
                    _this.loader.dismiss();
                    console.log(_this.Notifications);
                }, function (err) {
                    _this.loader.dismiss();
                    // if (refresher !== undefined && refresher !== null) {
                    //   refresher.complete();
                    // }
                    _this.presentAlert("Server Message - Get User Push Notification Detail : " +
                        JSON.stringify(err)
                    //err.error.SystemMessage
                    );
                });
            });
        });
    };
    /*
    scrollTo(elementId: string) {
      let y = document.getElementById(elementId).offsetTop;
      this.content.scrollTo(0, y);
    }*/
    NotificationPage.prototype.ngAfterViewInit = function () {
        var _this = this;
        this.content.ionScrollEnd.subscribe(function (data) {
            var scrollTop = _this.content.scrollTop;
            var dimensions = _this.content.getContentDimensions();
            var totalScrolled = (scrollTop + dimensions.contentHeight + 20);
            if (totalScrolled > dimensions.scrollHeight && data.directionY == "down" && _this.Notifications.length % 10 == 0) {
                _this.currentIndex = _this.currentIndex + 1;
                _this.LoadMessage();
            }
        });
    };
    /*
    AddFromEvents() {
      this.events.subscribe("NotificationDetail", lstPushNotificationDetail => {
        let _Notifications = lstPushNotificationDetail.filter(
          q => q.IsRead == false
        );
        for (var i = 0; i < _Notifications.length; i++) {
          _Notifications[i].showfull = false;
          if (_Notifications[i].Message.length <= 55) {
            _Notifications[i].HideButton = true;
          }
  
          if (
            this.Notifications.filter(q => q.Id !== _Notifications[i].Id)
              .length == 0
          ) {
            console.log(
              _Notifications[i].Id,
              "current Id not found - add in list"
            );
            this.Notifications.push(_Notifications[i]);
          } else {
            console.log(
              _Notifications[i].Id,
              "current Id found - not add in list"
            );
          }
        }
        // if (_Notifications.length > 0) {
        //   this.MarkMessageRead();
        // }
      });
    }
    */
    // MarkMessageRead(item: any) {
    //   if (!item.IsRead) {
    //     const userAcc = {
    //       DeviceId: localStorage.getItem("deviceid"),
    //       SecretToken: localStorage.getItem("SecretToken"),
    //       Id: item.Id
    //     };
    //     this.notificationService
    //       .UpdateIsReadPushNotificationDetail(userAcc)
    //       .subscribe(
    //         (resp: any) => {
    //           console.log("Marked user read data");
    //           item.IsRead = true;
    //         },
    //         err => {
    //           this.loader.dismiss();
    //           this.presentAlert(
    //             "Server Message - Update Is Read Push Notification Detail : " +
    //               err.error.SystemMessage
    //           );
    //         }
    //       );
    //   }
    // }
    NotificationPage.prototype.OpenArticle = function (article, showfull) {
        // if (showfull) {
        //   article.showfull = false;
        // } else {
        //   article.showfull = true;
        // }
        localStorage.setItem("noti-item", JSON.stringify(article));
        article.IsRead = true;
        localStorage.setItem("NotificationsList", JSON.stringify(this.Notifications));
        this.navCtrl.push(__WEBPACK_IMPORTED_MODULE_6__notificationmsg_notimsg__["a" /* NotificationMsgPage */]);
    };
    NotificationPage.prototype.removeItem = function (item) {
        this.currentIndex = 1;
        var ids = [];
        ids.push(item.Id);
        this.deleteMessage(ids);
    };
    NotificationPage.prototype.clearall = function () {
        this.currentIndex = 1;
        var ids = [];
        for (var i = 0; i < this.Notifications.length; i++) {
            ids.push(this.Notifications[i].Id);
        }
        if (ids.length > 0)
            this.deleteMessage(ids);
    };
    NotificationPage.prototype.deleteMessage = function (lstIds) {
        var _this = this;
        var userAcc = {
            DeviceId: localStorage.getItem("deviceid"),
            SecretToken: localStorage.getItem("SecretToken"),
            Ids: lstIds
        };
        console.log(userAcc);
        this.loader = this.loadingCtrl.create({
            content: "Removing message..."
        });
        this.loader.present().then(function () {
            _this.notificationService
                .DeleteUserPushNotificationDetail(userAcc)
                .subscribe(function (resp) {
                _this.loader.dismiss();
                _this.LoadMessage();
                console.log(_this.Notifications);
            }, function (err) {
                _this.loader.dismiss();
                _this.presentAlert("Server Message - Delete User Push Notification Detail : " +
                    JSON.stringify(err));
            });
        });
    };
    NotificationPage.prototype.presentAlert = function (msg) {
        return __awaiter(this, void 0, void 0, function () {
            var alert;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.alertCtl.create({
                            message: msg,
                            cssClass: "action-sheets-basic-page",
                            buttons: [
                                {
                                    text: "OK",
                                    handler: function () { }
                                }
                            ]
                        })];
                    case 1:
                        alert = _a.sent();
                        return [4 /*yield*/, alert.present()];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    NotificationPage.prototype.goBack = function () {
        localStorage.removeItem("NotificationsList");
        this.navCtrl.setRoot(__WEBPACK_IMPORTED_MODULE_5__my_wellness_wallet_my_wellness_wallet__["a" /* MyWellnessWalletPage */]);
    };
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('navbar'),
        __metadata("design:type", __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["Navbar"])
    ], NotificationPage.prototype, "navBar", void 0);
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])(__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["Content"]),
        __metadata("design:type", __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["Content"])
    ], NotificationPage.prototype, "content", void 0);
    NotificationPage = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: "page-notification",template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\notification\notification.html"*/'<ion-header>\n\n  <ion-navbar color="primary">\n    <ion-buttons start>\n      <button ion-button id="custom-back-btn" (click)="goBack();">\n        <ion-icon name="arrow-back"></ion-icon>\n      </button>\n    </ion-buttons>\n    <ion-title>Notifications</ion-title>\n    <ion-buttons end>\n      <button id="clear-btn" ion-button (click)="clearall();">\n        Clear All\n      </button>\n    </ion-buttons>\n  </ion-navbar>\n  <ion-toolbar>\n    <ion-item>\n      <ion-label>Want to receive Notification?</ion-label>\n      <ion-toggle slot="start" color="secondary" [checked]="ischecked" (ionChange)="notify($event)">\n        <div class="toggle-text">{{ischecked ? "YES" : "NO"}}</div>\n      </ion-toggle>\n    </ion-item>\n  </ion-toolbar>\n</ion-header>\n<ion-content>\n  <!-- <ion-refresher (ionRefresh)="doRefresh($event)">\n    <ion-refresher-content refreshingSpinner="circles"> -->\n      <ion-list>\n        <ion-item-sliding class="content-msg-list" *ngFor="let item of Notifications">\n          <ion-item [class.msg-unread]="!item.IsRead">\n            <img src="assets/img/appicon.png" item-left>\n            <span text-wrap *ngIf="!item.showfull" id="less-{{item.Id}}"\n              class="msg-col">{{item.Message.length > 55 ? item.Message.substring(0, 55) + \'...\' : item.Message }}</span>\n            <span text-wrap *ngIf="item.showfull" class="msg-col" id="full-{{item.Id}}">{{item.Message}}</span>\n            <!-- <button ion-button color="primary" class="big-btn" \n        *ngIf="!item.HideButton"\n          (click)="OpenArticle(item, item.showfull)"> -->\n            <button ion-button color="primary" class="big-btn" (click)="OpenArticle(item, item.showfull)">\n              Read\n              <!-- {{ item.showfull ? \'less\': \'more\'}} -->\n            </button>\n            <br />\n            <span class="msg-col2"> {{ item.CreateDate | date: \'longDate\'}} </span>\n\n          </ion-item>\n          <ion-item-options>\n            <button ion-button (click)="removeItem(item)" class="warning">Delete</button>\n          </ion-item-options>\n        </ion-item-sliding>\n\n      </ion-list>\n    <!-- </ion-refresher-content>\n  </ion-refresher> -->\n</ion-content>\n<ion-footer>\n\n</ion-footer>'/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\notification\notification.html"*/
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavController"],
            __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormBuilder"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavParams"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["LoadingController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["AlertController"],
            __WEBPACK_IMPORTED_MODULE_2__angular_common_http__["a" /* HttpClient */],
            __WEBPACK_IMPORTED_MODULE_4__providers__["f" /* UserService */],
            __WEBPACK_IMPORTED_MODULE_4__providers__["c" /* NotificationService */],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["Events"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["MenuController"]])
    ], NotificationPage);
    return NotificationPage;
}());

//# sourceMappingURL=notification.js.map

/***/ }),

/***/ 92:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return LoginPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__ngx_translate_core__ = __webpack_require__(34);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__providers__ = __webpack_require__(15);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__signup_signup__ = __webpack_require__(147);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__my_wellness_wallet_my_wellness_wallet__ = __webpack_require__(94);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__forgetpassword_forgetpassword__ = __webpack_require__(258);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};







var LoginPage = /** @class */ (function () {
    function LoginPage(navCtrl, user, translateService, events, alertCtl, loading, menu) {
        this.navCtrl = navCtrl;
        this.user = user;
        this.translateService = translateService;
        this.events = events;
        this.alertCtl = alertCtl;
        this.loading = loading;
        this.menu = menu;
        // The account fields for the login form.
        // If you're using the username field with or without email, make
        // sure to add it to the type
        this.account = {
            UserName: "",
            Password: "",
            apiKey: "",
            deviceid: ""
        };
        this.menu.enable(false);
        this.Rememberme = true;
        // set remember me
        if (window.localStorage.getItem("remuser") == null || window.localStorage.getItem("remuser") == 'null' || window.localStorage.getItem("remuser") == "") {
            this.account.UserName = "";
        }
        else {
            this.account.UserName = window.localStorage.getItem("remuser");
        }
        if (window.localStorage.getItem("rempwd") == null || window.localStorage.getItem("rempwd") == 'null' || window.localStorage.getItem("rempwd") == "") {
            this.account.Password = "";
        }
        else {
            this.account.Password = window.localStorage.getItem("rempwd");
        }
    }
    LoginPage.prototype.signup = function () {
        this.navCtrl.push(__WEBPACK_IMPORTED_MODULE_4__signup_signup__["a" /* SignupPage */]);
    };
    LoginPage.prototype.validateForm = function () {
        if (this.account.UserName == "" || this.account.Password == "") {
            this.presentAlert("Please enter Username and Password");
            return false;
        }
        else {
            return true;
        }
    };
    LoginPage.prototype.doForget = function () {
        this.navCtrl.setRoot(__WEBPACK_IMPORTED_MODULE_6__forgetpassword_forgetpassword__["a" /* ForgetPasswordPage */]);
    };
    LoginPage.prototype.SaveUserTokenInDB = function () {
        return __awaiter(this, void 0, void 0, function () {
            var accountinfo;
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        if (!(localStorage.getItem("deviceid") !== null &&
                            localStorage.getItem("deviceid") !== undefined &&
                            localStorage.getItem("FCMToken") !== null &&
                            localStorage.getItem("FCMToken") !== undefined &&
                            localStorage.getItem("DeviceType") !== null &&
                            localStorage.getItem("DeviceType") !== undefined)) return [3 /*break*/, 2];
                        accountinfo = {
                            deviceid: localStorage.getItem("deviceid"),
                            SecretToken: localStorage.getItem("SecretToken"),
                            DeviceType: localStorage.getItem("DeviceType"),
                            UserTokenId: localStorage.getItem("FCMToken")
                        };
                        return [4 /*yield*/, this.user.SaveUserTokenIdData(accountinfo).subscribe(function (resp) {
                                console.log("save token success");
                            }, function (err) {
                                _this.presentAlert("Server Message - Save User TokenId Data : " + JSON.stringify(err));
                            })];
                    case 1:
                        _a.sent();
                        return [3 /*break*/, 3];
                    case 2:
                        console.log("Please allow your phone to access the device types");
                        _a.label = 3;
                    case 3: return [2 /*return*/];
                }
            });
        });
    };
    LoginPage.prototype.presentAlert = function (msg) {
        return __awaiter(this, void 0, void 0, function () {
            var alert;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.alertCtl.create({
                            message: msg,
                            cssClass: "action-sheets-basic-page",
                            buttons: [
                                {
                                    text: "OK",
                                    handler: function () { }
                                }
                            ]
                        })];
                    case 1:
                        alert = _a.sent();
                        return [4 /*yield*/, alert.present()];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    // Attempt to login in through our User service
    LoginPage.prototype.doLogin = function () {
        // if (this.account.UserName !== localStorage.getItem("remuser")) {
        //   this.fcm.deleteToken();
        // }
        var _this = this;
        if (this.validateForm()) {
            this.loader = this.loading.create({
                content: "Please wait..."
            });
            if (this.Rememberme) {
                window.localStorage.setItem("remuser", this.account.UserName);
                window.localStorage.setItem("rempwd", this.account.Password);
            }
            else {
                window.localStorage.removeItem("remuser");
                window.localStorage.removeItem("rempwd");
            }
            this.account.apiKey = "71e73c14-2723-4d6e-a489-c9675738fdf4";
            this.account.deviceid = localStorage.getItem("remUUID");
            //debugger;
            //alert('deviceid : '+this.account.deviceid);
            if (localStorage.getItem("remUUID") == null || localStorage.getItem("remUUID") == "null")
                this.account.deviceid = "c9675738fdf4";
            localStorage.setItem("deviceid", this.account.deviceid);
            this.loader.present().then(function () {
                _this.user.login(_this.account).subscribe(function (resp) {
                    console.log("user", { resp: resp });
                    _this.menu.enable(true);
                    if ((resp.SystemStatus = "Success")) {
                        _this.events.publish("user:created", resp);
                        _this.SaveUserTokenInDB();
                        setTimeout(function () {
                            _this.loader.dismiss();
                            _this.navCtrl.setRoot(__WEBPACK_IMPORTED_MODULE_5__my_wellness_wallet_my_wellness_wallet__["a" /* MyWellnessWalletPage */]);
                        }, 3000);
                    }
                    else {
                        _this.presentAlert(resp.SystemMessage);
                    }
                }, function (err) {
                    _this.loader.dismiss();
                    //this.presentAlert("Server Message - User Login : "+ JSON.stringify(err));
                    _this.presentAlert(err.error.SystemMessage);
                    _this.user._LogoutUser();
                });
            });
        }
    };
    LoginPage = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: "page-login",template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\login\login.html"*/'<ion-content scroll="false">\n  <div class="splash-bg">\n    <h1>Login to your account</h1>\n  </div>\n \n <ion-card class="login-card">\n  <ion-card-content>\n      <ion-list>\n          <ion-item class="custom-border">\n              <ion-label stacked>{{ \'USERNAME\' | translate }}</ion-label>\n            <ion-input type="text" [(ngModel)]="account.UserName" name="UserName"></ion-input>\n          </ion-item>\n          <ion-item class="custom-border">\n              <ion-label stacked>{{ \'PASSWORD\' | translate }}</ion-label>\n              <ion-input type="password" [(ngModel)]="account.Password" name="Password" (keyup.enter)="doLogin();"></ion-input>\n          </ion-item>\n          <ion-item >\n              <ion-label>Remember Me</ion-label>\n              <ion-checkbox [(ngModel)]="Rememberme" name="Rememberme" ></ion-checkbox>\n            </ion-item>\n        </ion-list>\n  </ion-card-content>\n</ion-card>\n  <div padding class="button-position">\n      <button ion-button clear full class="forgot-btn" (click)="doForget()">Forgot Password </button>\n      <button  (click)="doLogin()" ion-button color="primary"  class="big-btn" >{{ \'Login\' | translate }}</button>\n	<div class="signup-text">Don’t have an account? <a href="#" (click)="signup();">SIGN UP</a> </div>\n</div>\n</ion-content>\n '/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\login\login.html"*/
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_2_ionic_angular__["NavController"],
            __WEBPACK_IMPORTED_MODULE_3__providers__["f" /* UserService */],
            __WEBPACK_IMPORTED_MODULE_1__ngx_translate_core__["c" /* TranslateService */],
            __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["Events"],
            __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["AlertController"],
            __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["LoadingController"],
            __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["MenuController"]])
    ], LoginPage);
    return LoginPage;
}());

//# sourceMappingURL=login.js.map

/***/ }),

/***/ 93:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return TemplatePage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


/**
 * Generated class for the IntroPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
var TemplatePage = /** @class */ (function () {
    function TemplatePage(navCtrl, navParams) {
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.TempaletName = this.navParams.get("Content");
        if (this.TempaletName == "MHRScore") {
            this.Title = "MHR Score";
        }
        else if (this.TempaletName == "BioAge") {
            this.Title = "Bio Age";
        }
        else if (this.TempaletName == "TermsAndConditions") {
            this.Title = "Terms And Conditions";
        }
        else if (this.TempaletName == "PrecautionsandDisclaimer") {
            this.Title = "Precautions and Disclaimer";
        }
    }
    TemplatePage.prototype.ionViewDidLoad = function () {
        console.log("ionViewDidLoad TemplatePage");
    };
    TemplatePage = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: "page-template",template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\templates\template.html"*/'<!--\n\n  Generated template for the IntroPage page.\n\n\n\n  See http://ionicframework.com/docs/components/#navigation for more info on\n\n  Ionic pages and navigation.\n\n-->\n\n<ion-header>\n\n    <ion-navbar color="primary">\n\n        <ion-title>{{Title}}</ion-title>\n\n\n\n    </ion-navbar>\n\n</ion-header>\n\n<ion-content>\n\n    <ion-card class="signup-card custom-border intro-desc" *ngIf="TempaletName == \'MHRScore\'">\n\n        <ion-card-content>\n\n            <div>\n\n                <!-- <h2>\n\n                    <b>MHR Score</b>\n\n                </h2>\n\n                <br /> -->\n\n                <p>\n\n                    The Modifiable Health Risk Score is a total score out of 100. The higher your score the better you\n\n                    are managing lifestyle factors that impact risk for developing, and in some instances managing, many\n\n                    common degenerative diseases. The Modifiable Health Risk Score takes into account things in your\n\n                    life that you have absolute, or significant, control over, such as nutrition, weight, waist\n\n                    circumference, exercise habits, medication adherence, blood pressure, cholesterol levels, smoking,\n\n                    alcohol consumption, amount of sleep, as well as stress/anxiety/ relationship and emotional\n\n                    wellness.\n\n                </p>\n\n                <br />\n\n                <p>\n\n                    Once you know your score your goal is to work on the modifiable health risk factors that will help\n\n                    you achieve a higher score. Your feedback report contains the details as to how you can best\n\n                    accomplish this goal.</p>\n\n                <br />\n\n                <p>\n\n                    As you work towards improving you score we suggest that you re-take the Assessment every three\n\n                    months to see how much your Modifiable Health Risk Score has improved from your previous\n\n                    assessments. No matter what your score is today, there is usually some action you can take to\n\n                    improve your score over the next three months. Set the goal today and let’s keep your wellness\n\n                    journey moving in the right direction.\n\n                </p>\n\n            </div>\n\n        </ion-card-content>\n\n    </ion-card>\n\n    <ion-card class="signup-card custom-border intro-desc" *ngIf="TempaletName == \'BioAge\'">\n\n        <ion-card-content>\n\n            <div>\n\n                <!-- <h2>\n\n                    <b>Bio Age</b>\n\n                </h2>\n\n                <br /> -->\n\n                <p>Your Bio Age (biological age) indicates how old your body is compared to your actual chronological\n\n                    age. As you improve your Modifiable Health Risk Score you also improve your Bio Age Score. Science\n\n                    has shown us that you can slow and reverse the aging process by implementing proactive nutrition and\n\n                    lifestyle wellness practices. You can literally “turn back the clock” using a personalized wellness\n\n                    strategy.\n\n                </p>\n\n            </div>\n\n        </ion-card-content>\n\n    </ion-card>\n\n    <ion-card class="signup-card custom-border intro-desc" *ngIf=" TempaletName== \'PrecautionsandDisclaimer\'">\n\n        <ion-card-content>\n\n            <div>\n\n                <!-- <h2>\n\n                    <b>Precautions and Disclaimer</b>\n\n                </h2>\n\n                <br /> -->\n\n                <p>\n\n                    Be aware that The Meschino Wellness Platform cannot detect or provide advice about the management of\n\n                    the following health challenges:\n\n                </p>\n\n                <ul>\n\n                    <li>Food Allergies\n\n                    </li>\n\n                    <li>\n\n                        Drug Allergies\n\n                    </li>\n\n                    <li>\n\n                        Food Sensitivities or Intolerances\n\n                    </li>\n\n                </ul>\n\n                <br />\n\n                <p>\n\n                    In these cases, and other cases involving health conditions of the Intestinal Tract (i.e. active\n\n                    ulcer, inflammatory bowel diseases such as Crohn’s disease and Ulcerative Colitis, Gastric by-pass\n\n                    Surgery, or any previous Bowel Surgery etc.) you must seek nutritional guidance from your medical\n\n                    practitioner or designated registered dietician.\n\n                    <br />\n\n                    The Meschino Wellness Platform is intended for educational purposes only. It is a tool that can help\n\n                    users develop their own personal wellness plan in the majority of cases, but does not replace the\n\n                    requirement to seek medical evaluation, lifestyle advice and treatment from a medical professional.\n\n                    The Meschino Wellness Platform provides targeted nutrition and lifestyle content by collecting and\n\n                    analyzing personal health data. You may wish to share the contents of your Feedback Report (My\n\n                    Wellness Report), along with your tracking tool values (i.e. blood pressure tracker) with your\n\n                    medical doctor and other healthcare professionals, who help manage your health, as a means of\n\n                    keeping them informed about your self-care and enlisting their recommendations to help fine-tune\n\n                    your lifestyle management.\n\n                </p>\n\n\n\n            </div>\n\n        </ion-card-content>\n\n    </ion-card>\n\n\n\n    <ion-card class="signup-card custom-border intro-desc" *ngIf="TempaletName== \'TermsAndConditions\'">\n\n        <ion-card-content>\n\n            <div>\n\n                <!-- <h2>\n\n                    <b>Terms and Conditions</b>\n\n                </h2>\n\n                <br /> -->\n\n                <p>Meschino Wellness Account Privacy Statement</p>\n\n\n\n                <p>Meschino Health & Wellness (thereafter referred to as “Meschino Wellness”) is committed to protecting\n\n                    your\n\n                    privacy. This privacy statement applies to the personal information (which includes personal health\n\n                    information) collected by the Meschino Wellness Platform.\n\n                </p>\n\n                <br />\n\n                <h2>\n\n                    <b>Introduction</b></h2>\n\n                <p>\n\n                    <b>The Meschino Wellness Platform is intended for educational purposes only. It is a tool that can\n\n                        help users\n\n                        develop their own personal wellness plan, but does not replace the requirement to seek medical\n\n                        evaluation,\n\n                        lifestyle advice and treatment from a medical professional.</b>\n\n                </p>\n\n                <br />\n\n                <p>\n\n                    The Meschino Wellness Platform provides targeted nutrition and lifestyle content by collecting and\n\n                    analyzing personal health data, which you may wish to share with your medical doctor and other\n\n                    healthcare professionals who help manage your health. It can collect, analyze and store many\n\n                    different types of information such as medication use, immunization records, data originating from\n\n                    health and fitness\n\n                    devices (including pedometers, blood glucose monitors, blood pressure monitors) and from other\n\n                    applications\n\n                    (such as chronic management applications, fitness training applications, weight loss applications,\n\n                    blood\n\n                    pressure applications and more).\n\n                </p>\n\n                <br />\n\n                <p>\n\n                    Importantly, The Meschino Wellness Platform cannot detect or help manage food or drug allergies,\n\n                    food\n\n                    sensitivities or intolerances. In these cases, and other cases involving health conditions of the\n\n                    intestinal\n\n                    tract (i.e. active ulcer, inflammatory bowel disease, gastric by-pass surgery etc.) you must seek\n\n                    nutritional\n\n                    guidance from your medical practitioner or designated registered dietician.\n\n                </p>\n\n                <br />\n\n                <h2>\n\n                    <b>Integration</b></h2>\n\n                <p>\n\n                    You can utilize components of the Meschino Wellness Platform directly to view and manage your health\n\n                    information, or you can use selected websites and devices that have been created by application\n\n                    providers and\n\n                    device manufacturers to work with Meschino Wellness. Several mechanisms allow you to manage how your\n\n                    health\n\n                    information can be accessed, used and shared.\n\n                    Meschino Wellness provides you with the technology and services to assist you in collecting, storing\n\n                    and\n\n                    analyzing your health related information online. It is a technology platform that allows access by\n\n                    multiple\n\n                    applications and devices, in order to work with your health data to improve personal health literacy\n\n                    and\n\n                    overall wellness.\n\n\n\n                </p>\n\n\n\n                <br />\n\n                <h2>\n\n                    <b>Collection of Personal Information</b></h2>\n\n                <p>\n\n                    Meschino Wellness asks you to enter an identifier and password to sign in. The first time you sign\n\n                    in to\n\n                    Meschino Wellness, Meschino Wellness asks you to create an account. To create an account, you must\n\n                    provide\n\n                    personal information such as name, date of birth, e-mail address, postal code and country/region.\n\n                    Meschino\n\n                    Wellness may request other optional information, but Meschino Wellness will clearly indicate that\n\n                    such\n\n                    information is optional.\n\n                </p>\n\n\n\n                <br />\n\n                <h2>\n\n                    <b>How Meschino Wellness uses your personal information</b></h2>\n\n                <p>\n\n                    Meschino Wellness commits to use or disclose personal information collected through Meschino\n\n                    Wellness,\n\n                    including personal health information, exclusively to provide Meschino Wellness services (which\n\n                    includes the\n\n                    billing, support, maintenance and incident resolution services) and as described in this privacy\n\n                    statement,\n\n                    unless expressly otherwise agreed to by you. Usage of the personal information (including personal\n\n                    health\n\n                    information) for the provision of Meschino Wellness solutions includes that Meschino Wellness may\n\n                    use your\n\n                    personal information:\n\n                </p>\n\n                <ul>\n\n                    <li>to provide you with information about Meschino Wellness, including updates, and notifications\n\n                    </li>\n\n                    <li>\n\n                        to send you Meschino Wellness e-mail communication, if any.\n\n                    </li>\n\n                </ul>\n\n                <br />\n\n                <p>\n\n                    Meschino Wellness occasionally hires other companies to provide services on its behalf, such as\n\n                    answering\n\n                    customer questions about products and services. Meschino Wellness gives those companies only the\n\n                    personal\n\n                    information they need to deliver the service. Meschino Wellness requires the companies to maintain\n\n                    the\n\n                    confidentiality of the personal information and prohibits them from using such information for any\n\n                    other\n\n                    purpose.\n\n                    <br />\n\n                    In addition, Meschino Wellness may use and/or disclose your personal information if Meschino\n\n                    Wellness believes\n\n                    such action is necessary to comply with applicable legislation or legal process served on Meschino\n\n                    Wellness.\n\n                    <br />\n\n                    Personal information collected on Meschino Wellness is stored and processed in on servers in Canada\n\n                    or the\n\n                    United State.\n\n                    <br />Meschino Wellness has processes and employees (i.e. Head of Privacy and other resources) whose\n\n                    responsibility is to ensure the protection of your privacy and to notify you in the event that\n\n                    Meschino\n\n                    Wellness becomes aware of a breach affecting your personal information.\n\n\n\n                </p>\n\n\n\n                <br />\n\n                <h2>\n\n                    <b>How Meschino Wellness uses aggregate information and statistics</b></h2>\n\n                <p>\n\n                    Meschino Wellness may use aggregated information from Meschino Wellness to identify and compare the\n\n                    status of\n\n                    specific common health issues as compared to other bench marks established in the specified industry\n\n                    or\n\n                    organization. This data is represented as a percent of the entire organization only. This aggregated\n\n                    information is not associated with any individual account and would not identify you. Meschino\n\n                    Wellness will\n\n                    not use or disclose your individual account and record information from Meschino Wellness.\n\n                </p>\n\n\n\n                <br />\n\n                <h2>\n\n                    <b>Account access and controls</b></h2>\n\n                <p>\n\n                    The decision to create an account with Meschino Wellness is yours. The required account information\n\n                    consists\n\n                    of a small amount of information such as your name, e-mail address, region and Meschino Wellness\n\n                    credentials.\n\n                    Meschino Wellness may request other optional information, but clearly indicates that such\n\n                    information is\n\n                    optional. You can review and update your account information. You can modify, add or delete any\n\n                    optional\n\n                    account information by signing into your Meschino Wellness account and editing your account profile.\n\n                    <br />When you close your account (by signing into your Meschino Wellness account and editing your\n\n                    account\n\n                    profile), Meschino Wellness deletes all Records for which you are the sole Custodian. Meschino\n\n                    Wellness waits\n\n                    90 days before permanently deleting your account information in order to help avoid accidental or\n\n                    malicious\n\n                    removal of your health information.\n\n                    <br />When a user with "View and modify" or Custodian access deletes a piece of health information,\n\n                    Meschino\n\n                    Wellness archives the information so that it is visible only to Record of Custodians. Solutions and\n\n                    other\n\n                    users with whom you have shared your information, but who are not Custodians of the Record, are not\n\n                    able to\n\n                    see archived health information.\n\n\n\n                </p>\n\n                <br />\n\n                <h2>\n\n                    <b>E-mail controls</b></h2>\n\n                <p>\n\n                    Meschino Wellness may send you e-mail communications. You will only receive these communications\n\n                    because you\n\n                    have authorized Meschino Wellness do so. If you do not want to receive this information, you can\n\n                    unsubscribe\n\n                    through a link at the bottom of the newsletter.\n\n                </p>\n\n\n\n                <br />\n\n                <h2>\n\n                    <b>Security of your personal information</b></h2>\n\n                <p>\n\n                    Meschino Wellness is committed to protecting the security of your personal information. Meschino\n\n                    Wellness is\n\n                    hosted on a robust industry leading hosting partner. The hosting platform meets a broad set of\n\n                    international\n\n                    and industry-specific compliance standards, such as ISO 27001, HIPAA, FedRAMP, SOC 1 and SOC 2, as\n\n                    well as\n\n                    country-specific standards including Australia IRAP, UK G-Cloud, and Singapore MTCS. The hosting\n\n                    partner was\n\n                    also the first to adopt the uniform international code of practice for cloud privacy, ISO/IEC 27018,\n\n                    which\n\n                    governs the processing of personal information by cloud service providers.\n\n                    <br />\n\n                    Rigorous third-party audits, such as by the British Standards Institute, verify the hosting\n\n                    platforms\n\n                    adherence to the strict security controls these standards mandate. As part of our commitment to\n\n                    transparency,\n\n                    you can verify our implementation of many security controls by requesting audit results from the\n\n                    certifying\n\n                    third parties or through our Host Platform account representative.\n\n                </p>\n\n\n\n                <br />\n\n                <h2>\n\n                    <b>Use of cookies</b></h2>\n\n                <p>\n\n                    Meschino Wellness use cookies with Meschino Wellness to enable you to sign in and to help\n\n                    personalize Meschino\n\n                    Wellness. A cookie is a small text file that a web page server places on your hard disk. It is not\n\n                    possible to\n\n                    use cookies to run programs or deliver viruses to your computer. A Web server assigns cookies\n\n                    uniquely to you\n\n                    and only a Web server in the domain that issued the cookie to you can read the cookies.\n\n                    One of the primary purposes of cookies is to provide a convenience feature to save you time. For\n\n                    example, if\n\n                    you personalize a Web page, or navigate within a site, a cookie helps the site to recall your\n\n                    specific\n\n                    information on subsequent visits. Using cookies simplifies the process of delivering relevant\n\n                    content, eases\n\n                    site navigation, and so on. When you return to the Web site, you can retrieve the information you\n\n                    previously\n\n                    provided, so you can easily use the site\'s features that you customized.\n\n                    You have the ability to accept or decline cookies. Most Web browsers automatically accept cookies,\n\n                    but you can\n\n                    usually modify your browser setting to decline some or all cookies if you prefer. If you choose to\n\n                    decline all\n\n                    cookies, you may not be able to use interactive features of this or other Web sites that depend on\n\n                    cookies.\n\n                </p>\n\n                <br />\n\n                <h2>\n\n                    <b>Use of Web beacons</b></h2>\n\n\n\n                <p>\n\n                    Meschinowellness.com Web pages may contain electronic images known as Web beacons sometimes called\n\n                    single-pixel gifs that may be used:\n\n                </p>\n\n                <ul>\n\n                    <li> to assist in delivering cookies on Meschino Wellness sites</li>\n\n                    <li> to enable Meschino Wellness to count users who have visited those pages</li>\n\n                    <li> to deliver co-branded services</li>\n\n                </ul>\n\n                <p>\n\n                    Meschino Wellness may include Web beacons in e-mail messages or in its newsletters in order to\n\n                    determine\n\n                    whether you opened or acted upon those messages.\n\n                    <br />\n\n                    Meschino Wellness may also employ Web beacons from third parties to help it compile aggregated\n\n                    statistics and\n\n                    determine the effectiveness of its promotional campaigns. Meschino Wellness prohibits third parties\n\n                    from using\n\n                    Web beacons on Meschino Wellness sites to collect or access your personal information. Meschino\n\n                    Wellness may\n\n                    collect information about your visit to meschiowellness.com, including the pages you view, the links\n\n                    you\n\n                    click, and other actions taken in connection with the Service. Meschino Wellness also collects\n\n                    certain\n\n                    standard, non-personally identifiable information that your browser sends to every Web site you\n\n                    visit, such as\n\n                    your IP address, browser type and language, access times, and referring Web site addresses.\n\n\n\n                </p>\n\n                <br />\n\n                <h2>\n\n                    <b>Changes to this privacy statement</b></h2>\n\n                <p>\n\n                    Meschino Wellness may occasionally update this privacy statement. In such event, Meschino Wellness\n\n                    will notify\n\n                    you either by placing a prominent notice on the home page of the Meschino Wellness Web site or by\n\n                    sending you\n\n                    a notification directly. Meschino Wellness encourages you to review this privacy statement\n\n                    periodically to\n\n                    stay informed about how Meschino Wellness helps you to protect the personal information collected.\n\n                    Your\n\n                    continued use of Meschino Wellness constitutes your agreement to this privacy statement and any\n\n                    updates.\n\n                    Please be aware that this privacy statement does not apply to personal information you may have\n\n                    provided to\n\n                    Meschino Wellness in the context of other, separately operated, Meschino Wellness products or\n\n                    services.\n\n                </p>\n\n                <br />\n\n                <h2> <b>Contact information for privacy related questions and/or complaint</b></h2>\n\n\n\n                <p>\n\n                    Meschino Wellness welcomes your comments regarding this privacy statement. If you have a complaint\n\n                    concerning\n\n                    Meschino Wellness’ privacy standards, please submit it to the applicable Privacy Office, however, as\n\n                    recommended by the Office of the Privacy Commissioner of Canada, you are strongly encouraged to try\n\n                    first to\n\n                    settle the matter directly with us, please contact us at <span style="color: blue;">\n\n                        privacy@meschinowellness.com.</span> All questions and/or\n\n                    complaints will be treated as confidential.\n\n                </p>\n\n\n\n                <br />\n\n                <h2>\n\n                    <b>DISCLOSURE</b></h2>\n\n                <p>\n\n                    The assessment you’re about to complete will evaluate important aspects of your diet, lifestyle and\n\n                    other\n\n                    health practices, as well as health conditions and risk factors that play a significant role in your\n\n                    health\n\n                    and longevity profile. Based upon scientific evidence available from experimental and clinical\n\n                    studies, we\n\n                    will assemble a number of nutrition, exercise and other lifestyle considerations specific to your\n\n                    individual\n\n                    circumstances.\n\n                    <br />\n\n                    Your personalized wellness report is to be used for educational purposes only and does not take into\n\n                    account\n\n                    any food, drug or supplement allergies or sensitivities. You must consult your health practitioner\n\n                    before\n\n                    making any changes to your dietary, exercise or supplementation practices.\n\n\n\n                </p>\n\n            </div>\n\n            <br />\n\n            <p>By Signing up you have:</p>\n\n            <ul>\n\n                <li>Read and undersatnd the disclosure. </li>\n\n                <li>At least 15 years old.</li>\n\n                <li>Trust Meschino Health and Wellness with the personal and private health information you provide.\n\n                </li>\n\n            </ul>\n\n        </ion-card-content>\n\n    </ion-card>\n\n\n\n</ion-content>\n\n<ion-footer>\n\n</ion-footer>'/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\templates\template.html"*/
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavController"], __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavParams"]])
    ], TemplatePage);
    return TemplatePage;
}());

//# sourceMappingURL=template.js.map

/***/ }),

/***/ 94:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return MyWellnessWalletPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(4);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__providers__ = __webpack_require__(15);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__notification_notification__ = __webpack_require__(72);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__templates_template__ = __webpack_require__(93);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};





/**
 * Generated class for the DashboardPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
var MyWellnessWalletPage = /** @class */ (function () {
    function MyWellnessWalletPage(navCtrl, navParams, loading, userService, notificationService, events, menu) {
        var _this = this;
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.loading = loading;
        this.userService = userService;
        this.notificationService = notificationService;
        this.events = events;
        this.menu = menu;
        this.account = {
            deviceid: "",
            SecretToken: "",
            FirstName: "",
            LastName: ""
        };
        this.rewardpoints = 0;
        this.bio_age = "0";
        this.mhrs_score = "0";
        this.profilepic = "assets/img/loader.gif";
        this.IsHRACompleted = false;
        this.notificationCount = ""; //localStorage.getItem("NotificationCount");
        this.FCMToken = localStorage.getItem("FCMToken");
        this.menu.enable(true);
        this.IsHRACompleted =
            localStorage.getItem("IsHRACompleted") == "true" ? true : false;
        console.log(this.IsHRACompleted, "IsHRACompleted");
        this.loader = this.loading.create({
            content: "Please wait..."
        });
        this.loader.present().then(function () {
            _this.loadNotificationCount();
            _this.EmitterNotificationCount();
            _this.loadUserInfo();
        });
    }
    MyWellnessWalletPage.prototype.loadUserInfo = function () {
        var _this = this;
        this.account.FirstName = localStorage.getItem("FirstName");
        this.account.LastName = localStorage.getItem("LastName");
        this.rewardpoints = parseInt(localStorage.getItem("RewardPoint"));
        this.profilepic = localStorage.getItem("ProfileImage");
        this.bio_age = localStorage.getItem("bio_age");
        this.mhrs_score = localStorage.getItem("mhrs_score");
        var userAcc = {
            DeviceId: localStorage.getItem("deviceid"),
            SecretToken: localStorage.getItem("SecretToken")
        };
        this.userService.getUserData(userAcc).subscribe(function (res) {
            localStorage.setItem("RewardPoint", res.RewardPoint);
            localStorage.setItem("bio_age", res.bio_age);
            localStorage.setItem("mhrs_score", res.mhrs_score);
            _this.rewardpoints = parseInt(localStorage.getItem("RewardPoint"));
            _this.bio_age = localStorage.getItem("bio_age");
            _this.mhrs_score = localStorage.getItem("mhrs_score");
        });
        this.loader.dismiss();
    };
    MyWellnessWalletPage.prototype.ionViewDidLoad = function () {
        console.log("ionViewDidLoad MyWellnessWalletPage");
    };
    MyWellnessWalletPage.prototype.gotoUrl = function (navurl) {
        this.navCtrl.setRoot(navurl);
    };
    MyWellnessWalletPage.prototype.loadNotificationCount = function () {
        var _this = this;
        var userAcc = {
            DeviceId: localStorage.getItem("deviceid"),
            SecretToken: localStorage.getItem("SecretToken")
        };
        this.userService.GetPushNotificationCount(userAcc).subscribe(function (res) {
            var msgcount = res.Count;
            _this.notificationCount = msgcount > 0 ? msgcount : "";
            localStorage.setItem("notificationCount", _this.notificationCount);
        });
    };
    MyWellnessWalletPage.prototype.EmitterNotificationCount = function () {
        var _this = this;
        this.events.subscribe("PushNotification", function (PushNotification) {
            var msgcount = PushNotification.Count;
            _this.notificationCount = msgcount > 0 ? msgcount : "";
            localStorage.setItem("notificationCount", _this.notificationCount);
        });
    };
    MyWellnessWalletPage.prototype.goNotification = function () {
        this.navCtrl.push(__WEBPACK_IMPORTED_MODULE_3__notification_notification__["a" /* NotificationPage */]);
    };
    MyWellnessWalletPage.prototype.ViewTemplate = function (Name) {
        this.navCtrl.push(__WEBPACK_IMPORTED_MODULE_4__templates_template__["a" /* TemplatePage */], {
            Content: Name
        });
    };
    MyWellnessWalletPage = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: "page-my-wellness-wallet",template:/*ion-inline-start:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\my-wellness-wallet\my-wellness-wallet.html"*/'<!--\n\n  Generated template for the DashboardPage page.\n\n\n\n  See http://ionicframework.com/docs/components/#navigation for more info on\n\n  Ionic pages and navigation.\n\n-->\n\n<ion-header>\n\n\n\n  <ion-navbar color="primary">\n\n    <button ion-button menuToggle icon-only>\n\n      <ion-icon name=\'menu\'></ion-icon>\n\n    </button>\n\n    <ion-title>My Wellness Wallet</ion-title>\n\n    <ion-buttons end class="left-nav-buttons">\n\n      <button id="notification-button" ion-button icon-only (click)="goNotification()">\n\n        <ion-icon name="notifications">\n\n          <ion-badge id="notifications-badge" color="danger">{{notificationCount}}</ion-badge>\n\n        </ion-icon>\n\n      </button>\n\n    </ion-buttons>\n\n  </ion-navbar>\n\n  <div class="div-center">\n\n    <table style="width: 100%">\n\n      <tr>\n\n        <td>\n\n          <div class="div-bio-bg fr">\n\n            <ion-title>{{bio_age}}</ion-title>\n\n            <ion-title class="no-pd-title" (click)="ViewTemplate(\'BioAge\')">Bio Age</ion-title>\n\n          </div>\n\n        </td>\n\n        <td>\n\n          &nbsp;&nbsp;\n\n        </td>\n\n        <td>\n\n          <div class="div-bio-bg fl">\n\n            <ion-title>{{mhrs_score}}</ion-title>\n\n            <ion-title class="no-pd-title" (click)="ViewTemplate(\'MHRScore\')">MHR Score</ion-title>\n\n          </div>\n\n        </td>\n\n      </tr>\n\n    </table>\n\n    <table style="width: 100% ;" class="tbl-hra">\n\n      <tr (click)="gotoUrl(\'MyHraPage\')">\n\n        <td>\n\n          <img src="assets/img/MyHRA_icn.png">\n\n        </td>\n\n        <td>\n\n          My HRA\n\n        </td>\n\n        <td>\n\n          <img src="assets/img/arrow.png" style="padding: 10px">\n\n        </td>\n\n      </tr>\n\n      <!-- <tr (click)="gotoUrl(\'MyWellnessPlanPage\')">\n\n        <td>\n\n          <img src="assets/img/MyWellness_icn.png">\n\n        </td>\n\n        <td>\n\n          My Wellness Report\n\n        </td>\n\n        <td>\n\n          <img src="assets/img/arrow.png" style="padding: 10px">\n\n        </td>\n\n      </tr> -->\n\n      <tr (click)="gotoUrl(\'MyWellnessPlanPage\')" *ngIf="IsHRACompleted">\n\n        <td>\n\n          <img src="assets/img/create_wellness_icn.png">\n\n        </td>\n\n        <td>\n\n          Create My Wellness Plan\n\n        </td>\n\n        <td>\n\n          <img src="assets/img/arrow.png" style="padding: 10px">\n\n        </td>\n\n      </tr>\n\n    </table>\n\n    <!--<br/> <input type="text" value="{{FCMToken}}"> -->\n\n\n\n  </div>\n\n</ion-header>\n\n<ion-content>\n\n\n\n</ion-content>'/*ion-inline-end:"C:\Lenevo\downloads\meschino-master\PushDemo\MeschinoWellness\src\pages\my-wellness-wallet\my-wellness-wallet.html"*/
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavController"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavParams"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["LoadingController"],
            __WEBPACK_IMPORTED_MODULE_2__providers__["f" /* UserService */],
            __WEBPACK_IMPORTED_MODULE_2__providers__["c" /* NotificationService */],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["Events"],
            __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["MenuController"]])
    ], MyWellnessWalletPage);
    return MyWellnessWalletPage;
}());

//# sourceMappingURL=my-wellness-wallet.js.map

/***/ })

},[344]);
//# sourceMappingURL=main.js.map