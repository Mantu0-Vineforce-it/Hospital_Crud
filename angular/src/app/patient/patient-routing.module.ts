import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import{PatientComponent} from  './patient.component';
import { CreatePatientDialogComponent } from "./create/create.component";
import { EditPatientDialogComponent } from "./edit/edit.component";
const routes: Routes = [
  {
    path: '',
    component: PatientComponent,
    pathMatch: 'full',
  },
  {
    path: 'create',
    component: CreatePatientDialogComponent,
    pathMatch: 'full',
  },
  {
    path: 'edit/:id',
    component: EditPatientDialogComponent,
    pathMatch: 'full',
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PatientRoutingModule {}
