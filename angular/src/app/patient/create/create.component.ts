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
import moment from 'moment';
import { finalize } from 'rxjs/operators';

import { AppComponentBase } from '../../../shared/app-component-base';
import {
  CreatePatientDto,
  PatientDto,
  PatientCrudServiceServiceProxy,
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
export class CreatePatientDialogComponent extends AppComponentBase implements OnInit {
  saving = false;
  patient: PatientDto = new PatientDto();
  genderOptions: string[] = ['Male', 'Female', 'Other'];
  selectedFile: File | null = null;
  photoPreview: string | ArrayBuffer | null = null;

  // Field-specific backend errors
  formErrors: { patientCode?: string; email?: string; phoneNumber?: string; general?: string } = {};

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    private _patientService: PatientCrudServiceServiceProxy,
    public bsModalRef: BsModalRef,
    private cd: ChangeDetectorRef
  ) {
    super(injector);
  }

  ngOnInit(): void {}

  onFileSelected(event: any) {
    if (event.target.files && event.target.files.length > 0) {
      this.selectedFile = event.target.files[0];

      const reader = new FileReader();
      reader.onload = () => {
        this.photoPreview = reader.result;
      };
      reader.readAsDataURL(this.selectedFile);
    }
  }

  save(): void {
    this.saving = true;
    this.formErrors = {}; // reset errors

    const dto = new CreatePatientDto();
    dto.patientCode = this.patient.patientCode;
    dto.firstName = this.patient.firstName;
    dto.lastName = this.patient.lastName;
    dto.dateOfBirth = moment(this.patient.dateOfBirth);
    dto.gender = this.patient.gender;
    dto.phoneNumber = this.patient.phoneNumber;
    dto.email = this.patient.email;
    dto.address = this.patient.address;

    if (this.selectedFile) {
      const reader = new FileReader();
      reader.onload = () => {
        dto.photoBase64 = (reader.result as string).split(',')[1];
        this.sendPatient(dto);
      };
      reader.readAsDataURL(this.selectedFile);
    } else {
      this.sendPatient(dto);
    }
  }

  sendPatient(dto: CreatePatientDto): void {
    this._patientService.createPatient(dto)
      .pipe(finalize(() => {
        this.saving = false;
        this.cd.detectChanges();
      }))
      .subscribe({
        next: () => {
          this.notify.info(this.l('SavedSuccessfully'));
          this.bsModalRef.hide();
          this.onSave.emit(null);
        },
        error: (err) => {
          this.formErrors = {}; // reset errors

          // Map backend validation errors
          if (err?.error?.details) {
            if (err.error.details.PatientCode) {
              this.formErrors.patientCode = err.error.details.PatientCode[0];
            }
            if (err.error.details.Email) {
              this.formErrors.email = err.error.details.Email[0];
            }
            if (err.error.details.PhoneNumber) {
              this.formErrors.phoneNumber = err.error.details.PhoneNumber[0];
            }
          }

          // Fallback for general messages
          if (!this.formErrors.patientCode && err.error?.message?.toLowerCase().includes('patient code')) {
            this.formErrors.patientCode = err.error.message;
          }
          if (!this.formErrors.email && err.error?.message?.toLowerCase().includes('email')) {
            this.formErrors.email = err.error.message;
          }
          if (!this.formErrors.phoneNumber && err.error?.message?.toLowerCase().includes('phone')) {
            this.formErrors.phoneNumber = err.error.message;
          }

          if (!this.formErrors.patientCode && !this.formErrors.email && !this.formErrors.phoneNumber) {
            this.formErrors.general = err.error?.message || 'Save failed';
          }
        },
      });
  }
}
