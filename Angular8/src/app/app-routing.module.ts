import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { SigninOidcComponent } from "./shared/oidc/signin-oidc/signin-oidc.component";
import { RedirectSilentRenewComponent } from "./shared/oidc/redirect-silent-renew/redirect-silent-renew.component";

const routes: Routes = [
  { path: "note", loadChildren: "./note/note.module#NoteModule" },
  { path: "signin-oidc", component: SigninOidcComponent },
  { path: "redirect-silentrenew", component: RedirectSilentRenewComponent },
  { path: "**", redirectTo: "note" },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
