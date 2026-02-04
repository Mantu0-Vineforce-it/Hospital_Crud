import { Component, Injector, ChangeDetectorRef, ViewChild } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

import { EditPatientDialogComponent } from './edit/edit.component';
import { Table, TableModule } from 'primeng/table';
import { LazyLoadEvent, PrimeTemplate } from 'primeng/api';
import { Paginator, PaginatorModule } from 'primeng/paginator';
import { FormsModule } from '@angular/forms';
import { CommonModule, NgIf } from '@angular/common';
import { appModuleAnimation } from '../../shared/animations/routerTransition';
import { PagedListingComponentBase } from '../../shared/paged-listing-component-base';
import { LocalizePipe } from '../../shared/pipes/localize.pipe';
import { PatientDto, PatientCrudServiceServiceProxy} from '../../shared/service-proxies/service-proxies';
import { CreatePatientDialogComponent } from './create/create.component';


@Component({
  templateUrl: './patient.component.html',
   styleUrl: './patient.component.css',
  animations: [appModuleAnimation()],
  // standalone: true,
 imports: [FormsModule, TableModule, PrimeTemplate, PaginatorModule, LocalizePipe,CommonModule]
})
export class PatientComponent extends PagedListingComponentBase<PatientDto> {
getStudents() {
throw new Error('Method not implemented.');
}

  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  Patients: PatientDto[] = [];
  keyword = '';
 primengTableHelper: any = { 
    records: [], 
    showLoadingIndicator: () => {}, 
    hideLoadingIndicator: () => {}, 
    shouldResetPaging: () => false 
};

  constructor(
    injector: Injector,
    private _patientService: PatientCrudServiceServiceProxy,
    private _modalService: BsModalService,
    cd: ChangeDetectorRef
  ) {
    super(injector, cd);
  }

  list(event?: LazyLoadEvent): void {
    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);

      if (this.primengTableHelper.records?.length) {
        return;
      }
    }

    this.primengTableHelper.showLoadingIndicator();

    debugger
    this._patientService
      .getAllPatients(
      )
      .pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator()))
      .subscribe((result) => {
        this.primengTableHelper.records = result;
        // this.primengTableHelper.totalRecordsCount = result.totalCount;PagedRoleResultRequestDto
        this.cd.detectChanges();
      });
  }

 createPatient(): void {
    const modalRef = this._modalService.show(CreatePatientDialogComponent, {
      class: 'modal-lg',
    });

    modalRef.content.onSave.subscribe(() => this.refresh());
  }

 editPatient(patient: PatientDto): void {
  const modalRef = this._modalService.show(EditPatientDialogComponent, {
    class: 'modal-lg',
    initialState: {
      patient: patient  // ✅ use parameter
    }
  });

  modalRef.content.onSave.subscribe(() => {
    this.refresh();
  });
}

  delete(patient: PatientDto): void {
    abp.message.confirm(
      ('StudentDeleteWarningMessage'),
      undefined,
      (result: boolean) => {
        if (result) {
          this._patientService.deletePatient(patient.id).subscribe(() => {
            abp.notify.success(('SuccessfullyDeleted'));
            this.refresh();
          });
        }
      }
    );
  }
}
