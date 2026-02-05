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
  CreateBedDto,
  BedCrudServiceServiceProxy,
  RoomDto,
  RoomDtoServiceServiceProxy,
} from '../../../shared/service-proxies/service-proxies';
import { AbpModalHeaderComponent } from '../../../shared/components/modal/abp-modal-header.component';
import { AbpModalFooterComponent } from '../../../shared/components/modal/abp-modal-footer.component';
import { AbpValidationSummaryComponent } from '../../../shared/components/validation/abp-validation.summary.component';
import { LocalizePipe } from '../../../shared/pipes/localize.pipe';

@Component({
  templateUrl: './create.component.html',
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
export class CreateBedDialogComponent extends AppComponentBase implements OnInit {
  saving = false;

  // Form model
  bed: CreateBedDto = new CreateBedDto();

  // Rooms for dropdown
  rooms: RoomDto[] = [];

  // Field-specific backend validation errors
  formErrors: { bedNumber?: string; roomId?: string; general?: string } = {};

  @Output() onSave = new EventEmitter<any>();

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
    this._roomService.getAllRooms()
      .pipe(finalize(() => this.cd.detectChanges()))
      .subscribe((rooms: RoomDto[]) => {
        this.rooms = rooms;
      });
  }

  save(): void {
    this.saving = true;
    this.formErrors = {}; // reset previous errors

    const input = new CreateBedDto();
    input.init(this.bed);

    this._bedService.createBed(input)
      .pipe(finalize(() => {
        this.saving = false;
        this.cd.detectChanges();
      }))
      .subscribe(
        () => {
          this.notify.info(this.l('SavedSuccessfully'));
          this.bsModalRef.hide();
          this.onSave.emit(null);
        },
        (err) => {
          this.formErrors = {};

          // Field-specific backend validation errors
          if (err?.error?.details) {
            if (err.error.details.BedNumber) {
              this.formErrors.bedNumber = err.error.details.BedNumber[0];
            }
            if (err.error.details.RoomId) {
              this.formErrors.roomId = err.error.details.RoomId[0];
            }
          }

          // Check general error messages for duplicates
          if (!this.formErrors.bedNumber && err.error?.message?.toLowerCase().includes('bednumber')) {
            this.formErrors.bedNumber = err.error.message;
          }

          if (!this.formErrors.roomId && !this.formErrors.bedNumber) {
            this.formErrors.general = err.error?.message || 'Save failed';
          }
        }
      );
  }
}
