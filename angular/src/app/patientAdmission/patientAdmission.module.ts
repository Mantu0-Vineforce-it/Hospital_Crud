import { CommonModule } from "@angular/common";
import { SharedModule } from "primeng/api";
import { PatientAdmissionRoutingModule } from "./patientAdmission-routing.module";
import { PatientAdmissionComponent } from "./patientAdmission.component";
import { EditPatientAdmissionDialogComponent } from "./edit/edit.component";
import { NgModule } from "@angular/core";
import { PatientAdmissionDto,PatientAdmissionCrudServiceServiceProxy } from "../../shared/service-proxies/service-proxies";
import { CreatePatientAdmissionDialogComponent } from "./create/create.component";

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
       PatientAdmissionRoutingModule
        ,

        // standalone components
        PatientAdmissionComponent,
        EditPatientAdmissionDialogComponent,
        CreatePatientAdmissionDialogComponent
    ],
        
})
export class PatientAdmissionModule {

}


