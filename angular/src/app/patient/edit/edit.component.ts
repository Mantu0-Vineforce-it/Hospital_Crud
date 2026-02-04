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

import { AppComponentBase } from '../../../shared/app-component-base';

import {
  PatientDto,
  PatientCrudServiceServiceProxy,
  UpdatePattientDto,
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
export class EditPatientDialogComponent
  extends AppComponentBase
  implements OnInit
{
  saving = false;

  /** 👇 THIS is how data is passed in */
  patient!: PatientDto;

  @Output() onSave = new EventEmitter<any>();

  genderOptions: string[] = [];

  constructor(
    injector: Injector,
    private _patientService: PatientCrudServiceServiceProxy,
    public bsModalRef: BsModalRef,
    private cd: ChangeDetectorRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    // gender enum
    this.genderOptions = Object.keys(this.l('PatientEnum')).filter(
      (k) => isNaN(Number(k))
    );
  }

  save(): void {
    this.saving = true;

    const input = new UpdatePattientDto();
    input.init(this.patient);

    this._patientService.updatePatient(input).subscribe(
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
