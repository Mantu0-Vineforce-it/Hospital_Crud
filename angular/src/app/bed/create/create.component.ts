import { Component, Injector, OnInit, ChangeDetectorRef, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';

import { AppComponentBase } from '../../../shared/app-component-base';
import { CreateBedDto, BedCrudServiceServiceProxy, RoomDto, RoomDtoServiceServiceProxy } from '../../../shared/service-proxies/service-proxies';
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

  // form model
  bed: CreateBedDto = new CreateBedDto();

  // List of rooms to populate the room dropdown
  rooms: RoomDto[] = [];

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
    // Load all rooms for selection
    this._roomService.getAllRooms()
      .pipe(finalize(() => this.cd.detectChanges()))
      .subscribe((rooms: RoomDto[]) => {
        this.rooms = rooms;
      });
  }

  save(): void {
    this.saving = true;

    // Creating the Bed input DTO
    const input = new CreateBedDto();
    input.bedNumber = this.bed.bedNumber;
    input.isOccupied = this.bed.isOccupied;
    input.roomId = this.bed.roomId;

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
        () => {
          this.notify.error(this.l('SaveFailed'));
        }
      );
  }
}
