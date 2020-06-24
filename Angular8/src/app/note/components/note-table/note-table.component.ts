import { Component, OnInit, ViewChild } from "@angular/core";
import { NoteService } from "../../services/note.service";
import { Note } from "../../models/note";
import {
  MatPaginator,
  MatSort,
  Sort,
  PageEvent,
  MatTableDataSource,
} from "@angular/material";
import { Subject } from "rxjs";
import { debounceTime, distinctUntilChanged } from "rxjs/operators";
import { PageMeta } from "../../../shared/models/page-meta";
import { ResultWithLinks } from "../../../shared/models/result-with-links";
import { NoteParameters } from "../../models/note-parameters";

@Component({
  selector: "app-note-table",
  templateUrl: "./note-table.component.html",
  styleUrls: ["./note-table.component.scss"],
})
export class NoteTableComponent implements OnInit {
  pageMeta: PageMeta;
  noteParameter = new NoteParameters({
    orderBy: "id desc",
    pageSize: 10,
    pageNumber: 0,
  });

  displayedColumns: string[] = ["id", "title", "username", "updateTime"];
  searchKeyUp = new Subject<string>();
  dataSource;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(private noteService: NoteService) {
    const subscription = this.searchKeyUp
      .pipe(debounceTime(500), distinctUntilChanged())
      .subscribe(() => {
        this.load();
      });
  }

  ngOnInit() {
    this.load();
  }

  load() {
    this.noteService.getPagedNotes(this.noteParameter).subscribe((resp) => {
      this.pageMeta = JSON.parse(resp.headers.get("X-Pagination")) as PageMeta;
      const pagedResult = { ...resp.body } as ResultWithLinks<Note>;
      this.dataSource = new MatTableDataSource(pagedResult.value);
      this.dataSource.paginator = this.paginator;
    });
  }

  applyFilter(filterValue: String) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  sortData(sort: Sort) {
    this.noteParameter.orderBy = null;
    if (sort.direction) {
      this.noteParameter.orderBy = sort.active;
      if (sort.direction === "desc") {
        this.noteParameter.orderBy += " desc";
      }
    }
    this.load();
  }

  onPaging(pageEvent: PageEvent) {
    this.noteParameter.pageNumber = pageEvent.pageIndex;
    this.noteParameter.pageSize = pageEvent.pageSize;
    this.load();
  }
}
