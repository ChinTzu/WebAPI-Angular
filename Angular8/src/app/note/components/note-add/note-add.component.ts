import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { NoteService } from "../../services/note.service";
import { TinymceService } from "../../services/tinymce.service";
import { MatSnackBar } from "@angular/material";

@Component({
  selector: "app-note-add",
  templateUrl: "./note-add.component.html",
  styleUrls: ["./note-add.component.scss"],
})
export class NoteAddComponent implements OnInit {
  editorSettings;
  noteForm: FormGroup;

  constructor(
    private router: Router,
    private noteService: NoteService,
    private tinymce: TinymceService,
    private form: FormBuilder,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.noteForm = this.form.group({
      title: ["", [Validators.required]],
      body: ["", [Validators.required]],
    });

    this.editorSettings = this.tinymce.getSettings();
  }

  submit() {
    if (this.noteForm.dirty && this.noteForm.valid) {
      this.noteService.addNote(this.noteForm.value).subscribe(
        (note) => {
          this.router.navigate(["/note/notes/", note.id]);
        },
        (validationResult) => {
          this.snackBar.open("There are validation errors!", "Close", {
            duration: 3000,
          });
        }
      );
    }
  }
}
