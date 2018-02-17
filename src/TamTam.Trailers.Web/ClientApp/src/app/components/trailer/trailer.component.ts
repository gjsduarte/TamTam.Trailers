import { Component, Input } from "@angular/core";

import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { ShareButtons } from "@ngx-share/core";
import {VideoService} from "../../services";

@Component({
  selector: 'app-trailer',
  templateUrl: './trailer.component.html'
})
export class TrailerComponent {
  @Input() video: Video;

  constructor(public activeModal: NgbActiveModal, public share: ShareButtons, public videoService: VideoService) {}
}
