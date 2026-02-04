import { CommonModule } from "@angular/common";
import { SharedModule } from "primeng/api";
import { BedRoutingModule } from "./bed-routing.module";
import { BedComponent } from "./bed.component";
import { EditBedDialogComponent } from "./edit/edit.component";
import { NgModule } from "@angular/core";
import { BedCrudServiceServiceProxy } from "../../shared/service-proxies/service-proxies";
import { CreateBedDialogComponent } from "./create/create.component";
import { RootRoutingModule } from "../../root-routing.module";

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        BedRoutingModule,

        // standalone components
        BedComponent,
        EditBedDialogComponent,
        CreateBedDialogComponent
    ],
        
})
export class BedModule {

}


