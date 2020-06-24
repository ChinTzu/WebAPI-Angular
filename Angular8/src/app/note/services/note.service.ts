import { Injectable } from "@angular/core";
import { BaseService } from "src/app/shared/base.service";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { NoteParameters } from "../models/note-parameters";
import { Note } from "../models/note";
import { NoteAdd } from "../models/note-add";
import { Observable } from "rxjs";
import { Operation } from "fast-json-patch";

@Injectable({
  providedIn: "root",
})
export class NoteService extends BaseService {
  constructor(private http: HttpClient) {
    super();
  }

  getPagedNotes(noteParameter?: any | NoteParameters) {
    return this.http.get(`${this.apiUrlBase}/notes`, {
      headers: new HttpHeaders({
        Accept: "application/vnd.mycompany.hateoas+json",
      }),
      observe: "response",
      params: noteParameter,
    });
  }

  addNote(note: NoteAdd) {
    const httpOptions = {
      headers: new HttpHeaders({
        "Content-Type": "application/vnd.mycompany.note.create+json",
        Accept: "application/vnd.mycompany.hateoas+json",
      }),
    };

    return this.http.post<Note>(`${this.apiUrlBase}/notes`, note, httpOptions);
  }

  getNoteById(id: number | string): Observable<Note> {
    return this.http.get<Note>(`${this.apiUrlBase}/notes/${id}`);
  }

  partiallyUpdateNote(
    id: number | string,
    patchDocument: Operation[]
  ): Observable<any> {
    return this.http.patch(`${this.apiUrlBase}/notes/${id}`, patchDocument, {
      headers: { "Content-Type": "application/json-patch+json" },
    });
  }
}
