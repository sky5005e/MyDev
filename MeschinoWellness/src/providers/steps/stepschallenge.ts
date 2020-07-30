import "rxjs/add/operator/toPromise";

import { Injectable } from "@angular/core";
import { Api } from "../api/api";

@Injectable()
export class StepChallengeService {
  constructor(public api: Api) {}
  GetUserOverallProgressOfStepChallenge(accountInfo: any) {
    let seq = this.api
      .post(
        "api/WellnessAPI/GetUserOverallProgressOfStepChallenge",
        accountInfo
      )
      .share();
    seq.subscribe(
      (res: any) => {},
      err => {
        console.error("ERROR", err);
      }
    );
    return seq;
  }
  GetUserStepChallengeProgressOverviewCount(accountInfo: any) {
    let seq = this.api
      .post(
        "api/WellnessAPI/GetUserStepChallengeProgressOverviewCount",
        accountInfo
      )
      .share();
    seq.subscribe(
      (res: any) => {},
      err => {
        console.error("ERROR", err);
      }
    );
    return seq;
  }
  SaveLogStepsNumber(accountInfo: any) {
    let seq = this.api
      .post("api/WellnessAPI/SaveLogStepsNumber", accountInfo)
      .share();
    seq.subscribe(
      (res: any) => {},
      err => {
        console.error("ERROR", err);
      }
    );
    return seq;
  }

  GetLoggedStepsActivitiesByDate(accountInfo: any) {
    let seq = this.api
      .post("api/WellnessAPI/GetLoggedStepsActivitiesByDate", accountInfo)
      .share();
    seq.subscribe(
      (res: any) => {},
      err => {
        console.error("ERROR", err);
      }
    );
    return seq;
  }

  DeleteLoggedStepsActivities(accountInfo: any) {
    let seq = this.api
      .post("api/WellnessAPI/DeleteLoggedStepsActivities", accountInfo)
      .share();
    seq.subscribe(
      (res: any) => {},
      err => {
        console.error("ERROR", err);
      }
    );
    return seq;
  }

  SearchStepsActivities(accountInfo: any) {
    let seq = this.api
      .post("api/WellnessAPI/SearchStepsActivities", accountInfo)
      .share();
    seq.subscribe(
      (res: any) => {},
      err => {
        console.error("ERROR", err);
      }
    );
    return seq;
  }

  SaveStepsActivitiesData(accountInfo: any) {
    let seq = this.api
      .post("api/WellnessAPI/SaveStepsActivitiesData", accountInfo)
      .share();
    seq.subscribe(
      (res: any) => {},
      err => {
        console.error("ERROR", err);
      }
    );
    return seq;
  }

  GetOverviewAndGraphicViewData(accountInfo: any) {
    let seq = this.api
      .post("api/WellnessAPI/GetOverviewAndGraphicViewData", accountInfo)
      .share();
    seq.subscribe(
      (res: any) => {},
      err => {
        console.error("ERROR", err);
      }
    );
    return seq;
  }

  GetUserStepChallengeHistory(accountInfo: any) {
    let seq = this.api
      .post("api/WellnessAPI/GetUserStepChallengeHistory", accountInfo)
      .share();
    seq.subscribe(
      (res: any) => {},
      err => {
        console.error("ERROR", err);
      }
    );
    return seq;
  }

  DeletePreviousChallengeHistory(accountInfo: any) {
    let seq = this.api
      .post("api/WellnessAPI/DeletePreviousChallengeHistory", accountInfo)
      .share();
    seq.subscribe(
      (res: any) => {},
      err => {
        console.error("ERROR", err);
      }
    );
    return seq;
  }

  ResetUserStepChallenge(accountInfo: any) {
    let seq = this.api
      .post("api/WellnessAPI/ResetUserStepChallenge", accountInfo)
      .share();
    seq.subscribe(
      (res: any) => {},
      err => {
        console.error("ERROR", err);
      }
    );
    return seq;
  }

  StartUserNewStepChallenge(accountInfo: any) {
    let seq = this.api
      .post("api/WellnessAPI/StartUserNewStepChallenge", accountInfo)
      .share();
    seq.subscribe(
      (res: any) => {},
      err => {
        console.error("ERROR", err);
      }
    );
    return seq;
  }
}
