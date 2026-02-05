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
  BedDto,
  RoomDto,
  BedCrudServiceServiceProxy,
  RoomDtoServiceServiceProxy,
  UpdateBedDto,
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
export class EditBedDialogComponent
  extends AppComponentBase
  implements OnInit {
  saving = false;

  bed: BedDto = new BedDto();
  rooms: RoomDto[] = [];


  @Output() onSave = new EventEmitter<BedDto>();

  constructor(
    injector: Injector,
    private _bedService: BedCrudServiceServiceProxy,
    private _roomService: RoomDtoServiceServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }
  ngOnInit(): void {
  this.loadRooms();
}

loadRooms(): void {
  this._roomService.getAllRooms().subscribe((result: RoomDto[]) => {
    this.rooms = result;

    // Now assign the bed after rooms are loaded
    if (this.bsModalRef.content?.bed) {
      this.bed = this.bsModalRef.content.bed;

      // Ensure the types match
      this.bed.roomId = Number(this.bed.roomId);
    } else {
      this.notify.warn(this.l('BedDataNotFound'));
    }
  });
}



  save(): void {
    if (this.saving) {
      return;
    }

    this.saving = true;

    const input = new UpdateBedDto();
    input.init(this.bed);

    this._bedService
      .updateBed(input)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe({
        next: () => {
          this.notify.info(this.l('SavedSuccessfully'));
          this.bsModalRef.hide();
          this.onSave.emit(this.bed);
        },
        error: (error) => {
          this.notify.error(this.l('SaveFailed'));
          console.error('Update bed error:', error);
        },
      });
  }
}
