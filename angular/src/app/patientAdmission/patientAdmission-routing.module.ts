import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import{PatientAdmissionComponent} from  './patientAdmission.component';
import { CreatePatientAdmissionDialogComponent } from "./create/create.component";
import { EditPatientAdmissionDialogComponent } from "./edit/edit.component";
const routes: Routes = [
  {
    path: '',
    component: PatientAdmissionComponent,
    pathMatch: 'full',
  },
  {
    path: 'create',
    component: CreatePatientAdmissionDialogComponent,
    pathMatch: 'full',
  },
  {
    path: 'edit/:id',
    component: EditPatientAdmissionDialogComponent,
    pathMatch: 'full',
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PatientAdmissionRoutingModule {}
