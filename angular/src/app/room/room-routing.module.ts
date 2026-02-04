import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import{RoomComponent} from  './room.component';
import { CreateRoomDialogComponent } from "./create/create.component";
import { EditRoomDialogComponent } from "./edit/edit.component";
const routes: Routes = [
  {
    path: '',
    component: RoomComponent,
    pathMatch: 'full',
  },
  {
    path: 'create',
    component: CreateRoomDialogComponent,
    pathMatch: 'full',
  },
  {
    path: 'edit/:id',
    component: EditRoomDialogComponent,
    pathMatch: 'full',
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class RoomRoutingModule {}
