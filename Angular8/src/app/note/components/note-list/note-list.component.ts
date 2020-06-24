import { Component, OnInit } from "@angular/core";
import { NoteParameters } from "../../models/note-parameters";
import { PageMeta } from "src/app/shared/models/page-meta";
import { Note } from "../../models/note";
import { ResultWithLinks } from "src/app/shared/models/result-with-links";
import { NoteService } from "../../services/note.service";
import { OpenIdConnectService } from "src/app/shared/oidc/open-id-connect.service";

@Component({
  selector: "app-note-list",
  templateUrl: "./note-list.component.html",
  styleUrls: ["./note-list.component.scss"],
})
export class NoteListComponent implements OnInit {
  notes: Note[];
  pageMeta: PageMeta;
  noteParameter = new NoteParameters({
    orderBy: "id desc",
    pageSize: 10,
    pageNumber: 0,
  });

  constructor(
    private noteService: NoteService,
    private openIdConnectService: OpenIdConnectService
  ) {}

  ngOnInit() {
    this.getNotes();
  }

  getNotes() {
    this.noteService.getPagedNotes(this.noteParameter).subscribe((resp) => {
      this.pageMeta = JSON.parse(resp.headers.get("X-Pagination")) as PageMeta;
      const result = { ...resp.body } as ResultWithLinks<Note>;
      this.notes = result.value;
    });
  }

  onScroll() {
    this.noteParameter.pageNumber++;
    if (this.noteParameter.pageNumber < this.pageMeta.totalCount) {
      this.getNotes();
    }
  }
}
