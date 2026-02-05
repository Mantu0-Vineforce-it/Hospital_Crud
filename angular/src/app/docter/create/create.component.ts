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
  CreateDocterDto,
  DocterDto,
  DoctorCrudServiceServiceProxy,
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
export class CreateDoctorDialogComponent
  extends AppComponentBase
  implements OnInit
{
  saving = false;

  // Form model
  doctor: DocterDto = new DocterDto();

  // Dropdown options for doctor specialization
  specializationOptions: string[] = [
    'GeneralPractitioner',
    'Cardiologist',
    'Dermatologist',
    'Neurologist',
    'Pediatrician',
    'Psychiatrist',
    'Oncologist',
    'OrthopedicSurgeon',
    'Gynecologist',
    'Endocrinologist',
  ];

  // Field-specific backend validation errors
  formErrors: { doctorCode?: string; email?: string; phoneNumber?: string; general?: string } = {};

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    private _doctorService: DoctorCrudServiceServiceProxy,
    public bsModalRef: BsModalRef,
    private cd: ChangeDetectorRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    // No-op
  }

 save(): void {
  this.saving = true;
  this.formErrors = {}; // Reset errors before saving

  const input = new CreateDocterDto();
  input.init(this.doctor);

  this._doctorService.createDoctor(input)
    .pipe(finalize(() => {
      this.saving = false;
      this.cd.detectChanges(); // Refresh UI
    }))
    .subscribe(
      () => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.bsModalRef.hide();
        this.onSave.emit(null);
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

        // Handle general errors that might actually be field-specific
        // (e.g., duplicate doctor code returned in the 'message' field)
        if (!this.formErrors.doctorCode && err.error?.message?.toLowerCase().includes('doctor code')) {
          this.formErrors.doctorCode = err.error.message;
        }

        if (!this.formErrors.email && err.error?.message?.toLowerCase().includes('email')) {
          this.formErrors.email = err.error.message;
        }

        if (!this.formErrors.phoneNumber && err.error?.message?.toLowerCase().includes('phone')) {
          this.formErrors.phoneNumber = err.error.message;
        }

        // General error if nothing else matched
        if (!this.formErrors.doctorCode && !this.formErrors.email && !this.formErrors.phoneNumber) {
          this.formErrors.general = err.error?.message || 'Save failed';
        }
      }
    );
}

}
