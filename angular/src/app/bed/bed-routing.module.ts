import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import{BedComponent} from  './bed.component';
import { CreateBedDialogComponent } from "./create/create.component";
import { EditBedDialogComponent } from "./edit/edit.component";
const routes: Routes = [
  {
    path: '',
    component: BedComponent,
    pathMatch: 'full',
  },
  {
    path: 'create',
    component: CreateBedDialogComponent,
    pathMatch: 'full',
  },
  {
    path: 'edit/:id',
    component: EditBedDialogComponent,
    pathMatch: 'full',
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BedRoutingModule {}
