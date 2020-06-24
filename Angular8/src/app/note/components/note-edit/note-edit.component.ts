import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { MatSnackBar } from "@angular/material";
import { TinymceService } from "../../services/tinymce.service";
import { NoteService } from "../../services/note.service";
import { Router, ActivatedRoute, ParamMap } from "@angular/router";
import { switchMap } from "rxjs/operators";
import { compare } from "fast-json-patch";

@Component({
  selector: "app-note-edit",
  templateUrl: "./note-edit.component.html",
  styleUrls: ["./note-edit.component.scss"],
})
export class NoteEditComponent implements OnInit {
  note: any;
  noteForm: FormGroup;
  editorSettings;
  id: string | number;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private noteService: NoteService,
    private tinymce: TinymceService,
    private form: FormBuilder,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.route.paramMap
      .pipe(
        switchMap((params: ParamMap) => {
          this.id = params.get("id");
          return this.noteService.getNoteById(this.id);
        })
      )
      .subscribe((note) => {
        this.note = { title: note.title, body: note.body };

        this.noteForm = this.form.group({
          title: [this.note.title, [Validators.required]],
          body: [this.note.body, [Validators.required]],
        });
      });

    this.editorSettings = this.tinymce.getSettings();
  }

  save() {
    if (this.noteForm.dirty && this.noteForm.valid) {
      const patchDocument = compare(this.note, this.noteForm.value);

      this.noteService
        .partiallyUpdateNote(this.id, patchDocument)
        .subscribe(() => {
          this.router.navigate(["/note/note-list/", this.id]);
        });
    }
  }
}
