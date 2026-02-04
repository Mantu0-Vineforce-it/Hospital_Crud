import { CommonModule } from "@angular/common";
import { SharedModule } from "primeng/api";
import { RoomRoutingModule } from "./room-routing.module";
import { RoomComponent } from "./room.component";
import { EditRoomDialogComponent } from "./edit/edit.component";
import { NgModule } from "@angular/core";
import { RoomDtoServiceServiceProxy } from "../../shared/service-proxies/service-proxies";
import { CreateRoomDialogComponent } from "./create/create.component";
import { RootRoutingModule } from "../../root-routing.module";

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        RoomRoutingModule,

        // standalone components
        RoomComponent,
        EditRoomDialogComponent,
        CreateRoomDialogComponent
    ],
        
})
export class RoomModule {

}


