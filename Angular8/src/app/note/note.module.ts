import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { NoteRoutingModule } from "./note-routing.module";
import { NoteAppComponent } from "./note-app.component";
import { SidenavComponent } from "./components/sidenav/sidenav.component";
import { ToolbarComponent } from "./components/toolbar/toolbar.component";
import { MaterialModule } from "../shared/material/material.module";
import { NoteService } from "./services/note.service";
import { NoteListComponent } from "./components/note-list/note-list.component";
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { AuthorizationHeaderInterceptor } from "../shared/oidc/authorization-header-interceptor.interceptor";
import { NoteTileComponent } from "./components/note-tile/note-tile.component";
import { NoteAddComponent } from "./components/note-add/note-add.component";
import { TinymceService } from "./services/tinymce.service";
import { ReactiveFormsModule, FormsModule } from "@angular/forms";
import { EditorModule } from "@tinymce/tinymce-angular";
import { NoteEditComponent } from "./components/note-edit/note-edit.component";
import { InfiniteScrollModule } from "ngx-infinite-scroll";
import { NoteTableComponent } from './components/note-table/note-table.component';
import { NoteDetailComponent } from './components/note-detail/note-detail.component';

@NgModule({
  declarations: [
    NoteAppComponent,
    SidenavComponent,
    ToolbarComponent,
    NoteListComponent,
    NoteTileComponent,
    NoteAddComponent,
    NoteEditComponent,
    NoteTableComponent,
    NoteDetailComponent,
  ],
  imports: [
    CommonModule,
    NoteRoutingModule,
    MaterialModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    EditorModule,
    InfiniteScrollModule,
  ],
  providers: [
    NoteService,
    TinymceService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthorizationHeaderInterceptor,
      multi: true,
    },
  ],
})
export class NoteModule {}
