import { CommonModule } from "@angular/common";
import { SharedModule } from "primeng/api";
import { DoctorRoutingModule } from "./doctor-routing.module";
import { DoctorComponent } from "./doctor.component";
import { EditDoctorDialogComponent } from "./edit/edit.component";
import { NgModule } from "@angular/core";
import { DocterDto, DoctorCrudServiceServiceProxy } from "@shared/service-proxies/service-proxies";
import { CreateDoctorDialogComponent } from "./create/create.component";

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        DoctorRoutingModule
        ,

        // standalone components
        DoctorComponent,
        EditDoctorDialogComponent,
        CreateDoctorDialogComponent
    ],
        
})
export class DoctorModule {

}


