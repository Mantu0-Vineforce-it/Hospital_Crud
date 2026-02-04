import {
  Component,
  Injector,
  OnInit,
  EventEmitter,
  Output,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';

import { AppComponentBase } from '../../../shared/app-component-base';
import {
  RoomDto,
  RoomDtoServiceServiceProxy,
  UpdateRoomDto,
} from '../../../shared/service-proxies/service-proxies';

import { AbpModalHeaderComponent } from '../../../shared/components/modal/abp-modal-header.component';
import { AbpModalFooterComponent } from '../../../shared/components/modal/abp-modal-footer.component';
import { AbpValidationSummaryComponent } from '../../../shared/components/validation/abp-validation.summary.component';
import { LocalizePipe } from '../../../shared/pipes/localize.pipe';

@Component({
  templateUrl: './edit.component.html',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    AbpModalHeaderComponent,
    AbpModalFooterComponent,
    AbpValidationSummaryComponent,
    LocalizePipe,
  ],
})
export class EditRoomDialogComponent
  extends AppComponentBase
  implements OnInit {

  saving = false;
  room: RoomDto = new RoomDto();

  @Output() onSave = new EventEmitter<RoomDto>();

  /** Room types for dropdown */
readonly roomTypes = [
  { value: 1, label: 'General Ward' },
  { value: 2, label: 'Semi Private' },
  { value: 3, label: 'Private' },
  { value: 4, label: 'ICU' },
  { value: 5, label: 'VIP Suite' },
];



  constructor(
    injector: Injector,
    private _roomService: RoomDtoServiceServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    if (this.bsModalRef.content?.room) {
      this.room = this.bsModalRef.content.room;
    } else {
      this.notify.warn(this.l('RoomDataNotFound'));
    }
  }

  save(): void {
    if (this.saving) {
      return;
    }

    this.saving = true;

    const input = new UpdateRoomDto();
    input.init(this.room);

    this._roomService
      .updateRoom(input)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe({
        next: () => {
          this.notify.info(this.l('SavedSuccessfully'));
          this.bsModalRef.hide();
          this.onSave.emit(this.room);
        },
        error: (error) => {
          this.notify.error(this.l('SaveFailed'));
          console.error('Update room error:', error);
        },
      });
  }
}
