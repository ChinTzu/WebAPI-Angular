import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { HttpClientModule } from "@angular/common/http";
import { SigninOidcComponent } from "./shared/oidc/signin-oidc/signin-oidc.component";
import { RedirectSilentRenewComponent } from "./shared/oidc/redirect-silent-renew/redirect-silent-renew.component";
import { OpenIdConnectService } from "./shared/oidc/open-id-connect.service";
import { RequireAuthenticatedUserRouteGuard } from "./shared/oidc/require-authenticated-user-route.guard";

@NgModule({
  declarations: [
    AppComponent,
    SigninOidcComponent,
    RedirectSilentRenewComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
  ],
  providers: [OpenIdConnectService, RequireAuthenticatedUserRouteGuard],
  bootstrap: [AppComponent],
})
export class AppModule {}
