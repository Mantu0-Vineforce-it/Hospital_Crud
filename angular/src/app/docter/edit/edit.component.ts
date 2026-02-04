import { Component, Injector, OnInit, ChangeDetectorRef, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';

import { AppComponentBase } from '../../../shared/app-component-base';
import { DocterDto, DoctorCrudServiceServiceProxy, UpdateDocterDto } from '../../../shared/service-proxies/service-proxies';

import { AbpModalHeaderComponent } from '../../../shared/components/modal/abp-modal-header.component';
import { AbpModalFooterComponent } from '../../../shared/components/modal/abp-modal-footer.component';
import { LocalizePipe } from '../../../shared/pipes/localize.pipe';
import { AbpValidationSummaryComponent } from '@shared/components/validation/abp-validation.summary.component';

@Component({
  templateUrl: './edit.component.html',
  standalone: true,
 imports: [
  CommonModule,
  FormsModule,
  AbpModalHeaderComponent,
  AbpModalFooterComponent,
  AbpValidationSummaryComponent,
  LocalizePipe
]

})
export class EditDoctorDialogComponent extends AppComponentBase implements OnInit {
  saving = false;

  doctor: DocterDto = new DocterDto(); // initialize to avoid undefined

  @Output() onSave = new EventEmitter<DocterDto>();

 readonly doctorSpecializationOptions = [
  { value: 1, label: 'Cardiology' },
  { value: 2, label: 'Dermatology' },
  { value: 3, label: 'Neurology' },
  { value: 4, label: 'Orthopedics' },
];

  constructor(
    injector: Injector,
    private _doctorService: DoctorCrudServiceServiceProxy,
    public bsModalRef: BsModalRef,
    private cd: ChangeDetectorRef
  ) {
    super(injector);
  }

 ngOnInit(): void {
  if (this.bsModalRef.content && this.bsModalRef.content.doctor) {
    this.doctor = this.bsModalRef.content.doctor;

    // Fix NG0100
    this.cd.detectChanges();
  } else {
    this.notify.warn(this.l('DoctorDataNotFound'));
  }
}


  update(): void {
  this.saving = true;

  const input = new UpdateDocterDto();
  input.init(this.doctor);

  this._doctorService.updateDoctor(input).subscribe(
    () => {
      this.notify.info(this.l('UpdatedSuccessfully'));

    this.onSave.emit(this.doctor.clone());
 // 👈 key fix
      this.bsModalRef.hide();
    },
    (error) => {
      this.saving = false;
      this.notify.error(this.l('SaveFailed'));
      console.error('Update doctor error:', error);
    }
  );
}

}
