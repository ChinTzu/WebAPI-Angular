import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { NoteAppComponent } from "./note-app.component";
import { NoteListComponent } from "./components/note-list/note-list.component";
import { NoteAddComponent } from "./components/note-add/note-add.component";
import { RequireAuthenticatedUserRouteGuard } from "../shared/oidc/require-authenticated-user-route.guard";
import { NoteTableComponent } from "./components/note-table/note-table.component";
import { NoteEditComponent } from "./components/note-edit/note-edit.component";
import { NoteDetailComponent } from "./components/note-detail/note-detail.component";

const routes: Routes = [
  {
    path: "",
    component: NoteAppComponent,
    children: [
      { path: "note-list", component: NoteListComponent },
      { path: "note-table", component: NoteTableComponent },
      {
        path: "note-add",
        component: NoteAddComponent,
        canActivate: [RequireAuthenticatedUserRouteGuard],
      },
      {
        path: "note-edit/:id",
        component: NoteEditComponent,
        canActivate: [RequireAuthenticatedUserRouteGuard],
      },
      { path: "note-detail/:id", component: NoteDetailComponent },
      { path: "**", redirectTo: "note-list" },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NoteRoutingModule {}
