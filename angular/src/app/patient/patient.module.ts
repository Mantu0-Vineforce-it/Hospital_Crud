import { CommonModule } from "@angular/common";
import { SharedModule } from "primeng/api";
import { PatientRoutingModule } from "./patient-routing.module";
import { PatientComponent } from "./patient.component";
import { EditPatientDialogComponent } from "./edit/edit.component";
import { NgModule } from "@angular/core";
import { PatientCrudServiceServiceProxy } from "../../shared/service-proxies/service-proxies";
import { CreatePatientDialogComponent } from "./create/create.component";

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        PatientRoutingModule,

        // standalone components
        PatientComponent,
        EditPatientDialogComponent,
        CreatePatientDialogComponent
    ],
        
})
export class PatientModule {

}


