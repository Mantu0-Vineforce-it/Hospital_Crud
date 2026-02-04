import { Component, Injector, OnInit, ChangeDetectorRef, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { forkJoin } from 'rxjs';
import { finalize } from 'rxjs/operators';
import moment from 'moment';

import { AppComponentBase } from '../../../shared/app-component-base';
import {
  CreatePatientAdmissionDto,
  PatientAdmissionDto,
  PatientCrudServiceServiceProxy,
  DoctorCrudServiceServiceProxy,
  BedCrudServiceServiceProxy,
  PatientAdmissionCrudServiceServiceProxy,
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
export class EditPatientAdmissionDialogComponent extends AppComponentBase implements OnInit {
  saving = false;

  admission: PatientAdmissionDto = new PatientAdmissionDto();

  statusOptions: string[] = ['Admitted', 'Discharged', 'UnderObservation'];

  patients: any[] = [];
  doctors: any[] = [];
  beds: any[] = [];

  @Output() onSave = new EventEmitter<PatientAdmissionDto>();

  constructor(
    injector: Injector,
    private _admissionService: PatientAdmissionCrudServiceServiceProxy,
    private _patientService: PatientCrudServiceServiceProxy,
    private _doctorService: DoctorCrudServiceServiceProxy,
    private _bedService: BedCrudServiceServiceProxy,
    public bsModalRef: BsModalRef,
    private cd: ChangeDetectorRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    // If editing, bsModalRef.content.admission contains existing data
    if (this.bsModalRef.content && this.bsModalRef.content.admission) {
      this.admission = this.bsModalRef.content.admission;

      // Convert string dates to Moment and format for input[type=date]
      if (this.admission.admissionDate) {
         this.admission.admissionDate = moment(this.admission.admissionDate);
       }
      if (this.admission.dischargeDate) {
         this.admission.dischargeDate = moment(this.admission.dischargeDate);
       }
    }

    this.loadDropdowns();
  }

  loadDropdowns(): void {
    forkJoin({
      patients: this._patientService.getAllPatients(),
      doctors: this._doctorService.getAllDoctors(),
      beds: this._bedService.getAllBeds(),
    }).subscribe({
      next: ({ patients, doctors, beds }) => {
        this.patients = patients;
        this.doctors = doctors;
        this.beds = beds;

        // Fix ExpressionChangedAfterItHasBeenCheckedError
        this.cd.detectChanges();
      },
      error: (err) => console.error('Failed to load dropdown data', err),
    });
  }

  save(): void {
    if (this.saving) return; // prevent double click
    this.saving = true;

    const dto = new CreatePatientAdmissionDto();

    // Convert date strings to Moment for backend
    dto.admissionDate = this.admission.admissionDate
      ? moment(this.admission.admissionDate, 'YYYY-MM-DD')
      : null;
    dto.dischargeDate = this.admission.dischargeDate
      ? moment(this.admission.dischargeDate, 'YYYY-MM-DD')
      : null;

    dto.diagnosis = this.admission.diagnosis;
    dto.status = this.admission.status;
    dto.patientId = this.admission.patientId;
    dto.doctorId = this.admission.doctorId;
    dto.bedId = this.admission.bedId;

    this._admissionService
      .createPatientAdmission(dto)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe(
        () => {
          this.notify.success('Patient admission saved successfully.');
          this.onSave.emit(this.admission); // emit updated object
          this.bsModalRef.hide();
        },
        (error) => {
          console.error(error);
          this.notify.error('Error saving patient admission.');
        }
      );
  }
}
