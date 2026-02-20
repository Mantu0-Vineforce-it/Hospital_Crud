import {
    Component,
    Injector,
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    OnInit
} from '@angular/core';

import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { LocalizePipe } from '@shared/pipes/localize.pipe';

import {
    PatientCrudServiceServiceProxy,
    DoctorCrudServiceServiceProxy,
    RoomDtoServiceServiceProxy,
    BedCrudServiceServiceProxy,
    PatientAdmissionCrudServiceServiceProxy,
    DocterDto,
    PatientDto,
    PatientAdmissionDto,
    RoomDto,
    BedDto
} from '@shared/service-proxies/service-proxies';
import { CommonModule } from '@node_modules/@angular/common';

@Component({
    templateUrl: './home.component.html',
    animations: [appModuleAnimation()],
    changeDetection: ChangeDetectionStrategy.OnPush,
    standalone: true,
    imports: [CommonModule, LocalizePipe],
})

export class HomeComponent extends AppComponentBase implements OnInit {

    // 🔢 Counts
    totalDoctors = 0;
    totalPatients = 0;
    totalAdmissions = 0;
    totalRooms = 0;
    totalBeds = 0;
    availableBeds = 0;

    // 📦 Full Data Lists (for table display)
    doctors: DocterDto[] = [];
    patients: PatientDto[] = [];
    admissions: PatientAdmissionDto[] = [];
    rooms: RoomDto[] = [];
    beds: BedDto[] = [];
    occupiedBeds: BedDto[] = [];
    availableBedsList: BedDto[] = [];


    // 🟢 Selected Section (for click behavior)
    selectedSection: string | null = null;

    constructor(
        injector: Injector,
        private patientService: PatientCrudServiceServiceProxy,
        private doctorService: DoctorCrudServiceServiceProxy,
        private roomService: RoomDtoServiceServiceProxy,
        private bedService: BedCrudServiceServiceProxy,
        private admissionService: PatientAdmissionCrudServiceServiceProxy,
        private cdr: ChangeDetectorRef
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.loadDashboardData();
    }

    loadDashboardData(): void {

        // 🩺 Doctors
        this.doctorService.getAllDoctors().subscribe(res => {
            this.doctors = res || [];
            this.totalDoctors = this.doctors.length;
            this.cdr.markForCheck();
        });

        // 🤕 Patients
        this.patientService.getAllPatients().subscribe(res => {
            this.patients = res || [];
            this.totalPatients = this.patients.length;
            this.cdr.markForCheck();
        });

        // 🏥 Admissions
        this.admissionService.getAllPatientAdmissions().subscribe(res => {
            this.admissions = res || [];
            this.totalAdmissions = this.admissions.length;
            this.cdr.markForCheck();
        });

        // 🚪 Rooms
        this.roomService.getAllRooms().subscribe(res => {
            this.rooms = res || [];
            this.totalRooms = this.rooms.length;
            this.cdr.markForCheck();
        });

        this.bedService.getAllBeds().subscribe(res => {
            this.beds = res || [];
            this.totalBeds = this.beds.length;

            this.availableBedsList = this.beds.filter(b => !b.isOccupied);
            this.availableBeds = this.availableBedsList.length;

            // NEW: occupied beds list
            this.occupiedBeds = this.beds.filter(b => b.isOccupied);

            this.cdr.markForCheck();
        });

    }

    showSection(section: string): void {
        this.selectedSection = section;
        this.cdr.markForCheck();
    }

    closeDetails(): void {
        this.selectedSection = null;
        this.cdr.markForCheck();
    }

}
