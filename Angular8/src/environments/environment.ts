// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  //apiUrlBase: "https://localhost:5001/api"
  apiUrlBase: "/api", //use proxy.conf.js instead of hardcode the url

  //follow the required settings property from oidc UserManager
  //same as mvc authorization, but use implicit flow
  openIdConnectSettings: {
    authority: "https://localhost:6001/",
    client_id: "note-client",
    redirect_uri: "http://localhost:4200/signin-oidc", //redirect to here after login
    scope: "openid profile email restapi",
    response_type: "id_token token",
    post_logout_redirect_uri: "http://localhost:4200/", //redirect to here after logout
    automaticSilentRenew: true, //renew acess token when timmeout
    silent_redirect_uri: "http://localhost:4200/redirect-silent-renew",
  },
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
