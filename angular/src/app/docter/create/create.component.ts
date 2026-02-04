
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
  CreateDocterDto,
  DocterDto,
  DoctorCrudServiceServiceProxy,
} from '../../../shared/service-proxies/service-proxies';

import { AbpModalHeaderComponent } from '../../../shared/components/modal/abp-modal-header.component';
import { AbpModalFooterComponent } from '../../../shared/components/modal/abp-modal-footer.component';
import { AbpValidationSummaryComponent } from '../../../shared/components/validation/abp-validation.summary.component';
import { LocalizePipe } from '../../../shared/pipes/localize.pipe';
import { finalize } from 'rxjs/operators';

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

  // form model
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
    // no-op
  }

  save(): void {
    this.saving = true;

    const input = new CreateDocterDto();
    input.init(this.doctor);

    this._doctorService
      .createDoctor(input)
      .pipe(
        finalize(() => {
          this.saving = false;       // ✅ Always reset saving
          this.cd.detectChanges();   // ✅ Refresh UI
        })
      )
      .subscribe(
        () => {
          this.notify.info(this.l('SavedSuccessfully'));
          this.bsModalRef.hide();
          this.onSave.emit(null);     // ✅ Consistent with Patient component
        },
        () => {
          this.notify.error(this.l('SaveFailed'));  // Show error
        }
      );
  }
}
