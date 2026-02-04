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
export class CreatePatientDialogComponent
  extends AppComponentBase
  implements OnInit {

  saving = false;
  patient: PatientDto = new PatientDto();
  genderOptions: string[] = ['Male', 'Female', 'Other'];

  selectedFile: File | null = null;

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

photoPreview: string | ArrayBuffer | null = null;


  save(): void {
    this.saving = true;

    const dto = new CreatePatientDto();
    dto.patientCode = this.patient.patientCode;
    dto.firstName = this.patient.firstName;
    dto.lastName = this.patient.lastName;

    // ✅ FIX: use moment
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

  // ✅ FIX: add missing method
  sendPatient(dto: CreatePatientDto): void {
    this._patientService.createPatient(dto).subscribe(
      () => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.bsModalRef.hide();
        this.onSave.emit(null);
      },
      () => {
        this.saving = false;
        this.cd.detectChanges();
      }
    );
  }
}
