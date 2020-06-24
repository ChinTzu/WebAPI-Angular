import { Component, OnInit } from "@angular/core";
import { Note } from "../../models/note";
import { ActivatedRoute } from "@angular/router";
import { NoteService } from "../../services/note.service";
import { OpenIdConnectService } from "src/app/shared/oidc/open-id-connect.service";

@Component({
  selector: "app-note-detail",
  templateUrl: "./note-detail.component.html",
  styleUrls: ["./note-detail.component.scss"],
})
export class NoteDetailComponent implements OnInit {
  note: Note;

  constructor(
    private route: ActivatedRoute,
    private noteService: NoteService,
    private openIdConnectService: OpenIdConnectService
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params) => {
      let id = +params["id"];
      if (!id) {
        id = 1;
      }
      this.note = null;

      this.noteService.getNoteById(id).subscribe((note) => {
        this.note = note;
      });
    });
  }
}
