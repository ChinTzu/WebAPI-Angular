import { Component, OnInit, ViewChild, NgZone } from "@angular/core";
import { Router } from "@angular/router";
import { MatDrawer } from "@angular/material";

const SMALL_WIDTH_BREAKPOINT = 720;

@Component({
  selector: "app-sidenav",
  templateUrl: "./sidenav.component.html",
  styleUrls: ["./sidenav.component.scss"],
})
export class SidenavComponent implements OnInit {
  private mediaMatcher: MediaQueryList = matchMedia(
    `(max-width: ${SMALL_WIDTH_BREAKPOINT}px)`
  );

  constructor(private router: Router, zone: NgZone) {
    this.mediaMatcher.addListener((mql) =>
      zone.run(
        () =>
          (this.mediaMatcher = matchMedia(
            `(max-width: ${SMALL_WIDTH_BREAKPOINT}px)`
          ))
      )
    );
  }

  @ViewChild(MatDrawer, { static: false }) drawer: MatDrawer;

  ngOnInit() {
    this.router.events.subscribe(() => {
      if (this.isScreenSmall()) {
        this.drawer.close();
      }
    });
  }

  isScreenSmall(): boolean {
    return this.mediaMatcher.matches;
  }
}
