import { Component, Injector, OnInit, ChangeDetectorRef, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';

import { AppComponentBase } from '../../../shared/app-component-base';
import { RoomDto, CreateRoomDto, RoomDtoServiceServiceProxy } from '../../../shared/service-proxies/service-proxies';
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
export class CreateRoomDialogComponent extends AppComponentBase implements OnInit {
  saving = false;

  // form model
  room: RoomDto = new RoomDto();

  // Room type options for dropdown
  readonly roomOptions = [
    { value: 1, label: 'General Ward' },
    { value: 2, label: 'Semi Private' },
    { value: 3, label: 'Private' },
    { value: 4, label: 'ICU' },
    { value: 5, label: 'VIP Suite' },
  ];

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    private _roomService: RoomDtoServiceServiceProxy,
    public bsModalRef: BsModalRef,
    private cd: ChangeDetectorRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    // no-op
  }

  save(): void {
    this.saving = true;

    const input = new CreateRoomDto();
    input.init(this.room);

    this._roomService.createRoom(input)
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
