import {
  Component,
  Injector,
  OnInit,
  ChangeDetectorRef,
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
export class EditBedDialogComponent extends AppComponentBase implements OnInit {
  saving = false;

  // Form model
  bed: BedDto = new BedDto();

  // Rooms for dropdown
  rooms: RoomDto[] = [];

  // Field-specific backend validation errors
  formErrors: { bedNumber?: string; roomId?: string; general?: string } = {};

  @Output() onSave = new EventEmitter<BedDto>();

  constructor(
    injector: Injector,
    private _bedService: BedCrudServiceServiceProxy,
    private _roomService: RoomDtoServiceServiceProxy,
    public bsModalRef: BsModalRef,
    private cd: ChangeDetectorRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.loadRooms();
  }

  loadRooms(): void {
    this._roomService.getAllRooms()
      .pipe(finalize(() => this.cd.detectChanges()))
      .subscribe({
        next: (rooms: RoomDto[]) => {
          this.rooms = rooms;

          // Assign the bed data after rooms are loaded
          if (this.bsModalRef.content?.bed) {
            this.bed = this.bsModalRef.content.bed;

            // Ensure roomId is a number for <select> binding
            this.bed.roomId = Number(this.bed.roomId);
          } else {
            this.notify.warn(this.l('BedDataNotFound'));
          }
        },
        error: (err) => {
          this.notify.error(this.l('FailedToLoadRooms'));
          console.error('Load rooms error:', err);
        },
      });
  }

 save(): void {
  this.saving = true;
  this.formErrors = {};

  const input = new UpdateBedDto();
  input.init(this.bed);

  this._bedService.updateBed(input)
    .pipe(finalize(() => {
      this.saving = false;
      this.cd.detectChanges();
    }))
    .subscribe({
      next: () => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.bsModalRef.hide();
        this.onSave.emit(this.bed);
      },
      error: (err) => {
        this.formErrors = {};

        if (err?.error?.details) {
          if (err.error.details.BedNumber) {
            this.formErrors.bedNumber = err.error.details.BedNumber[0];
          }
          if (err.error.details.RoomId) {
            this.formErrors.roomId = err.error.details.RoomId[0];
          }
        }

        if (!this.formErrors.bedNumber && err.error?.message?.toLowerCase().includes('bednumber')) {
          this.formErrors.bedNumber = err.error.message;
        }

        if (!this.formErrors.roomId && !this.formErrors.bedNumber) {
          this.formErrors.general = err.error?.message || 'Save failed';
        }
      },
    });
}

}
