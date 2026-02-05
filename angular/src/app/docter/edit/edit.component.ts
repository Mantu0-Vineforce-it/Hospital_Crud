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
  DocterDto,
  UpdateDocterDto,
  DoctorCrudServiceServiceProxy,
} from '../../../shared/service-proxies/service-proxies';
import { AbpModalHeaderComponent } from '../../../shared/components/modal/abp-modal-header.component';
import { AbpModalFooterComponent } from '../../../shared/components/modal/abp-modal-footer.component';
import { AbpValidationSummaryComponent } from '@shared/components/validation/abp-validation.summary.component';
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
export class EditDoctorDialogComponent
  extends AppComponentBase
  implements OnInit
{
  saving = false;

  doctor: DocterDto = new DocterDto();

  @Output() onSave = new EventEmitter<DocterDto>();

  formErrors: { doctorCode?: string; email?: string; phoneNumber?: string; general?: string } = {};

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
      this.cd.detectChanges(); // Fix NG0100 errors
    } else {
      this.notify.warn(this.l('DoctorDataNotFound'));
    }
  }

  update(): void {
    this.saving = true;
    this.formErrors = {}; // Reset errors

    const input = new UpdateDocterDto();
    input.init(this.doctor);

    this._doctorService
      .updateDoctor(input)
      .pipe(
        finalize(() => {
          this.saving = false;
          this.cd.detectChanges();
        })
      )
      .subscribe(
        () => {
          this.notify.info(this.l('UpdatedSuccessfully'));
          this.onSave.emit(this.doctor.clone());
          this.bsModalRef.hide();
        },
        (err) => {
          // Reset field errors first
          this.formErrors = {};

          if (err?.error?.details) {
            // Field-specific backend validation errors
            if (err.error.details.DoctorCode) {
              this.formErrors.doctorCode = err.error.details.DoctorCode[0];
            }
            if (err.error.details.Email) {
              this.formErrors.email = err.error.details.Email[0];
            }
            if (err.error.details.PhoneNumber) {
              this.formErrors.phoneNumber = err.error.details.PhoneNumber[0];
            }
          }

          // Handle duplicate errors returned as general message
          if (!this.formErrors.doctorCode && err.error?.message?.toLowerCase().includes('doctor code')) {
            this.formErrors.doctorCode = err.error.message;
          }

          if (!this.formErrors.email && err.error?.message?.toLowerCase().includes('email')) {
            this.formErrors.email = err.error.message;
          }

          if (!this.formErrors.phoneNumber && err.error?.message?.toLowerCase().includes('phone')) {
            this.formErrors.phoneNumber = err.error.message;
          }

          if (!this.formErrors.doctorCode && !this.formErrors.email && !this.formErrors.phoneNumber) {
            this.formErrors.general = err.error?.message || 'Save failed';
          }
        }
      );
  }
}
